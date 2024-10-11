namespace EmailSenderApp.Auth.Models
{
    public class OAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
    }
}
