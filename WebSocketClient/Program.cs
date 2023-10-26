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
        connection.InvokeAsync("SaveClient", connectionId, "Test");
        Console.WriteLine($"Connected: {DateTime.Now}");
    });

    connection.On<TabletDocumentModel>("ReceiveDocument", async documents =>
    {
        var documentRequest = documents.DigitalSignatureDocuments.FirstOrDefault();

        if (documentRequest == null) return;

        var apiRequestHeader = SetPortalGatewayApi(documentRequest.DocumentLanguageId.ToString());

		var apiResponse = await HttpClientHelper
			  .GetAsync<ApiResponse<ApplicationEntryFormApiResponse>>
			  ("/api/DigitalSignature/GetApplicationEntryForm/" + documents.ApplicationId + "/" + documentRequest.DocumentLanguageId, "https://localhost:7071", apiRequestHeader)
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

Dictionary<string, string>? SetPortalGatewayApi(string languageId)
{
	return new Dictionary<string, string>
	{
		{ "apiKey", "Gateway.ApiKey.2021" },
		{ "languageId", languageId },
		{ "corporateId", "TEST" },
		{ "UserId", "853" }
	};
}