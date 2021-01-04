using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Repository;
using Core.Common;
using Core.Common.EnumExtensions;

namespace Blog.Application.Service.imp
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        private UserDTO ConvertToDto(User user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Username = user.Username;
            userDTO.Account = user.Account;
            userDTO.Sex = user.Sex.GetEnumText();
            userDTO.BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value.ToString("yyyy-MM-dd") : "";
            userDTO.Email = user.Email;
            userDTO.Sign = user.Sign;
            userDTO.Phone = user.Phone;
            userDTO.IsValid = user.IsValid;
            userDTO.HeadPhoto = user.HeadPhoto;
            return userDTO;
        }
        public UserDTO SelectSingle(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }
        public UserDTO Login(string account,string password)
        {
            User user = _userRepository.SelectSingle(s => s.Account == account);
            if (user == null)
                throw new AuthException("用户名不存在或密码错误");
            if(user.Password!= EncrypUtil.MD5Encry(password))
                throw new AuthException("用户名不存在或密码错误");
            return ConvertToDto(user);
        }
    }
}
