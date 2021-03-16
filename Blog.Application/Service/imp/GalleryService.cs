using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain.Gallery;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service.imp
{
    public class GalleryService : IGalleryService
    {
        private IGalleryRepository _galleryRepository;
        public GalleryService(IGalleryRepository galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }
        public void Upload(GalleryDTO galleryDTO)
        {
            if (string.IsNullOrEmpty(galleryDTO.Account))
                throw new ArgumentException("account为空");
            if (string.IsNullOrEmpty(galleryDTO.Url))
                throw new ArgumentException("url为空");
            Gallery gallery = new Gallery();
            gallery.Account = galleryDTO.Account;
            gallery.Url = galleryDTO.Url;
            gallery.Lable = galleryDTO.Lable;
            gallery.Description = galleryDTO.Description;
            gallery.CreateTime = DateTime.Now;
            _galleryRepository.Insert(gallery);
        }
        public List<GalleryDTO> GetListPage(int currentPage, int pageSize, GalleryCondition condition = null)
        {
           IEnumerable<Gallery> galleries= _galleryRepository.SelectByPage(currentPage, pageSize);
            List<GalleryDTO> list = new List<GalleryDTO>();
            foreach(var item in galleries)
            {
                GalleryDTO galleryDTO = new GalleryDTO();
                galleryDTO.Url = item.Url;
                list.Add(galleryDTO);
            }
            return list;
        }

        public int GetCount(GalleryCondition condition = null)
        {
            return _galleryRepository.SelectCount();
        }
    }
}
