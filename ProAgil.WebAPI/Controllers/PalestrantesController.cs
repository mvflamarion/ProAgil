using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.ViewModels;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestrantesController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;

        public PalestrantesController(IProAgilRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var palestrantes = await _repo.GetAllPalestrantesAsync(true);
                var results = _mapper.Map<IEnumerable<PalestranteVm>>(palestrantes);
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
                var palestrante = await _repo.GetPalestranteAsyncById(palestranteId, true);
                var results = _mapper.Map<PalestranteVm>(palestrante);
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
                var palestrante = await _repo.GetAllPalestrantesAsyncByNome(nome, true);
                var results = _mapper.Map<PalestranteVm>(palestrante);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteVm model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                _repo.Add(palestrante);

                if (await _repo.SaveChangesAsync())
                    return Created($"/api/palestrantes/{model.Id}", _mapper.Map<PalestranteVm>(palestrante));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, PalestranteVm model)
        {
            try
            {
                var palestrante = await _repo.GetPalestranteAsyncById(palestranteId);
                if (palestrante == null) return NotFound();

                _mapper.Map(model, palestrante);

                _repo.Add(palestrante);

                if (await _repo.SaveChangesAsync())
                    return Created($"/api/Palestrantes/{model.Id}", _mapper.Map<PalestranteVm>(palestrante));
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