using Blog.Domain.Tidings;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository
{
    public interface ITidingsRepository : IRepository<Tidings, int>
    {
    }
}
