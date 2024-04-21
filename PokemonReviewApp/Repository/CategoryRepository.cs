using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id) => _context.Categories.Any(c => c.Id == id);

        public ICollection<Category> GetCategories() => _context.Categories.ToList();

        public Category? GetCategory(int id) => _context.Categories.Where(c => c.Id == id).FirstOrDefault();

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId) => _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(c => c.Pokemon).ToList();

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public bool Save()
        {
            int res = _context.SaveChanges();
            return res > 0 ? true : false;
        }
    }
}
