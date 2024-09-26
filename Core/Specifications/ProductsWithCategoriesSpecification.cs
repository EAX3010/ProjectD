using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;
namespace Core.Specifications
{
    public class ProductsWithCategoriesSpecification : BaseSpecification<Product>
    {
        public ProductsWithCategoriesSpecification()
        {
            AddInclude(x => x.Category);
        }

        public ProductsWithCategoriesSpecification(int id) : 
            base(x => x.Id == id)
        {
            AddInclude(x => x.Category);
        }
    }
}
