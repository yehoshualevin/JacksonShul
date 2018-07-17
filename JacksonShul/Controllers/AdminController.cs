using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Data;
using JacksonShul.Models;
using JacksonShul.Properties;

namespace JacksonShul.Controllers
{
    [AuthorizeUserAccessLevel (UserRole = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult AddExpense()
        {
            HomePageViewModel vm = new HomePageViewModel();
            if (TempData["Notify"] != null)
            {
                vm.Notify = (string)TempData["Notify"];
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            if (expense.Name == null)
            {
                return RedirectToAction("addexpense");
            }
            var repo = new FinancialRepository(Settings.Default.ConStr);
            repo.AddExpense(expense);
            TempData["Notify"] = $"{expense.Name} added to expenses";
            return RedirectToAction("addexpense");
        }
        public ActionResult ViewMembers()
        {
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            List<Member> members = fr.GetMembers();           
            return View(members);
        }
        public ActionResult ViewExpenses()
        {
            var repo = new FinancialRepository(Settings.Default.ConStr);
            var fr = new FinancialRepository(Settings.Default.ConStr);
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
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            IEnumerable<Payment>payments = fr.GetAllPayments();
            return View();
        }
        public ActionResult GetById(int id)
        {
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
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
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
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
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            IEnumerable<Pledge> payments = fr.GetPledgesByExpenseId(expenseId);
            IEnumerable<PledgeWithName> pwn = payments.Select(p => new PledgeWithName
            {
                Amount = p.Amount,
                Date = p.Date,
                Name = p.Member.FirstName + " " + p.Member.LastName
            });
            return View(pwn);
        }

        public ActionResult AddMessage()
        {
            HomePageViewModel vm = new HomePageViewModel();
            if (TempData["Notify"] != null)
            {
                vm.Notify = (string)TempData["Notify"];
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddMessage(Message message)
        {
            if(message.Story == null)
            {
                return RedirectToAction("AddMessage");
            }
            var mr = new MessageRepository(Settings.Default.ConStr);
            mr.AddMessage(message);
            TempData["Notify"] = "message added!";
            return RedirectToAction("AddMessage");
        }
        public ActionResult DeleteMessage()
        {
            var mr = new MessageRepository(Settings.Default.ConStr);
            return View(mr.GetAllMessages());
        }
        public ActionResult Delete(int id)
        {
            var mr = new MessageRepository(Settings.Default.ConStr);
            mr.DeleteMessage(id);
            return RedirectToAction("DeleteMessage");
        }
    }
}