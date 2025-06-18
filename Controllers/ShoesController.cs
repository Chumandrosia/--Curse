using Microsoft.AspNetCore.Mvc;
using ShoeStore.Models;
using ShoeStore.Services;

namespace ShoeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoesController : ControllerBase
    {
        private readonly IShoeService _shoeService;
        private readonly ILogger<ShoesController> _logger;

        public ShoesController(IShoeService shoeService, ILogger<ShoesController> logger)
        {
            _shoeService = shoeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shoe>>> GetShoes()
        {
            try
            {
                var shoes = await _shoeService.GetAllShoesAsync();
                return Ok(shoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні списку взуття");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shoe>> GetShoe(int id)
        {
            try
            {
                var shoe = await _shoeService.GetShoeByIdAsync(id);
                if (shoe == null)
                {
                    return NotFound($"Взуття з ID {id} не знайдено");
                }
                return Ok(shoe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні взуття з ID {Id}", id);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Shoe>> CreateShoe([FromBody] Shoe shoe)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdShoe = await _shoeService.CreateShoeAsync(shoe);
                return CreatedAtAction(nameof(GetShoe), new { id = createdShoe.Id }, createdShoe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при створенні взуття");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoe(int id, [FromBody] Shoe shoe)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _shoeService.UpdateShoeAsync(id, shoe);
                if (!updated)
                {
                    return NotFound($"Взуття з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при оновленні взуття з ID {Id}", id);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoe(int id)
        {
            try
            {
                var deleted = await _shoeService.DeleteShoeAsync(id);
                if (!deleted)
                {
                    return NotFound($"Взуття з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при видаленні взуття з ID {Id}", id);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Shoe>>> SearchShoes([FromQuery] string term)
        {
            try
            {
                var shoes = await _shoeService.SearchShoesAsync(term);
                return Ok(shoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при пошуку взуття");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<Shoe>>> GetShoesByBrand(string brand)
        {
            try
            {
                var shoes = await _shoeService.GetShoesByBrandAsync(brand);
                return Ok(shoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні взуття бренду {Brand}", brand);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Shoe>>> GetShoesByCategory(string category)
        {
            try
            {
                var shoes = await _shoeService.GetShoesByCategoryAsync(category);
                return Ok(shoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні взуття категорії {Category}", category);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            try
            {
                if (quantity < 0)
                {
                    return BadRequest("Кількість не може бути від'ємною");
                }

                var updated = await _shoeService.UpdateStockAsync(id, quantity);
                if (!updated)
                {
                    return NotFound($"Взуття з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при оновленні кількості взуття з ID {Id}", id);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }
    }
}