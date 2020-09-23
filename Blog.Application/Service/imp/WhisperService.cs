using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Domain.Core;
using Blog.Repository;
using Core.Cache;

namespace Blog.Application.Service.imp
{
    public class WhisperService : IWhisperService
    {
        private IWhisperRepository _whisperRepoistory;
        private ICacheFactory _cacheFactory;
        private IUserRepository _userRepository;
        public WhisperService(IWhisperRepository whisperRepoistory, ICacheFactory cacheFactory, IUserRepository userRepository)
        {
            _whisperRepoistory = whisperRepoistory;
            _cacheFactory = cacheFactory;
            _userRepository = userRepository;
        }
        public async Task<IList<WhisperDTO>> SelectPageCache(int pageIndex, int pageSize)
        {
            ICacheClient cacheClient = _cacheFactory.CreateClient(CacheType.Redis);
            IList<WhisperDTO> whisperDTOs= await cacheClient.ListRange<WhisperDTO>(ConstantKey.CACHE_SQUARE_WHISPER, pageIndex, pageSize);
            if (whisperDTOs.Count == 0)
            {
                Expression<Func<Whisper, DateTime>> orderBy = s => s.CreateTime;
                IList<Whisper>  whispers= _whisperRepoistory.SelectByPage(1,6,null,s=>s.CreateTime).ToList();
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
                    await cacheClient.AddListTail(ConstantKey.CACHE_SQUARE_WHISPER, whisperDTO);
                }
            }
            return whisperDTOs;

        }
    }
}
