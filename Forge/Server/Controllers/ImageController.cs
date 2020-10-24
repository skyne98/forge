using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using System.Linq;
using System.Threading.Tasks;
using Forge.Server.Data;
using Forge.Shared.Data;
using Forge.Shared.Filters;
using Forge.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using Forge.Shared.Requests;

namespace Forge.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageController: ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IDbImageRepository _dbImageRepository;

        public ImageController(ILogger<ImageController> logger, IDbImageRepository dbImageRepository)
        {
            _dbImageRepository = dbImageRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Download(Guid id, int? width, int? height, string resize)
        {
            var file = _dbImageRepository.FindOne(id);
            if (file == null)
            {
                return NotFound();
            }

            var content = _dbImageRepository.GetContent(file.Id);

            if (width.HasValue && height.HasValue)
            {
                var resizeMode = ResizeMode.Max;
                if (resize == "stretch")
                    resizeMode = ResizeMode.Stretch;
                if (resize == "crop")
                    resizeMode = ResizeMode.Crop;
                if (resize == "pad")
                    resizeMode = ResizeMode.Pad;
                var resizeOptions = new ResizeOptions()
                {
                    Size = new Size(width.Value, height.Value),
                    Compand = true,
                    Mode = resizeMode
                };

                var image = Image.Load(content);
                image.Mutate(x => x.Resize(resizeOptions));
                var afterMutations = image.Size();

                //Encode here for quality
                var encoder = new PngEncoder();
                var resultStream = new MemoryStream();
                image.Save(resultStream, encoder);
                resultStream.Position = 0;

                return File(resultStream, "image/png");
            }
            else
                return File(content, file.Type);
        }

        [HttpGet]
        public IEnumerable<ImageModel> Get(bool includeDeleted = false)
        {
            return _dbImageRepository.FindAll(includeDeleted)
                .OrderBy(character => character.Title);
        }

        [HttpGet]
        public ActionResult<ImageModel> GetOne(Guid id, bool includeDeleted = false)
        {
            var result = _dbImageRepository.FindOne(id, includeDeleted);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult GetFiltered(ImageFilter filter)
        {
            var tags = _dbImageRepository.FindAll();
            var total = tags.Count();

            // Order
            if (filter.Sorting == "Id")
                if (filter.SortingDirection == SortingDirection.Ascending)
                    tags = tags.OrderBy(tag => tag.Id);
                else
                    tags = tags.OrderByDescending(tag => tag.Id);
            else if (filter.Sorting == "Title")
                if (filter.SortingDirection == SortingDirection.Ascending)
                    tags = tags.OrderBy(tag => tag.Title);
                else
                    tags = tags.OrderByDescending(tag => tag.Title);
            else
                tags = tags.OrderBy(tag => tag.Title);

            var result = tags.Where(tag => {
                    var result = true;
                    if (string.IsNullOrEmpty(filter.Title) == false)
                    {
                        if (tag.Title.ToLower().Contains(filter.Title.ToLower()) == false)
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

            return Ok(new ImageFiltered()
            {
                Models = result.ToList(),
                Filtered = filtered,
                Total = total
            });
        }

        [HttpPost]
        public ActionResult<ImageModel> Insert(ImageRequest request)
        {
            if (string.IsNullOrEmpty(request.ContentBase64))
            {
                throw new ArgumentNullException("No content of the image was provided");
            }

            var id = _dbImageRepository.Insert(request.Model, request.ContentBase64);
            if (id != default)
                return CreatedAtAction("GetOne", _dbImageRepository.FindOne(id));
            else
                return BadRequest();
        }

        [HttpPost]
        public ActionResult<ImageModel> Update(ImageRequest request)
        {
            var result = _dbImageRepository.Update(request.Model, request.ContentBase64);
            if (result)
                return NoContent();
            else
                return NotFound();
        }
        
        [HttpPost]
        public ActionResult<ImageModel> DeleteOne([FromBody] Guid id)
        {
            var result = _dbImageRepository.DeleteOne(id);
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
        public ActionResult<ImageModel> RestoreOne([FromBody] Guid id)
        {
            var result = _dbImageRepository.RestoreOne(id);
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
