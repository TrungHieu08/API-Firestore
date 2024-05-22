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
