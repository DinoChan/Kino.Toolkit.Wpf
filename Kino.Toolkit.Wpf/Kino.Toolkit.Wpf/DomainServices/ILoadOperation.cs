using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public interface ILoadOperation : ILoadResult
    {
        event EventHandler Completed;

        object UserState { get; }

        bool CanCancel { get; }

        void Cancel();
    }
}
