using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Repository;
using Core.Cache;
using Core.Common.Http;
using Core.CPlatform.Domain;
using Core.Log;
using Newtonsoft.Json;

namespace Blog.Application.Service.imp
{
    public class WhisperService : IWhisperService
    {
        private IWhisperRepository _whisperRepoistory;
        private ICacheClient _cacheClient;
        private IUserRepository _userRepository;
        private IHttpClientFactory _httpClientFactory;

        public WhisperService(IWhisperRepository whisperRepoistory, ICacheFactory cacheFactory, IUserRepository userRepository, IHttpClientFactory httpClientFactory)
        {
            _whisperRepoistory = whisperRepoistory;
            _cacheClient = cacheFactory.CreateClient(CacheType.Redis);
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Create(string content, string account,string username,string authorization)
        {
            Whisper whisper = new Whisper();
            whisper.Account = account;
            whisper.Content = content;
            whisper.CreateTime = DateTime.Now;
            whisper = _whisperRepoistory.Insert(whisper);

            WhisperDTO whisperDTO = new WhisperDTO();
            whisperDTO.Id = whisper.Id.ToString();
            whisperDTO.Account = whisper.Account;
            whisperDTO.AccountName = username;
            whisperDTO.Content = content;
            whisperDTO.CreateDate = whisper.CreateTime.ToString("yyyy-MM-dd HH:mm");

            long length = await _cacheClient.AddListTop(ConstantKey.CACHE_SQUARE_WHISPER, whisperDTO);
            int listLength = Convert.ToInt32(length);
            if (listLength > 12)
                await _cacheClient.listPop(ConstantKey.CACHE_SQUARE_WHISPER);
            List<WhisperDTO> whispers = await _cacheClient.ListRange<WhisperDTO>(ConstantKey.CACHE_SQUARE_WHISPER, 0, 6);
            List<WhisperDTO> whisperDTOs = new List<WhisperDTO>();
            Dictionary<string, string> accountWithName = _userRepository.AccountWithName(whispers.Select(s => s.Account));
            foreach (var item in whispers)
            {
                whisperDTO = new WhisperDTO();
                whisperDTO.Id = item.Id.ToString();
                whisperDTO.Account = item.Account;
                whisperDTO.AccountName = accountWithName[item.Account];
                whisperDTO.Content = item.Content;
                whisperDTO.CreateDate = item.CreateDate;
                whisperDTOs.Add(whisperDTO);
            }

            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
            string jsonValue = JsonConvert.SerializeObject(whisperDTOs);
            string httpUrl = ConstantKey.GATEWAY_HOST + "/sms/singlar/whisper";
            try
            {
                var response = await httpClient.PostAsync(httpUrl, new StringContent(jsonValue, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                ApiResult apiResult = JsonConvert.DeserializeObject<ApiResult>(await response.Content.ReadAsStringAsync());
                if (apiResult.Code != HttpStatusCode.SUCCESS)
                    throw new HttpRequestException(apiResult.Msg);
            }
            catch (HttpRequestException ex)
            {
                LogUtils.LogError(ex, "WhisperService.Create", ex.Message);
            }

        }

        public int SelectCount(WhisperCondition condition = null)
        {
           return _whisperRepoistory.SelectCount();
        }

        public List<WhisperDTO> SelectPage(int pageIndex, int pageSize, WhisperCondition condition=null)
        {
            Expression<Func<Whisper, object>> orderBy = s => s.CreateTime;
            List<Whisper> whispers = _whisperRepoistory.SelectByPage(pageIndex, pageSize,null,orderBy).ToList();
            List<WhisperDTO> whisperDTOs = new List<WhisperDTO>();
            Dictionary<string, string> accountWithName = _userRepository.AccountWithName(whispers.Select(s => s.Account));
            foreach (var item in whispers)
            {
                WhisperDTO whisperDTO = new WhisperDTO();
                whisperDTO.Id = item.Id.ToString();
                whisperDTO.Account = item.Account;
                whisperDTO.AccountName = accountWithName[item.Account];
                whisperDTO.Content = item.Content;
                whisperDTO.CreateDate = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                whisperDTOs.Add(whisperDTO);
            }
            return whisperDTOs;
        }

        public async Task<IList<WhisperDTO>> SelectPageCache(int pageIndex, int pageSize)
        {
            IList<WhisperDTO> whisperDTOs= await _cacheClient.ListRange<WhisperDTO>(ConstantKey.CACHE_SQUARE_WHISPER, pageIndex, pageSize);
            if (whisperDTOs.Count == 0)
            {
                Expression<Func<Whisper, DateTime>> orderBy = s => s.CreateTime;
                IList<Whisper>  whispers= _whisperRepoistory.SelectByPage(1,12,null,s=>s.CreateTime).ToList();
                IEnumerable<string> accounts = whispers.Select(s => s.Account);
                Dictionary<string,string> accountWithName = _userRepository.AccountWithName(accounts);
                foreach (var item in whispers)
                {
                    WhisperDTO whisperDTO = new WhisperDTO();
                    whisperDTO.Id = item.Id.ToString();
                    whisperDTO.Account = item.Account;
                    whisperDTO.AccountName = accountWithName[item.Account];
                    whisperDTO.Content = item.Content;
                    whisperDTO.CreateDate = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                    whisperDTOs.Add(whisperDTO);
                    await _cacheClient.AddListTail(ConstantKey.CACHE_SQUARE_WHISPER, whisperDTO);
                }
            }
            return whisperDTOs;

        }
    }
}
