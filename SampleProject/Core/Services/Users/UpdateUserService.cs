using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, int age, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);
            user.SetMonthlySalary(annualSalary.HasValue ? Math.Round(annualSalary.Value / 12, 2) : (decimal?)null); // Test comments: since the AnnualSalary is an optional property and allows null, MonthlySalary should also accept null
            user.SetAge(age);
            user.SetTags(tags);
        }
    }
}