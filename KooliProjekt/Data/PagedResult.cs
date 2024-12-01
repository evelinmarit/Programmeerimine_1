using System.Collections;

namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase, IEnumerable<T> where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Results).GetEnumerator();
        }
    }
}