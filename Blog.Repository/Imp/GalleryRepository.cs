using Blog.Domain.Gallery;
using Blog.Repository.DB;
using Core.Repository.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository.Imp
{
   public class GalleryRepository: Repository<Gallery, int>, IGalleryRepository
    {
        public GalleryRepository(DBContext dbContext) : base(dbContext)
        {

        }
    }
}
