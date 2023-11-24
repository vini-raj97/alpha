// Inside TeamInfo.cs
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	[FirestoreData]
	public class TeamInfo
	{
		[Required]
		public int TeamId { get; set; }

		[Required]
		public string TeamName { get; set; }
	}
}
