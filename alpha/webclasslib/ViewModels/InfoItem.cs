using Entities;
using Google.Cloud.Firestore;

[FirestoreData]
public class InfoItem
{
    [FirestoreProperty("FirstName")]
    public string? FirstName { get; set; }

    [FirestoreProperty("LastName")]
    public string? LastName { get; set; }

    [FirestoreProperty("LicensePlateNo")]
    public string? LicensePlateNo { get; set; }

    [FirestoreProperty("Phone")]
    public string? Phone { get; set; }

    [FirestoreProperty("Email")]
    public string? Email { get; set; }

    [FirestoreProperty("TeamId")]
    public int TeamId { get; set; }

    [FirestoreProperty("FOB")]
    public string? FOB { get; set; }

    [FirestoreProperty("Picture")]
    public string? Picture { get; set; }

    [FirestoreProperty("Notes")]
    public string? Notes { get; set; }

    [FirestoreProperty("id")]
    public int Id { get; set; }

    [FirestoreProperty("Created_At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [FirestoreProperty("ClientID")]
    public int ClientID { get; set; }

    [FirestoreProperty]
    public TeamInfo TeamInfoReference { get; set; }
}