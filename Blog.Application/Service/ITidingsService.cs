using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service
{
    public interface ITidingsService
    {
        int SelectCount(TidingsCondition tidingsCondition =null);
        List<TidingsDTO> GetTidingsDTOs(int currentPage,int pageSize, TidingsCondition tidingsCondition = null);
        void Create(TidingsDTO tidingsDTO);
    }
}
