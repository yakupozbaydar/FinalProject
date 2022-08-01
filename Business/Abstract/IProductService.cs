using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();

        IDataResult<List<Product>> GetAllByCategoryID(int id);
        
        IDataResult<List<Product>> GetByUnitPrice(decimal min,decimal max);
        IDataResult<Product> GetByProductID(int productID);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IResult Add(Product product);
        IDataResult<Product> GetById(int productId);
    
    }
}
