using System;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
namespace Bulky.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{

    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbset;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbset = _db.Set<T>();
        _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
    }

    public void Add(T entity)
    {
        dbset.Add(entity);
    }

    public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter, string? includeProprieties = null)
    {
        IQueryable<T> query = dbset;
        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProprieties))
        {
            foreach (var includeProp in includeProprieties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(string? includeProprieties = null)
    {
        IQueryable<T> query = dbset;

        if (!string.IsNullOrEmpty(includeProprieties))
        {
            foreach (var includeProp in includeProprieties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbset.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbset.RemoveRange(entity);
    }
}
