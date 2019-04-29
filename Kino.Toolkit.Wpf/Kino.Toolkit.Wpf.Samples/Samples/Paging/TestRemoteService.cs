using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Kino.Toolkit.Wpf.Samples
{
    public class TestRemoteService : ILoadOperation
    {
        private readonly DispatcherTimer _timer;
        private readonly Collection<TestModel> _source;

        public TestRemoteService()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += OnTmerTick;
            _source = new Collection<TestModel>();
            for (int i = 0; i < 100; i++)
            {
                _source.Add(new TestModel { Name = "Id" + i });
            }
            TotalCount = _source.Count;

        }

        public IEnumerable Result { get; set; }

        public object UserState { get; set; }

        public Exception Error { get; set; }

        public int TotalCount { get; set; }

        public bool IsCanceled { get; set; }

        public bool CanCancel { get; set; }

        public event EventHandler Completed;

        public void Cancel()
        {
        }

        public void LoadData(int pageIndex, int pageSize)
        {
            _timer.Start();
            Result = new Collection<TestModel>(_source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public async Task<ILoadResult> LoadDataAsync(int pageIndex, int pageSize)
        {
            var collection = new Collection<TestModel>(_source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
            var result = new LoadResult
            {
                Result = collection,
                TotalCount = TotalCount
            };
            await Task.Delay(TimeSpan.FromSeconds(2));
            return result;
        }

        private void OnTmerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }


    //public class LoadDataCompletedEventArgs : EventArgs
    //{
    //    public Exception Error { get; private set; }

    //    public Collection<TestModel> Result { get; private set; }

    //    public int TotalItemCount { get; private set; }

    //    public LoadDataCompletedEventArgs(Collection<TestModel> result, int toalItemCount, Exception error)
    //    {
    //        Result = result;
    //        TotalItemCount = toalItemCount;
    //        Error = error;
    //    }
    //}


}
