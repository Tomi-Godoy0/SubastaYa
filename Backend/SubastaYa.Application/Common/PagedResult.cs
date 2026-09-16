namespace SubastaYa.Application.Common
{
    public class PagedResult<T>
    {
        //Vamos a usar para la páginación
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
