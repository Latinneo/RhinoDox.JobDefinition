using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

namespace RhinoDox.JobDefinition.Domain.Singletons
{
    /// <summary>
    /// An implementation of <see cref="IPropertyConvention"/> and <see cref="IPropertyConventionAcceptance"/>
    /// to allow using enums when mapping entity properties.
    /// </summary>
    internal class EnumConvention :
        IPropertyConvention,
        IPropertyConventionAcceptance
    {
        #region IPropertyConvention Members

        /// <inheritdoc />
        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        #endregion

        #region IPropertyConventionAcceptance Members

        /// <inheritdoc />
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum ||
                                 (x.Property.PropertyType.IsGenericType &&
                                  x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                  x.Property.PropertyType.GetGenericArguments()[0].IsEnum)
            );
        }

        #endregion
    }
}
