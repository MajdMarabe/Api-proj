using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICategoryService
    {

        public Task< List<CategoryResponse> >GetAll();
        public Task< CategoryResponse> Create(CategoryRequest request);





    }
}
