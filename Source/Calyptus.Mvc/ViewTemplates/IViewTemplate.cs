using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public interface IViewTemplate
	{
	}

	public interface IViewTemplate<TMaster> : IViewTemplate where TMaster : IViewTemplate
	{
		TMaster Master { get; set; }
	}
}
