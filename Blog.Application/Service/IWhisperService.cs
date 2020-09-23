using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service
{
  public  interface IWhisperService
    {
        Task<IList<WhisperDTO>> SelectPageCache(int pageIndex, int pageSize);
    }
}
