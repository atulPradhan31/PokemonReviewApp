using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Owner? GetOwner(int ownerId) => _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();

        public ICollection<Owner>? GetOwnerOfAPokemon(int pokeId) => _context.PokemonOwners.Where(po => po.Pokemon.Id == pokeId).Select(p => p.Owner).ToList();

        public ICollection<Owner> GetOwners() => _context.Owners.ToList();

        public ICollection<Pokemon>? GetPokemonsByOwner(int ownerId) => _context.PokemonOwners.Where(po => po.Owner.Id == ownerId).Select(o => o.Pokemon).ToList();

        public bool OwnerExists(int id) => _context.Owners.Any(o => o.Id == id);

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public bool Save()
        {
            int res = _context.SaveChanges();
            return res > 0 ? true : false;
        }
    }
}
