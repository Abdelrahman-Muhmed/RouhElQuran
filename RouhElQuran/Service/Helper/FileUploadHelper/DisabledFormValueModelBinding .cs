
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Service.Helper.FileUploadHelper
{


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
	{
		/// <summary>
		/// Disables model binding for the request by removing all value providers.
		/// </summary>

		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			// Remove all value providers to disable model binding
			var valueProviderFactories = context.ValueProviderFactories;
			valueProviderFactories.RemoveType<FormValueProviderFactory>();
			valueProviderFactories.RemoveType<QueryStringValueProviderFactory>();
			valueProviderFactories.RemoveType<RouteValueProviderFactory>();
		}

		public void OnResourceExecuted(ResourceExecutedContext context)
		{
		}
	}
}