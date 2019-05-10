using FluentNHibernate.Mapping;

namespace RhinoDox.JobDefinition.Domain.Entities.Mappings
{
    /// <summary>
    /// Defines a mapping for a job definition column map.
    /// </summary>
    public class JobDefinitionColumnMapClassMap : ClassMap<JobDefinitionColumnMap>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionColumnMapClassMap"/> class.
        /// </summary>
        public JobDefinitionColumnMapClassMap()
        {
            Table("job_definition_column_map");
            Id(x => x.JobDefinitionColumnMapId);

            Map(x => x.CSVColumnIndex, "cvs_column_index");
            Map(x => x.MappingType, "mapping_type");
            Map(x => x.AttributeIndex, "attribute_index");

            References(x => x.JobDefinition)
                .Column("job_definition_id");
        }
    }
}
