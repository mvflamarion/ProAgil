using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestrantesController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public PalestrantesController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllPalestrantesAsync(true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("{palestranteId}")]
        public async Task<IActionResult> Get(int palestranteId)
        {
            try
            {
                var results = await _repo.GetPalestranteAsyncById(palestranteId, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByNome/{nome}")]
        public async Task<IActionResult> Get(string nome)
        {
            try
            {
                var results = await _repo.GetAllPalestrantesAsyncByNome(nome, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                _repo.Add(model);

                if (await _repo.SaveChangesAsync())
                    return Created($"/api/palestrantes/{model.Id}", model);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await _repo.GetPalestranteAsyncById(palestranteId);
                if (palestrante == null) return NotFound();

                _repo.Add(model);

                if (await _repo.SaveChangesAsync())
                    return Created($"/api/Palestrantes/{model.Id}", model);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try
            {
                var palestrante = await _repo.GetPalestranteAsyncById(palestranteId);
                if (palestrante == null) return NotFound();

                _repo.Add(palestrante);

                if (await _repo.SaveChangesAsync())
                    return NoContent();
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }
    }
}