using Health_Care_System.Entities;
namespace Health_Care_System.BLL.Interfaces;
public interface IGenericRepository<TEntity> where TEntity:BaseEntity { Task CreateAsync(TEntity entity); void Update(TEntity entity); void Delete(TEntity entity); Task<TEntity?> GetAsync(int id); Task<IEnumerable<TEntity>> GetAllAsync(); Task<IEnumerable<TEntity>> SearchAsync(string searchValue); }
