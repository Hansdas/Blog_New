using Blog.Domain;
using Core.Repository;

namespace Blog.Repository
{
   public interface ILeaveMessageRepository : IRepository<LeaveMessage, int>
    {
    }
}
