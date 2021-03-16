using Blog.Application.Condition;
using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service
{
   public interface IGalleryService
    {
        /// <summary>
        /// 存储上传图片内容
        /// </summary>
        /// <param name="account"></param>
        /// <param name="url"></param>
        void Upload(GalleryDTO galleryDTO);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        List<GalleryDTO> GetListPage(int currentPage, int pageSize, GalleryCondition condition = null);
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCount(GalleryCondition condition = null);
    }
}
