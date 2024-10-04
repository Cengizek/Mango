﻿using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
  
    public class ProductAPIController : Controller
    {
        
       
            private readonly AppDbContext _db;
            private readonly IMapper _mapper;
            private readonly ResponseDto _responseDto;
            public ProductAPIController(AppDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
                _responseDto = new ResponseDto();
            }

            [HttpGet]
            public ResponseDto Get()
            {
                try
                {
                    IEnumerable<Product> objList = _db.Products.ToList();
                    _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
                }
                catch (Exception ex)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = ex.Message;
                }
                return _responseDto;

            }

            [HttpGet]
            [Route("{id:int}")]
            public ResponseDto Get(int id)
            {
                try
                {
                    Product obj = _db.Products.First(u => u.ProductId == id);
                    _responseDto.Result = _mapper.Map<ProductDto>(obj);
                }
                catch (Exception ex)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = ex.Message;
                }
                return _responseDto;

            }

            
            [HttpPost]
            [Authorize(Roles = "Admin")]
            public ResponseDto Post(ProductDto productDto)
            {
                try
                {
                    Product product = _mapper.Map<Product>(productDto);
                    _db.Products.Add(product);
                    _db.SaveChanges();

                    if(productDto.Image != null)
                    {
                        string fileName= product.ProductId + Path.GetExtension(productDto.Image.FileName);
                        string filePath = @"wwwroot\ProductImages\" + fileName;
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            productDto.Image.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                        product.ImageLocalPath = filePath;
                        

                    }
                    else
                    {
                    product.ImageUrl = "https://placeholder.co/600x400";
                    }
                    _db.Products.Update(product);
                    _db.SaveChanges();
                    _responseDto.Result = _mapper.Map<ProductDto>(product);
                }
                    
                catch (Exception ex)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = ex.Message;
                }
                return _responseDto;

            }
            [HttpPut]
            [Authorize(Roles = "Admin")]
            public ResponseDto Put(ProductDto productDto)
            {
                try
                {
                    Product product = _mapper.Map<Product>(productDto);
                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }


                        string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                        string filePath = @"wwwroot\ProductImages\" + fileName;
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            productDto.Image.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                        product.ImageLocalPath = filePath;


                    }
                }



                     _db.Products.Update(product);
                    _db.SaveChanges();
                    _responseDto.Result = _mapper.Map<ProductDto>(product);
                }
                catch (Exception ex)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = ex.Message;
                }
                return _responseDto;

            }
            [HttpDelete]
            [Route("{id:int}")]
           [Authorize(Roles = "Admin")]
            public ResponseDto Delete(int id)
            {
                try
                {
                    Product obj = _db.Products.FirstOrDefault(u => u.ProductId == id);
                    if (!string.IsNullOrEmpty(obj.ImageLocalPath))
                    {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                    _db.Products.Remove(obj);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = ex.Message;
                }
                return _responseDto;

            }

    }
}


