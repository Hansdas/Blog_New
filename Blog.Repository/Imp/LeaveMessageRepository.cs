using Blog.Domain;
using Blog.Repository.DB;
using Core.Repository.Imp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Imp
{
   public class LeaveMessageRepository : Repository<LeaveMessage, int>, ILeaveMessageRepository
    {
        public LeaveMessageRepository(DBContext dbContext) : base(dbContext)
        {

        }
    }
}
