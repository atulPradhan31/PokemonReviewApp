using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Pokemon> GetPokemons() => _context.Pokemons.ToList();

        public Pokemon? GetPokemon(int id) => _context.Pokemons.FirstOrDefault(p => p.Id == id); 

        public Pokemon? GetPokemon(string name) => _context.Pokemons.FirstOrDefault(x => x.Name == name);

        public decimal GetPokemonRating(int id)
        {
            ICollection<Review> reviews = _context.Reviews.Where(review => review.Id == id).ToList();

            if(reviews.Count == 0)
                return 0;

            return reviews.Sum(review => review.Rating)/reviews.Count();
        }

        public bool PokemonExists(int id) => _context.Pokemons.Any(p => p.Id == id);


        public bool CreatePokemon(Pokemon pokemon)
        {
            _context.Add(pokemon);
            return Save();
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public bool Save()
        {
            int res = _context.SaveChanges();
            return res > 0 ? true : false;
        }
    }
}
