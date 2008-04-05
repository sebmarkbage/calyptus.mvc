using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	public class MockViewFactory : IViewFactory
	{
		public IView CreateTemplate(IHttpContext context, IViewTemplate view)
		{
			return new MockView(view);
		}

		private class MockView : IView
		{
			IViewTemplate _template;

			public MockView(IViewTemplate template)
			{
				_template = template;
			}

			public IViewTemplate Master { get; set; }

			public void Render(IHttpContext context)
			{
				context.Response.ContentType = "text/html";
				context.Response.Write("<html><head><title>Test</title></head><body><h1>");
				context.Response.Write(String.Format("TEST VIEW ({0})", _template.GetType().FullName));
				context.Response.Write("</h1></body></html>");
			}
		}
	}
}
