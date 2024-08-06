using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PEPRN231_SU24_009909_LamMinhDang_BE.ViewModel;
using Repository.Repositories.Interface;
using Repository.UnitOfWork;

namespace PEPRN231_SU24_009909_LamMinhDang_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {
            var exitedUser = await _unitOfWork.AccountRepository
                .GetAsync(x => x.EmailAddress.Equals(loginDTO.email) && x.Password.Equals(loginDTO.password));
            if (exitedUser == null)
            {
                return NotFound("Can not found user");
            }
            return Ok(_tokenGenerator.GenerateToken(exitedUser));
        }
    }
}
