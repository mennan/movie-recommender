using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MovieRecommender.Entity;
using MovieRecommender.Repository.Contracts;

namespace MovieRecommender.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MovieRecommenderContext _context;
        private readonly DbSet<TEntity> _entity;

        public Repository(MovieRecommenderContext context)
        {
            _context = context;
            _entity = context.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll()
        {
            return _entity.AsQueryable();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.Where(predicate).AsQueryable();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _entity.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
    }
}