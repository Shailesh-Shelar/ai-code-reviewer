using OpenAI.Chat;

namespace CodeReviewAPI.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;

        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new InvalidOperationException("OpenAI API key not configured");
        }

        public async Task<string> ReviewCodeAsync(string code, string language)
        {
            var client = new ChatClient("gpt-4o-mini", _apiKey);

            var prompt = $@"You are a senior code reviewer. Analyze this {language} code and provide:

1. Potential bugs or errors
2. Code quality issues
3. Best practices violations
4. Security concerns
5. Suggested improvements

Be specific and constructive. Format your response clearly.

Code to review:
````{language}
{code}
```";

            var response = await client.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }
    }
}