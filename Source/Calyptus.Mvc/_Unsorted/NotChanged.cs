using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Calyptus.Mvc
{
	public class NotChanged : Exception, IView
	{
		void IResponse.Render(IHttpContext context)
		{
			IHttpResponse response = context.Response;
			response.Clear();
			response.StatusCode = 304;
		}

		string IView.ContentType
		{
			get { return null; }
		}

		void IView.Render(TextWriter stream)
		{
			throw new NotImplementedException();
		}
	}
}
