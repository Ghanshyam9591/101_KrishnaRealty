using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsAppApi;
using EMS.Common;
using Npgsql;
using MicroORM;
using EMS.Web.Models;

namespace EMS.Web.Controllers
{
    public class SendMessageController : Controller
    {
        // GET: SendMessage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendAddress()
        {
            string To = "7237822495";
            string Message = "Test";
            string status = "";
            WhatsApp wa = new WhatsApp("9594893631", "jaishreerma", "Ghanshyam", false, false);
            wa.OnConnectSuccess += () =>
            {
                wa.OnLoginSuccess += (PhoneNumber, data) =>
                {
                    status = "connection success";
                    wa.SendMessage(To, Message);
                    status = "Message sent successfully";
                };
                wa.OnLoginFailed += (data) =>
                {
                    status = "Login Failed" + data;
                };
                wa.Login();
            };
            wa.OnConnectFailed += (ex) =>
            {
                status = "connection failed" + ex.StackTrace;
            };
            return new JsonNetResult(null);
        }
    }
}