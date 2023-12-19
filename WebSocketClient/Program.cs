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
        connection.InvokeAsync("SaveClient", connectionId, "test20", "1.0.0");

        Console.WriteLine($"Connected: {DateTime.Now}");
    });

    connection.On<string>("GetDeviceName", tabletName =>
    {
        Console.WriteLine($"Tablet-name: {tabletName}");
    });

    connection.On<TabletDocumentModel>("ReceiveDocument", async documents =>
    {
        var documentRequest = documents.DigitalSignatureDocuments.FirstOrDefault();

        if (documentRequest == null) return;

        var apiRequestHeader = SetRequestHeader(documentRequest.DocumentLanguageId.ToString());

        var apiRequest = new DocumentRequestModel
        { 
            Signatures = documents.MinorApplicantParents.Select(p => new DocumentSignatureRequestModel
            {
                Name = p.Name
            }).ToList() 
        };

		var apiResponse = await HttpClientHelper
			  .PostAsJsonAsync<ApiResponse<ApplicationEntryFormApiResponse>>
			  (apiRequest, "/api/DigitalSignature/CargoReceiptForm/" + documents.ApplicationId + "/" + documentRequest.DocumentLanguageId, "https://localhost:7071", apiRequestHeader)
			  .ConfigureAwait(false);

		Console.WriteLine($"document received: {DateTime.Now}, documentId:{documentRequest.DigitalSignatureDocumentId}");

        var saveDocumentApiRequest = new SaveDocumentsRequestModel
        {
            ApplicationId = documents.ApplicationId,
            CreatedBy = documents.CreatedBy,
            DigitalSignatureDocuments = new List<DigitalSignatureDocumentRequestModel>
            {
                new DigitalSignatureDocumentRequestModel
                {
                    DigitalSignatureDocumentId = documentRequest.DigitalSignatureDocumentId,
                    Html = apiResponse.Data.Html,                    
                    Signatures = apiRequest.Signatures.Select(p => new DocumentSignatureRequestModel
                    {
                        Name = p.Name,
                        Signature = File.ReadAllText(@"C:\\Users\\Lenovo\\source\\repos\\WebSocketClient\\WebSocketClient\\signature.txt")
                    }).ToList()
                }              
            }
        };

        var saveProcess = await HttpClientHelper
              .PostAsJsonAsync<ApiResponse<SaveDocumentsApiResponse>>
              (saveDocumentApiRequest, "/api/DigitalSignature/SaveDocuments", "https://localhost:7071", apiRequestHeader)
              .ConfigureAwait(false);

        Console.WriteLine($"document saved: {DateTime.Now}, isSuccess: {saveProcess.IsSuccess}");
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

Dictionary<string, string> SetRequestHeader(string languageId)
{
	return new Dictionary<string, string>
	{
		{ "languageId", languageId },
		{ "corporateId", "TEST" }
	};
}