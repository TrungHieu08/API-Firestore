using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get Firebase config JSON from configuration
string firebaseConfigJson = builder.Configuration.GetValue<string>("FIREBASE_CONFIG");

// Create GoogleCredential from JSON string
GoogleCredential googleCredential = GoogleCredential.FromJson(firebaseConfigJson);

// Initialize Firebase App with the GoogleCredential
FirebaseApp.Create(new AppOptions()
{
    Credential = googleCredential
});

// Write the JSON config to a temporary file
string tempFilePath = Path.GetTempFileName();
File.WriteAllText(tempFilePath, firebaseConfigJson);

// Set the GOOGLE_APPLICATION_CREDENTIALS environment variable to the path of the temporary file
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);

// Initialize FirestoreDb and add it as a singleton service
FirestoreDb firestoreDb = FirestoreDb.Create("db-server-b5343");
builder.Services.AddSingleton(firestoreDb);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
