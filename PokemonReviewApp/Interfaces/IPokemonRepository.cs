using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        IEnumerable<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);

        Pokemon GetPokemon(string name);

        decimal GetPokemonRating(int id);

        bool PokemonExists(int id);

        bool CreatePokemon(Pokemon pokemon);

        bool UpdatePokemon(Pokemon pokemon);

        bool DeletePokemon(Pokemon pokemon);

        bool Save();
    }
}
