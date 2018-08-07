using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public interface ILoadOperation
    {
        IEnumerable Result { get; }

        object UserState { get; }

        Exception Error { get; }

        int TotalCount { get; }

        event EventHandler Completed;

        bool IsCanceled { get; set; }

        bool CanCancel { get; }

        void Cancel();
    }
}
