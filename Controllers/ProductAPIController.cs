using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    //[Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;

        //Note: Such [Routes + Response + Mapper] is not available in MVC (basically not needed),
        // becuase IActionResult handles it by default
        private ResponseDto _response;
        private IMapper _mapper;

        //TODO: With Clean Architecture Try To Decouple This
        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> productList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                //TODO: What happes if used FirstOrDefault and no record found?
                Product product = _db.Products.First(u => u.Id == id);
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDto GetByName(string name)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(u => u.Name.ToLower() == name.ToLower());
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCategory/{category}")]
        public ResponseDto GetByCategory(string category)
        {
            //TODO: Write the correct logic here to get products by category
            try
            {
                IEnumerable<Product> productList = _db.Products.Where(p => p.CategoryName == category).ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);

                _db.Products.Add(product);
                _db.SaveChanges();
                //_db.SaveChangesAsync(); ---> If use async, id will not be assigned the latest one.
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]//-> internal name for URL
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);

                // It will used id available in coupon
                _db.Products.Update(product);
                _db.SaveChanges();
                //_db.SaveChangesAsync(); ---> If use async, id will not be assigned the latest one.
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        
        [HttpDelete]//-> internal name for URL
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _db.Products.First(u => u.Id == id);
                // It will used id available in coupon
                _db.Products.Remove(product);
                _db.SaveChanges();
                //_db.SaveChangesAsync(); ---> If use async, id will not be assigned the latest one.
                //_response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
