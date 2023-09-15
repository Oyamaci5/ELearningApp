using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LearningApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [HttpGet]
        public IActionResult Contact()
        {
            // Diğer işlemler
            return View(); // Görünüm adını belirtin
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kullanıcı adı ve şifreyi doğrula, oturum aç ve yönlendirme yap.
            if (IsValidUser(username, password))
            {
                // Oturum açma işlemi burada gerçekleşir.
                // Örneğin, Forms Authentication veya Identity kullanabilirsiniz.
                return RedirectToAction("Dashboard"); // Kullanıcıyı yönlendirin.
            }
            else
            {
                // Geçersiz giriş durumu için hata mesajı göstermek için ModelState kullanabilirsiniz.
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Kullanıcıyı doğrulama işlemini gerçekleştirin.
        private bool IsValidUser(string username, string password)
        {
            // Kullanıcı doğrulama mantığı burada gerçekleşir.
            // Örnek bir kullanıcı doğrulama kodu eklemelisiniz.
            return (username == "example" && password == "password");
        }


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}