using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            IEnumerable<PokemonDTO> pokemons =  _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);

        }

        [HttpGet("pokeId")]
        [ProducesResponseType(200, Type = typeof(Pokemon) )]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            PokemonDTO pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

        }


        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            decimal rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] PokemonDTO pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemon = _pokemonRepository.GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId, [FromBody] PokemonDTO pokemon)
        {
            if (pokemonId == null)
                return BadRequest(ModelState);

            if (pokemon.Id != pokemonId)
            {
                ModelState.AddModelError("", "Invalid owner id");
                return NotFound(ModelState);
            }

            Pokemon pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating pokemon model!");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (pokemonId == null)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                ModelState.AddModelError("", "No id matches with the passed id");
                return NotFound(ModelState);
            }

            Pokemon pokemon = _pokemonRepository.GetPokemon(pokemonId);
            if (pokemon == null)
                return StatusCode(500, "Something went wrong. Try again later");

            if (!_pokemonRepository.DeletePokemon(pokemon))
                return StatusCode(500, "something went wrong while removing the pokemon model");

            return NoContent();
        }
    }
}
