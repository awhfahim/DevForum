namespace StackOverflow.Application.Contracts.Features;

public interface IRecaptchaService
{
    Task<bool> IsCaptchaValidAsync(string recaptchaResponse);
}