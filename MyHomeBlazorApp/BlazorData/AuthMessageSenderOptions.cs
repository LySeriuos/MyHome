namespace MyHomeBlazorApp.BlazorData
{
    //public class AuthMessageSenderOptions
    //{        public string? SendGridKey { get; set; } = Environment.GetEnvironmentVariable("EmailApiKey") ?? "Production";

    //}

    public class AuthMessageSenderOptions
    {
        public string? GmailPassword { get; set; } = Environment.GetEnvironmentVariable("GmailPassword") ?? "Production";
        public string? MyHomeGmailPassword { get; set; } = Environment.GetEnvironmentVariable("MyHomeGmailPassword") ?? "No Secret password";
        public string? GmailSecretKey { get; set; } = Environment.GetEnvironmentVariable("GmailSecretKey") ?? "No Secret key";
    }
}



