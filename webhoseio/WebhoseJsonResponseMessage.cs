namespace webhoseio
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public class WebhoseJsonResponseMessage
    {
        public JObject Json { get; }

        internal WebhoseJsonResponseMessage(string content)
        {
            Json = JObject.Parse(content);
        }

        public WebhoseJsonResponseMessage GetNext()
        {
            return GetNextAsync().Result;
        }

        public async Task<WebhoseJsonResponseMessage> GetNextAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = Json["next"].Value<string>();
            using (var client = Helpers.GetHttpClient())
            {
                var response = await client.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatusCode();
                return new WebhoseJsonResponseMessage(await response.Content.ReadAsStringAsync());
            }
        }

        public JToken this[string key] => Json[key];
    }
}