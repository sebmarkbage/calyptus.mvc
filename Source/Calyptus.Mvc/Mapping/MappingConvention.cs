using System;

namespace Calyptus.Mvc.Mapping
{
	public abstract class MappingConvention : IMappingConvention
	{
		public abstract IEntryMapping[] GetEntryMappings();

		public abstract IInstanceMapping[] GetInstanceMappings(object instance);
	}
}
