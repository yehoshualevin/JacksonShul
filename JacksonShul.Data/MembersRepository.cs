using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacksonShul.Data
{
    public class MembersRepository
    {
        public void SignUp(Member member, string password)
        {
            string salt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.HashPassword(password, salt);
            member.PasswordSalt = salt;
            member.PasswordHash = passwordHash;
            using (var context = new ShulDataContext()) 
            {
                context.Members.InsertOnSubmit(member);
                context.SubmitChanges();
            }
        }
        public Member Login(string email,string password)
        {
            Member member = GetByEmail(email);
            if(member == null)
            {
                return null;
            }
            bool isCorrectPassword = PasswordHelper.PasswordMatch(password, member.PasswordSalt, member.PasswordHash);
            if (!isCorrectPassword)
            {
                return null;
            }
            return member;
        }
        public Member GetByEmail(string email)
        {
            using (var context = new ShulDataContext())
            {
                return context.Members.FirstOrDefault(m => m.Email == email);
            }
        }
    }
}
