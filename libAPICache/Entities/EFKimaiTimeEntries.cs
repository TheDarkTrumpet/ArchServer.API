using libAPICache.Abstract;

namespace libAPICache.Entities
{
    public class EFKimaiTimeEntries : IKimaiTimeEntries
    {
        private readonly EFDbContext _context;
        public EFKimaiTimeEntries() : this(new EFDbContext()) { }

        public EFKimaiTimeEntries(EFDbContext context)
        {
            _context = context;
        }
    }
}