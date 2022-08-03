using Business.Abstract;
using Core.Utilities.Results;
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

        public IDataResult<List<Category>> GetAll()
        {

            //iş kodları
            return new SuccessDataResult<List<Category>> (_catagoryDal.GetAll());
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(_catagoryDal.Get(c => c.CategoryId == categoryId));
        }
    }
}
