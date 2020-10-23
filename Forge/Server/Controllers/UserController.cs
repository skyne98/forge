using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forge.Server.Data;
using Forge.Shared.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Forge.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IDbUserRepository _dbUserService;

        public UserController(ILogger<UserController> logger, IDbUserRepository dbUserService)
        {
            _dbUserService = dbUserService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            return _dbUserService.FindAll();
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> Get(Guid id)
        {
            var result = _dbUserService.FindOne(id);
            if (result != default)
                return Ok(_dbUserService.FindOne(id));
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<UserModel> Insert(UserModel dto)
        {
            var id = _dbUserService.Insert(dto);
            if (id != default)
                return CreatedAtAction("FindOne", _dbUserService.FindOne(id));
            else
                return BadRequest();
        }

        [HttpPut]
        public ActionResult<UserModel> Update(UserModel dto)
        {
            var result = _dbUserService.Update(dto);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult<UserModel> Delete(Guid id)
        {
            var result = _dbUserService.DeleteOne(id);
            if (result)
                return NoContent();
            else
                return NotFound();
        }
    }
}
