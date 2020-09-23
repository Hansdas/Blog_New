using Blog.Domain;
using Blog.Repoistory;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository
{
   public interface ICommentRepository: IRepository<Comment, string>
    {
    }
}
