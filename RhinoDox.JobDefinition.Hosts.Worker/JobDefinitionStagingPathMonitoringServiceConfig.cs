namespace RhinoDox.JobDefinition.Hosts.Worker
{
    /// <summary>
    /// Configuration data for the job definition staging path monitoring service
    /// </summary>
    public class JobDefinitionStagingPathMonitoringServiceConfig
    {
        /// <summary>
        /// Gets or sets the name of the daemon.
        /// </summary>
        public string DaemonName { get; set; }

        /// <summary>
        /// Gets or sets the message queue domain name.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ uri.
        /// </summary>
        public string RabbitMqUri { get; set; }

        /// <summary>
        /// Gets or sets the Postgres database connection string.
        /// </summary>
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the monitoring settings.
        /// </summary>
        public MonitoringConfig Monitoring { get; set; }
    }

    /// <summary>
    /// Configuration data for the monitoring section of the daemon config.
    /// </summary>
    public class MonitoringConfig
    {
        /// <summary>
        /// Gets or sets the search pattern.
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Gets or sets the monitoring sleep time (in milliseconds)
        /// </summary>
        public int SleepTimeInMilliseconds { get; set; }
    }
}
