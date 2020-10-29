using Blog.Domain;
using Blog.Repository.DB;
using Blog.Repository;
using Blog.Repository.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Repository.Imp;

namespace Blog.Repository.Imp
{
  public  class UserRepository: Repository<User, int>, IUserRepository
    {
        public UserRepository(DBContext dbContext) : base(dbContext)
        {

        }

        public Dictionary<string, string> AccountWithName(IEnumerable<string> accounts)
        {
            Expression<Func<User, bool>> where = s => accounts.Contains(s.Account);
            IEnumerable<User> users = AsQueryable(where).Select(s => new User() { Username = s.Username, Account = s.Account }).AsEnumerable();
            Dictionary<string, string> accountWithName = new Dictionary<string, string>();
            foreach (var item in users)
                accountWithName.Add(item.Account, item.Username);
            return accountWithName;
        }
    }
}
