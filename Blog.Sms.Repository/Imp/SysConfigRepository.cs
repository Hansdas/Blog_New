using Blog.Sms.Domain;
using Blog.Sms.Repository.DB;
using Core.Repository.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Sms.Repository.Imp
{
    public class SysConfigRepository:Repository<SysConfig,int>,ISysConfigRepository
    {
        public SysConfigRepository(DBContext dbContext) : base(dbContext)
        {

        }

        public string SelectValue(string key)
        {
            string value= AsQueryable(s => s.Key == key).Select(s => s.Value).First();
            return value;
        }
    }
}
