using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacksonShul.Data
{
    public class MessageRepository
    {
        public void AddMessage(Message message)
        {
            using (var context = new ShulDataContext())
            {
                context.Messages.InsertOnSubmit(message);
                context.SubmitChanges();                
            }
        }
        public void DeleteMessage(int id)
        {
            using (var context = new ShulDataContext())
            {
                context.ExecuteCommand("Delete from messages where id = {0}", id);
            }
        }
        public IEnumerable<Message> GetAllMessages()
        {
            using (var context = new ShulDataContext())
            {
                return context.Messages.ToList();
            }
        }
    }
}
