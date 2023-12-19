using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebSocketClient
{
	internal class HttpClientHelper
	{
		private static HttpClient _client = new HttpClient();

		private static async Task<T> ExecuteAsJsonAsync<T>(HttpMethod httpMethod, object model, string apiMethod, string apiUrl, bool isBinaryContent, Dictionary<string, string> headers = null, byte[] data = null) where T : class, new()
		{
			try
			{
				Uri baseUri = new Uri(apiUrl);
				Uri uri = new Uri(baseUri, apiMethod);

				var request = new HttpRequestMessage(httpMethod, uri);
				if (model != null)
				{
					request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));
					request.Content.Headers.Clear();
					request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				}

				request.Headers.Clear();
				_client.DefaultRequestHeaders.Clear();

				if (headers != null)
				{
					foreach (var key in headers.Keys)
					{
						if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(headers[key]))
						{
							if (key == "Authorization") 
							{
								_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", headers[key]);
							}
							else if (key == "Bearer")
							{
								_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headers[key]);
							}
							else
							{
								request.Headers.Add(key, headers[key]);
							}
						}
					}
				}				

				var response = await _client.SendAsync(request);
				if (!response.IsSuccessStatusCode)
				{
					T obj = new T();
					return obj;
				}

				var stream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception e)
			{
				T obj = new T();
				return obj;
			}
		}

		public static async Task<T> GetAsync<T>(string apiMethod, string apiUrl, Dictionary<string, string> headers = null) where T : class, new()
		{
			return await ExecuteAsJsonAsync<T>(HttpMethod.Get, null, apiMethod, apiUrl, false, headers);
		}

        public static async Task<T> PostAsJsonAsync<T>(object model, string apiMethod, string apiUrl, Dictionary<string, string> headers = null) where T : class, new()
        {
            return await ExecuteAsJsonAsync<T>(HttpMethod.Post, model, apiMethod, apiUrl, false, headers);
        }
    }
}
