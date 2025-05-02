using System;
using System.Linq;
using System.Web.Mvc;

namespace Service.Helper.FileUploadHelper
{
	/// <summary>
	/// This attribute disables the default model binding for form values in ASP.NET MVC.
	/// It is used to prevent the automatic binding of form values to action method parameters.
	/// </summary>
	/// <remarks>
	/// This is useful when you want to handle form values manually or when you want to use a different model binding approach.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DisableFormValueModelBindingAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var factories = filterContext.Controller.ValueProviderFactories;

			// Remove factories responsible for processing form and query values
			factories.Remove(factories.OfType<FormValueProviderFactory>().FirstOrDefault());
			factories.Remove(factories.OfType<FormCollectionValueProviderFactory>().FirstOrDefault());
			factories.Remove(factories.OfType<QueryStringValueProviderFactory>().FirstOrDefault());

			base.OnActionExecuting(filterContext);
		}
	}
}
