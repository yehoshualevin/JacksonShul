using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Data;
using JacksonShul.Models;

namespace JacksonShul.Controllers
{
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
            return View();
        }
    }
}