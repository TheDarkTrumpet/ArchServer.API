using System.Collections.Generic;
using libAPICache.Models;

namespace libAPICache.Abstract
{
    public interface IBase<T, T1> where T : Base, new() where T1 : class
    {
        IEnumerable<T> Entries { get; set; }
        bool SaveEntry(T1 input);
        bool SaveEntry(T input, bool saveChanges = true);
        bool SaveEntries(List<T1> entries);
        T GetOrReturnNull(long id);
    }
}