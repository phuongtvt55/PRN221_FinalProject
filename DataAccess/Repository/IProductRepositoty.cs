using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Repository
{
    public interface IProductRepositoty
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductByBrand(int id);

        IEnumerable<Product> GetProductListAdmin();
        IEnumerable<Product> Search(string name);
        Product GetProductById(int productId);

        void Insert(Product product);

        void Update(Product product);
        
        void Delete(int productId);   
    }
}
