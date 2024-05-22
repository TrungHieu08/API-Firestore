using Google.Cloud.Firestore;

namespace WebApplication1.Service
{
    public interface IFirestoreService
    {
        Task<string> AddDocumentAsync(string collection, Dictionary<string, object> data);
        Task UpdateDocumentAsync(string collection, string documentId, Dictionary<string, object> data);
        Task DeleteDocumentAsync(string collection, string documentId);
        Task<DocumentSnapshot> GetDocumentAsync(string collection, string documentId);
    }

    public class FirestoreService : IFirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        public async Task<string> AddDocumentAsync(string collection, Dictionary<string, object> data)
        {
            DocumentReference docRef = _firestoreDb.Collection(collection).Document();
            await docRef.SetAsync(data);
            return docRef.Id;
        }


        public async Task UpdateDocumentAsync(string collection, string documentId, Dictionary<string, object> data)
        {
            DocumentReference docRef = _firestoreDb.Collection(collection).Document(documentId);
            await docRef.UpdateAsync(data);
        }

        public async Task DeleteDocumentAsync(string collection, string documentId)
        {
            DocumentReference docRef = _firestoreDb.Collection(collection).Document(documentId);
            await docRef.DeleteAsync();
        }

        public async Task<DocumentSnapshot> GetDocumentAsync(string collection, string documentId)
        {
            DocumentReference docRef = _firestoreDb.Collection(collection).Document(documentId);
            return await docRef.GetSnapshotAsync();
        }
    }
}
