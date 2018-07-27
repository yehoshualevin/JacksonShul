using JacksonShul.Data;
using System;
using System.Collections.Generic;
using JacksonShul.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Properties;
using System.Net.Mail;
using System.Net;

namespace JacksonShul.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        [HttpPost]
        public ActionResult Index(int amount, string name, string street, string zipcode, string exp, string xCardNum, string xCVV, int memberId, int expenseId, int? count, string account, string routing, bool credit)
        {
            if (amount == 0 || string.IsNullOrEmpty(name) || memberId == 0 || expenseId == 0 || amount > 1000000 || name.Length > 100)
            {
                return RedirectToAction("Index","Home");
            }

            if (credit)
            {
                if (string.IsNullOrEmpty(street) || string.IsNullOrEmpty(zipcode) || string.IsNullOrEmpty(exp) || string.IsNullOrEmpty(xCardNum) || string.IsNullOrEmpty(xCVV) || street.Length > 250 || zipcode.Length > 9 || exp.Length > 4)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(routing))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            string command = credit ? "cc:Sale" : "check:Sale";
            System.Collections.Specialized.NameValueCollection MyPost = new System.Collections.Specialized.NameValueCollection();
            MyPost.Add("xKey", "YehoshuaLevinDev_Test_12301258f0bf44f2a6fcf0e");
            MyPost.Add("xVersion", "4.5.5");
            MyPost.Add("xSoftwareName", "ylevin");
            MyPost.Add("xSoftwareVersion", " 1.4.2");
            MyPost.Add("xCommand", command);
            MyPost.Add("xName", name);
            MyPost.Add("xAmount", amount.ToString());
            if (credit)
            {
                MyPost.Add("xStreet", street);
                MyPost.Add("xZip", zipcode);
                MyPost.Add("xExp", exp);
                MyPost.Add("xCardNum", xCardNum);
                MyPost.Add("xCVV", xCVV);
            }
            else
            {
                MyPost.Add("xAccount", account);
                MyPost.Add("xRouting", routing);
            }
            

            System.Net.WebClient MyClient = new System.Net.WebClient();
            string MyResponse = System.Text.Encoding.ASCII.GetString(MyClient.UploadValues("https://x1.cardknox.com/gateway", MyPost));
            // Response
            System.Collections.Specialized.NameValueCollection MyResponseData = HttpUtility.ParseQueryString(MyResponse);
            string MyStatus = "";
            if (MyResponseData.AllKeys.Contains("xStatus"))
                MyStatus = MyResponseData["xStatus"];
            string MyError = "";
            if (MyResponseData.AllKeys.Contains("xError"))
                MyError = MyResponseData["xError"];
            string MyRefNum = "";
            if (MyResponseData.AllKeys.Contains("xRefNum"))
                MyRefNum = MyResponseData["xRefNum"];
            string MyToken = "";
            if (MyResponseData.AllKeys.Contains("xToken"))
                MyToken = MyResponseData["xToken"];

            Response response = new Response
            {
                Status = MyStatus,
                RefNum = MyRefNum,
                Error = MyError,
            };

            
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            if(MyStatus == "Approved")
            {
                Payment p = new Payment
                {
                    MemberId = memberId,
                    ExpenseId = expenseId,
                    Amount = amount,
                    Date = DateTime.Today
                };
                fr.AddPayment(p);
            }
            Member member = fr.GetMember(memberId);
            SendEmail(member, response, expenseId, amount);
            count--;
            if (MyStatus == "Approved" && count != null && credit)
            {
                MonthlyPayment mp = new MonthlyPayment
                {
                    Amount = amount,
                    Token = MyToken,
                    Name = name,
                    MemberId = memberId,
                    ExpenseId = expenseId,
                    Count = count.Value,
                    Street = street,
                    Zipcode = zipcode,
                    Exp = exp,
                    Credit = true
                };
                fr.AddMonthlyPayment(mp);
            }
            else if (MyStatus == "Approved" && count != null && !credit)
            {
                MonthlyPayment mp = new MonthlyPayment
                {
                    Amount = amount,
                    Token = MyToken,
                    Name = name,
                    MemberId = memberId,
                    ExpenseId = expenseId,
                    Count = count.Value,
                    Credit = false
                };
                fr.AddMonthlyPayment(mp);
            }
            return View(response);
        }



        [AuthorizeUserAccessLevel(UserRole = "Admin")]
        public ActionResult MonthlyPayments(string password)
        {
            if (password != "MAWV29mp")
            {
                return RedirectToAction("Index", "Home");
            }
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            IEnumerable<MonthlyPayment> monthlyPayments = fr.GetMonthlyPayments();
            List<Response> responses = new List<Response>();
            foreach (MonthlyPayment mp in monthlyPayments)
            {
                string command = mp.Credit ? "cc:Sale" : "check:Sale";
                System.Collections.Specialized.NameValueCollection MyPost = new System.Collections.Specialized.NameValueCollection();
                MyPost.Add("xKey", "YehoshuaLevinDev_Test_12301258f0bf44f2a6fcf0e");
                MyPost.Add("xVersion", "4.5.5");
                MyPost.Add("xSoftwareName", "ylevin");
                MyPost.Add("xSoftwareVersion", "1.4.2");
                MyPost.Add("xCommand", "cc:Sale");
                MyPost.Add("xAmount", mp.Amount.ToString());
                MyPost.Add("xToken", mp.Token);
                MyPost.Add("xName", mp.Name);
                if (mp.Credit)
                {
                    MyPost.Add("xStreet", mp.Street);
                    MyPost.Add("xZip", mp.Zipcode);
                    MyPost.Add("xExp", mp.Exp);
                }

                System.Net.WebClient MyClient = new System.Net.WebClient();
                string MyResponse = System.Text.Encoding.ASCII.GetString(MyClient.UploadValues("https://x1.cardknox.com/gateway", MyPost));
                // Response
                System.Collections.Specialized.NameValueCollection MyResponseData = HttpUtility.ParseQueryString(MyResponse);
                string MyStatus = "";
                if (MyResponseData.AllKeys.Contains("xStatus"))
                    MyStatus = MyResponseData["xStatus"];
                string MyError = "";
                if (MyResponseData.AllKeys.Contains("xError"))
                    MyError = MyResponseData["xError"];
                string MyRefNum = "";
                if (MyResponseData.AllKeys.Contains("xRefNum"))
                    MyRefNum = MyResponseData["xRefNum"];

                Response response = new Response
                {
                    Status = MyStatus,
                    RefNum = MyRefNum,
                    Error = MyError
                };

                if (MyStatus == "Approved")
                {
                    Payment p = new Payment
                    {
                        MemberId = mp.MemberId,
                        ExpenseId = mp.ExpenseId,
                        Amount = mp.Amount,
                        Date = DateTime.Today
                    };
                    fr.AddPayment(p);
                    fr.UpdateMonthlyPayment(mp.Id);
                }
                
                Member member = fr.GetMember(mp.MemberId);
                SendEmail(member,response,mp.ExpenseId,mp.Amount);
                responses.Add(new Response
                {
                    Status = MyStatus,
                    RefNum = MyRefNum,
                    Error = MyError
                });
            }
            
            return View(responses);
        }

        private void SendEmail(Member m,Response r,int expenseId,int amount)
        {
            FinancialRepository fr = new FinancialRepository(Settings.Default.ConStr);
            Expense expense = fr.GetExpenseByExpenseId(expenseId);
            var fromAddress = new MailAddress("yehoshualevin22@gmail.com", "Beis Medrash of Jackson");
            var toAddress = new MailAddress(m.Email, m.FirstName + " " + m.LastName);

            string fromPassword = "pencup11";
            string subject = "Your Donation";
            string body = $"Hi {m.FirstName},\n\n";
            if (r.Status == "Approved")
            {
                body += $"Thank you for your payment of ${amount}.00 towards {expense.Name}.\n";
            }
            else if (r.Status == "Error")
            {
                body += $"There was an error with your payment of ${amount}.00 towards {expense.Name}.\n{r.Error}\n";
            }
            else if (r.Status == "Declined")
            {
                body += $"Your payment of ${amount}.00 towards {expense.Name} has been declined.\n{r.Error}\n";
            }
            body += $"Reference Number: {r.RefNum}";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}