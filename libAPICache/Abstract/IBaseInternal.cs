using libAPICache.Models;

namespace libAPICache.Abstract
{
    internal interface IBaseInternal<T, T1> where T : Base, new() where T1 : class
    {
        T UpdateEnumerables(T1 source, T destination);
         void UpdateEntityData(T destination, T source);
    }
}