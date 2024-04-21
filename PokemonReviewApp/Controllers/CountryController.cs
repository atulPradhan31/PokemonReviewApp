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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository,  IMapper mapper)
        {
            _countryRepository = countryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            IEnumerable<CountryDTO> countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            CountryDTO country = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);

        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type=typeof(Country))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            if(!_ownerRepository.OwnerExists(ownerId))
               return NotFound();

            CountryDTO country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owner/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersFromACountry(int countryId)
        {
            IEnumerable<OwnerDTO> owners = _mapper.Map<List<OwnerDTO>>(_countryRepository.GetOwnersFromACountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO createCountry)
        {
            if (createCountry == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == createCountry.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Country countryMap = _mapper.Map<Country>(createCountry);
            if(!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving country");
                return StatusCode(500, ModelState);
            }

            return Ok("Country added successfully");

        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO country)
        {
            if (countryId == null)
                return BadRequest(ModelState);

            if (country.Id != countryId)
            {
                ModelState.AddModelError("", "Invalid country id");
                return NotFound(ModelState);
            }

            Country countryMap = _mapper.Map<Country>(country);
            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating country model!");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (countryId == null)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
            {
                ModelState.AddModelError("", "No id matches with the passed id");
                return NotFound(ModelState);
            }

            Country country = _countryRepository.GetCountry(countryId);
            if (country == null)
                return StatusCode(500, "Something went wrong. Try again later");

            if (!_countryRepository.DeleteCountry(country))
                return StatusCode(500, "something went wrong while removing the country model");

            return NoContent();
        }

    }
}
