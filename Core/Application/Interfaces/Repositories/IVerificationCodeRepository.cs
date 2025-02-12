﻿using Project.Models.Entities;
using System.Linq.Expressions;

namespace HealthChatBox.Core.Application.Interfaces.Repositories
{
    public interface IVerificationCodeRepository 
    {
        Task<int> Save();
        Task<VerificationCode> Create(VerificationCode entity);
        VerificationCode Update(VerificationCode entity);
        Task<VerificationCode> Get(int id);
        Task<VerificationCode> Get(string email);
        Task<VerificationCode> Get(Expression<Func<VerificationCode, bool>> expression);
    }
}
