namespace webhoseio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class WebhoseClient
    {
        private readonly WebhoseOptions options;

        public WebhoseClient(WebhoseOptions options = null)
        {
            this.options = options ?? new WebhoseOptions();
        }

        public WebhoseJsonResponseMessage Query(
            string endpoint, 
            IDictionary<string, string> parameters)
        {
            return QueryAsync(endpoint, parameters).Result;
        }

        public async Task<WebhoseJsonResponseMessage> QueryAsync(
            string endpoint, 
            IDictionary<string, string> parameters, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = new Uri(Constants.BaseUri + endpoint + ToQueryString(WithMandatoryParameters(parameters)));
            using (var client = Helpers.GetHttpClient())
            {
                var response = await client.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatusCode();
                return new WebhoseJsonResponseMessage(await response.Content.ReadAsStringAsync());
            }
        }

        private IDictionary<string, string> WithMandatoryParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            if (!parameters.ContainsKey("token"))
            {
                parameters.Add("token", options.Token);
            }
            if (!parameters.ContainsKey("format"))
            {
                parameters.Add("format", options.Format);
            }
            return parameters;
        }

        private static string ToQueryString(IDictionary<string, string> parameters)
        {
            var query = string.Join("&", parameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}")); // todo: should the key be escaped too?
            return string.IsNullOrEmpty(query) ? query : "?" + query;
        }
    }
}
