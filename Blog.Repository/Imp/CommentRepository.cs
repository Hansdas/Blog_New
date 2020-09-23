using Blog.Domain;
using Blog.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Repository.Imp
{
   public class CommentRepository : Repository<Comment, string>, ICommentRepository
    {
        public CommentRepository(DBContext dbContext) : base(dbContext)
        {

        }
    }
}
