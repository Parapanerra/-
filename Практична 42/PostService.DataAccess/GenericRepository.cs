using PostService.Models;

namespace PostService.DataAccess;

public class GenericRepository<T> : IRepository<T> where T : Model
{
    // Кожен конкретний тип T має власний список і лічильник.
    private static readonly List<T> Entities = new();
    private static int _nextId = 1;

    public T Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (Entities.Contains(entity))
        {
            throw new InvalidOperationException("Цю сутність уже додано до репозиторію.");
        }

        entity.Id = _nextId++;
        Entities.Add(entity);
        return entity;
    }

    public T? GetById(int id) => Entities.FirstOrDefault(entity => entity.Id == id);

    // Копія списку захищає його склад від змін ззовні.
    public List<T> GetAll() => new(Entities);

    public bool Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        int index = Entities.FindIndex(existing => existing.Id == entity.Id);
        if (index < 0) return false;

        Entities[index] = entity;
        return true;
    }

    public bool Delete(int id) => Entities.RemoveAll(entity => entity.Id == id) > 0;
}
