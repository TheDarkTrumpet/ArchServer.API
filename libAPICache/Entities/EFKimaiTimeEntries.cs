using System.Collections.Generic;
using System.Linq;
using libAPICache.Abstract;
using libAPICache.Models.Kimai;
using libAPICache.util;
using libKimai.models;

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

        public IEnumerable<TimeEntry> TimeEntries => _context.KimaiTimeEntries;

        public bool SaveEntry(Activity timeEntry)
        {
            TimeEntry saveTimeEntry = new TimeEntry();

            saveTimeEntry.Copy(timeEntry);
            SaveEntry(saveTimeEntry);

            return true;
        }

        public bool SaveEntry(TimeEntry timeEntry)
        {
            TimeEntry srcEntry = GetOrReturnNull(timeEntry.Id);

            if (srcEntry == null)
            {
                
            }
        }

        public TimeEntry GetOrReturnNull(int id)
        {
            return TimeEntries.FirstOrDefault(x => x.Id == id);
        }
    }
}