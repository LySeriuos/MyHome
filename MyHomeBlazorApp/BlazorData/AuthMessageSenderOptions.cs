namespace MyHomeBlazorApp.BlazorData
{
    public class AuthMessageSenderOptions
    {        public string? SendGridKey { get; set; } = Environment.GetEnvironmentVariable("EmailApiKey") ?? "Production";

    }
}


