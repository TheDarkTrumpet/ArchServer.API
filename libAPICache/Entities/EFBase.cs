using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libAPICache.Models;
using libAPICache.util;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public class EFBase<T> where T : Base
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
            var inputType = input.GetType();
            var newEntry = Activator.CreateInstance(inputType);
            newEntry.Copy(input);

            SaveEntry(newEntry, true);
            return true;
        }

        public bool SaveEntry(Object input, bool saveChanges)
        {
            var type = input.GetType();
            long id = (long) type.GetProperty("Id").GetValue(input);
            var srcEntry = GetOrReturnNull(id);

            if (srcEntry != null)
            {
                _context.Entry(srcEntry).CurrentValues.SetValues(input);
            }
            else
            {
                _dbSet.Add((T) input);
            }

            if (saveChanges)
            {
                _context.SaveChanges();
            }

            return true;
        }

        public Object GetOrReturnNull(long id)
        {
            return Entries.FirstOrDefault(x => x.Id == id);
        }
    }
}