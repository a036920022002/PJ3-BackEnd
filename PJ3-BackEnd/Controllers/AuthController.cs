using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ3_BackEnd.Dtos;
using PJ3_BackEnd.Models;
using PJ3_BackEnd.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly profileContext _profileContext;
        private readonly IPasswordService _passwordService;

        public AuthController(profileContext profileContext, IPasswordService passwordService)
        {
            _profileContext = profileContext;
            _passwordService = passwordService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<auth>> Get()
        {
            return await _profileContext.auth.ToListAsync();
        }

        // POST api/<authController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthPostDto value)
        {
            if (value == null || string.IsNullOrEmpty(value.email))
            {
                return BadRequest("資料不完整");
            }
            var exist = await _profileContext.auth.AnyAsync(a => a.email == value.email);
            if ( exist ==true)
            {
                return BadRequest("帳號已註冊");
            }
            var hashPassword = _passwordService.HashPassword(value.password);
            var insert = new auth
            {
                email = value.email,
                password = hashPassword,
                name = value.name
            };
            _profileContext.Add(insert);
            await _profileContext.SaveChangesAsync();
            return Ok(new { message = "註冊成功", email = insert.email });
        }

        // PUT api/<authController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] LoginDto value)
        {
            var update =await (from a in _profileContext.auth
                          where a.id == id
                          select a).SingleOrDefaultAsync();

            if (update != null) 
            {
                update.password = value.password;

                await _profileContext.SaveChangesAsync();
                return Ok();
            
            }
            return BadRequest();

        }

        // DELETE api/<authController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var delete =await (from a in _profileContext.auth
                          where a.id == id
                          select a).SingleOrDefaultAsync();
            if (delete != null)
            {
                _profileContext.auth.Remove(delete);
                await _profileContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
