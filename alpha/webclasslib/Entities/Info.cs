using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;

namespace Entities
{
    [FirestoreData]
    public partial class Info
    {
        [FirestoreProperty]
        public string? FirstName { get; set; }

        [FirestoreProperty]
        public string? LastName { get; set; }

        [FirestoreProperty]
        public string? LicensePlateNo { get; set; }

        [FirestoreProperty]
        public string? Phone { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }

        [FirestoreProperty]
        public int TeamId { get; set; }

        [FirestoreProperty]
        public string? FOB { get; set; }

        // Firestore doesn't directly handle file uploads, so you might want to store a reference or URL to the file
        [FirestoreProperty]
        public string? Picture { get; set; }

        [FirestoreProperty]
        public string? Notes { get; set; }

        // Additional property to store the reference to TeamInfo
        [FirestoreProperty]
        public TeamInfo TeamInfoReference { get; set; }

        // This is not needed; Firestore generates the IDs automatically
        public int ClientID { get; internal set; }

        // This might not be needed, depending on your usage
        public object? TeamInfo { get; internal set; }

        // This method seems to be incomplete; consider removing it or implementing it properly
        internal string Key()
        {
            throw new NotImplementedException();
        }

        // This conversion method is not implemented; consider removing it or implementing it properly
        // public static implicit operator Info(Info v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
