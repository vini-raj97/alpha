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
    public class TeamInfoServices
    {
        private readonly FirebaseClient _firebaseClient;

        public TeamInfoServices(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient ?? throw new ArgumentNullException(nameof(firebaseClient));
        }

        public List<SelectionList> ListTeams()
        {
            var info = _firebaseClient
                .Child("TeamInfo")
                .OrderBy("TeamId")
                .OnceAsync<TeamInfo>()
                .Result
                .Select(x => new SelectionList
                {
                    ValueField = x.Object.TeamId,
                    DisplayField = x.Object.TeamName
                })
                .OrderBy(x => x.ValueField)
                .ToList();

            // Add a debug statement
            Console.WriteLine($"TeamInfo count: {info.Count}");
            return info;
        }
    }
}
