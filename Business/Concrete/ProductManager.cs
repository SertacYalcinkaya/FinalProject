using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [PerformanceAspect(10)]
        public IDataResult<List<Product>> GetAll()
        {
            // Test for error.
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        [PerformanceAspect(5)]
        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == categoryId));
        }

        [PerformanceAspect(5)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [PerformanceAspect(5)]
        public IDataResult<List<Product>> GetByUnitPrice(decimal minPrice, decimal maxPrice)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(
                CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfCategoryLimitExceeded()
                );

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        [SecuredOperation("product.update,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            IResult result = BusinessRules.Run(
                CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfCategoryLimitExceeded()
                );

            if (result != null)
            {
                return result;
            }

            _productDal.Update(product);

            return new SuccessResult(Messages.ProductUpdated);
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            bool result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
                return new ErrorResult(Messages.ProductNameAlreadyExists);

            return new SuccessResult();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var products = _productDal.GetAll(p => p.CategoryId == categoryId);

            if (products.Count > 49)
                return new ErrorResult(Messages.ProductCountOfCategoryError);

            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceeded()
        {
            var count = _categoryService.GetAll().Data.Count;

            if (count > 15)
                return new ErrorResult(Messages.CategoryLimitExceeded);

            return new SuccessResult();
        }

        [TransactionScopeAspect]
        [SecuredOperation("product.update,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
