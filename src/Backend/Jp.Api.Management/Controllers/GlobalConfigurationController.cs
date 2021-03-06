﻿using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jp.Api.Management.Controllers
{
    [Route("global-configuration"), Authorize(Policy = "Default")]
    public class GlobalConfigurationController : ApiController
    {
        private readonly IGlobalConfigurationAppService _globalConfigurationSettingsAppService;
        private readonly ISystemUser _systemUser;

        public GlobalConfigurationController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            IGlobalConfigurationAppService globalConfigurationSettingsAppService,
            ISystemUser systemUser) : base(notifications, mediator)
        {
            _globalConfigurationSettingsAppService = globalConfigurationSettingsAppService;
            _systemUser = systemUser;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ConfigurationViewModel>>> List()
        {
            var userAdmin = _systemUser.IsInRole("Administrator");
            var settings = await _globalConfigurationSettingsAppService.ListSettings();
            return ResponseGet(settings);
        }

        [HttpPut("")]
        public async Task<ActionResult> Update([FromBody] IEnumerable<ConfigurationViewModel> settings)
        {
            await _globalConfigurationSettingsAppService.UpdateSettings(settings);
            return ResponsePutPatch();
        }
    }
}