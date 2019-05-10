using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using RhinoDox.JobDefinition.Domain.Entities.Mappings;
using System;

namespace RhinoDox.JobDefinition.Domain.Singletons
{
    /// <summary>
    /// Encapsulate management of the NHibernate session factory.
    /// </summary>
    public sealed class NHibernateSessionFactorySingleton
    {
        private readonly string _connectionString;
        private static NHibernateSessionFactorySingleton _instance;
        private static readonly object LockObj = new object();

        /// <summary>
        /// Initialize a new instance of the <see cref="NHibernateSessionFactorySingleton"/> class.
        /// </summary>
        private NHibernateSessionFactorySingleton(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(_connectionString = connectionString).DefaultSchema("app"))
                .Mappings(m => m.FluentMappings.Add<JobDefinitionClassMap>())
                .Mappings(m => m.FluentMappings.Add<JobDefinitionColumnMapClassMap>()
                    .Conventions.Add<EnumConvention>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        public ISessionFactory SessionFactory { get; }

        /// <summary>
        /// Gets the NHibernate session factory singleton.
        /// </summary>
        /// <returns></returns>
        public static NHibernateSessionFactorySingleton GetInstance(string connectionString)
        {
            lock (LockObj)
            {
                // if this is not the first invocation and either:
                //  - the given connection string is null, or
                //  - the existing instance connection string is the same
                // then return the existing instance.
                if (_instance != null && (connectionString == null || IsSameKey(connectionString)))
                {
                    return _instance;
                }

                // Otherwise...
                // Validate connectionString is not null or whitespace only
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentNullException(nameof(connectionString));
                }

                // If a previous instance exists, dispose it.
                _instance?.SessionFactory.Dispose();

                // Then, create and return a new instance
                return _instance = new NHibernateSessionFactorySingleton(connectionString);
            }
        }

        private static bool IsSameKey(string connectionString)
        {
            return _instance != null && string.Compare(_instance._connectionString, connectionString,
                       StringComparison.InvariantCulture) == 0;
        }
    }
}
