using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transcore.Services.Membership.Authentication;
using Transcore.Services.Membership.DataStore;
using Transcore.Services.Membership.IdentityModels;
using Transcore.Services.Membership.Models;


namespace Transcore.Services.Membership.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        public MembershipController(IRepository repository, IMapper mapper, ITokenManager tokenManager)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserModel userModel)
        {
            try
            {
                if (userModel == null)
                    return BadRequest("User cannot be null");

                var dbUser = await _repository.GetUser(userModel.UserName);

                if (dbUser != null)
                    return BadRequest("User with same name already exist");

                var result = await _repository.CreateUser(_mapper.Map<ApplicationUser>(userModel));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {

            try
            {
                var dbUser = await _repository.GetUser(loginModel.UserName);

                if (dbUser == null)
                    return Unauthorized("Invalid username/password");

                dbUser.Password = loginModel.Password;

                bool isCorrectPassword =
                    await _repository.CheckPassword(dbUser);

                if (!isCorrectPassword)
                    return Unauthorized("Invalid username/password");

                var claimsDict = new Dictionary<string, string>();

                claimsDict.Add("UserName", loginModel.UserName);

                var jwt = _tokenManager.GenerateToken(claimsDict);

                return Ok(jwt);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public List<string> Get()
        {
            return new List<string>()
            {
                "Admin",
                "Accounts",
                "HR"
            };
        }
    }
}
