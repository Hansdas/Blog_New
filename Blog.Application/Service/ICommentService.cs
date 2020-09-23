using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
  public  interface ICommentService
    {
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IList<CommentDTO> Select(IEnumerable<string> ids);
    }
}
