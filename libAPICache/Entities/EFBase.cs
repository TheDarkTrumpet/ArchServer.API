using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libAPICache.Abstract;
using libAPICache.Models;
using libAPICache.util;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public abstract class EFBase<T,T1> : IBase<T,T1>  where T: Base, new() where T1 : class
    {
        protected readonly EFDbContext _context;
        protected DbSet<T> _dbSet;
        public IEnumerable<T> Entries { get; set; }

        public EFBase() : this(new EFDbContext())
        {
        }

        public EFBase(EFDbContext context)
        {
            _context = context;
        }

        public bool SaveEntry(T1 input)
        {
            var newEntry = new T();
            newEntry.Copy(input);

            SaveEntry(newEntry, true);
            return true;
        }

        public bool SaveEntry(T input, bool saveChanges = true)
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

        public bool SaveEntries(List<T1> entries)
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