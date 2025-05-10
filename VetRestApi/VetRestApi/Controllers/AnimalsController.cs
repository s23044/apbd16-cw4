using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetRestApi.Models;

namespace VetRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private static int _nextId = 1;

        private static readonly List<Animals> _animals = new()
        {
            new Animals { Id = _nextId++, Name = "Reksio", Category = "Pies", Weight = 12.4f, FurColor = "Rudy" },
            new Animals { Id = _nextId++, Name = "Filemon", Category = "Kot", Weight = 3.2f, FurColor = "Szary" },
            new Animals { Id = _nextId++, Name = "Gucio", Category = "Chomik", Weight = 0.4f, FurColor = "Biało-brązowy" },
            new Animals { Id = _nextId++, Name = "Bąbel", Category = "Żółw", Weight = 1.1f, FurColor = "Zielony" },
            new Animals { Id = _nextId++, Name = "Perełka", Category = "Pies", Weight = 8.9f, FurColor = "Czarny" }
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(_animals);

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            return animal is not null ? Ok(animal) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Animals animal)
        {
            var newAnimal = new Animals
            {
                Id = _nextId++,
                Name = animal.Name,
                Category = animal.Category,
                Weight = animal.Weight,
                FurColor = animal.FurColor
            };
            _animals.Add(newAnimal);
            return CreatedAtAction(nameof(GetById), new { id = newAnimal.Id }, newAnimal);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Animals updated)
        {
            var existing = _animals.FirstOrDefault(a => a.Id == id);
            if (existing is null) return NotFound();

            existing.Name = updated.Name;
            existing.Category = updated.Category;
            existing.Weight = updated.Weight;
            existing.FurColor = updated.FurColor;

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            if (animal is null) return NotFound();

            _animals.Remove(animal);
            return Ok(animal);
        }

        [HttpGet("search/{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _animals.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return results.Count > 0 ? Ok(results) : NotFound();
        }
    }
}
