using MovieRecommender.DTO.Models;

namespace MovieRecommenderService.Contracts
{
    public interface IMailService
    {
        string Host { get; set; }
        int Port { get; set; }
        bool UseSsl { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        
        void Send(MailDto data);
    }
}