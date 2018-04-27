﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Interfaces
{
    public interface ICreateable<TModel>
    {
        Task<int> Create(TModel model);
    }
}
