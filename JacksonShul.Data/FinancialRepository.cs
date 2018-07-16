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

        public List<Expense> GetExpensesWithPaps()
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Expense>(e => e.Payments);
                loadOptions.LoadWith<Expense>(e => e.Pledges);
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
        public List<Member> GetMembers()
        {
            using (var context = new ShulDataContext())
            {
                return context.Members.ToList();
            }
        }
        public Member GetMember(int id)
        {
            using (var context = new ShulDataContext())
            {
                return context.Members.FirstOrDefault(m => m.Id == id);
            }
        }
        public List<Payment> GetAllPayments()
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Payment>(p => p.Expense);
                context.LoadOptions = loadOptions;
                return context.Payments.ToList();
            }
        }
        public List<Payment> GetPaymentsByExpenseId(int id)
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Payment>(p => p.Member);
                context.LoadOptions = loadOptions;
                return context.Payments.Where(p => p.ExpenseId == id).ToList();
            }
        }
        public List<Pledge> GetPledgesByExpenseId(int id)
        {
            using (var context = new ShulDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Pledge>(p => p.Member);
                context.LoadOptions = loadOptions;
                return context.Pledges.Where(p => p.ExpenseId == id).ToList();
            }
        }

    }
}
