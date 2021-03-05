using Blog.Quartz.Application.Condition;
using Blog.Quartz.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz.Application.Service
{
    public interface IQuartzOptionService
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QuartzOptionDTO GetById(int id);
        /// <summary>
        /// 初始化任务
        /// </summary>
        void Init();
        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="quartzOptionDTO"></param>
        Task AddJob(QuartzOptionDTO quartzOptionDTO);
        /// <summary>
        /// 根据id更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Update(int id, QuartzOptionDTO quartzOptionDTO);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="quartzOptionCondition"></param>
        /// <returns></returns>
        List<QuartzOptionDTO> getListPage(QuartzOptionCondition quartzOptionCondition);
        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int Count(QuartzOptionCondition condition);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task DeleteById(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task Delete(bool all, IEnumerable<int> ids = null);
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> Start(int id);
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Start(bool all,IEnumerable<int> ids=null);
        /// <summary>
        /// 运行一次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> Run(int id);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> Stop(int id);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Stop(bool all, IEnumerable<int> ids = null);
    }
}
