using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Match.Controllers
{
    [Authorize(Roles = "Manager,Candidate")]
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IService<CandidateDto> service;
        public CandidateController(IService<CandidateDto> service)
        {
            this.service = service;
        }

        // GET: api/<CandidateController>
        [HttpGet]
        public async Task<List<CandidateDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<CandidateController>/5
        [HttpGet("{id}")]
        public async Task<CandidateDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<CandidateController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CandidateDto candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AddItem(candidate);
            return CreatedAtAction(nameof(Get), new { id = result.CandidateId }, result);
        }


        // PUT api/<CandidateController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CandidateDto candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await service.UpdateItem(id, candidate);
            return NoContent();
        }


        // DELETE api/<CandidateController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
        }
    }
}
