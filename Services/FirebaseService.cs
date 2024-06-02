// Services/FirebaseService.cs
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace LedConnector.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://dotnet-8a9fb-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async Task SendMessageToFirebase(string message)
        {
            await _firebaseClient
                .Child("messages")
                .PostAsync(message);
        }
    }
}