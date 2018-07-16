using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Data;
using JacksonShul.Models;

namespace JacksonShul.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            HomePageViewModel vm = new HomePageViewModel();
            if (TempData["Message"] != null)
            {
                vm.Message = (string)TempData["Message"];
            }
            return View(vm);
        }
        public ActionResult AddExpense()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            var repo = new FinancialRepository();
            repo.AddExpense(expense);
            TempData["Message"] = $"{expense.Name} added to expenses";
            return RedirectToAction("index");
        }
        public ActionResult ViewMembers()
        {
            FinancialRepository fr = new FinancialRepository();
            List<Member> members = fr.GetMembers();           
            return View(members);
        }
        public ActionResult ViewExpenses()
        {
            var repo = new FinancialRepository();
            var fr = new FinancialRepository();
            List<Expense> expenses = fr.GetExpensesWithPaps();
            IEnumerable<ExpensePlus> expensesPlus = expenses.Select(e => new ExpensePlus
            {
                Id = e.Id,
                Name = e.Name,
                Cost = e.Cost,
                TotalDonations = e.Payments.Sum(p => p.Amount),
                Date = e.Date
            });
            return View(expensesPlus);
        }
        public ActionResult ViewAllPayments()
        {
            FinancialRepository fr = new FinancialRepository();
            IEnumerable<Payment>payments = fr.GetAllPayments();
            return View();
        }
        public ActionResult GetById(int id)
        {
            FinancialRepository fr = new FinancialRepository();
            List<Payment> payments = fr.GetPaymentsByMemberId(id);
            IEnumerable<Pledge> pledges = fr.GetPledgesByMemberId(id);
            Member m = fr.GetMember(id);
            IEnumerable<PaymentWithName> pwn = payments.Select(p => new PaymentWithName
            {
                Amount = p.Amount,
                Date = p.Date,
                Name = p.Expense.Name
            });
            IEnumerable<PledgeWithName> plwn = pledges.Select(p => new PledgeWithName
            {
                Id = p.Id,
                Amount = p.Amount,
                Date = p.Date,
                Name = p.Expense.Name
            });
            PaymentsAndPledges paps = new PaymentsAndPledges
            {
                Payments = pwn,
                Pledges = plwn,
                Member = m
            };
            return View(paps);
        }
        public ActionResult ViewDonations(int expenseId,string expenseName)
        {
            ViewBag.expensename = expenseName;
            FinancialRepository fr = new FinancialRepository();
            IEnumerable<Payment> payments = fr.GetPaymentsByExpenseId(expenseId);
            IEnumerable<PaymentWithName> pwn = payments.Select(p => new PaymentWithName
            {
                Amount = p.Amount,
                Date = p.Date,
                Name = p.Member.FirstName + " " + p.Member.LastName
            });
            return View(pwn);
        }
        public ActionResult ViewPledges(int expenseId, string expenseName)
        {
            ViewBag.expensename = expenseName;
            FinancialRepository fr = new FinancialRepository();
            IEnumerable<Pledge> payments = fr.GetPledgesByExpenseId(expenseId);
            IEnumerable<PledgeWithName> pwn = payments.Select(p => new PledgeWithName
            {
                Amount = p.Amount,
                Date = p.Date,
                Name = p.Member.FirstName + " " + p.Member.LastName
            });
            return View(pwn);
        }
    }
}