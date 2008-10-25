using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public class NotChanged : Exception, IView
	{
		void IRenderable.Render(IHttpContext context)
		{
			IHttpResponse response = context.Response;
			response.Clear();
			response.StatusCode = 304;
		}

		string IView.ContentType
		{
			get { return null; }
		}

		void IView.Render(System.IO.Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
