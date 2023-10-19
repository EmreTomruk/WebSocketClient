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
    connection.On<string>("SetConnection", connectionId =>
    {
        connection.InvokeAsync("SaveClient", connectionId, "test");
        Console.WriteLine($"Connected: {DateTime.Now}");
    });

    connection.On<TabletDocumentModel>("ReceiveDocument", model =>
    {
	    //connection.InvokeAsync("SaveClient", connectionId, "test");
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