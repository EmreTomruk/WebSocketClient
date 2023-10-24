using Microsoft.AspNetCore.SignalR.Client;
using WebSocketClient;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/digitalsignaturehub")
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
    var xx = Directory.GetCurrentDirectory();

    connection.On<string>("SetConnection", connectionId =>
    {
        connection.InvokeAsync("SaveClient", connectionId, "test");
        Console.WriteLine($"Connected: {DateTime.Now}");
    });

    connection.On<TabletDocumentModel>("ReceiveDocument", async documents =>
    {
        var documentRequest = documents.DigitalSignatureDocuments.FirstOrDefault();

        if (documentRequest == null) return;

        var apiRequestHeader = SetPortalGatewayApi(documentRequest.DocumentLanguageId.ToString());

		var apiResponse = await HttpClientHelper
			  .GetAsync<ApiResponse<ApplicationEntryFormApiResponse>>
			  ("/api/Appointment/GetApplicationEntryForm/" + documents.ApplicationId + "/" + documentRequest.DocumentLanguageId, "http://localhost:5000", apiRequestHeader)
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