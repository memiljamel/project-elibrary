using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(ELibraryContext context) : base(context)
        {
        }

        public async Task<IPagedList<Member>> GetPagedMembersWithPhones(string? search, int pageNumber, int pageSize = 15)
        {
            var query = _context.Members
                .Include(m => m.Phones)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.MemberNumber.ToLower().Contains(search.ToLower()) ||
                                         m.Name.ToLower().Contains(search.ToLower()) ||
                                         m.Email.ToLower().Contains(search.ToLower()) ||
                                         m.Phones.Any(p => p.PhoneNumber.Contains(search)) ||
                                         m.Address.ToLower().Contains(search.ToLower()));
            }

            return await query.OrderByDescending(e => e.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Member?> GetMemberWithPhonesById(Guid? id)
        {
            return await _context.Members
                .Include(m => m.Phones)
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public bool IsMemberNumberUnique(string memberNumber, Guid? id)
        {
            return !_context.Members.Any(m => m.MemberNumber == memberNumber && m.ID != id);
        }
    }
}