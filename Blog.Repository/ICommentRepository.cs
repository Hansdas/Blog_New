using Blog.Domain;
using Core.Repository;

namespace Blog.Repository
{
    public interface ICommentRepository : IRepository<Comment, string>
    {
    }
}
