using System;

namespace Calyptus.Mvc.Mapping
{
    public interface IMappingConvention
    {
		IEntryMapping[] GetEntryMappings();
		IInstanceMapping[] GetInstanceMappings(object instance);
	}
}
