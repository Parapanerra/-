using PostService.Models;

namespace PostService.DataAccess;

public interface IRepository<T> where T : Model
{
    T Add(T entity);
    T? GetById(int id);
    List<T> GetAll();
    bool Update(T entity);
    bool Delete(int id);
}
