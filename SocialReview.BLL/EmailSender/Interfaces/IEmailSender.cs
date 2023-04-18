namespace SocialReview.BLL.EmailSender.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
