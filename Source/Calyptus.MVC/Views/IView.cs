using System;

namespace Calyptus.MVC
{
	public interface IView : IViewTemplate, IRenderable
	{
	}

	public interface IView<TemplateType> : IView where TemplateType : IViewTemplate
	{
		void Render(IHttpContext context, TemplateType view);
	}

	public interface IView<TemplateType, MasterType> : IView<TemplateType> where TemplateType : IViewTemplate<MasterType> where MasterType : IViewTemplate
	{
		IView<MasterType> Master { get; set; }
	}

}