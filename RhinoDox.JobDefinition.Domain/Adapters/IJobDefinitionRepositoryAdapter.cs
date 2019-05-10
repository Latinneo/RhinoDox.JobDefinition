using RhinoDox.JobDefinition.Domain.Entities;
using System.Collections.Generic;

namespace RhinoDox.JobDefinition.Domain.Adapters
{
    /// <summary>
    /// Protocol for a job definition repository adapter.
    /// </summary>
    public interface IJobDefinitionRepositoryAdapter
    {
        /// <summary>
        /// Gets the job definition using the given identifier.
        /// </summary>
        /// <param name="jobDefinitionId">The job definition identifier.</param>
        /// <returns>The job definition instance if one is found; otherwise, null.</returns>
        Entities.JobDefinition Get(int jobDefinitionId);

        /// <summary>
        /// Gets all the job definitions.
        /// </summary>
        /// <returns>An enumerable of all job definitions.</returns>
        IEnumerable<Entities.JobDefinition> GetAll();

        /// <summary>
        /// Gets all the job definitions owned by the given company.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>An enumerable of all job definitions owned by the given company.</returns>
        IEnumerable<Entities.JobDefinition> GetAll(string companyId);

        /// <summary>
        /// Creates a new job definition using the given arguments.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="description">The job definition description.</param>
        /// <param name="targetCabinetId">The target cabinet id.</param>
        /// <param name="stagingPath">The staging path.</param>
        /// <param name="columnMaps">The column mappings.</param>
        /// <param name="rowsToSkip">The number of rows to skip when parsing the CSV file.</param>
        /// <param name="dataOnly">Whether the CSV file contains only data and no structure.</param>
        /// <returns>The new job definition instance.</returns>
        Entities.JobDefinition Create(string companyId, string description, string targetCabinetId, string stagingPath,
            IList<JobDefinitionColumnMap> columnMaps, int rowsToSkip = 1, bool dataOnly = false);

        /// <summary>
        /// Updates the given job definition.
        /// </summary>
        /// <param name="jobDefinition">The job definition instance.</param>
        void Update(Entities.JobDefinition jobDefinition);

        /// <summary>
        /// Deletes the job definition using the given identifier.
        /// </summary>
        /// <param name="jobDefinitionId">The job definition identifier.</param>
        void Delete(int jobDefinitionId);
    }
}
