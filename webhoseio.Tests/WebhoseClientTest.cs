namespace webhoseio.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using webhoseio;
    using Xunit;
    using Xunit.Abstractions;

    public class WebhoseClientTest
    {
        private readonly ITestOutputHelper console;

        public WebhoseClientTest(ITestOutputHelper console)
        {
            this.console = console;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        [Fact]
        public async Task SimpleTest()
        {
            var client = new WebhoseClient();
            var output = await client.QueryAsync("search", new Dictionary<string, string> { { "q", "github" } });

            console.WriteLine(output["posts"][0]["text"].ToString());
            console.WriteLine(output["posts"][0]["published"].ToString());

            output = await output.GetNextAsync();
            console.WriteLine(output["posts"][0]["thread"]["site"].ToString());
        }
    }
}
