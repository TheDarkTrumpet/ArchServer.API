using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libAPICache.Models;
using libAPICache.util;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public class EFBase<T> where T : Base, new()
    {
        protected readonly EFDbContext _context;
        protected DbSet<T> _dbSet;
        public IEnumerable<T> Entries;

        public EFBase() : this(new EFDbContext())
        {
        }

        public EFBase(EFDbContext context)
        {
            _context = context;
        }

        public bool SaveEntry(Object input)
        {
            var newEntry = new T();
            newEntry.Copy(input);

            SaveEntry(newEntry, true);
            return true;
        }

        public bool SaveEntry(T input, bool saveChanges)
        {
            var srcEntry = GetOrReturnNull(input.Id);

            if (srcEntry != null)
            {
                _context.Entry(srcEntry).CurrentValues.SetValues(input);
            }
            else
            {
                _dbSet.Add(input);
            }

            if (saveChanges)
            {
                _context.SaveChanges();
            }

            return true;
        }

        public bool SaveEntries(List<Object> entries)
        {
            bool result = false;
            foreach (var te in entries)
            {
                result = SaveEntry(te);
            }

            return result;
        }
        
        public T GetOrReturnNull(long id)
        {
            return Entries.FirstOrDefault(x => x.Id == id);
        }
    }
}