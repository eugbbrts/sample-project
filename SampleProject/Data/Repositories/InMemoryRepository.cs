using BusinessEntities;
using Common;
using System;
using System.Collections.Concurrent;

namespace Data.Repositories
{    
    public class InMemoryRepository<T> : IRepository<T> where T : IdObject
    {
        protected static ConcurrentDictionary<Guid, T> _dataStore = new ConcurrentDictionary<Guid, T>();

        public void Save(T entity)
        {
            _dataStore[entity.Id] = entity;
        }

        public void Delete(T entity)
        {
            _dataStore.TryRemove(entity.Id, out _);
        }

        public T Get(Guid id)
        {
            if (_dataStore.TryGetValue(id, out T data))
                return data;

            return null;
        }
    }
}
