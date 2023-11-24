using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL;
using ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;

namespace MyApp.Namespace
{
	public class QueryCrudModel : PageModel
	{
		private readonly ClientInfoServices ClientInfoServices;
		private readonly TeamInfoServices TeamInfoServices;

		public QueryCrudModel(ClientInfoServices clientservices, TeamInfoServices teamservices)
		{
			ClientInfoServices = clientservices;
			TeamInfoServices = teamservices;
		}

		public string? SuccessMessage { get; set; }
		public string? ErrorMessage { get; set; }
		public List<Exception> Errors { get; set; } = new();
		[BindProperty]
		public string ButtonPressed { get; set; }
		[BindProperty]
		public string FilterType { get; set; }
		[BindProperty]
		public string LPNumber { get; set; }
		[BindProperty]
		public int SelectedTeamId { get; set; }
		[BindProperty]
		public List<InfoList> SearchedClients { get; set; }
		[BindProperty]
		public InfoItem Info { get; set; } = new();
		[BindProperty]
		public List<SelectionList> SelectListOfTeams { get; set; }

		public IActionResult OnGet()
		{
			try
			{
				Console.WriteLine("QueryModel: OnGet");
				PopulateSelectLists();
			}
			catch (Exception ex)
			{
				ErrorMessage = GetInnerException(ex);
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				Console.WriteLine("QueryModel: OnPost");

				if (ButtonPressed == "SearchByLicensePlateNo")
				{
					FilterType = "PartialString";
					SuccessMessage = "Searched by LP number";
				}
				else if (ButtonPressed == "SearchByTeam")
				{
					FilterType = "DropDown";
					SuccessMessage = "Searched by team Dropdown";
				}
				else if (ButtonPressed == "Add")
				{
					FormValidation();
					Info.ClientID = await ClientInfoServices.Add(Info);
					SuccessMessage = "Add Successful";
				}
				else if (ButtonPressed == "Update")
				{
					FormValidation();
					ClientInfoServices.Edit(Info);
					SuccessMessage = "Update Successful";
				}
				else if (ButtonPressed == "Delete")
				{
					ClientInfoServices.Delete(Info);
					Info = new InfoItem();
					SuccessMessage = "Delete Successful";
				}
				else if (ButtonPressed == "Clear")
				{
					Info = new InfoItem();
					SuccessMessage = "Clear Successful";
				}
				else if (Info.ClientID != 0)
				{
					Info = ClientInfoServices.Retrieve(Info.ClientID);
					SuccessMessage = "Program Retrieve Successful";
				}
				else
				{
					ErrorMessage = "Danger: At the end of our ropes!";
				}
			}
			catch (AggregateException ex)
			{
				ErrorMessage = ex.Message;
			}
			catch (Exception ex)
			{
				ErrorMessage = GetInnerException(ex);
			}
			PopulateSelectLists();
			GetClients(FilterType);
			return Page();
		}

		private void PopulateSelectLists()
		{
			try
			{
				Console.WriteLine("Querymodel: PopulateSelectLists");
				SelectListOfTeams = TeamInfoServices.ListTeams();

				Console.WriteLine($"SelectListOfTeams count: {SelectListOfTeams.Count}");
			}
			catch (Exception ex)
			{
				ErrorMessage = GetInnerException(ex);
			}
		}

		private void GetClients(string filterType)
		{
			try
			{
				Console.WriteLine($"QueryCrudModel: GetProducts:filtertype= {filterType}");
				if (filterType == "PartialString")
					SearchedClients = ClientInfoServices.FindClientByLicensePlateNo(LPNumber);
				else if (filterType == "DropDown")
					SearchedClients = ClientInfoServices.FindClientsByTeam(SelectedTeamId);
			}
			catch (Exception ex)
			{
				ErrorMessage = GetInnerException(ex);
			}
		}

		public void FormValidation()
		{
			if (string.IsNullOrEmpty(Info.FirstName))
				Errors.Add(new Exception("FirstName"));
			if (string.IsNullOrEmpty(Info.LastName))
				Errors.Add(new Exception("LastName"));
			if (string.IsNullOrEmpty(Info.LicensePlateNo))
				Errors.Add(new Exception("LicensePlateNo"));
			if (string.IsNullOrEmpty(Info.Phone))
				Errors.Add(new Exception("Phone"));
			if (string.IsNullOrEmpty(Info.Email))
				Errors.Add(new Exception("Email"));
			if (Info.TeamId == 0)
				Errors.Add(new Exception("Team"));
			if (string.IsNullOrEmpty(Info.FOB))
				Errors.Add(new Exception("FOB"));
			if (Errors.Count > 0)
				throw new AggregateException("Invalid Data: ", Errors);

			if (Info.FirstName.Length > 100)
				Errors.Add(new Exception("FirstName > 100"));
			if (Info.LastName.Length > 100)
				Errors.Add(new Exception("LastName > 100"));
			if (Info.LicensePlateNo.Length > 12)
				Errors.Add(new Exception("LicensePlateNo > 12"));

			if (Errors.Count > 0)
				throw new AggregateException("Invalid Data: ", Errors);
		}

		public string GetInnerException(Exception e)
		{
			Exception rootCause = e;
			while (rootCause.InnerException != null)
				rootCause = rootCause.InnerException;
			return rootCause.Message;
		}
	}
}
