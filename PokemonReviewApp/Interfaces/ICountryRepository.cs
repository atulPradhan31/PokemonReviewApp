using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();

        Country GetCountry(int id);

        bool CountryExists(int countryId);

        Country GetCountryByOwner(int ownerId);

        ICollection<Owner> GetOwnersFromACountry(int countryId);

        bool CreateCountry(Country coutry);

        bool UpdateCountry(Country coutry);

        bool DeleteCountry(Country coutry);

        bool Save();
    }
}
