using Blog.Application.Condition;
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
        List<WhisperDTO> SelectPage(int pageIndex, int pageSize,WhisperCondition condition=null);
        Task Create(string content,string account,string username,string authorization);
        int SelectCount(WhisperCondition condition=null);
    }
}
