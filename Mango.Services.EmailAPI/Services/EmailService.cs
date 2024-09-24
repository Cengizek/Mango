﻿using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total" + cartDto.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.AppendLine("<li>");
                message.AppendLine(item.Product.Name + "x"+ item.Count);
                message.AppendLine("</li>");
            }
            message.AppendLine("/<ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
        {
            string message = "New Order Placed.<br/> Order ID : " + rewardsMessage.OrderId;
            await LogAndEmail(message, "cengizekmek@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
           string message ="User Registeration Successfully. : "+ email;
            await LogAndEmail(message, "cengizekmek@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
               
                
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}