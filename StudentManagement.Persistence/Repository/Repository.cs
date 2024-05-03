using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StudentManagement.Persistence.Repository.IRepository;

namespace StudentManagement.Persistence.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbset = _db.Set<T>();

        }


        public async Task CreateAsync(T entity)
        {

            await dbset.AddAsync(entity);
            await SaveAsync();

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, int pageSize = 3, int pageNumber = 1)
        {

            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            }
           

            //the query hasn't been executed against the database yet. It's just a query definition.
            return await query.ToListAsync(); //Deferred execution  occurs here 
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbset;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //the query hasn't been executed against the database yet. It's just a query definition.
            return await query.FirstOrDefaultAsync(); //Deferred execution  occurs here 

        }

        public async Task RemoveAsync(T entity)
        {
            dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

     
    }
}
