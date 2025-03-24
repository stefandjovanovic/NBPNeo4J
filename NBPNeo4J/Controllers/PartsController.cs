    using Microsoft.AspNetCore.Mvc;
using NBPNeo4J.DTOs;
using NBPNeo4J.Services;

namespace NBPNeo4J.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartsController : ControllerBase
    {
        private readonly ILogger<PartsController> _logger;
        private readonly IPartsService _partsService;
        public PartsController(ILogger<PartsController> logger, IPartsService service)
        {
            _logger = logger;
            _partsService = service;
        }
  
        [HttpGet("parts")]
        public async Task<ActionResult<List<ReturnPartDTO>>> GetAllParts()
        {
            var parts = await _partsService.GetAllParts();
            return Ok(parts);
        }

  
        [HttpGet("parts/category/{categoryId}")]
        public async Task<ActionResult<List<ReturnPartDTO>>> GetAllPartsOfCategory(string categoryId)
        {
            var parts = await _partsService.GetAllPartsOfCategory(categoryId);
            return Ok(parts);
        }

   
        [HttpPost("parts")]
        public async Task<ActionResult<ReturnPartDTO>> CreatePart([FromBody] CreatePartDTO createPartDTO)
        {
            var createdPart = await _partsService.CreatePart(createPartDTO);
            return CreatedAtAction(nameof(GetAllParts), new { serialCode = createdPart.SerialCode }, createdPart);
        }

        [HttpPut("parts/{serialCode}")]
        public async Task<ActionResult<ReturnPartDTO>> UpdatePart(string serialCode, [FromBody] CreatePartDTO createPartDTO)
        {
            var updatedPart = await _partsService.UpdatePart(serialCode, createPartDTO);
            return Ok(updatedPart);
        }


        [HttpDelete("parts/{serialCode}")]
        public async Task<IActionResult> DeletePart(string serialCode)
        {
            await _partsService.DeletePart(serialCode);
            return NoContent();
        }


        [HttpGet("categories")]
        public async Task<ActionResult<List<PartCategoryDTO>>> GetAllPartCategories()
        {
            var categories = await _partsService.GetAllPartCategories();
            return Ok(categories);
        }

     
        [HttpGet("categories/{categoryId}")]
        public async Task<ActionResult<PartCategoryDTO>> GetPartCategory(string categoryId)
        {
            var category = await _partsService.GetPartCategory(categoryId);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

      
        [HttpPost("categories")]
        public async Task<ActionResult<PartCategoryDTO>> CreateCategory([FromBody] PartCategoryDTO createCategoryDTO)
        {
            var createdCategory = await _partsService.CreateCategory(createCategoryDTO);
            return CreatedAtAction(nameof(GetPartCategory), new { categoryId = createdCategory.Id }, createdCategory);
        }


        [HttpPut("categories/{categoryId}")]
        public async Task<ActionResult<PartCategoryDTO>> UpdateCategory(string categoryId, [FromBody] PartCategoryDTO createCategoryDTO)
        {
            var updatedCategory = await _partsService.UpdateCategory(categoryId, createCategoryDTO);
            return Ok(updatedCategory);
        }

     
        [HttpDelete("categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            await _partsService.DeleteCategory(categoryId);
            return NoContent();
        }
    }
}
