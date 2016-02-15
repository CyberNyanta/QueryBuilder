using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.DAL.Contexts;
using QueryBuilder.DAL.DTO;
namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new QueryBuilderContext())
            {
                for (int i = 0; i < 10; i++)
                {
                    var user = new User { Delflag = i, Email = $"somemail{i}@test.com", FirstName = $"FirstName{i}", LastName = $"LastName{i}", PasswordHash = new Guid($"ca761232ed4211cebacd00aa0057b223") };
                    
                    if (context.Users.FirstOrDefault(x => x.Email.Equals(user.Email)) == null)
                    {
                        context.Users.Add(user);
                    }
                }
                context.SaveChanges();
                foreach (var user in context.Users) 
                {
                    Console.WriteLine(user.FirstName);
                }
            }
            
        }
    }
}
