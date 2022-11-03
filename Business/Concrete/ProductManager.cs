﻿using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
          _productDal = productDal;
        }

        public ProductManager(IProductDal productDal, CategoryManager categoryManager) : this(productDal)
        {
        }

        public IDataResult<List<Product>> GetAll()
        {
          if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

          return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>> (_productDal.GetAll(p => p.CategoryId == id));
        }



        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        // TEK BİR ÜRÜN GETİRDİK 

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        //   Ürün ekledik 



        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            // business codes -> FluentValidation

            
          
             _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

       // IDataResult<List<Product>> IProductService.Add(Product product)
        //{
          //  throw new NotImplementedException();
        //}

        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        public IResult AddTransactionalTest(Product product)
        {
            throw new NotImplementedException();
        }
    }
}