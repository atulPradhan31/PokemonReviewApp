using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Reviewer GetReviewer(int reviewerId) => _context.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        
        public ICollection<Reviewer> GetReviewers() => _context.Reviewers.ToList();

        public ICollection<Review> GetReviewsByReviewer(int reviewerId) => _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();

        public bool ReviewerExists(int id) => _context.Reviewers.Any(r => r.Id == id);

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public bool Save()
        {
            int res = _context.SaveChanges();
            return res > 0 ? true : false;
        }
    }
}
