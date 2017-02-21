namespace webhoseio
{
    using System.Configuration;

    public class WebhoseOptions
    {
        public string Token { get; } = ConfigurationManager.AppSettings["webhoseio:token"];
        public string Format => "json"; // todo: support for either JSON, XML, RSS or Excel (the default is json)
    }
}