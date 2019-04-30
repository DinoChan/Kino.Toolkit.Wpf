using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf.Samples
{
    public class TestCurdService
    {
        private readonly List<TestModel> _models;
        //private readonly Func<TestModel, object> sort;

        public TestCurdService()
        {
            _models = new List<TestModel>();
            var random = new Random();

            for (int i = 0; i < 200; i++)
            {
                _models.Add(new TestModel { Name = "this is " + i, Age = random.Next(100), Id = i });
            }

        }

        public async Task<LoadResult> ReadModelsAsync(string filter, int pageSize, int pageIndex, Func<TestModel, object> sort, bool isDesc)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            IEnumerable<TestModel> resultModels = _models;
            if (string.IsNullOrWhiteSpace(filter) == false)
                resultModels = resultModels.Where(m => m.Name.Contains(filter));

            if (isDesc)
                resultModels = resultModels.OrderByDescending(sort);
            else
                resultModels = resultModels.OrderBy(sort);

            var totalCount = resultModels.Count();
            IEnumerable<TestModel> items = resultModels.Skip(pageIndex * pageSize).Take(pageSize);
            var result = new LoadResult { TotalCount = totalCount, Result = items };
            return result;
        }

        public async Task<TestModel> Create(TestModel model)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            model.Id = _models.Count;
            _models.Add(model);
            return model;
        }

        public async Task Delete(IEnumerable<int> ids)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            var models = _models.Where(m =>ids.Contains( m.Id )).ToList();
            foreach (TestModel model in models)
            {
                _models.Remove(model);
            }
        }

        public async Task Update(TestModel model)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            TestModel modelToUpdate = _models.FirstOrDefault(m => m.Id == model.Id);
            if (modelToUpdate != null)
            {
                modelToUpdate.Name = model.Name;
                modelToUpdate.Age = model.Age;
            }
        }
    }
}
