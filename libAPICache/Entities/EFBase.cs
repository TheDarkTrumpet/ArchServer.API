using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using libAPICache.Abstract;
using libAPICache.Models;
using libAPICache.util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using IConfiguration = libAPICache.util.IConfiguration;

namespace libAPICache.Entities
{
    public abstract class EFBase<T,T1> : IBaseInternal<T,T1>, IBase<T,T1>  where T: Base, new() where T1 : class
    {
        protected readonly EFDbContext Context;
        protected DbSet<T> DbSet;
        private T1 _cachedInput;
        protected IConfiguration Configuration;
        
        public IEnumerable<T> Entries { get; set; }

        public EFBase() : this(new EFDbContext(), new Configuration())
        {
        }

        public EFBase(EFDbContext context, IConfiguration configuration)
        {
            Context = context;
        }

        public virtual T SaveEntry(T1 input, bool saveChanges = true)
        {
            _cachedInput = input;
            var newEntry = new T();
            newEntry.Copy(input);

            return SaveEntry(newEntry, saveChanges);
        }

        public virtual T SaveEntry(T input, bool saveChanges = true)
        {
            var srcEntry = GetOrReturnNull(input.Id);

            if (srcEntry != null)
            {
                UpdateEntityData(srcEntry, input);

                if (srcEntry.GetEnumerables().Any())
                {
                    UpdateEnumerables(_cachedInput, srcEntry);
                }
            }
            else
            {
                DbSet.Add(input);
            }

            if (saveChanges)
            {
                Context.SaveChanges();
            }

            return input;
        }

        public virtual List<T> SaveEntries(List<T1> entries, bool saveChanges = true)
        {
            List<T> savedEntries = new List<T>();
            foreach (var te in entries)
            {
                savedEntries.Add(SaveEntry(te, false));
            }

            if (saveChanges)
            {
                Context.SaveChanges();
            }
            return savedEntries;
        }
        
        public virtual T GetOrReturnNull(long id)
        {
            return Entries.FirstOrDefault(x => x.Id == id);
        }

        public virtual string GetAPIKey(string identifier)
        {
            
            IConfiguration config = util.Configuration.GetConfiguration();
            string apiKey = (string) config[identifier];
            
            if (String.IsNullOrEmpty(apiKey))
            {
                throw new Exception(
                    $"{@Directory.GetCurrentDirectory()}/appsettings.json entry does not exist for {identifier}, please make sure it's defined!");
            }

            return apiKey;
        }

        public virtual T UpdateEnumerables(T1 source, T destination)
        {
            throw new Exception("This method must be implemented in the derived class!");
            // return destination
        }

        public virtual void UpdateEntityData(T destination, T source)
        {
            Context.Entry(destination).CurrentValues.SetValues(source);
        }
    }
}