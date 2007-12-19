using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	interface IController
	{
		IHttpContext Context { get; set; }

		IRoutingEngine Routing { get; set; }

		IViewEngine ViewEngine { get; set; }
	}
}
