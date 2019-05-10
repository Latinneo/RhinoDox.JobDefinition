using RabbitMQ.Client;
using RhinoDox.Core.V2.Logging;
using RhinoDox.Core.V2.Messaging;
using System;

namespace RhinoDox.JobDefinition.Domain.Singletons
{
    /// <summary>
    /// Encapsulate management of the message broker.
    /// </summary>
    public sealed class MessageBrokerSingleton : HasLogger
    {
        private static MessageBrokerSingleton _instance;
        private static readonly object LockObj = new object();

        private MessageBrokerSingleton(string connectionUri)
        {
            try
            {
                var connFactor = new ConnectionFactory()
                {
                    Uri = new Uri(connectionUri),
                    AutomaticRecoveryEnabled = true,
                    RequestedHeartbeat = 500,
                    RequestedConnectionTimeout = 5000,
                    ContinuationTimeout = TimeSpan.FromSeconds(10),
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                MessageBroker = new MessageBroker(connFactor.CreateConnection());
            }
            catch (Exception ex)
            {
                Error("Could not connect to RabbitMq", ex);
            }
        }

        /// <summary>
        /// Gets the current singleton or creates a new one if one doesn't already exists.
        /// </summary>
        /// <param name="connectionUri">The connection uri</param>
        /// <returns>The singleton instance</returns>
        public static MessageBrokerSingleton GetInstance(string connectionUri)
        {
            lock (LockObj)
            {
                return _instance ?? (_instance = new MessageBrokerSingleton(connectionUri));
            }
        }

        /// <summary>
        /// Gets the message broker instance.
        /// </summary>
        public IMessageBroker MessageBroker { get; }
    }
}
