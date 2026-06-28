using KASHOP.BLL.Extensions;
using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Stripe;
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
            var product = request.Adapt<DAL.Models.Product>();
            product.SubImages.Clear();
            if (request.MainImage != null)
            {
                var imageUrl = await _fileService.UploadFileAsync(request.MainImage);
                product.MainImage = imageUrl;
            }
            /////
            if (request.SubImages != null) { 
            foreach(var subImage in request.SubImages)
                {
                    var imageUrl = await _fileService.UploadFileAsync(subImage);
                    product.SubImages.Add(new ProductImage
                    {
                        ImagePath =imageUrl
                    });
                }
            }
            ////
            var result = await _productRepository.CreateAsync(product);
            return product.Adapt<ProductResponse>(); 

        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(p => p.Id == id,
                new string[] {nameof(DAL.Models.Product.SubImages)});
            if (product == null) return false;
            _fileService.Delete(product.MainImage);     
            foreach (var subImage in product.SubImages)
            {
                _fileService.Delete(subImage.ImagePath);

            }
            var result = await _productRepository.DeleteAsync(product);
            return result;
        }

        public async Task<PaginationResponse<ProductResponse>> GetAll(PaginationRequest request)
        {
            var query =  _productRepository.GetQueryable(p=>p.Status==EntityState.Active,
                new string[] {  nameof(DAL.Models.Product.Translations),nameof(DAL.Models.Product.CreatedBy), "SubImages" });

            var paginated = await query.ToPaginationAsync(request.page, request.Limit);

            return new PaginationResponse<ProductResponse>
            {
                Data = paginated.Data.Adapt<List<ProductResponse>>(),
                TotalCount = paginated.TotalCount,
                Page = paginated.Page,
                Limit = paginated.Limit
            };


        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<DAL.Models.Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[] { nameof(DAL.Models.Product.Translations), nameof(DAL.Models.Product.CreatedBy) });
            if (product == null)
                return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> UpdateProduct(ProductUpdateRequest request, int id)
        {
            var productdb = await _productRepository.GetOne(p => p.Id == id, new string[] { nameof(DAL.Models.Product.Translations),
                nameof(DAL.Models.Product.CreatedBy),"SubImages" });
            if (productdb == null) return false;
            if (request.MainImage != null)
            { 
                _fileService.Delete(productdb.MainImage);
                var imageUrl = await _fileService.UploadFileAsync(request.MainImage);
                productdb.MainImage = imageUrl;
            }
            if (request.SubImages != null)
            {
                foreach (var subimage in productdb.SubImages)
                {
                    _fileService.Delete(subimage.ImagePath);

                }
                productdb.SubImages.Clear();// clear the list


                foreach (var subimage in request.SubImages)
                {
                    var imageUrl = await _fileService.UploadFileAsync(subimage);
                    productdb.SubImages.Add(new ProductImage { ImagePath = imageUrl });                }
            }
            if (request.NewImges != null)
            {

                foreach (var NewImage in request.NewImges)
                {
                    var imageUrl = await _fileService.UploadFileAsync(NewImage);
                    productdb.SubImages.Add(new ProductImage { ImagePath = imageUrl });
                }
            }
            if (request.Translations != null)
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
