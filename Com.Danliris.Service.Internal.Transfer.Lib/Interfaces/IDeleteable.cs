﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Interfaces
{
    public interface IDeleteable
    {
        Task<int> Delete(int id);
    }
}
