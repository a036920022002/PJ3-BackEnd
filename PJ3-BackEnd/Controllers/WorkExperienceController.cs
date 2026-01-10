using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ3_BackEnd.Dtos;
using PJ3_BackEnd.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkExperienceController : ControllerBase
    {
        private readonly profileContext _profileContext;
        public WorkExperienceController(profileContext profileContext)
        {
            _profileContext = profileContext;
        }
        // GET: api/<WorkExperienceController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<workexperience>> Get()
        {
            return await _profileContext.workexperience.ToListAsync();
        }

        // POST api/<WorkExperienceController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] WorkExperiencePostDto value)
        {
            var insert = new workexperience 
            {
                company = value.company,
                companyEng = value.companyEng,
                companyType = value.companyType,
                location = value.location,
                yearInService   = value.yearInService,
                tenure  = value.tenure,
                jobPosition = value.jobPosition,
                jobPositionEng =    value.jobPositionEng,
                descript = value.descript,
                descriptEng = value.descriptEng,
            };
            _profileContext.Add(insert);
            await _profileContext.SaveChangesAsync();
            return Ok("已新增"+ insert);
        }

        // PUT api/<WorkExperienceController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] workexperience value)
        {
            var update = await (from a in _profileContext.workexperience
                          where a.id == id
                          select a ).SingleOrDefaultAsync();

            if (update != null) 
            {
                update.company = value.company;
                update.companyEng = value.companyEng;
                update.companyType = value.companyType;
                update.location = value.location;
                update.yearInService = value.yearInService;
                update.tenure = value.tenure;
                update.jobPosition = value.jobPosition;
                update.jobPositionEng = value.jobPositionEng;   
                update.descript = value.descript;
                update.descriptEng = value.descriptEng;

                await _profileContext.SaveChangesAsync();

                return Ok(update);
            }

            return BadRequest();

        }

        // DELETE api/<WorkExperienceController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> ActionResult(int id)
        {
            var delete = await (from a in _profileContext.workexperience
                          where a.id == id
                          select a).SingleOrDefaultAsync();
            if(delete != null)
            {
                _profileContext.workexperience.Remove(delete);
                await _profileContext.SaveChangesAsync();

                return Ok(delete);
            }

            return BadRequest();
            
            
        }
    }
}
