using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacksonShul.Data
{
    public class FinancialRepository
    {
        public void AddExpense(Expense expense)
        {
            using (var context = new ShulDataContext())
            {
                context.Expenses.InsertOnSubmit(expense);
                context.SubmitChanges();
            }
        }


        public void AddPayment(Payment payment)
        {
            using (var context = new ShulDataContext())
            {
                context.Payments.InsertOnSubmit(payment);
                context.SubmitChanges();
            }
        }

        public void AddPledge(Pledge pledge)
        {
            using (var context = new ShulDataContext())
            {
                context.Pledges.InsertOnSubmit(pledge);
                context.SubmitChanges();
            }
        }
        public void UpdatePledge(int amount, int id)
        {
            using (var context = new ShulDataContext())
            {
                context.ExecuteCommand("update Pledges set amount = amount-{0} where id = {1}",amount,id);
            }
        }
        public Pledge GetPledge(int id)
        {
            using (var context = new ShulDataContext())
            {
                return context.Pledges.FirstOrDefault(p => p.Id == id);
            }
        }
        public void DeletePledge(int id)
        {
            using (var context = new ShulDataContext())
            {
                context.ExecuteCommand("Delete from pledges where id = {0}",id);
            }
        }
        public IEnumerable<Payment> GetPayments()
        {
            using (var context = new ShulDataContext())
            {
                return context.Payments.ToList();
            }
        }

        public List<Expense> GetExpensesWithPayments()
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Expense>(e => e.Payments);
                context.LoadOptions = loadOptions;
                return context.Expenses.ToList();
            }
        }

        public List<Payment> GetPaymentsByMemberId(int memberId)
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Payment>(p => p.Expense);
                context.LoadOptions = loadOptions;
                return context.Payments.Where(p => p.MemberId == memberId).ToList();
            }
        }

        public IEnumerable<Pledge> GetPledgesByMemberId(int memberId)
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Pledge>(p => p.Expense);
                context.LoadOptions = loadOptions;
                return context.Pledges.Where(p => p.MemberId == memberId).ToList();
            }
        }
    }
}
