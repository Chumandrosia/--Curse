using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Ласкаво просимо до API магазину взуття!",
                Version = "1.0.0",
                Endpoints = new[]
                {
                    "GET /api/shoes - Отримати всі взуття",
                    "GET /api/shoes/{id} - Отримати взуття за ID",
                    "POST /api/shoes - Створити нове взуття",
                    "PUT /api/shoes/{id} - Оновити взуття",
                    "DELETE /api/shoes/{id} - Видалити взуття",
                    "GET /api/shoes/search?term={term} - Пошук взуття",
                    "GET /api/shoes/brand/{brand} - Взуття за брендом",
                    "GET /api/shoes/category/{category} - Взуття за категорією"
                }
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}