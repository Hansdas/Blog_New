using Blog.Domain.Gallery;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
   public interface IGalleryRepository: IRepository<Gallery, int>
    {
    }
}
