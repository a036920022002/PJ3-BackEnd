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
    public class EducationController : ControllerBase
    {
        private readonly profileContext _profileContext;

        public EducationController(profileContext profileContext)
        {
            _profileContext = profileContext;
        }

        // GET: api/<educationController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<education>> Get()
        {
            return await _profileContext.education.ToListAsync();
        }
        // POST api/<educationController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EducationPostDto value)
        {

            var insert = new education
            {
                school=value.school,
                schoolEng = value.schoolEng,
                department=value.department,
                departmentEng = value.departmentEng,
                degree=value.degree,
                degreeEng= value.degreeEng,
                periodOfStudytime=value.periodOfStudytime,
            };

            _profileContext.Add(insert);
            await _profileContext.SaveChangesAsync();
            return Ok("已新增"+ insert);
        }

        // PUT api/<educationController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] education value)
        {
            var update = await (from a in _profileContext.education
                         where a.id == id
                         select a).SingleOrDefaultAsync();

            if (update != null) 
            {
                update.school=value.school;
                update.schoolEng=value.schoolEng;
                update.department=value.department;
                update.departmentEng  = value.departmentEng;
                update.degree = value.degree;
                update.degreeEng =value.degreeEng;
                update.periodOfStudytime=value.periodOfStudytime;
                await _profileContext.SaveChangesAsync();

                return Ok(update);
            }

            return BadRequest();

        }

        // DELETE api/<educationController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var delete = await (from a in _profileContext.education
                         where a.id == id
                         select a).SingleOrDefaultAsync();
            if (delete != null) 
            {   
                _profileContext.education.Remove(delete);
                await _profileContext.SaveChangesAsync();
                return Ok(delete);
            }
            return BadRequest();
        }
    }
}
