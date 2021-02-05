using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SocialMediaApp.DomainLayer.Entities.Interface;
using SocialMediaApp.DomainLayer.Repository.BaseRepository;
using SocialMediaApp.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Infrastructure.Repository.BaseRepository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _applicationDbContext;
        protected DbSet<T> _table;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
            this._table = _applicationDbContext.Set<T>();
        }


        public async Task Add(T entity) => await _table.AddAsync(entity);
        public async Task<bool> Any(Expression<Func<T, bool>> expression) => await _table.AnyAsync(expression);
        public void Delete(T entity) => _table.Remove(entity); // delete methodu asenkron olmaz.    
        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression) => await _table.Where(expression).FirstOrDefaultAsync();
        public async Task<List<T>> Get(Expression<Func<T, bool>> expression) => await _table.Where(expression).ToListAsync();
        public async Task<List<T>> GetAll() => await _table.ToListAsync();
        public async Task<T> GetById(int id) => await _table.FindAsync(id);



        //AsNoTracking; Entity Framework tarafından uygulamaların performansını optimize etmemize yardımcı olmak için geliştirilmiş bir fonksiyondur. İşlevsel olarak veritabanından sorgu neticesinde elde edilen nesnelerin takip mekanizması ilgili fonksiyon tarafından kırılarak, sistem tarafından izlenmelerine son verilmesini sağlamakta ve böylece tüm verisel varlıkların ekstradan işlenme yahut lüzumsuz depolanma süreçlerine maliyet ayrılmamaktadır.
        //AsNoTracking fonksiyonu ile takibi kırılmış tüm nesneler doğal olarak güncelleme durumlarında “SaveChanges” fonksiyonundan etkilenmeyecektirler.
        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _table;
            if (disableTracking) query = query.AsNoTracking(); // önemli bir olgu eğer getirdiklerimiz crud işlemlerinde kullanılırsa açık bırakırız. burada filtrede kullanılacağı için notracking kullanıcaz. 
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);
            if (orderby != null) return await orderby(query).Select(selector).FirstOrDefaultAsync();
            else return await query.Select(selector).FirstOrDefaultAsync();           
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true, int pageIndex = 1, int pageSize = 3)
        {
            IQueryable<T> query = _table;
            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);
            if (orderby != null) return await orderby(query).Select(selector).Skip((pageIndex - 1) * pageIndex).Take(pageSize).ToListAsync();
            else return await query.Select(selector).ToListAsync();
        }

        public void Update(T entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
