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
        private readonly string _conStr;

        public FinancialRepository(string conStr)
        {
            _conStr = conStr;
        }
        public void AddExpense(Expense expense)
        {

            using (var context = new ShulDataContext(_conStr))
            {
                context.Expenses.InsertOnSubmit(expense);
                context.SubmitChanges();
            }
        }


        public void AddPayment(Payment payment)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.Payments.InsertOnSubmit(payment);
                context.SubmitChanges();
            }
        }

        public void AddMonthlyPayment(MonthlyPayment mp)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.MonthlyPayments.InsertOnSubmit(mp);
                context.SubmitChanges();
            }
        }

        public void AddPledge(Pledge pledge)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.Pledges.InsertOnSubmit(pledge);
                context.SubmitChanges();
            }
        }
        public void UpdatePledge(int amount, int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.ExecuteCommand("update Pledges set amount = amount-{0} where id = {1}",amount,id);
            }
        }
        public Pledge GetPledge(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                return context.Pledges.FirstOrDefault(p => p.Id == id);
            }
        }
        public void DeletePledge(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.ExecuteCommand("Delete from pledges where id = {0}",id);
            }
        }
        public IEnumerable<MonthlyPayment> GetMonthlyPayments()
        {
            using (var context = new ShulDataContext(_conStr))
            {
                return context.MonthlyPayments.Where(p => p.Count > 0).ToList();
            }
        }
        public void UpdateMonthlyPayment(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                context.ExecuteCommand("update monthlypayments set count = count-1 where id = {0}", id);
            }
        }
        public List<Expense> GetExpensesWithPaps()
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Expense>(e => e.Payments);
                loadOptions.LoadWith<Expense>(e => e.Pledges);
                context.LoadOptions = loadOptions;
                return context.Expenses.ToList();
            }
        }

        public IEnumerable<Payment> GetPaymentsByMemberId(int memberId)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Payment>(p => p.Expense);
                context.LoadOptions = loadOptions;
                return context.Payments.Where(p => p.MemberId == memberId).ToList();                 
            }
        }

        public IEnumerable<Pledge> GetPledgesByMemberId(int memberId)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Pledge>(p => p.Expense);
                context.LoadOptions = loadOptions;
                return context.Pledges.Where(p => p.MemberId == memberId).ToList();
            }
        }
        public IEnumerable<MonthlyPayment> GetMonthlyPaymentsByMemberId(int memberId)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<MonthlyPayment>(mp => mp.Expense);
                context.LoadOptions = loadOptions;
                return context.MonthlyPayments.Where(mp => mp.MemberId == memberId).ToList();
            }
        }
        public List<Member> GetMembers()
        {
            using (var context = new ShulDataContext(_conStr))
            {
                return context.Members.ToList();
            }
        }
        public Member GetMember(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                return context.Members.FirstOrDefault(m => m.Id == id);
            }
        }
        
        public IEnumerable<Payment> GetPaymentsByExpenseId(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Payment>(p => p.Member);
                context.LoadOptions = loadOptions;
                return context.Payments.Where(p => p.ExpenseId == id).ToList();
            }
        }
        public IEnumerable<MonthlyPayment> GetMonthlyPaymentsByExpenseId(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<MonthlyPayment>(mp => mp.Member);
                context.LoadOptions = loadOptions;
                return context.MonthlyPayments.Where(mp => mp.ExpenseId == id).ToList();
            }
        }
        public IEnumerable<Pledge> GetPledgesByExpenseId(int id)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Pledge>(p => p.Member);
                context.LoadOptions = loadOptions;
                return context.Pledges.Where(p => p.ExpenseId == id).ToList();
            }
        }
        public Expense GetExpenseByExpenseId(int expenseId)
        {
            using (var context = new ShulDataContext(_conStr))
            {
                return context.Expenses.FirstOrDefault(e => e.Id == expenseId);
            }
        }
    }
}
