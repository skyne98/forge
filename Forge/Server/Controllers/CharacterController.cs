using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forge.Server.Data;
using Forge.Shared.Data;
using Forge.Shared.Filters;
using Forge.Shared.ViewModels;
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
        public IEnumerable<CharacterModel> Get(bool includeDeleted = false)
        {
            return _dbCharacterService.FindAll(includeDeleted)
                .OrderBy(character => character.Name);
        }

        [HttpGet]
        public ActionResult<CharacterModel> GetOne(Guid id, bool includeDeleted = false)
        {
            var result = _dbCharacterService.FindOne(id, includeDeleted);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult GetFiltered(CharacterFilter filter)
        {
            var characters = _dbCharacterService.FindAll();
            var total = characters.Count();

            // Order
            characters = characters.OrderBy(character => character.Name);

            var result = characters.Where(character =>
            {
                var result = true;
                if (string.IsNullOrEmpty(filter.Name) == false)
                {
                    if (character.Name.ToLower().Contains(filter.Name.ToLower()) == false)
                    {
                        result = false;
                    }
                }
                if (filter.Tags != null && filter.Tags.Count > 0)
                {
                    foreach (var tag in filter.Tags)
                    {
                        if (character.Tags.Any(charTag => charTag.Id == tag) == false)
                        {
                            result = false;
                        }
                    }
                }

                return result;
            });

            var filtered = result.Count();

            if (filter.Skip.HasValue)
                result = result.Skip(filter.Skip.Value);
            if (filter.Take.HasValue)
                result = result.Take(filter.Take.Value);

            return Ok(new CharacterFiltered()
            {
                Models = result.ToList(),
                Filtered = filtered,
                Total = total
            });
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
        public ActionResult<bool> DeleteRange([FromBody] Guid[] ids)
        {
            var result = _dbCharacterService.DeleteRange(ids);
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
                return Ok(result);
            }
            else
            {
                return NotFound();
            } 
        }

        [HttpPost]
        public ActionResult<CharacterModel> RestoreRange([FromBody] Guid[] ids)
        {
            var result = _dbCharacterService.RestoreRange(ids);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            } 
        }
    }
}
