using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
   public abstract class Entity<T>
    {
        public  T Id { get; set; }
    }
}
