using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Assignment.External
{
    public class TwitterConnector : IConnector
    {
        private readonly TwitterConfig? _settings;
        public TwitterConnector(IConfiguration config)
        {
            _settings = config.GetRequiredSection("Connectors:Twitter").Get<TwitterConfig>();
        }

        private async Task<string> GetAccessToken(string? key, string? secret)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.twitter.com/oauth2/token");

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            var auth = Base64Encode($"{key}:{secret}");
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {auth}");

            request.Content = new FormUrlEncodedContent(keyValues);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Error getting access token");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(content, new { access_token = "" });
            return result?.access_token;
        }

        private string Base64Encode(string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        // async strteam of tweets
        public async IAsyncEnumerable<WebHookMessage> GetMessag()
        {
            var _accessToken = GetAccessToken(_settings?.ApiKey, _settings?.ApiSecret).GetAwaiter().GetResult();
            var client = new HttpClient { BaseAddress = new Uri("https://api.twitter.com") };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            using var response = await client.GetAsync("/2/tweets/sample/stream", HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var tresp = JsonConvert.DeserializeObject<TwitterResponse>(reader.ReadLine());
                yield return new WebHookMessage { Id = tresp?.data.id, Message = tresp?.data.text };
                //await Task.Delay(1);
            }

        }

        private class TwitterConfig
        {
            public string ApiKey { get; set; }
            public string ApiSecret { get; set; }
        }
        private class TwitterResponse
        {
            public Tweet data { get; set; }
        }
        private class Tweet
        {
            public string? id { get; set; }
            public string? text { get; set; }
        }
    }
}
