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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly ILiteDbCharacterService _dbCharacterService;

        public CharacterController(ILogger<CharacterController> logger, ILiteDbCharacterService dbCharacterService)
        {
            _dbCharacterService = dbCharacterService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CharacterModel> Get()
        {
            return _dbCharacterService.FindAll()
                .OrderBy(character => character.Name);
        }

        [HttpGet]
        public ActionResult<CharacterModel> GetOne(Guid id)
        {
            var result = _dbCharacterService.FindOne(id);
            if (result != default)
                return Ok(_dbCharacterService.FindOne(id));
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<CharacterModel> Insert(CharacterModel dto)
        {
            var id = _dbCharacterService.Insert(dto);
            if (id != default)
                return CreatedAtAction("GetOne", _dbCharacterService.FindOne(id));
            else
                return BadRequest();
        }

        [HttpPost]
        public ActionResult<CharacterModel> Update(CharacterModel dto)
        {
            var result = _dbCharacterService.Update(dto);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<bool> DeleteOne([FromBody] Guid id)
        {
            var result = _dbCharacterService.DeleteOne(id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            } 
        }
        
        [HttpPost]
        public ActionResult<CharacterModel> RestoreOne([FromBody] Guid id)
        {
            var result = _dbCharacterService.RestoreOne(id);
            if (result)
            {
                return Ok(_dbCharacterService.RestoreOne(id));
            }
            else
            {
                return NotFound();
            } 
        }
    }
}
