using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepositoty
    {
        public IEnumerable<Product> GetProducts()
        {
            return ProductDAO.Instance.GetProductList();
        }

        public IEnumerable<Product> GetProductByBrand(int id)
        {
            return ProductDAO.Instance.GetProductByBrand(id);
        }

        public IEnumerable<Product> GetProductListAdmin()
        {
            return ProductDAO.Instance.GetProductListAdmin();
        }

        public IEnumerable<Product> Search(string name)
        {
            return ProductDAO.Instance.Search(name);
        }

        public Product GetProductById(int productId)
        {
            return ProductDAO.Instance.GetProductByID(productId);
        }

        public void Insert(Product product)
        {
            ProductDAO.Instance.AddNew(product);
        }

        public void Update(Product product)
        {
            ProductDAO.Instance.Update(product);
        }

        public void Delete(int productId)
        {
            ProductDAO.Instance.Remove(productId);
        }


    }
}
