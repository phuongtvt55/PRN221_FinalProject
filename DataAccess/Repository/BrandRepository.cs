using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BrandRepository : IBrandRepository
    {
        public Brand GetBrandById(int brandId)
        {
            return BrandDAO.Instance.GetBrandByID(brandId);
        }

        public IEnumerable<Brand> GetBrands()
        {
            return BrandDAO.Instance.GetBrandList();
        }

        public void Insert(Brand brand)
        {
            BrandDAO.Instance.AddNew(brand);
        }

        public void Update(Brand brand)
        {
            BrandDAO.Instance.Update(brand);
        }
    }
}
