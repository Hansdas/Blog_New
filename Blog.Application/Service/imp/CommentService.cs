using Blog.Application.DTO;
using Blog.Domain;
using Blog.Repository;
using Core.Common.EnumExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Application.Service.imp
{
   public class CommentService: ICommentService
    {
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }
        public IList<CommentDTO> Select(IEnumerable<string> ids)
        {
            if (ids.Count() == 0 || ids == null)
                return null;
            IEnumerable<Comment> comments = _commentRepository.Select(s=>ids.Contains(s.Guid));
            List<string> accounts = comments.Select(s => s.PostUser).ToList();
            accounts.AddRange(comments.Select(s => s.RevicerUser));
            IList<User> users=_userRepository.Select(s => accounts.Distinct().Contains(s.Account)).ToList();
            IList<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach (var item in comments)
            {
                User postUser = users.First(s=>s.Account==item.PostUser);
                CommentDTO commentDTO = new CommentDTO();
                commentDTO.Guid = item.Id;
                commentDTO.Content = item.Content;
                commentDTO.PostUser = item.PostUser;
                commentDTO.PostUsername = postUser.Username;
                commentDTO.PostUserPhoto = postUser.HeadPhoto;
                commentDTO.Revicer = item.RevicerUser;
                commentDTO.RevicerName = users.FirstOrDefault(s=>s.Account==item.RevicerUser).Username;
                commentDTO.AdditionalData = item.AdditionalData;
                commentDTO.CommentType = item.CommentType.GetEnumValue();
                commentDTO.PostDate = item.PostDate.ToString("yyyy-MM-dd hh:mm");
                commentDTOs.Add(commentDTO);
            }
            return commentDTOs;
        }
    }
}
                                           