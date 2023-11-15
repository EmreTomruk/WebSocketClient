using Microsoft.AspNetCore.SignalR.Client;
using WebSocketClient;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7071/digitalsignaturehub")
    .WithAutomaticReconnect()
    .Build();

connection.Reconnecting += sender =>
{
    ReConnecting();
    return Task.CompletedTask;
};

OpenConnection();
Console.ReadLine();

void OpenConnection()
{
    connection.On<string>("SetConnection", connectionId =>
    {
        connection.InvokeAsync("SaveClient", connectionId, "hostname");

        Console.WriteLine($"Connected: {DateTime.Now}");
    });

    connection.On<TabletDocumentModel>("ReceiveDocument", async documents =>
    {
        var documentRequest = documents.DigitalSignatureDocuments.FirstOrDefault();

        if (documentRequest == null) return;

        var apiRequestHeader = SetRequestHeader(documentRequest.DocumentLanguageId.ToString());

		var apiResponse = await HttpClientHelper
			  .GetAsync<ApiResponse<ApplicationEntryFormApiResponse>>
			  ("/api/DigitalSignature/ExplicitConsentText/" + documents.ApplicationId + "/" + documentRequest.DocumentLanguageId, "https://localhost:7071", apiRequestHeader)
			  .ConfigureAwait(false);

		Console.WriteLine($"Connected: {DateTime.Now}");
    });

	try
    {
        connection.StartAsync();
    }
    catch (Exception ex)
    {
        //
    }
}

void ReConnecting()
{
    Console.WriteLine($"Reconnecting: {DateTime.Now}");
}

Dictionary<string, string>? SetRequestHeader(string languageId)
{
	return new Dictionary<string, string>
	{
		{ "languageId", languageId },
		{ "corporateId", "TEST" }
	};
}