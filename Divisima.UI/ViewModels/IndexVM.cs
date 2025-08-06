using Divisima.DAL.Entities;

namespace Divisima.UI.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<Slide> Slide { get; set; }
        public IEnumerable<Product> LatestProduct { get; set; }
        public IEnumerable<Product> SalesProduct { get; set; }
    }
}
