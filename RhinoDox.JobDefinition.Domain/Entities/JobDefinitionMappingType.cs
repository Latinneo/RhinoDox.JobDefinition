namespace RhinoDox.JobDefinition.Domain.Entities
{
    /// <summary>
    /// Enumeration of the different job definition mapping types.
    /// </summary>
    public enum JobDefinitionMappingType
    {
        /// <summary>
        /// Mapping refers to a document set.
        /// </summary>
        DocSet,
        /// <summary>
        /// Mapping refers to a document type.
        /// </summary>
        DocType,

        /// <summary>
        /// Mapping refers to a file path.
        /// </summary>
        FilePath,

        /// <summary>
        /// Mapping refers to a folder attribute.
        /// </summary>
        FolderAttribute,

        /// <summary>
        /// Mapping refers to a document attribute.
        /// </summary>
        DocumentAttribute
    }
}
