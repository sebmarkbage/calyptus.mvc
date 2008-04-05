using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
    public interface IViewFactory
    {
		IView CreateTemplate(IHttpContext context, IViewTemplate view);
	}
}
