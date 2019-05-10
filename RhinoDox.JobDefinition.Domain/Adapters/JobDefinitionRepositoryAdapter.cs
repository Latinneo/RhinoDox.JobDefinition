using NHibernate;
using RhinoDox.JobDefinition.Domain.Entities;
using RhinoDox.JobDefinition.Domain.Exceptions;
using RhinoDox.JobDefinition.Domain.Singletons;
using System.Collections.Generic;
using System.Linq;

namespace RhinoDox.JobDefinition.Domain.Adapters
{
    /// <inheritdoc />
    public class JobDefinitionRepositoryAdapter : IJobDefinitionRepositoryAdapter
    {
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionRepositoryAdapter"/> class.
        /// </summary>
        public JobDefinitionRepositoryAdapter(string connectionString)
        {
            _sessionFactory = NHibernateSessionFactorySingleton.GetInstance(connectionString).SessionFactory;
        }

        /// <inheritdoc />
        public Entities.JobDefinition Get(int jobDefinitionId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<Entities.JobDefinition>(jobDefinitionId);
            }
        }

        /// <inheritdoc />
        public IEnumerable<Entities.JobDefinition> GetAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<Entities.JobDefinition>().List();
            }
        }

        /// <inheritdoc />
        public IEnumerable<Entities.JobDefinition> GetAll(string companyId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<Entities.JobDefinition>().Where(x => x.CompanyId == companyId).List();
            }
        }

        /// <inheritdoc />
        public Entities.JobDefinition Create(string companyId, string description, string targetCabinetId, string stagingPath,
            IList<JobDefinitionColumnMap> columnMaps, int rowsToSkip = 1, bool dataOnly = false)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var jobDefinition = new Entities.JobDefinition
                    {
                        CompanyId = companyId,
                        Description = description,
                        TargetCabinetId = targetCabinetId,
                        StagingPath = stagingPath,
                        RowsToSkip = rowsToSkip,
                        DataOnly = dataOnly
                    };

                    jobDefinition.ColumnMaps = columnMaps.Select(m =>
                    {
                        m.JobDefinition = jobDefinition;
                        return m;
                    }).ToList();

                    session.Save(jobDefinition);
                    transaction.Commit();

                    return jobDefinition;

                }
            }
        }

        /// <inheritdoc />
        public void Update(Entities.JobDefinition jobDefinition)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(jobDefinition);
                    transaction.Commit();
                }
            }
        }

        /// <inheritdoc />
        public void Delete(int jobDefinitionId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var jobDefinition = session.Get<Entities.JobDefinition>(jobDefinitionId);
                    if (jobDefinition == null)
                    {
                        throw new JobDefinitionNotFoundException(
                            $"Job definition with id {jobDefinitionId} was not found");
                    }

                    session.Delete(jobDefinition);
                    transaction.Commit();
                }
            }
        }
    }
}
