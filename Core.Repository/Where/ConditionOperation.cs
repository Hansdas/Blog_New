using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Where
{
   public enum ConditionOperation
    {
        None = 0,
        Equal = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThan = 4,
        LessThanOrEqual = 5,
        Contains = 6,
        StartWith = 7,
        EndWidth = 8,
        Range = 9
    }
}
