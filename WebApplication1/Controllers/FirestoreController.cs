using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreController(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        [HttpGet("Chat")]
        public async Task<List<ChatMessager>> GetMessagesFromFirestore()
        {
            List<ChatMessager> messages = new List<ChatMessager>();

            // Get a reference to the "messenger" collection
            CollectionReference collectionReference = _firestoreDb.Collection("messenger");

            // Query documents within the "messenger" collection
            QuerySnapshot querySnapshot = await collectionReference.GetSnapshotAsync();

            // Iterate through the documents and map them to ChatMessage objects
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                ChatMessager message = documentSnapshot.ConvertTo<ChatMessager>();
                messages.Add(message);
            }

            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] ChatMessager messenger)
        {
            try
            {
                CollectionReference collectionReference = _firestoreDb.Collection("messenger");
                DocumentReference documentReference = await collectionReference.AddAsync(messenger);
                var idCollection = documentReference.Id;
                return Ok(idCollection);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    [FirestoreData]
    public class ChatMessager
    {
        [FirestoreProperty]
        public string? User { get; set; }

        [FirestoreProperty]
        public string? Message { get; set; }
    }
}
