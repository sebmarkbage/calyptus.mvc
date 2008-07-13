using System;
using System.IO;

namespace Calyptus.MVC
{
	public interface IView : IViewTemplate, IRenderable
	{
		string ContentType { get; }
		void Render(Stream stream);
	}

	public interface IView<TTemplate> : IView where TTemplate : class, IViewTemplate
	{
		TTemplate Template { get; set; }
	}

	public interface IView<TTemplate, TMaster> : IView<TTemplate>
		where TTemplate : class, IViewTemplate<TMaster>
		where TMaster : class, IViewTemplate
	{
		IView<TMaster> Master { get; set; }
	}

}