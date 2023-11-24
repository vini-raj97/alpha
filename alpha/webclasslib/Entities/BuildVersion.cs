using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class BuildVersion
    {
        [FirestoreProperty]
        public int Major { get; set; }

        [FirestoreProperty]
        public int Minor { get; set; }

        [FirestoreProperty]
        public int Build { get; set; }

        [FirestoreProperty]
        public Timestamp ReleaseDate { get; set; }

        public override string ToString()
        {
            return $"Major: {Major}, Minor: {Minor}, Build: {Build}, Release Date: {ReleaseDate.ToDateTime()}";
        }
    }
}
