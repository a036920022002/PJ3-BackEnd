using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using PJ3_BackEnd.Models;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PJ3_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly profileContext _profileContext;
        private readonly IWebHostEnvironment _env;
        public WorksController(profileContext profileContext, IWebHostEnvironment env)
        {
            _profileContext = profileContext;
            _env = env;
        }
        // GET: api/<WorkController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<works>> Get()
        {
            return await _profileContext.works.ToListAsync();
        }

        // GET api/<WorkController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<works> Get(int id)
        {
            var result = await _profileContext.works.FindAsync(id);
            return result;
        }

        // POST api/<WorkController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] works value)
        {
            var insert = new works
            {
                name = value.name,
                descript=value.descript,
                frontEnd = value.frontEnd,
                backEnd = value.backEnd,
                database_name = value.database_name,
                tool=value.tool,
                function_name   = value.function_name,
                gitHub_link = value.gitHub_link,
                page_link   = value.page_link,
                item_label = value.item_label
            };
            _profileContext.works.Add(insert);
            await _profileContext.SaveChangesAsync();

            return Ok("Create" + insert);
                
        }
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult> PostCertificate(int id, List<IFormFile> files)
        {
            var upload = await (from a in _profileContext.works
                         where a.id == id
                         select a).SingleOrDefaultAsync();
            var root = Path.Combine(_env.ContentRootPath,"public","works",id.ToString());
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            List<string> imageList = new List<string>();
            if (!string.IsNullOrEmpty(upload.image))
            {
                try
                {
                    // 解析資料庫現有的 JSON 陣列
                    imageList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(upload.image) ?? new List<string>();
                }
                catch
                {
                    // 如果原本手動輸入的格式有誤，就重設為空清單
                    imageList = new List<string>();
                }
            }
            if (files == null || files.Count == 0) return BadRequest("未含檔案");
            foreach (var file in files) 
            {
                
                    var extension = Path.GetExtension(file.FileName);
                    var PJNo = "PJ" + id;
                    var random = Random.Shared.Next(0, 1000);
                    var fileName = PJNo + "-" + id + random + extension;
                    var fillPath = Path.Combine(root, fileName);

                    using(var stream = new FileStream(fillPath, FileMode.Create))
                    {
                       await file.CopyToAsync(stream);
                    }

                    imageList.Add(fileName);                          
               
            }
            //使用 Serialize 轉成 JSON 字串 ["pic1","pic2"]
            var jsonString = System.Text.Json.JsonSerializer.Serialize(imageList);
            upload.image = jsonString;
            await _profileContext.SaveChangesAsync();
            return Ok("新增照片"+ imageList);
        }

        // PUT api/<WorkController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] works value)
        {
            var update = await (from a in _profileContext.works
                         where a.id == id
                         select a).SingleOrDefaultAsync();
            if (update != null)
            {
                update.name = value.name;
                update.descript = value.descript;
                update.tool = value.tool;
                update.frontEnd = value.frontEnd;
                update.backEnd  = value.backEnd;
                update.database_name= value.database_name;
                update.function_name= value.function_name;
                update.gitHub_link= value.gitHub_link;
                update.page_link= value.page_link;
                update.item_label = value.item_label;
                await _profileContext.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }

        // DELETE api/<WorkController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var delete = await(from a in _profileContext.works
                          where a.id == id
                          select a).SingleOrDefaultAsync();
            if (delete != null) 
            {
                _profileContext.works.Remove(delete);
                await _profileContext.SaveChangesAsync();
                return Ok(delete);
            }

            return BadRequest();
        }

    }
}
