using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public class MockViewFactory : IViewFactory
	{
		public IView FindView(IHttpContext context, IViewTemplate view)
		{
			return new MockView(view);
		}

		public static IView CreateView(IHttpContext context, IViewTemplate view)
		{
			return new MockView(view);
		}

		public class MockView : IView
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

			public string ContentType
			{
				get { return "text/html"; }
			}

			public void Render(System.IO.Stream stream)
			{
				throw new NotImplementedException();
			}
		}
	}
}
