using Blog.Domain.Article;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repoistory
{
   public interface IArticleRepoistory:IRepoistory<Article,int>
    {
    }
}
