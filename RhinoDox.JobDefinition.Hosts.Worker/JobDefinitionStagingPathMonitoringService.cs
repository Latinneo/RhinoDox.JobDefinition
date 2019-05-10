using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RhinoDox.Core.V2.Messaging;
using RhinoDox.JobDefinition.Domain.Singletons;
using RhinoDox.JobDefinition.Events.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RhinoDox.JobDefinition.Domain.Adapters;

namespace RhinoDox.JobDefinition.Hosts.Worker
{
    /// <summary>
    /// A service that monitor the staging path of job definitions and then
    /// send notification events when a file matching the search pattern is dropped.
    /// </summary>
    public class JobDefinitionStagingPathMonitoringService : IHostedService, IDisposable
    {
        private readonly ILogger<JobDefinitionStagingPathMonitoringService> _logger;
        private readonly IOptions<JobDefinitionStagingPathMonitoringServiceConfig> _config;
        private readonly IJobDefinitionRepositoryAdapter _repositoryAdapter;
        private readonly IMessageBroker _messageBroker;

        private readonly Dictionary<int, Tuple<CancellationTokenSource, Task>> _tasks =
            new Dictionary<int, Tuple<CancellationTokenSource, Task>>();

        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionStagingPathMonitoringService"/> class
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="config">The config instance.</param>
        /// <param name="repositoryAdapter">The job definition repository adapter.</param>
        public JobDefinitionStagingPathMonitoringService(ILogger<JobDefinitionStagingPathMonitoringService> logger,
            IOptions<JobDefinitionStagingPathMonitoringServiceConfig> config,
            IJobDefinitionRepositoryAdapter repositoryAdapter)
        {
            _logger = logger;
            _config = config;
            _repositoryAdapter = repositoryAdapter;
            _messageBroker = MessageBrokerSingleton.GetInstance(config.Value.RabbitMqUri).MessageBroker;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting job definition service: " + _config.Value.DaemonName);
            if (!cancellationToken.IsCancellationRequested)
            {
                // 1) Read all job definitions.

                var jobDefinitions = _repositoryAdapter.GetAll();
                foreach (var item in jobDefinitions)
                {
                    // 2) Spawn a background task for each active job definition.
                    AddStagingMonitoringTask(item.JobDefinitionId, item.StagingPath);
                }

                // 3) Subscribe to job definition created or updated events.
                var handler = new Handler(_messageBroker, _config.Value.DomainName);
                handler
                    .WithHandle<JobDefinitionCreated>(HandleJobDefinitionCreated)
                    .WithHandle<JobDefinitionUpdated>(HandleJobDefinitionUpdated);

                _messageBroker.Start();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping job definition service.");

            if (!cancellationToken.IsCancellationRequested)
            {
                // 1) Unsubscribe from job definition created or updated events.
                _messageBroker.Stop();

                // 2) Request cancellation of all background tasks.
                foreach (var kv in _tasks)
                {
                    var (cancellationTokenSource, _) = kv.Value;
                    cancellationTokenSource.Cancel();
                }

                // 3) Wait until the task completes or the stop token triggers
                Task.WaitAll(_tasks.Values.Select(tuple => tuple.Item2).ToArray(), cancellationToken);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _messageBroker?.Stop();

            foreach (var kv in _tasks)
            {
                var (cancellationTokenSource, _) = kv.Value;
                cancellationTokenSource.Dispose();
            }
        }

        private void AddStagingMonitoringTask(int jobDefinitionId, string stagingPath)
        {
            var taskCancellationTokenSource = new CancellationTokenSource();

            _tasks.Add(jobDefinitionId,
                new Tuple<CancellationTokenSource, Task>(taskCancellationTokenSource,
                    Task.Run(() => MonitorStagingPath(jobDefinitionId, stagingPath, taskCancellationTokenSource.Token),
                        taskCancellationTokenSource.Token)));
        }

        private void HandleJobDefinitionCreated(JobDefinitionCreated request)
        {
            // Add new staging monitoring task.
            AddStagingMonitoringTask(request.Data.JobDefinitionId, request.Data.StagingPath);
        }

        private void HandleJobDefinitionUpdated(JobDefinitionUpdated request)
        {
            // If a task already exist for a given job definition
            if (_tasks.ContainsKey(request.Data.JobDefinitionId))
            {
                // locate it, request cancellation, and wait until its finished
                var (cancellationTokenSource, task) = _tasks[request.Data.JobDefinitionId];
                cancellationTokenSource.Cancel();
                task.Wait();

                // then, remove it from the tasks collection.
                _tasks.Remove(request.Data.JobDefinitionId);
            }

            // Finally, add the new monitoring task.
            AddStagingMonitoringTask(request.Data.JobDefinitionId, request.Data.StagingPath);
        }

        private void MonitorStagingPath(int jobDefinitionId, string stagingPath, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Monitoring staging path " + stagingPath);

                var publisher = new Publisher(_messageBroker, _config.Value.DomainName);
                var zipFiles = Directory.GetFiles(stagingPath, _config.Value.Monitoring.SearchPattern);
                foreach (var zipFile in zipFiles)
                {
                    publisher.PublishTell(new FileReadyForProcessing()
                    {
                        JobDefinitionId = jobDefinitionId,
                        FilePath = zipFile
                    });
                }

                Thread.Sleep(_config.Value.Monitoring.SleepTimeInMilliseconds);
            }
        }
    }
}