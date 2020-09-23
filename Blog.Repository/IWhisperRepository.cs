using Blog.Domain;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository
{
   public interface IWhisperRepository: IRepository<Whisper, int>
    {
    }
}
