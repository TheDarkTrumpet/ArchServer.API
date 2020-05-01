using System.Collections.Generic;
using libAPICache.Models;

namespace libAPICache.Abstract
{
    public interface IBase<T, T1> where T : Base, new() where T1 : class
    {
        IEnumerable<T> Entries { get; set; }
        T SaveEntry(T1 input, bool saveChanges = true);
        T SaveEntry(T input, bool saveChanges = true);
        List<T> SaveEntries(List<T1> entries, bool saveChanges = true);
        T GetOrReturnNull(long id);
    }
}