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
            public ResponseDto Post([FromBody] ProductDto productDto)
            {
                try
                {
                    Product obj = _mapper.Map<Product>(productDto);
                    _db.Products.Add(obj);
                    _db.SaveChanges();
                    _responseDto.Result = _mapper.Map<ProductDto>(obj);
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
            public ResponseDto Put([FromBody] ProductDto productDto)
            {
                try
                {
                    Product obj = _mapper.Map<Product>(productDto);
                    _db.Products.Update(obj);
                    _db.SaveChanges();
                    _responseDto.Result = _mapper.Map<ProductDto>(obj);
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


