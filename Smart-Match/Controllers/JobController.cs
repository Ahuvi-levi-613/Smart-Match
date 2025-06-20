using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Match.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class JobController : ControllerBase
    {

        private readonly IService<JobDto> service;
        public JobController(IService<JobDto> service)
        {
            this.service = service;
        }
        // GET: api/<JobController>
        [HttpGet]
        public async Task<List<JobDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<JobController>/5
        [HttpGet("{id}")]
        public async Task<JobDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<JobController>
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JobDto job)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdJob = await service.AddItem(job);
            return CreatedAtAction(nameof(Get), new { id = createdJob.JobId }, createdJob);
        }

        // PUT api/<JobController>/5
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] JobDto job)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await service.UpdateItem(id, job);
            return NoContent();
        }

        // DELETE api/<JobController>/5
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
        }
    }
}
