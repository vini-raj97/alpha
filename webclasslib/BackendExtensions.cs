using Microsoft.Extensions.DependencyInjection;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using DAL;
using BLL;
using Google.Cloud.Firestore.V1;
using Google.Apis.Services;

namespace webclasslib
{
    public static class BackendExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services, string projectId, string serviceAccountJson)
        {
            // Initialize Firestore client with service account
            FirestoreDb firestoreDb = FirestoreDb.Create(projectId, new FirestoreClientBuilder
            {
                CredentialsPath = GetJsonCredentialsPath(serviceAccountJson)
            }.Build());

            services.AddSingleton<FirestoreDb>(firestoreDb);

            services.AddTransient<DbVersionServices>();
            services.AddTransient<TeamInfoServices>();
            services.AddTransient<ClientInfoServices>();
        }

        private static string GetJsonCredentialsPath(string serviceAccountJson)
        {
            // Save the service account JSON to a temporary file
            var tempPath = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(tempPath, serviceAccountJson);
            return tempPath;
        }
    }
}
