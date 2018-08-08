using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
   public interface ILoadResult
    {
        IEnumerable Result { get; }

        Exception Error { get; }

        int TotalCount { get; }

        bool IsCanceled { get; set; }
    }
}
