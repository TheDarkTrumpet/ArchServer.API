namespace libKimai.query
{
    public class Base
    {
        protected readonly string _mysqlConnectionString; 
        public Base(string queryString)
        {
            _mysqlConnectionString = queryString;
        }
    }
}