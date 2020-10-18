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
    public class CharacterTagController: ControllerBase
    {
        private readonly ILogger<CharacterTagController> _logger;
        private readonly ILiteDbCharacterTagService _dbCharacterTagService;

        public CharacterTagController(ILogger<CharacterTagController> logger, ILiteDbCharacterTagService dbCharacterTagService)
        {
            _dbCharacterTagService = dbCharacterTagService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CharacterTagModel> Get()
        {
            return _dbCharacterTagService.FindAll()
                .OrderBy(character => character.Name);
        }

        [HttpGet]
        public ActionResult<CharacterTagModel> GetOne(Guid id)
        {
            var result = _dbCharacterTagService.FindOne(id);
            if (result != default)
                return Ok(_dbCharacterTagService.FindOne(id));
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<CharacterTagModel> Insert(CharacterTagModel dto)
        {
            var id = _dbCharacterTagService.Insert(dto);
            if (id != default)
                return CreatedAtAction("GetOne", _dbCharacterTagService.FindOne(id));
            else
                return BadRequest();
        }

        [HttpPost]
        public ActionResult<CharacterTagModel> Update(CharacterTagModel dto)
        {
            var result = _dbCharacterTagService.Update(dto);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<CharacterModel> DeleteOne([FromBody] Guid id)
        {
            var result = _dbCharacterTagService.DeleteOne(id);
            if (result)
            {
                return Ok(_dbCharacterTagService.DeleteOne(id));
            }
            else
            {
                return NotFound();
            } 
        }

        public ActionResult<CharacterModel> RestoreOne([FromBody] Guid id)
        {
            var result = _dbCharacterTagService.RestoreOne(id);
            if (result)
            {
                return Ok(_dbCharacterTagService.RestoreOne(id));
            }
            else
            {
                return NotFound();
            } 
        }

        [HttpPost("{id}")]
        public ActionResult<CharacterTagModel> Delete(Guid id)
        {
            var result = _dbCharacterTagService.Delete(id);
            if (result)
                return NoContent();
            else
                return NotFound();
        }
    }
}
