using Blog.Domain;
using Blog.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Repository.Imp
{
  public  class WhisperRepository: Repository<Whisper, int>,IWhisperRepository
    {
        public WhisperRepository(DBContext dbContext) : base(dbContext)
        {

        }
        public override IEnumerable<Whisper> SelectByPage(int currentPage, int pageSize, Expression<Func<Whisper, bool>> where = null, Expression<Func<Whisper, object>> orderBy = null)
        {
            base.desc = true;
            return base.SelectByPage(currentPage, pageSize, where, orderBy);
        }
    }
}
