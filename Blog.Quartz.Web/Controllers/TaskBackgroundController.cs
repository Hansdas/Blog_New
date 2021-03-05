using Blog.Quartz.Application.Condition;
using Blog.Quartz.Application.DTO;
using Blog.Quartz.Application.Service;
using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Quartz.Web.Controllers
{
    [Route("task")]
    public class TaskBackgroundController : Controller
    {
        private IQuartzOptionService _quartzOptionService;
        public TaskBackgroundController(IQuartzOptionService quartzOptionService)
        {
            _quartzOptionService = quartzOptionService;
        }
        [Route("list")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Route("list/page")]
        [HttpPost]
        public ApiResult getListPage([FromBody]QuartzOptionCondition condition)
        {
            List<QuartzOptionDTO> quartzOptionDTOs = _quartzOptionService.getListPage(condition);
            int count = _quartzOptionService.Count(condition);
            return ApiResult.Success(new { data = quartzOptionDTOs, count = count });
        }
        [Route("add")]
        [HttpPost]
        public async Task<ApiResult> AddJob()
        {
            QuartzOptionDTO quartzOptionDTO = new QuartzOptionDTO();
            quartzOptionDTO.JobName = Request.Form["jobName"];
            quartzOptionDTO.GroupName = Request.Form["groupName"];
            quartzOptionDTO.Cron = Request.Form["cron"];
            quartzOptionDTO.Api = Request.Form["api"];
            quartzOptionDTO.RequestType = Request.Form["requestType"];
            quartzOptionDTO.ParameterValue = Request.Form["parameterValue"];
            quartzOptionDTO.Description = Request.Form["description"];
            try
            {
                await _quartzOptionService.AddJob(quartzOptionDTO);
                return ApiResult.Success();
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("{id}")]
        [HttpGet]
        public ApiResult Get(int id)
        {
            try
            {
                QuartzOptionDTO result =_quartzOptionService.GetById(id);
                return ApiResult.Success(result);
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("update/{id}")]
        [HttpPost]
        public ApiResult Update(int id)
        {
            QuartzOptionDTO quartzOptionDTO = new QuartzOptionDTO();
            quartzOptionDTO.JobName = Request.Form["jobName"];
            quartzOptionDTO.GroupName = Request.Form["groupName"];
            quartzOptionDTO.Cron = Request.Form["cron"];
            quartzOptionDTO.Api = Request.Form["api"];
            quartzOptionDTO.RequestType = Request.Form["requestType"];
            quartzOptionDTO.ParameterValue = Request.Form["parameterValue"];
            quartzOptionDTO.Description = Request.Form["description"];
            try
            {
                _quartzOptionService.Update(id,quartzOptionDTO);
                return ApiResult.Success();
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<ApiResult> Delete(int id)
        {
            try
            {
                await _quartzOptionService.DeleteById(id);
                return ApiResult.Success();
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("start/{id}")]
        [HttpDelete]
        public async Task<ApiResult> Start(int id)
        {
            try
            {
                string result = await _quartzOptionService.Start(id);
                return ApiResult.Success(result);
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("stop/{id}")]
        [HttpGet]
        public async Task<ApiResult> Stop(int id)
        {
            try
            {
                string result = await _quartzOptionService.Stop(id);
                return ApiResult.Success(result);
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("run/{id}")]
        [HttpGet]
        public async Task<ApiResult> Run(int id)
        {
            try
            {
                string result = await _quartzOptionService.Run(id);
                return ApiResult.Success(result);
            }
            catch (ArgumentException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        [Route("operate")]
        [HttpPost]
        public async Task<ApiResult> Operate()
        {
            bool all = Convert.ToBoolean(Request.Form["all"]);
            List<int> idList = null;
            string ids = Request.Form["ids"];
            if(ids!=null)
                idList = JsonConvert.DeserializeObject<List<int>>(Request.Form["ids"]);
            string operation = Request.Form["operation"];
            switch (operation)
            {
                case "start":
                    if (all)
                        await _quartzOptionService.Start(true);
                    else
                        await _quartzOptionService.Start(false, idList);
                    break;
                case "stop":
                    if (all)
                        await _quartzOptionService.Stop(true);
                    else
                        await _quartzOptionService.Stop(false, idList);
                    break;
                case "delete":
                    if (all)
                        await _quartzOptionService.Delete(true);
                    else
                        await _quartzOptionService.Delete(false, idList);
                    break;
            }
            return ApiResult.Success();

        }
    
    }
}
