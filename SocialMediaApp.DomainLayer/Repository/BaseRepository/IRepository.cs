using Microsoft.EntityFrameworkCore.Query;
using SocialMediaApp.DomainLayer.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.DomainLayer.Repository.BaseRepository
{
    //Repository temel olarak veritabanı sorgulama işlemlerinin bir merkezden yapılmasını sağlayarak iş katmanına bu işlerin taşınmasını önler bu şekilde sorgu ve kod tekrarına engel olmuş olur.
    public interface IRepository<T> where T:class, IBaseEntity // ???
    {
        Task<List<T>> GetAll(); // Herşeyi getir.
        Task<List<T>> Get(Expression<Func<T, bool>> expression); // yazdığımız lingto sorgusunu listeliyor. 
        Task<T> GetById(int id); // id ye göre getiriyor.
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression); // yazdığımız ling to sorgusunu first or default olarak getir.
        Task<bool> Any(Expression<Func<T, bool>> expression); // yazdığımız lingto sorgusu bool dönüyor.

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        
        //Lingto sorgusuna birden çok tablosunun dahil edilmesi ve bunların 
        Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector,
                                                         Expression<Func<T, bool>> expression = null, 
                                                         Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true);
        Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> selector,
                                                     Expression<Func<T, bool>> expression = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true, int pageIndex = 1, int pageSize = 3);
    }
}
