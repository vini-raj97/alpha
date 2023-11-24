using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Entities;
using BLL;
#nullable disable

namespace MyApp.Namespace
{
	public class AboutModel : PageModel
	{
		private readonly DbVersionServices Services;

		public AboutModel(DbVersionServices services)
		{
			Services = services;
		}

		public BuildVersion DatabaseVersion { get; set; }

		public string SuccessMessage { get; set; }
		public string ErrorMessage { get; set; }

		public async Task OnGet()
		{
			try
			{
				Console.WriteLine($"AboutModel: OnGet");
				DatabaseVersion = await Services.GetDbVersion();
				SuccessMessage = $"Database Retrieve Successful";
			}
			catch (Exception ex)
			{
				ErrorMessage = GetInnerException(ex);
			}
		}

		public string GetInnerException(Exception ex)
		{
			Exception rootCause = ex;
			while (rootCause.InnerException != null)
				rootCause = rootCause.InnerException;
			return rootCause.Message;
		}
	}
}
