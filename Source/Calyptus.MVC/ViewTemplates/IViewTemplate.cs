using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	public interface IViewTemplate
	{
	}

	public interface IViewTemplate<MasterType> : IViewTemplate where MasterType : IViewTemplate
	{
		MasterType Master { get; set; }
	}
}
