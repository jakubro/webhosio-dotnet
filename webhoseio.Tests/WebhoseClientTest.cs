namespace webhoseio.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
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

            console.WriteLine(output["posts"].Count().ToString());
            console.WriteLine(output["posts"][0]["language"].Count().ToString());

            output = await output.GetNextAsync();
            console.WriteLine(output["posts"][0]["thread"]["site"].ToString());
        }
    }
}
