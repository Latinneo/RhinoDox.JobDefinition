using System.Collections.Generic;

namespace RhinoDox.JobDefinition.Domain.Entities
{
    /// <summary>
    /// Encapsulate the details of a job definition.
    /// </summary>
    public class JobDefinition
    {
        /// <summary>
        /// Gets or sets the job definition identifier.
        /// </summary>
        public virtual int JobDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the job definition description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the company that owns the job definition.
        /// </summary>
        public virtual string CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the cabinet identifier targeted by this job definition.
        /// </summary>
        public virtual string TargetCabinetId { get; set; }

        /// <summary>
        /// Gets or sets the staging path used to watch for dropped files.
        /// </summary>
        public virtual string StagingPath { get; set; }

        /// <summary>
        /// Gets or sets how many rows to skip from CSV files before parsing starts.
        /// </summary>
        public virtual int RowsToSkip { get; set; }

        /// <summary>
        /// Gets or sets whether the CSV file contain only data (no column headers)
        /// </summary>
        public virtual bool DataOnly { get; set; }

        /// <summary>
        /// Gets or sets the job definition column mappings.
        /// </summary>
        public virtual IList<JobDefinitionColumnMap> ColumnMaps { get; set; }
    }
}
