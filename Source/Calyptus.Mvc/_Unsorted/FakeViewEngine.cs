using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Calyptus.Mvc
{
	public class FakeViewEngine : IResponseEngine
	{
		public bool TryWriteResponse(IHttpContext context, string contentType, object template)
		{
			IViewTemplate t = template as IViewTemplate;
			if (t == null) return false;
			IView v = new MockView(t);
			v.Render(context);
			return true;
		}

		public class FakeView : IView
		{
			IViewTemplate _template;

			public FakeView(IViewTemplate template)
			{
				_template = template;
			}

			public IViewTemplate Master { get; set; }

			public void Render(IHttpContext context)
			{
				context.Response.ContentType = ContentType;
				Render(context.Response.Output);
			}

			public string ContentType
			{
				get { return "text/html"; }
			}

			public void Render(TextWriter writer)
			{
				writer.Write("<html><head><title>Test</title></head><body><h1>");
				writer.Write(String.Format("TEST VIEW ({0})", _template.GetType().FullName));
				writer.Write("</h1></body></html>");
			}
		}
	}
}
