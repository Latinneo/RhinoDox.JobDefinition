using FluentNHibernate.Mapping;

namespace RhinoDox.JobDefinition.Domain.Entities.Mappings
{
    /// <summary>
    /// Defines a mapping for a job definition.
    /// </summary>
    public class JobDefinitionClassMap : ClassMap<JobDefinition>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionClassMap"/> class.
        /// </summary>
        public JobDefinitionClassMap()
        {
            Table("job_definition");
            Id(x => x.JobDefinitionId, "job_definition_id");
            Map(x => x.Description, "description");
            Map(x => x.CompanyId, "company_id");
            Map(x => x.TargetCabinetId, "target_cabinet_id");
            Map(x => x.StagingPath, "staging_path");
            Map(x => x.RowsToSkip, "rows_to_skip");
            Map(x => x.DataOnly, "data_only");
            HasMany<JobDefinitionColumnMap>(x => x.ColumnMaps)
                .Not.LazyLoad()
                .KeyColumn("job_definition_id")
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}
