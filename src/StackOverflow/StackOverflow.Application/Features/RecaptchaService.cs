using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Properties;

namespace StackOverflow.Application.Features;

public class RecaptchaService : IRecaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly string _recaptchaSecretKey;

    public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaSettings> recaptchaSecretKey)
    {
        _httpClient = httpClient;
        _recaptchaSecretKey = recaptchaSecretKey.Value.SecretKey;
    }


    public async Task<bool> IsCaptchaValidAsync(string recaptchaResponse)
    {
        var response = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecretKey}&response={recaptchaResponse}", null);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var captchaValidation = JsonConvert.DeserializeObject<CaptchaValidation>(jsonResponse);
        return captchaValidation!.Success;
    }
}

public class CaptchaValidation
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}