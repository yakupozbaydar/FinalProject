using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _catagoryDal;

        public CategoryManager(ICategoryDal catagoryDal)
        {
            _catagoryDal = catagoryDal;
        }

        public List<Category> GetAll()
        {

            //iş kodları
            return _catagoryDal.GetAll();
        }

        public Category GetById(int categoryId)
        {
            return _catagoryDal.Get(c => c.CategoryId == categoryId);
        }
    }
}
