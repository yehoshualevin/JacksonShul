using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JacksonShul.Data;
using JacksonShul.Models;
using System.Dynamic;

namespace JacksonShul.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(Member member,string password)
        {
            var repo = new VerifyRepository();
            repo.SignUp(member, password);
            TempData["Message"] = "You successfuly signed up!";
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            AuthorizeAttribute authorize = new AuthorizeAttribute();
            authorize.Roles = "admin";
            var repo = new VerifyRepository();
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
            var repo = new FinancialRepository();            
            var fr = new FinancialRepository();
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
            var repo = new VerifyRepository();
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
            var fr = new FinancialRepository();
            fr.AddPayment(payment);
            return RedirectToAction("ViewShulExpenses");
        }
        [Authorize]
        public ActionResult Pledge(int expenseId,string expenseName)
        {
            var repo = new VerifyRepository();
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
            var fr = new FinancialRepository();
            fr.AddPledge(pledge);
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdatePledge(int amount,int id)
        {
            var fr = new FinancialRepository();
            fr.UpdatePledge(amount,id);
            Pledge pledge = fr.GetPledge(id);
            decimal a = pledge.Amount;
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeletePledge(int id)
        {
            var fr = new FinancialRepository();
            fr.DeletePledge(id);
            return Json(id);
        }
        [Authorize]
        public ActionResult GetPledge(int id)
        {
            var fr = new FinancialRepository();
            Pledge pledge = fr.GetPledge(id);
            decimal amount = pledge.Amount;
            return Json(amount,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ViewMyActivity()
        {
            var repo = new VerifyRepository();
            Member member = repo.GetByEmail(User.Identity.Name);
            if (member == null)
            {
                return RedirectToAction("ViewExpenses");
            }
            var fr = new FinancialRepository();
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
    }
}