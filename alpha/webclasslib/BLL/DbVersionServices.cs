using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Entities;

namespace BLL
{
	public class DbVersionServices
	{
		private readonly FirebaseClient _firebaseClient;

		public DbVersionServices(FirebaseClient firebaseClient)
		{
			_firebaseClient = firebaseClient ?? throw new ArgumentNullException(nameof(firebaseClient));
		}

		public async Task<BuildVersion> GetDbVersion()
		{
			Console.WriteLine($"DbServices: GetDbVersion;");

			var result = await _firebaseClient
				.Child("BuildVersions")
				.OnceSingleAsync<BuildVersion>();

			return result;
		}
	}
}
