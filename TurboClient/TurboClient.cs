using System.Net.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TurboClient.Models;

namespace TurboClient
{
	public class TurboClient : ITurboClient
	{
		private HttpClient client;

		public TurboClient(string authSid = null)
		{
			client = new HttpClient
			{
				BaseAddress = new Uri("https://loclhost:44365")
			};

			if (authSid != null)
			{
				client.DefaultRequestHeaders.Add("auth.sid", authSid);
			}
		}

		public async Task<string> Login(User user)
		{
			var body = JsonConvert.SerializeObject(user);
			var request = new HttpRequestMessage(HttpMethod.Post, new Uri("auth/login"))
			{
				Content = new ByteArrayContent(Encoding.UTF8.GetBytes(body))
			};
			var result = await client.SendAsync(request).ConfigureAwait(false);

			result.EnsureSuccessStatusCode();
			return JsonConvert.DeserializeObject<string>(result.Content.ToString()); 
		}
	}
}
