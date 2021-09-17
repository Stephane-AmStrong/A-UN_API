
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMailRepository
    {
        Task SendEmailAsync(EmailData emailData);
    }
}
