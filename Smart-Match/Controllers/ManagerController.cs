using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Match.Controllers
{
    //Authenticate-אימות משתמש
    //Authorization-הרשאת גישה

    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IService<ManagerDto> service;
        public ManagerController(IService<ManagerDto> service)
        {
                this.service = service;
        }

        // GET: api/<ManagerController>
        [HttpGet]
        public List<ManagerDto> Get()
        {
            return service.GetAll().ToList();
        }

        // GET api/<ManagerController>/5
        [HttpGet("{id}")]
        public ManagerDto Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<ManagerController>
        [HttpPost]
       // [Authorize (Roles ="admin")]//הכוונה פה זה לתת אופציה רק למשתמש שילו token שיש לו הרשאת גישה(לדוגמא מי שיש לו תפקיד מסוים)
        public ManagerDto Post([FromBody] ManagerDto manager)
        {
          return  service.AddItem(manager);
        }

        // PUT api/<ManagerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ManagerDto manager)
        {
            service.UpdateItem(id, manager);
        }

        // DELETE api/<ManagerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           service.DeleteItem(id);
        }
    }


   
}
