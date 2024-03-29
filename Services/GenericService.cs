﻿using Microsoft.EntityFrameworkCore;
using Blog.Context;
using Blog.Models;
using Blog.ReponseExceptions;

namespace Blog.Services
{
    public interface IGenericService<T> where T : BaseEntity
    {
        public IEnumerable<T> FindAll(int? page, int? offset);
        public T? FindById(int id);
        public T? Create(T creationDto);
        public T? Update(T updateDto);
        public T? Delete(int id);

    }
    public class GenericService<T>: IGenericService<T> where T : BaseEntity
    {   
        protected readonly BlogDbContext _context;
        protected readonly DbSet<T> _repository;

        public GenericService(BlogDbContext context)
        {
            _context = context;
            _repository = _context.Set<T>();
        }

        public IEnumerable<T> FindAll(int? page, int? offset)
        {
            var allElements = _repository.ToList();
            if(page != null)
            {
                if(offset == null)
                {
                    throw new ArgumentNullException(nameof(offset));
                }
                allElements = allElements.Skip((page-1) * offset ?? 0).Take(offset ?? 10).ToList();
            }
            return allElements;

        }


        public T? FindById(int id)
        {
            return _repository.Find(id);
        }

        public T? Create(T creationDto)
        {
            var created = _repository.Add(creationDto);
            _context.SaveChanges();
            //  return _repository.FirstOrDefault(el => el.Id == created.Id);
            return created.Entity; 
        }

        public T? Update(T updateDto)
        {
            var updated = _repository.Update(updateDto);
            _context.SaveChanges();
             
            return updated.Entity; 
        }

        public T? Delete(int id)
        {
            var entity = _repository.Find(id);
            if(entity == null)
            {
                throw new NotFoundException($"{typeof(T)} Not Found");
            }
            else
            {
                _repository.Remove(entity);
                _context.SaveChanges(); 
            }
            return entity;
        }
    }
}
