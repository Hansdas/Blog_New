using Blog.Domain.Tidings;
using Blog.Repository.DB;
using Core.Repository.Imp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Imp
{
    public class TidingsRepository : Repository<Tidings, int>, ITidingsRepository
    {
        public TidingsRepository(DBContext dbContext) : base(dbContext)
        {

        }
    }
}
