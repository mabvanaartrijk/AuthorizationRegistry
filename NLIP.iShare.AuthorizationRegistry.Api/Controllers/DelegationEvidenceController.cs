﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NLIP.iShare.Api;
using NLIP.iShare.AuthorizationRegistry.Api.Attributes;
using NLIP.iShare.AuthorizationRegistry.Api.ViewModels;
using NLIP.iShare.AuthorizationRegistry.Core.Api;
using NLIP.iShare.Models.DelegationEvidence;
using NLIP.iShare.Models.DelegationMask;
using System;
using System.Threading.Tasks;

namespace NLIP.iShare.AuthorizationRegistry.Api.Controllers
{
    [Route("delegation")]
    [Authorize]
    public class DelegationEvidenceController : Controller
    {
        private readonly IDelegationService _delegationService;
        private readonly IDelegationTranslateService _delegationTranslateService;
        private readonly IDelegationMaskValidationService _delegationMaskValidationService;
        private readonly IDelegationJwtBuilder _delegationJwtBuilder;

        public DelegationEvidenceController(
            IDelegationService delegationService,
            IDelegationTranslateService delegationTranslateService,
            IDelegationMaskValidationService delegationMaskValidationService,
            IDelegationJwtBuilder delegationJwtBuilder)
        {
            _delegationService = delegationService;
            _delegationTranslateService = delegationTranslateService;
            _delegationMaskValidationService = delegationMaskValidationService;
            _delegationJwtBuilder = delegationJwtBuilder;
        }

        /// <summary>
        /// Find the corresponding delegation policy and run the delegation translation algorithm
        /// </summary>
        /// <param name="mask">The delegation mask used to find the delegation policy</param>
        /// <returns>JWT encoded delegation evidence</returns>
        [HttpPost, TypeFilter(typeof(ValidateModelStateAttribute))]
        public async Task<IActionResult> Translate([FromBody]JObject mask)
        {
            var result = await TranslateDelegation(mask);

            var token = _delegationJwtBuilder.Create(result.Value.DelegationEvidence, User.GetRequestingPartyId());

            return Ok(new DelegationTokenResponse { DelegationToken = token });
        }

        /// <summary>
        /// Find the corresponding delegation policy and run the delegation translation algorithm
        /// </summary>
        /// <param name="mask">The delegation mask used to find the delegation policy</param>
        /// <returns>The delegation evidence</returns>
        [HttpPost, TypeFilter(typeof(ValidateModelStateAttribute)), Route("test"), ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<DelegationTranslationTestResponse>> TestDelegationTranslation([FromBody]JObject mask)
        {
            return await TranslateDelegation(mask);
        }

        private async Task<ActionResult<DelegationTranslationTestResponse>> TranslateDelegation(JObject mask)
        {
            var delegationMask = mask.ToObject<DelegationMask>();

            var validationResult = _delegationMaskValidationService.Validate(delegationMask);

            if (!validationResult.Success)
            {
                return BadRequest(new { error = validationResult.Error });
            }

            var delegation = await _delegationService
                .GetBySubject(delegationMask.DelegationRequest.Target.AccessSubject, delegationMask.DelegationRequest.PolicyIssuer)
                .ConfigureAwait(false);

            if (delegation == null)
            {
                return NotFound();
            }

            var delegationResponse = _delegationTranslateService.Translate(delegationMask, delegation.Policy);
            return delegationResponse;
        }
    }
}