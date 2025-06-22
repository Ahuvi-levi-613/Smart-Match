using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRepository<T>
{
    Task<T> GetById(int id);
    Task<List<T>> GetAll();
    Task<T> AddItem(T item);
    Task DeleteItem(int id);
    Task UpdateItem(int id, T item);
}

