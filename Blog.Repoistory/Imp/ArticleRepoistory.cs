using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Article;
using Blog.Repoistory.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repoistory.Imp
{
    public class ArticleRepoistory :Repoistory<Article,int>, IArticleRepoistory
    {
        public ArticleRepoistory(DBContext dbContext) : base(dbContext)
        {

        }
    }
}
