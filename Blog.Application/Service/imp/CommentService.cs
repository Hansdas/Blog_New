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
            IEnumerable<Comment> comments = _commentRepository.Select(s=>ids.Contains(s.Id));
            List<string> accounts = comments.Select(s => s.PostUser).ToList();
            accounts.AddRange(comments.Select(s => s.RevicerUser));
            Dictionary<string, string> accountWithName = _userRepository.AccountWithName(accounts.Distinct());
            IList<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach (var item in comments)
            {
                CommentDTO commentDTO = new CommentDTO();
                commentDTO.Guid = item.Id;
                commentDTO.Content = item.Content;
                commentDTO.PostUser = item.PostUser;
                commentDTO.PostUsername = accountWithName[item.PostUser];
                commentDTO.Revicer = item.RevicerUser;
                commentDTO.RevicerName = accountWithName[item.RevicerUser];
                commentDTO.AdditionalData = item.AdditionalData;
                commentDTO.CommentType = item.CommentType.GetEnumValue();
                commentDTOs.Add(commentDTO);
            }
            return commentDTOs;
        }
    }
}
                                           