using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Domain.Tidings;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service.imp
{
    public class TidingsService : ITidingsService
    {
        private ITidingsRepository _tidingsRepository;
        private IUserRepository _userRepository;
        public TidingsService(ITidingsRepository tidingsRepository, IUserRepository userRepository)
        {
            _tidingsRepository = tidingsRepository;
            _userRepository = userRepository;
        }
        public void Create(TidingsDTO tidingsDTO)
        {
            Tidings tidings = new Tidings();
            tidings.PostContent = tidingsDTO.PostContent;
            tidings.PostUser = tidingsDTO.PostUserAccount;
            tidings.ReviceUser = tidingsDTO.ReviceUserAccount;
            tidings.Url = tidingsDTO.Url;
            tidings.CreateTime = Convert.ToDateTime(tidingsDTO.PostDate);
            tidings.CommentId = tidingsDTO.CommentId;
            tidings.IsRead = tidingsDTO.IsRead;
            tidings.AdditionalData = tidingsDTO.Content;
            _tidingsRepository.Insert(tidings);
        }

        public List<TidingsDTO> GetTidingsDTOs(int currentPage, int pageSize, TidingsCondition tidingsCondition = null)
        {
            Expression<Func<Tidings, bool>> where = TidingsCondition.BuildExpression(tidingsCondition);
            List<Tidings> tidingsList = _tidingsRepository.SelectByPage(currentPage, pageSize, where).ToList();
            List<string> accounts = new List<string>();
            accounts.AddRange(tidingsList.Select(s => s.PostUser));
            accounts.AddRange(tidingsList.Select(s => s.ReviceUser));
            Dictionary<string, string> dic = _userRepository.AccountWithName(accounts.Distinct());
            List<TidingsDTO> tidingsModels = new List<TidingsDTO>();
            foreach (var item in tidingsList)
            {
                TidingsDTO tidingsModel = new TidingsDTO();
                tidingsModel.Id = item.Id;
                tidingsModel.Content = item.AdditionalData;
                tidingsModel.IsRead = item.IsRead;
                tidingsModel.PostContent = item.PostContent;
                tidingsModel.PostUserAccount = item.PostUser;
                tidingsModel.PostUsername = dic[item.PostUser];
                tidingsModel.ReviceUsername = dic[item.ReviceUser];
                tidingsModel.ReviceUserAccount = item.ReviceUser;
                tidingsModel.Url = item.Url;
                tidingsModel.PostDate = item.CreateTime.ToString("yyyy-MM-dd hh:mm");
                tidingsModels.Add(tidingsModel);
            }
            return tidingsModels;
        }

        public int SelectCount(TidingsCondition tidingsCondition = null)
        {
            Expression<Func<Tidings, bool>> where = TidingsCondition.BuildExpression(tidingsCondition);
            return _tidingsRepository.SelectCount(where);
        }
    }
}
