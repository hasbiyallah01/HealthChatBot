﻿using Microsoft.Extensions.Options;
using MimeKit;
using Project.Models.Entities;
using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models;

using MailKit.Net.Smtp;
using HealthChatBox.Core.Domain.Entities;
using System.Text;

namespace HealthChatBox.Core.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _hostenv;
        private readonly EmailConfiguration _emailConfiguration;
        public EmailService(IWebHostEnvironment hostenv, IOptions<EmailConfiguration> emailConfiguration)
        {
            _hostenv = hostenv;
            _emailConfiguration = emailConfiguration.Value;
        }



        public async Task<BaseResponse> SendNotificationToUserAsync(Profile profile)
        {
            var mailRecieverRequestDto = new MailRecieverDto
            {
                Email = profile.User.Email,
                Name = profile.User.FirstName + " " + profile.User.LastName,
            };

            string emailBody = $"<p>We are excited to announce that you've been granted access to Konsume, the cutting-edge AI-driven" +
                $" platform revolutionizing personalized nutrition and diet management!</p>\r\n<p>With Konsume, you have the power to harness AI technology to tailor your diet and nutritional" +
                $" intake according to your unique goals and health conditions. Whether you're aiming to shed a few pounds, bulk up, or manage specific health concerns like allergies or diabetes," +
                $" Konsume is your ultimate companion on your wellness journey.</p>\r\n<p>Say goodbye to the tedious process of scouring the internet for meal ideas and nutritional information." +
                $" Konsume's intuitive interface recommends customized meals and snacks, saving you time and effort while ensuring that every bite aligns with your dietary objectives.</p>\r\n" +
                $"<p>But that's not all—Konsume goes beyond mere recommendations. With our innovative image recognition feature, simply snap a photo of any meal or snack, and let the AI analyze its" +
                $" nutritional content. Discover how each food item impacts your meal plan, goals, and overall health, empowering you to make informed choices every step of the way.</p>\r\n" +
                $"<p>Welcome to Konsume, where personalized nutrition meets cutting-edge technology. We're thrilled to have you on board and can't wait to see the incredible progress you'll " +
                $"achieve with our platform!</p>\r\n<p>Best regards,<br/>\r\n[ADMIN/KONSUME]</p> <img src=\"https://drive.google.com/uc?export=view&id=1KH6x4h7J0PCGE7yDiJj4YUBDYPHPBd3B\" " +
                $"alt=\"Konsume Logo\" style=\"display: block; margin: 0 auto;\">\r\n";

            var mailRequest = new MailRequests
            {
                Body = emailBody,
                Title = "Health Tracker",
                HtmlContent = emailBody
            };

            await SendEmailAsync(mailRecieverRequestDto, mailRequest);

            return new BaseResponse
            {
                Message = "Notification sent successfully",
                IsSuccessful = true,
            };
        }

        public Task SendEmailClient(string msg, string title, string email)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Error: Email message content is null or empty.");
                throw new ArgumentNullException(nameof(msg), "Email message content cannot be null or empty");
            }

            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(new MailboxAddress(_emailConfiguration.EmailSenderName, _emailConfiguration.EmailSenderAddress));
            message.Subject = title;

            message.Body = new TextPart("html")
            {
                Text = msg
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine("Inside email client");
                    client.Connect(_emailConfiguration.SMTPServerAddress, _emailConfiguration.SMTPServerPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.EmailSenderAddress, _emailConfiguration.EmailSenderPassword);
                    var xxx = client.Send(message);
                    var ppp = xxx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred in email client: {ex.Message}", DateTime.UtcNow.ToLongTimeString());
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
            return Task.CompletedTask;
        }


        public async Task<bool> SendEmailAsync(MailRecieverDto model, MailRequests request)
        {
            try
            {
                Console.WriteLine("Calling email client");
                string buildContent = $"Dear {model.Name}," +
                                            $"<p>{request.Body}</p>";

                if (string.IsNullOrWhiteSpace(request.HtmlContent))
                {
                    throw new ArgumentNullException(nameof(request.HtmlContent), "Email content cannot be null or empty");
                }

                await SendEmailClient(request.HtmlContent, request.Title, model.Email);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("There was an error while sending email");
            }
        }

    }

}

