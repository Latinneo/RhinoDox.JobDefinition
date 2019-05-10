namespace RhinoDox.JobDefinition.Domain.Entities
{
    /// <summary>
    /// Encapsulate the mapping between CSV file columns and cabinet attributes.
    /// </summary>
    public class JobDefinitionColumnMap
    {
        /// <summary>
        /// Gets or sets the job definition column map identifier.
        /// </summary>
        public virtual int JobDefinitionColumnMapId { get; set; }

        /// <summary>
        /// Gets or sets the column index in the CSV file.
        /// </summary>
        public virtual int CSVColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets the mapping type.
        /// </summary>
        public virtual JobDefinitionMappingType MappingType { get; set; }

        /// <summary>
        /// Gets or sets the index of the folder or document attribute
        /// <remarks>This property is used only if mapping type refers to a folder or document attribute.</remarks>
        /// </summary>
        public virtual int? AttributeIndex { get; set; }

        /// <summary>
        /// Gets or sets the job definition that owns this column mapping.
        /// </summary>
        public virtual JobDefinition JobDefinition { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="JobDefinitionColumnMap"/> class using the given arguments.
        /// </summary>
        /// <param name="cvsColumnIndex">The index of the column in the CSV file.</param>
        /// <param name="mappingType">The mapping type.</param>
        /// <param name="attributeIndex">The index of the cabinet attribute.</param>
        /// <returns>The new job definition column map instance.</returns>
        public static JobDefinitionColumnMap Create(int cvsColumnIndex, JobDefinitionMappingType mappingType,
            int? attributeIndex = null)
        {
            return Create(null, cvsColumnIndex, mappingType, attributeIndex);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="JobDefinitionColumnMap"/> class using the given arguments.
        /// </summary>
        /// <param name="jobDefinition">The job definition that owns the new column mapping.</param>
        /// <param name="cvsColumnIndex">The index of the column in the CSV file.</param>
        /// <param name="mappingType">The mapping type.</param>
        /// <param name="attributeIndex">The index of the cabinet attribute.</param>
        /// <returns>The new job definition column map instance.</returns>
        public static JobDefinitionColumnMap Create(JobDefinition jobDefinition, int cvsColumnIndex, JobDefinitionMappingType mappingType, int? attributeIndex = null)
        {
            return new JobDefinitionColumnMap
            {
                JobDefinition = jobDefinition,
                CSVColumnIndex = cvsColumnIndex,
                MappingType = mappingType,
                AttributeIndex = attributeIndex
            };
        }
    }
}
