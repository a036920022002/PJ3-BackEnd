using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ3_BackEnd.Models;


namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly profileContext _profileContext;
        private readonly IWebHostEnvironment _env;
        public CertificateController (profileContext profileContext, IWebHostEnvironment env)
        {
            _profileContext = profileContext;
            _env = env;
        }
        // GET: api/<CertificateController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<certificate>> Get()
        {
            return await _profileContext.certificate.ToListAsync();
        }

        // POST api/<CertificateController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] certificate value)
        {
            var insert = new certificate
            {
                name = value.name,
                issuing_authority = value.issuing_authority
            };
            _profileContext.Add(insert);
            await _profileContext.SaveChangesAsync();
            return Ok("已新增"+insert);

        }

        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult> PostCertificate(int id, IFormFile file)
        {
            var upload = await (from a in _profileContext.certificate
                          where a.id == id
                          select a).SingleOrDefaultAsync();

            var root = Path.Combine(_env.ContentRootPath, "public", "certificate");
            if (file.Length > 0)
            {
                var timesheet = DateTime.Now.ToString("YYYYMMDDhhmmss");                
                var extension = Path.GetExtension(file.FileName);
                var fileName = id + "-" + timesheet+extension;
                var fullPath = Path.Combine(root, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                upload.photo = fileName;
                await _profileContext.SaveChangesAsync();
                
                return Ok();
            }
            return BadRequest();
        }
        // PUT api/<CertificateController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] certificate value)
        {
            var update = await (from a in _profileContext.certificate
                          where  a.id == id
                          select a).SingleOrDefaultAsync();
            if (update != null)
            {
                update.name = value.name;
                update.issuing_authority = value.issuing_authority;

                await _profileContext.SaveChangesAsync();

                return Ok();

            }
            return BadRequest();
        }

        // DELETE api/<CertificateController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var delete = await (from a in _profileContext.certificate
                          where a.id == id
                          select a ).SingleOrDefaultAsync();
            if (delete != null) 
            {
                _profileContext.certificate.Remove(delete);
                await _profileContext.SaveChangesAsync();
                return Ok(delete);
            }

            return BadRequest();
        }
    }
}
