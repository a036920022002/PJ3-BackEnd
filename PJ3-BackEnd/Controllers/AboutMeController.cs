using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PJ3_BackEnd.Dtos;
using PJ3_BackEnd.Models;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {

        private readonly profileContext _profileContext;
        private readonly IWebHostEnvironment _env;
        public AboutMeController(profileContext profileContext, IWebHostEnvironment env)
        {
            _profileContext = profileContext;
            _env = env;

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AboutMeDto>>> Get()
        {
            var result = await (from a in _profileContext.aboutme
                         select new AboutMeDto
                         {
                             name = a.name,
                             email = a.email,
                             gender = a.gender,
                             englishName = a.englishName,
                             birth = a.birth,
                             phone = a.phone,
                             photo = a.photo,
                             address = a.address,
                             intro = a.intro,
                             introEng = a.introEng,
                             fb = a.fb,
                             ig = a.ig,
                             linkedin = a.linkedin,
                             LINE = a.LINE,
                         }).ToListAsync();
            return result;
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] aboutme value)
        {
            var update = await _profileContext.aboutme.FirstOrDefaultAsync();
            if (update != null) 
            { 
                update.name = value.name;
                update.email = value.email;
                update.gender = value.gender;
                update.englishName = value.englishName;
                update.birth = value.birth;
                update.phone = value.phone;
                update.address = value.address;
                update.intro = value.intro;
                update.introEng = value.introEng;
                update.fb = value.fb;
                update.ig = value.ig;
                update.linkedin = value.linkedin;
                update.LINE = value .LINE;

                await _profileContext.SaveChangesAsync();
                return Ok(update);
            }

            return BadRequest();

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostPhoto(IFormFile file)
        {
            var root = Path.Combine(_env.ContentRootPath, "public", "aboutme");

            if (file.Length >0)
            {
                var timesheet = DateTime.Now.ToString("YYYYMMDDhhmmss");
                var random = Random.Shared.Next(0, 1000000001);
                var extension = Path.GetExtension(file.FileName);

                var fileName = timesheet + "-" + random + extension;
                var fullPath = Path.Combine(root, fileName);
                var upload = await _profileContext.aboutme.FirstOrDefaultAsync();

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                upload.photo = fileName;
                await _profileContext.SaveChangesAsync();


                return Ok("以存檔"+ file.Name);
            }
            return BadRequest();
        }

    }
}
