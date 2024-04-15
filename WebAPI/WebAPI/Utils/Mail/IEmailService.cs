namespace WebAPI.Utils.Mail
{
    public interface IEmailService
    {
        //método assíncrono para envio de e-mail
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
