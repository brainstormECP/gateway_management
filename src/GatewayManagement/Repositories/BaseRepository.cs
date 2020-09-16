using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatewayManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GatewayManagement.Repositories
{
    public class Result
    {
        public bool Status { get; set; }

        public string Detail { get; set; }

        public object Entity { get; set; }
    }

    public abstract class BaseRepository<T>
    {

        protected DbContext _db { get; set; }

        public BaseRepository(DbContext db)
        {
            _db = db;
            _db.Database.EnsureCreated();
        }

        public virtual async Task<IEnumerable<T>> FindAll()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> FindById(int id)
        {
            throw new NotImplementedException();
        }


        public virtual async Task<Result> Create(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Result> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Result> Delete(int id)
        {
            throw new NotImplementedException();
        }

        protected virtual bool Exists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
