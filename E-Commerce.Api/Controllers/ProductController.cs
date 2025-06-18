using AutoMapper;
using E_Commerce.Api.Helper;
using E_Commerce.Core.DTO.product;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Services;
using E_Commerce.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IImageManagementService service;

        public ProductsController(IUnitOfWork work, IMapper mapper, IImageManagementService service) : base(work, mapper)
        {
            this.service = service;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get([FromQuery] ProductParams productParams)
        {
            try
            {
                var Product = await _unitOfWork.ProductRepository
                    .GetAllAsync(productParams);

                return Ok(new Pagination<ProductDTO>(productParams.PageNumber, productParams.pageSize, productParams.TotatlCount, Product));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id,
                    x => x.Category, x => x.Photos);

                var result = _mapper.Map<ProductDTO>(product);

                if (product is null) return BadRequest(new ResponseAPI(400));


                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Add-Product")]
        public async Task<IActionResult> add(AddProductDTO productDTO)
        {
            try
            {
                await _unitOfWork.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpDelete("Delete-Product/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(Id, x => x.Photos, x => x.Category);

                await _unitOfWork.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseAPI(200));

            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
