using Core.Models;
using Core.Specifications;

namespace Core.Specifications
{
    public class ProductsWithCategoriesSpecification : BaseSpecification<Product>
    {
        // Retrieve all products with their categories
        public ProductsWithCategoriesSpecification()
        {
            AddInclude(p => p.Category);
            AddOrderBy(p => p.Name);
        }

        // Retrieve a specific product by ID with its category
        public ProductsWithCategoriesSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.Category);
        }

        // Example with pagination
        public ProductsWithCategoriesSpecification(int skip, int take)
        {
            AddInclude(p => p.Category);
            ApplyPaging(skip, take);
        }
    }
}
