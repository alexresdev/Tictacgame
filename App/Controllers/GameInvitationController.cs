﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    public class GameInvitationController : Controller
    {
        private IStringLocalizer<GameInvitationController> _stringLocalizer;
        private IUserService _userService;
        public GameInvitationController(IUserService userService, IStringLocalizer<GameInvitationController> stringLocalizer)
        {
            _userService = userService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpPost]
        public IActionResult Index(GameInvitationModel gameInvitationModel, [FromServices] IEmailService emailService)
        {
            var gameInvitationService = Request.HttpContext.RequestServices.GetService<IGameInvitationService>();

            if (ModelState.IsValid)
            {
                emailService.SendEmail(gameInvitationModel.EmailTo,
                 _stringLocalizer["Invitation for playing a Tic-Tac-Toe game"],
                 _stringLocalizer[$"Hello, you have been invited to play the Tic-Tac-Toe game by {0}." +
                 $"For joining the game, please click here {1} ", gameInvitationModel.InvitedBy,
                Url.Action("GameInvitationConfirmation", "GameInvitation",
                new
                {
                    gameInvitationModel.InvitedBy,
                    gameInvitationModel.EmailTo
                }, Request.Scheme, Request.Host.ToString())]);

                var invitation = gameInvitationService.Add(gameInvitationModel).Result;
                return RedirectToAction("GameInvitationConfirmation", new { id = invitation.Id });
            }
            return View(gameInvitationModel);
        }

        [HttpGet]
        public IActionResult GameInvitationConfirmation(Guid id, [FromServices] IGameInvitationService gameInvitationService)
        {
            var gameInvitation = gameInvitationService.Get(id).Result;
            return View(gameInvitation);
        }
    }
}
