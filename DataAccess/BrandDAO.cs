using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BrandDAO
    {
        private static BrandDAO instance = null;
        private static readonly object instanceLock = new object();
        public static BrandDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BrandDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Brand> GetBrandList()
        {
            var brand = new List<Brand>();
            try
            {
                using var context = new ShoesStoreContext();
                brand = context.Brands.Where(s => s.Status == 1).ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return brand;
        }

        public Brand GetBrandByID(int brandId)
        {
            Brand brand = null;
            try
            {
                using var context = new ShoesStoreContext();
                brand = context.Brands.SingleOrDefault(c => c.BrandId == brandId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return brand;
        }

        public void AddNew(Brand brand)
        {
            try
            {
                Brand _Brand = GetBrandByID(brand.BrandId);
                if (_Brand == null)
                {
                    using var context = new ShoesStoreContext();
                    context.Brands.Add(brand);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Brand is already exist.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void Update(Brand brand)
        {
            try
            {
                Brand _Brand = GetBrandByID(brand.BrandId);
                if (_Brand != null)
                {
                    using var context = new ShoesStoreContext();
                    context.Brands.Update(brand);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Brand does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int brandId)
        {
            try
            {
                Brand brand = GetBrandByID(brandId);
                if (brand != null)
                {
                    using var context = new ShoesStoreContext();
                    context.Brands.Remove(brand);
                    
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Brand does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
