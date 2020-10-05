using System;
using System.Net;
using System.Net.Mail;
using MovieRecommender.DTO.Models;
using MovieRecommenderService.Contracts;

namespace MovieRecommenderService.Services
{
    public class MailService : IMailService
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public void Send(MailDto data)
        {
            if(string.IsNullOrEmpty(Host)) throw new ArgumentNullException(Host);
            if(string.IsNullOrEmpty(UserName)) throw new ArgumentNullException(UserName);
            if(string.IsNullOrEmpty(Password)) throw new ArgumentNullException(Password);
            
            using var client = new SmtpClient();
            using var message = new MailMessage();

            client.Host = Host;
            client.Port = Port;
            client.Credentials = new NetworkCredential(UserName, Password);
            client.EnableSsl = UseSsl;

            message.From = new MailAddress(data.From);
            message.Subject = data.Subject;
            message.Body = data.Content;
            
            foreach (var to in data.To)
            {
                message.To.Add(new MailAddress(to));
            }
            
            client.Send(message);
        }
    }
}