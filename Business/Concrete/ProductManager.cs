using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    //entity managera kendisi hariç bir entity parametre gönderme
    //ihtiyacın varsa servis gönder
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        //claim
        [SecuredOperation("admin,moderator")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]

        public IResult Add(Product product)
        {

            //business kodlarını ayrı fonksiyonlar halinde tut
            //daha okunabilir
            IResult result = BusinessRules.Run(CheckIfProductNameSame(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.ProductId),
                CheckIfCatagoryCountValid());
            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //business kodları
            if (DateTime.Now.Hour == 9)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);

        }

        public IDataResult<List<Product>> GetAllByCategoryID(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }
        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }
        [PerformanceAspect(5)]
        public IDataResult<Product> GetByProductID(int productID)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productID));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product) => throw new NotImplementedException();
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count();
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();

        }
        private IResult CheckIfProductNameSame(string productName)
        {
            var result = _productDal.Get(p => p.ProductName == productName);
            if (result != null)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCatagoryCountValid()
        {

            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryCountInvalid);

            }
            return new SuccessResult();
        }
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            return null;
        }
    }
}
