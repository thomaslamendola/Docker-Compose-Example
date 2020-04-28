using FizzWare.NBuilder;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using SampleApp.MVC.Attributes;
using SampleApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApp.MVC.Repositories
{
    public class MockRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IDocument
    {
        private readonly IQueryable<TDocument> _collection;

        public MockRepository()
        {
            _collection = Builder<TDocument>.CreateListOfSize(5).Build().AsQueryable();
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Where(filterExpression).AsEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            var mockResult = _collection.Where(filterExpression).Select(projectionExpression).AsEnumerable();
            return mockResult;
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Where(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Where(filterExpression).FirstOrDefault());
        }

        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            return _collection.Where(d => d.Id.Equals(objectId)).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                return _collection.Where(d => d.Id.Equals(objectId)).SingleOrDefault();
            });
        }


        public virtual void InsertOne(TDocument document)
        {
            throw new NotImplementedException();
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            throw new NotImplementedException();
        }


        public virtual Task InsertManyAsync(ICollection<TDocument> documents)
        {
            throw new NotImplementedException();
        }

        public void ReplaceOne(TDocument document)
        {
            throw new NotImplementedException();
        }

        public virtual Task ReplaceOneAsync(TDocument document)
        {
            throw new NotImplementedException();
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }
    }
}
