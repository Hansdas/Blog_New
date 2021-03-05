using Blog.Quartz.Application.Condition;
using Blog.Quartz.Application.DTO;
using Blog.Quartz.Domain;
using Blog.Quartz.Repository;
using Core.Common.EnumExtensions;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using Blog.Quartz.Application.Quartz;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskStatus = Blog.Quartz.Domain.TaskStatus;
using System.Linq;

namespace Blog.Quartz.Application.Service.Imp
{
    public class QuartzOptionService : IQuartzOptionService
    {
        private IQuartzOptionRepository _quartzOptionRepository;
        private ISchedulerFactory _schedulerFactory;
        public QuartzOptionService(IQuartzOptionRepository quartzOptionRepository, ISchedulerFactory schedulerFactory)
        {
            _quartzOptionRepository = quartzOptionRepository;
            _schedulerFactory = schedulerFactory;
        }
        private  QuartzOptionDTO ConvertToDTO(QuartzOption item)
        {
            QuartzOptionDTO quartzOptionDTO = new QuartzOptionDTO();
            quartzOptionDTO.Id = item.Id;
            quartzOptionDTO.JobName = item.JobName;
            quartzOptionDTO.GroupName = item.GroupName;
            quartzOptionDTO.Cron = item.Cron;
            quartzOptionDTO.Api = item.Api;
            quartzOptionDTO.LastActionTime = item.LastActionTime.HasValue ? item.LastActionTime.Value.ToString("yyyy-MM-dd hh:MM:ss") : "";
            quartzOptionDTO.Description = item.Description;
            quartzOptionDTO.RequestType = item.RequestType;
            quartzOptionDTO.ParameterValue = item.ParameterValue;
            quartzOptionDTO.TaskStatus = item.TaskStatus.GetEnumText();
            return quartzOptionDTO;
        }

        private void Valid(QuartzOptionDTO quartzOptionDTO)
        {
            if (string.IsNullOrEmpty(quartzOptionDTO.JobName))
                throw new ArgumentException("作业名称为空");
            if (string.IsNullOrEmpty(quartzOptionDTO.GroupName))
                throw new ArgumentException("分组名称为空");
            if (string.IsNullOrEmpty(quartzOptionDTO.Cron))
                throw new ArgumentException("表达式为空");
            if (string.IsNullOrEmpty(quartzOptionDTO.Api))
                throw new ArgumentException("api为空");
            if (!quartzOptionDTO.Cron.ValidCron())
                throw new ArgumentException("表达式错误");
        }
        public void Init()
        {
           IEnumerable<QuartzOption> list= _quartzOptionRepository.Select(null);
            _schedulerFactory.InitQuartz(list);
        }
        public QuartzOptionDTO GetById(int id)
        {
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            return ConvertToDTO(quartzOption);
        }
        public async Task Update(int id, QuartzOptionDTO quartzOptionDTO)
        {
            Valid(quartzOptionDTO);
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            await _schedulerFactory.TriggerAction(quartzOption, JobAction.修改);        
            quartzOption.JobName = quartzOptionDTO.JobName;
            quartzOption.GroupName = quartzOptionDTO.GroupName;
            quartzOption.Cron = quartzOptionDTO.Cron;
            quartzOption.Api = quartzOptionDTO.Api;
            quartzOption.RequestType = quartzOptionDTO.RequestType;
            quartzOption.ParameterValue = quartzOptionDTO.ParameterValue;
            quartzOption.Description = quartzOptionDTO.Description;
            quartzOption.TaskStatus = TaskStatus.暂停;
            quartzOption.UpdateTime = DateTime.Now;
            _quartzOptionRepository.Update(quartzOption);
            await _schedulerFactory.AddJob(quartzOption);

        }

        public async Task AddJob(QuartzOptionDTO quartzOptionDTO)
        {
            Valid(quartzOptionDTO);
            int count = _quartzOptionRepository.SelectCount(s => s.GroupName == quartzOptionDTO.GroupName && s.JobName == quartzOptionDTO.JobName);
            if (count > 0)
                throw new ArgumentException(string.Format("分组：{0}，作业：{1}已存在", quartzOptionDTO.GroupName, quartzOptionDTO.JobName));
            QuartzOption quartzOption = new QuartzOption();
            quartzOption.JobName = quartzOptionDTO.JobName;
            quartzOption.GroupName = quartzOptionDTO.GroupName;
            quartzOption.Cron = quartzOptionDTO.Cron;
            quartzOption.Api = quartzOptionDTO.Api;
            quartzOption.RequestType = quartzOptionDTO.RequestType;
            quartzOption.ParameterValue = quartzOptionDTO.ParameterValue;
            quartzOption.Description = quartzOptionDTO.Description;
            quartzOption.TaskStatus = TaskStatus.暂停;
            quartzOption.CreateTime = DateTime.Now;
            _quartzOptionRepository.Insert(quartzOption);
            await _schedulerFactory.AddJob(quartzOption);

        }
        public List<QuartzOptionDTO> getListPage(QuartzOptionCondition quartzOptionCondition)
        {
            Expression<Func<QuartzOption, object>> desc = s => s.CreateTime;
            IEnumerable<QuartzOption> quartzOptions = _quartzOptionRepository.SelectByPage(quartzOptionCondition.CurrentPage, quartzOptionCondition.PageSize, null, desc);
            List<QuartzOptionDTO> list = new List<QuartzOptionDTO>();
            foreach (var item in quartzOptions)
            {
                QuartzOptionDTO quartzOptionDTO = ConvertToDTO(item);
                list.Add(quartzOptionDTO);
            }
            return list;
        }
     
        public int Count(QuartzOptionCondition condition)
        {
            return _quartzOptionRepository.SelectCount();
        }

        public async Task DeleteById(int id)
        {
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            _quartzOptionRepository.DeleteById(id);
            await _schedulerFactory.TriggerAction(quartzOption, JobAction.删除);
        }

        public async Task<string> Start(int id)
        {
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            quartzOption.TaskStatus = TaskStatus.运行;
            _quartzOptionRepository.Update(quartzOption);
            string result= await _schedulerFactory.TriggerAction(quartzOption, JobAction.开启);
            return result;
        }
        public async Task<string> Run(int id)
        {
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            string result = await _schedulerFactory.TriggerAction(quartzOption, JobAction.立即执行);
            return result;
        }

        public async Task<string> Stop(int id)
        {
            QuartzOption quartzOption = _quartzOptionRepository.SelectById(id);
            quartzOption.TaskStatus = TaskStatus.暂停;
            _quartzOptionRepository.Update(quartzOption);
            string result = await _schedulerFactory.TriggerAction(quartzOption, JobAction.暂停);
            return result;
        }
        public async Task Start(bool all, IEnumerable<int> ids)
        {
            List<QuartzOption> quartzOptions = null;
            if (all)
                quartzOptions = _quartzOptionRepository.Select(null).ToList();
            else
                quartzOptions = _quartzOptionRepository.Select(s => ids.Contains(s.Id)).ToList();
            quartzOptions.ForEach(s =>
            {
                s.TaskStatus = TaskStatus.运行;

            });
            await _schedulerFactory.TriggerAction(quartzOptions, JobAction.开启);
            _quartzOptionRepository.UpdateRange(quartzOptions);

        }
        public async Task Delete(bool all, IEnumerable<int> ids  = null)
        {
            List<QuartzOption> quartzOptions = null;
            if (all)
            {
                quartzOptions = _quartzOptionRepository.Select(null).ToList();
                _quartzOptionRepository.DeleteAll();
            }
            else {
                quartzOptions = _quartzOptionRepository.Select(s => ids.Contains(s.Id)).ToList();
                _quartzOptionRepository.Delete(s => ids.Contains(s.Id));
            }
            await _schedulerFactory.TriggerAction(quartzOptions, JobAction.删除);
        }

        public async Task Stop(bool all, IEnumerable<int> ids = null)
        {
            List<QuartzOption> quartzOptions = null;
            if (all)
                quartzOptions = _quartzOptionRepository.Select(null).ToList();
            else
                quartzOptions = _quartzOptionRepository.Select(s => ids.Contains(s.Id)).ToList();
            quartzOptions.ForEach(s =>
            {
                s.TaskStatus = TaskStatus.暂停;

            });
            await _schedulerFactory.TriggerAction(quartzOptions, JobAction.暂停);
            _quartzOptionRepository.UpdateRange(quartzOptions);
        }
    }
}
