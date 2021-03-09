using Blog.Quartz.Domain;
using Blog.Quartz.Repository;
using Core.Common.Http;
using Core.CPlatform;
using Core.Log;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Quartz.Application.Quartz
{
    public class Job : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                IQuartzOptionRepository quartzOptionRepository = ServiceLocator.Get<IQuartzOptionRepository>();
                IHttpClientFactory httpClientFactory = ServiceLocator.Get<IHttpClientFactory>();
                AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
                QuartzOption quartzOption = quartzOptionRepository.SelectSingle(s =>(s.JobName == trigger.Name && s.GroupName == trigger.Group)
                ||(s.JobName == trigger.JobName && s.GroupName == trigger.JobGroup));
                if (quartzOption == null)
                    throw new ArgumentException(string.Format("分组：{0}，作业：{1}不存在", quartzOption.GroupName, quartzOption.JobName));
                HttpClient httpClient= httpClientFactory.CreateClient();
                HttpResponseMessage responseMessage = null;
                switch (quartzOption.RequestType)
                {
                    case "GET":
                        responseMessage = await httpClient.GetAsync(quartzOption.Api);
                        responseMessage.EnsureSuccessStatusCode();
                        break;
                    case "POST":
                        string[] arr= quartzOption.ParameterValue.Split(',');
                        Dictionary<string, string> para = new Dictionary<string, string>();
                        foreach(var item in arr)
                        {
                            string[] itemArr=item.Split(':');
                            para.Add(itemArr[0], itemArr[2]);
                        }
                        FormUrlEncodedContent content = new FormUrlEncodedContent(para);
                        responseMessage = await httpClient.PostAsync(quartzOption.Api,content);
                        responseMessage.EnsureSuccessStatusCode();
                        break;
                    case "DELETE":
                        responseMessage = await httpClient.DeleteAsync(quartzOption.Api);
                        responseMessage.EnsureSuccessStatusCode();
                        break;
                }
                ApiResult apiResult = JsonConvert.DeserializeObject<ApiResult>(await responseMessage.Content.ReadAsStringAsync());
                if (apiResult.Code != HttpStatusCode.SUCCESS)
                    throw new HttpRequestException(apiResult.Msg);
                quartzOption.LastActionTime = DateTime.Now;
                quartzOptionRepository.Update(quartzOption);
            }
            catch (Exception ex)
            {
               LogUtils.LogError(ex, "Blog.Quartz.Application.Quartz.Job", ex.Message);
            }
        }
    }
}
