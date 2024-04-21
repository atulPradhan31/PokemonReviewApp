using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool CountryExists(int countryId) => _context.Countries.Any(c => c.Id == countryId);

        public ICollection<Country> GetCountries() => _context.Countries.ToList();

        public Country? GetCountry(int id) => _context.Countries.Where(c => c.Id == id).FirstOrDefault();

        public Country? GetCountryByOwner(int ownerId) => _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();

        public ICollection<Owner> GetOwnersFromACountry(int countryId) => _context.Owners.Where(o => o.Country.Id == countryId).ToList();

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public bool Save()
        {
            int res = _context.SaveChanges();
            return res > 0 ? true : false;
        }
    }
}
