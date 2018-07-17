using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JacksonShul.Data;
using JacksonShul.Models;
using JacksonShul.Properties;

namespace JacksonShul.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MessageRepository mr = new MessageRepository(Settings.Default.ConStr);
            HomePageViewModel vm = new HomePageViewModel();
            vm.Messages = mr.GetAllMessages();
            if (TempData["Notify"] != null)
            {
                vm.Notify = (string)TempData["Notify"];
            }
            return View(vm);
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(Member member,string password)
        {
            if(member.Cell == null || member.Email == null|| member.FirstName == null || member.LastName == null|| password == null)
            {
                return RedirectToAction("index");
            }
            if (member.Cell.Length > 12 || member.Email.Length > 40 || member.FirstName.Length > 40 || member.LastName.Length > 40 || password.Length > 25)
            {
                return RedirectToAction("index");
            }
            var repo = new VerifyRepository(Settings.Default.ConStr);
            repo.SignUp(member, password);
            TempData["Notify"] = "you successfuly signed up!";
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (email == null || password == null)
            {
                return RedirectToAction("index");
            }
            var repo = new VerifyRepository(Settings.Default.ConStr);
            var member = repo.Login(email, password);
            if (member == null)
            {
                return RedirectToAction("Login");
            }
            
            FormsAuthentication.SetAuthCookie(email, true);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ViewShulExpenses()
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
                TotalPledges = e.Pledges.Sum(p => p.Amount),
                Date = e.Date
            });
            return View(expensesPlus);
        }
        [Authorize]
        public ActionResult Donate(int expenseId,string expenseName)
        {
            var repo = new VerifyRepository(Settings.Default.ConStr);
            Member member = repo.GetByEmail(User.Identity.Name);
            if (member == null)
            {
                return RedirectToAction("ViewExpenses");
            }
            ViewBag.expenseName = expenseName;
            ViewBag.payment = true;
            Payment payment = new Payment
            {
                MemberId = member.Id,
                ExpenseId = expenseId,
                Date = DateTime.Now,
            };            
            return View(payment);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Donate(Payment payment)
        {
            var fr = new FinancialRepository(Settings.Default.ConStr);
            fr.AddPayment(payment);
            return RedirectToAction("ViewShulExpenses");
        }
        [Authorize]
        public ActionResult Pledge(int expenseId,string expenseName)
        {
            var repo = new VerifyRepository(Settings.Default.ConStr);
            Member member = repo.GetByEmail(User.Identity.Name);
            if (member == null)
            {
                return RedirectToAction("ViewExpenses");
            }
            ViewBag.expenseName = expenseName;
            ViewBag.payment = false;
            Payment pledge = new Payment
            {
                MemberId = member.Id,
                ExpenseId = expenseId,
                Date = DateTime.Now,
            };
            return View("Donate",pledge);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Pledge(Pledge pledge)
        {
            var fr = new FinancialRepository(Settings.Default.ConStr);
            fr.AddPledge(pledge);
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdatePledge(int amount,int id)
        {
            var fr = new FinancialRepository(Settings.Default.ConStr);
            fr.UpdatePledge(amount,id);
            Pledge pledge = fr.GetPledge(id);
            decimal a = pledge.Amount;
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeletePledge(int id)
        {
            var fr = new FinancialRepository(Settings.Default.ConStr);
            fr.DeletePledge(id);
            return Json(id);
        }
        [Authorize]
        public ActionResult GetPledge(int id)
        {
            var fr = new FinancialRepository(Settings.Default.ConStr);
            Pledge pledge = fr.GetPledge(id);
            decimal amount = pledge.Amount;
            return Json(amount,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ViewMyActivity()
        {
            var repo = new VerifyRepository(Settings.Default.ConStr);
            Member member = repo.GetByEmail(User.Identity.Name);
            if (member == null)
            {
                return RedirectToAction("ViewExpenses");
            }
            var fr = new FinancialRepository(Settings.Default.ConStr);
            List<Payment> payments = fr.GetPaymentsByMemberId(member.Id);
            IEnumerable<Pledge> pledges = fr.GetPledgesByMemberId(member.Id);
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
                Pledges = plwn
            };
            return View(paps);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}