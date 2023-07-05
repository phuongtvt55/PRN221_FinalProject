using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetProductList()
        {
            var products = new List<Product>();
            try
            {
                using var context = new ShoesStoreContext();
                products = context.Products.Include(b => b.Brand).Where(s => s.Status == 1 && s.Quantity > 0 && s.Brand.Status == 1).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return products;
        }

        public IEnumerable<Product> GetProductListAdmin()
        {
            var products = new List<Product>();
            try
            {
                using var context = new ShoesStoreContext();
                products = context.Products.Include(b => b.Brand).ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return products;
        }

        public Product GetProductByID(int productId)
        {
            Product product = null;
            try
            {
                using var context = new ShoesStoreContext();
                product = context.Products.Include(b => b.Brand).SingleOrDefault(c => c.ProductId == productId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return product;
        }

        public IEnumerable<Product> GetProductByBrand(int brandId)
        {
            var products = new List<Product>();
            try
            {
                using var context = new ShoesStoreContext();
                products = context.Products.Include(b => b.Brand).Where(b => b.BrandId == brandId && b.Status == 1).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return products;
        }

        public IEnumerable<Product> Search(string name)
        {
            var products = new List<Product>();
            try
            {
                using var context = new ShoesStoreContext();
                products = context.Products.Include(b => b.Brand).Where(b => b.Name.Contains(name) && b.Status == 1).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return products;
        }

        public void AddNew(Product product)
        {
            try
            {
                Product _Product = GetProductByID(product.ProductId);
                if (_Product == null)
                {
                    using var context = new ShoesStoreContext();
                    context.Products.Add(product);
                    
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product is already exist.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void Update(Product product)
        {
            try
            {
                Product _Product = GetProductByID(product.ProductId);
                if (_Product != null)
                {
                    using var context = new ShoesStoreContext();
                    context.Products.Update(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int productId)
        {
            try
            {
                Product product = GetProductByID(productId);
                if (product != null)
                {
                    using var context = new ShoesStoreContext();
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
