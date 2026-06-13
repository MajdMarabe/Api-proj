using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IProductService
    {
        public Task<ProductResponse> Create(ProductRequest request);
        public Task<List<ProductResponse>> GetAll();
        public Task<ProductResponse?> GetProduct(System.Linq.Expressions.Expression<Func<KASHOP.DAL.Models.Product, bool>> filter);
        public Task<bool> DeleteProduct(int id);
        public Task<bool> UpdateProduct(ProductUpdateRequest request, int id);
        public Task<bool> ToggleStatuse(int id);


    }
}

