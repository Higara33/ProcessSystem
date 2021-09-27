﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProcessSystem.Contracts;
using ProcessSystem.DB;
using Microsoft.Extensions.Options;
using ProcessSystem.Token;
using System.Text.Json;

namespace ProcessSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IRegisterRepository _registerRepository;
        private readonly IToken _token;
        private readonly ProcessContext processContext;
        private readonly IConfiguration configuration;
        private readonly IOptions <TestSettings> testSettings;
        public IConfiguration AppConfiguration { get; private set; }

        public RegisterController(ILogger<RegisterController> logger, IToken token, ProcessContext processContext, IConfiguration configuration, IOptions<TestSettings> testSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _registerRepository = new RegisterRepository(processContext);
            this.processContext = processContext;
            AppConfiguration = configuration;
            this.testSettings = testSettings;
            
        }

        [AllowAnonymous]
        [HttpPost("RegisterUrl")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        

        public async Task<ActionResult<BaseResponse<string>>> Register([FromBody] RegisterRequest registerRequest)
        {

            try
            {

                var check = testSettings.Value.millis;
                string json = JsonSerializer.Serialize<IOptions<TestSettings>>(testSettings);
                return CreatedAtAction(nameof(Register),
                    new BaseResponse<string>
                    {
                        Data = json
                    });
                

                _logger?.LogDebug("Register start");

                ValidateRequest(registerRequest);

                var token = new RegisterTokenResponse(_token.GenerateToken()).Token;

                Register register =
                    new Register(token,
                        registerRequest.Url,
                        registerRequest.Name);
                register.SetEventTypes(registerRequest.ProcessTypesList);

                if ( _registerRepository.FindByNameAndUrl(register) != null)
                    throw new ArgumentOutOfRangeException(nameof(registerRequest), $"Витрина с {registerRequest.Url} и {registerRequest.Name} уже зерегистрирована");

                var result = await _registerRepository.AddAsync(register);
                await _registerRepository.UnitOfWork.SaveEntitiesAsync();

                return CreatedAtAction(nameof(Register),
                    new BaseResponse<string>
                    {
                        Data = result.Token
                    });
            }
            catch (Exception ex) when
            (ex is ArgumentNullException ||
             ex is ArgumentOutOfRangeException)
            {

                _logger?.LogError(ex, $"Register {ex.Message}");
                return new ObjectResult(
                    new BaseResponse<object>
                    {
                        ErrorDescription = ex.Message,
                        ErrorCode = "RegisterError"
                    })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        private void ValidateRequest(RegisterRequest request)
        {
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, context, results, true))
            {
                var message = string.Join("\n", results.Select(i => i.ErrorMessage));
                throw new ArgumentException($"Request validation failed. Errors: \n {message}");
            }
        }

        [HttpPost("UnRegisterUrl")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse<Register>>> UnRegister()
        {
            try
            {
                _logger?.LogDebug("UnRegister start");

                var token = Request.Headers[HeaderNames.Authorization].ToString();

                var result = await _registerRepository.DeleteAsync(token.Replace("Bearer ", ""));
                await _registerRepository.UnitOfWork.SaveEntitiesAsync();

                return CreatedAtAction(nameof(UnRegister),
                    new BaseResponse<Register>
                    {
                        Data = result
                    });
            }
            catch (Exception ex) when
            (ex is ArgumentNullException ||
             ex is ArgumentOutOfRangeException)
            {

                _logger?.LogError(ex, $"UnRegister {ex.Message}");
                return new ObjectResult(new BaseResponse<object>
                {
                    ErrorDescription = ex.Message,
                    ErrorCode = "UnRegisterError"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

    }
}
