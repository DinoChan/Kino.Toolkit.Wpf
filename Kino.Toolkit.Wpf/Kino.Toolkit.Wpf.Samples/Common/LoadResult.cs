using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf.Samples
{
    public class LoadResult : ILoadResult
    {
        public IEnumerable Result { get; set; }

        public Exception Error { get; set; }

        public int TotalCount { get; set; }

        public bool IsCanceled { get; set; }
    }
}
