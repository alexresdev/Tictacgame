using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Application.Services;
using Application.Models;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        readonly ILogger<UserRegistrationController> _logger;

        public UserRegistrationController(IUserService userService, IEmailService emailService,
            ILogger<UserRegistrationController> logger)
        {
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterUser(userModel);
                return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
            }
            else
            {
                return View(userModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            _logger.LogInformation($"##Start## Email confirmation process for {email}");
            var user = await _userService.GetUserByEmail(email);
            var urlAction = new UrlActionContext
            {
                Action = "ConfirmEmail",
                Controller = "UserRegistration",
                Values = new { email },
                Protocol = Request.Scheme,
                Host = Request.Host.ToString()
            };

            var message = $"Thank you for your registration on our website, please click here to confirm your email " +
                $"{Url.Action(urlAction)}";

            try
            {
                _emailService.SendEmail(email, "Tic-Tac-Toe Email Confirmation", message).Wait();
            }
            catch (Exception e)
            {
            }

            if (user?.IsEmailConfirmed == true)
                return RedirectToAction("Index", "GameInvitation", new {email = email});
            ViewBag.Email = email;
            return View();
        }
    }
}
