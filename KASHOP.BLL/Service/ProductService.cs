using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        public ProductService(IProductRepository productRepository, IFileService fileService)
        {

            _productRepository = productRepository;
            _fileService = fileService;
        }
        public async Task<ProductResponse> Create(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if (request.MainImage != null)
            {
                var imageUrl = await _fileService.UploadFileAsync(request.MainImage);
                product.MainImage = imageUrl;
            }
            var result = await _productRepository.CreateAsync(product);
            return product.Adapt<ProductResponse>();
            


        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(p => p.Id == id);
            if (product == null) return false;
            _fileService.Delete(product.MainImage);     
            var result = await _productRepository.DeleteAsync(product);
            return result;
        }

        public async Task<List<ProductResponse>> GetAll()
        {
            var products = await _productRepository.GetAllAsync(p=>p.Status==EntityState.Active,new string[] {  nameof(Product.Translations),nameof(Product.CreatedBy) });
            return products.Adapt<List<ProductResponse>>();


        }
        
        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[] { nameof(Product.Translations), nameof(Product.CreatedBy) });
            if (product == null)
                return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> UpdateProduct(ProductUpdateRequest request, int id)
        {
            var productdb = await _productRepository.GetOne(p => p.Id == id, new string[] { nameof(Product.Translations), nameof(Product.CreatedBy) });
            if (productdb == null) return false;


            if (request.MainImage != null)
            { 
                _fileService.Delete(productdb.MainImage);
                var imageUrl = await _fileService.UploadFileAsync(request.MainImage);
                productdb.MainImage = imageUrl;
            }
            if(request.Translations != null)
            {
                foreach (var translation in request.Translations)
                {
                    var translationdb = productdb.Translations.FirstOrDefault(t => t.Language == translation.Language);
                    if (translationdb != null)
                    {
                         translation.Adapt(translationdb);
                    }
                    else
                    {
                        var newTranslation = translation.Adapt<ProductTranslation>();
                        productdb.Translations.Add(newTranslation);
                    }
                }
            }

            request.Adapt(productdb);

            return await _productRepository.UpdateAsync(productdb);
        }

        public async Task<bool> ToggleStatuse(int id)
        {
            var product = await _productRepository.GetOne(p => p.Id == id);

            if (product == null) return false;

            product.Status = product.Status == EntityState.Active ? EntityState.Inactive : EntityState.Active;

            return await _productRepository.UpdateAsync(product);

        }
    }
}
