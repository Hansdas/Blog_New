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
        private User ConvertFromDto(UserDTO userDTO, User user = null)
        {
            if (user == null)
                user = new User();
            user.Account = userDTO.Account;
            user.Username = userDTO.Username;
            user.LoginType = userDTO.LoginType;
            user.Sex = Enum.Parse<Sex>(userDTO.Sex);
            user.HeadPhoto = userDTO.HeadPhoto;
            if (!string.IsNullOrEmpty(userDTO.BirthDate))
                user.BirthDate = Convert.ToDateTime(userDTO.BirthDate);
            user.Email = userDTO.Email;
            user.Sign = userDTO.Sign;
            user.Phone = userDTO.Phone;
            user.IsValid = true;
            return user;
        }
        public UserDTO SelectSingle(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }
        public UserDTO Login(string account, string password)
        {
            User user = _userRepository.SelectSingle(s => s.Account == account);
            if (user == null)
                throw new AuthException("用户名不存在或密码错误");
            if (user.Password != EncrypUtil.MD5Encry(password))
                throw new AuthException("用户名不存在或密码错误");
            return ConvertToDto(user);
        }

        public void Create(UserDTO userDTO)
        {
            int count = _userRepository.SelectCount(s => s.Account == userDTO.Account);
            if (count > 0)
                throw new ValidationException(string.Format("已存在账号：{0}", userDTO.Account));
            userDTO.LoginType = LoginType.None;
            User user = ConvertFromDto(userDTO);
            user.CreateTime = DateTime.Now;
            _userRepository.Insert(user);

        }
        public void CreateQQUser(UserDTO userDTO)
        {
            User user = _userRepository.SelectSingle(s => s.Account == userDTO.Account);
            if (user == null)
            {
                user = ConvertFromDto(userDTO);
                user.CreateTime = DateTime.Now;
                _userRepository.Insert(user);
            }
            else
            {
                user = ConvertFromDto(userDTO,user);
                _userRepository.Update(user);
            }

        }
    }
}
