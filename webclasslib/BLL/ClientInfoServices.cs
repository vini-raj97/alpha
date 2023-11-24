using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Firebase.Database;
using Firebase.Database.Query;
using ViewModels;

namespace BLL
{
	public class ClientInfoServices
	{
		private readonly FirebaseClient _firebaseClient;

		public ClientInfoServices(FirebaseClient firebaseClient)
		{
			_firebaseClient = firebaseClient ?? throw new ArgumentNullException(nameof(firebaseClient));
		}

		public List<InfoList> FindClientByLicensePlateNo(string LPNumber)
		{
			Console.WriteLine($"Info: FindByLicensePlateNo(); partialName= {LPNumber}");

			var info = _firebaseClient
				.Child("ClientInfo")
				.OrderBy("LicensePlateNo")
				.StartAt(LPNumber)
				.EndAt(LPNumber + "\uf8ff")
				.OnceAsync<Info>()
				.Result
				.Select(x => new InfoList
				{
					ClientID = int.Parse(x.Key),
					FirstName = x.Object.FirstName,
					LastName = x.Object.LastName,
					LicensePlateNo = x.Object.LicensePlateNo,
					Phone = x.Object.Phone,
					Email = x.Object.Email,
					TeamId = x.Object.TeamId,
					FOB = x.Object.FOB,
					Picture = x.Object.Picture,
					Notes = x.Object.Notes
				})
				.OrderBy(x => x.FirstName)
				.ToList();

			return info;
		}

		public List<InfoList> FindClientsByTeam(int teamId)
		{
			Console.WriteLine($"ClientInfoServices: FindClientsByTeam; TeamId= {teamId}");
			var info = _firebaseClient
				.Child("ClientInfo")
				.OrderBy("TeamId")
				.EqualTo(teamId.ToString()) // Convert teamId to string
				.OnceAsync<Info>()
				.Result
				.Select(x => new InfoList
				{
					ClientID = int.Parse(x.Key),
					FirstName = x.Object.FirstName,
					LastName = x.Object.LastName,
					LicensePlateNo = x.Object.LicensePlateNo,
					Phone = x.Object.Phone,
					Email = x.Object.Email,
					TeamId = x.Object.TeamId,
					FOB = x.Object.FOB,
					Picture = x.Object.Picture,
					Notes = x.Object.Notes,
					TeamInfoReference = x.Object.TeamInfoReference // Include the reference to TeamInfo
				})
				.OrderBy(x => x.FirstName)
				.ToList();
			return info;
		}

		public InfoItem Retrieve(int clientID)
		{
			Console.WriteLine($"ClientInfoServices: Retrieve; clientID= {clientID}");
			var info = _firebaseClient
				.Child("ClientInfo")
				.Child(clientID.ToString())
				.OnceSingleAsync<Info>()
				.Result;

			// Retrieve associated TeamInfo
			var teamInfoReference = info.TeamInfoReference;
			var teamInfo = _firebaseClient
				.Child("TeamInfo")
				.Child(teamInfoReference.TeamId.ToString())
				.OnceSingleAsync<TeamInfo>()
				.Result;

			return new InfoItem
			{
				ClientID = clientID,
				FirstName = info.FirstName,
				LastName = info.LastName,
				LicensePlateNo = info.LicensePlateNo,
				Phone = info.Phone,
				Email = info.Email,
				TeamId = info.TeamId,
				FOB = info.FOB,
				Picture = info.Picture,
				Notes = info.Notes,
				TeamInfoReference = teamInfo
			};
		}


		public void Edit(InfoItem item)
		{
			Console.WriteLine($"ClientInfoServices: Edit; ClientID= {item.ClientID}");

			var existing = _firebaseClient
				.Child("ClientInfo")
				.Child(item.ClientID.ToString())
				.OnceSingleAsync<Info>()
				.Result;

			if (existing == null)
				throw new Exception("Client does not exist");

			existing.FirstName = item.FirstName;
			existing.LastName = item.LastName;
			existing.LicensePlateNo = item.LicensePlateNo;
			existing.Phone = item.Phone;
			existing.Email = item.Email;
			existing.TeamId = item.TeamId;
			existing.FOB = item.FOB;
			existing.Picture = item.Picture;
			existing.Notes = item.Notes;

			_firebaseClient
				.Child("ClientInfo")
				.Child(item.ClientID.ToString())
				.PutAsync(existing);
		}

		public async Task<int> Add(InfoItem item)
		{
			Console.WriteLine($"ClientInfoServices: Add; clientID= {item.ClientID}");

			var exists = await _firebaseClient
				.Child("ClientInfo")
				.OrderBy("LicensePlateNo")
				.EqualTo(item.LicensePlateNo)
				.OnceSingleAsync<Info>();

			if (exists != null)
				throw new Exception("A record with the same License Plate Number already exists");

			var newClient = new Info
			{
				FirstName = item.FirstName,
				LastName = item.LastName,
				LicensePlateNo = item.LicensePlateNo,
				Phone = item.Phone,
				Email = item.Email,
				TeamId = item.TeamId,
				FOB = item.FOB,
				Picture = item.Picture,
				Notes = item.Notes
			};

			var result = await _firebaseClient
				.Child("ClientInfo")
				.PostAsync(newClient);

			return int.Parse(result.Key);
		}

		public void Delete(InfoItem item)
		{
			Console.WriteLine($"ClientInfoServices: Delete; ClientID= {item.ClientID}");

			var existing = _firebaseClient
				.Child("ClientInfo")
				.Child(item.ClientID.ToString())
				.OnceSingleAsync<Info>()
				.Result;

			if (existing == null)
				throw new Exception("Client does not exist");

			_firebaseClient
				.Child("ClientInfo")
				.Child(item.ClientID.ToString())
				.DeleteAsync();
		}
	}
}