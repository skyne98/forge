﻿using System;
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
    public class CharacterTagController: ControllerBase
    {
        private readonly ILogger<CharacterTagController> _logger;
        private readonly IDbCharacterTagRepository _dbCharacterTagService;

        public CharacterTagController(ILogger<CharacterTagController> logger, IDbCharacterTagRepository dbCharacterTagService)
        {
            _dbCharacterTagService = dbCharacterTagService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CharacterTagModel> Get(bool includeDeleted = false)
        {
            return _dbCharacterTagService.FindAll(includeDeleted)
                .OrderBy(character => character.Name);
        }

        [HttpGet]
        public ActionResult<CharacterTagModel> GetOne(Guid id, bool includeDeleted = false)
        {
            var result = _dbCharacterTagService.FindOne(id, includeDeleted);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult GetFiltered(CharacterTagFilter filter)
        {
            var tags = _dbCharacterTagService.FindAll();
            var total = tags.Count();

            // Order
            if (filter.Sorting == "Id")
                if (filter.SortingDirection == SortingDirection.Ascending)
                    tags = tags.OrderBy(tag => tag.Id);
                else
                    tags = tags.OrderByDescending(tag => tag.Id);
            else if (filter.Sorting == "Name")
                if (filter.SortingDirection == SortingDirection.Ascending)
                    tags = tags.OrderBy(tag => tag.Name);
                else
                    tags = tags.OrderByDescending(tag => tag.Name);
            else
                tags = tags.OrderBy(tag => tag.Name);

            var result = tags.Where(tag => {
                    var result = true;
                    if (string.IsNullOrEmpty(filter.Name) == false)
                    {
                        if (tag.Name.ToLower().Contains(filter.Name.ToLower()) == false)
                        {
                            result = false;
                        }
                    }

                    return result;
                });

            var filtered = result.Count();

            if (filter.Skip.HasValue)
                result = result.Skip(filter.Skip.Value);
            if (filter.Take.HasValue)
                result = result.Take(filter.Take.Value);

            return Ok(new CharacterTagFiltered()
            {
                Models = result.ToList(),
                Filtered = filtered,
                Total = total
            });
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
            var result = _dbCharacterTagService.RestoreOne(id);
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
