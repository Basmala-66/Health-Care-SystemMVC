using Health_Care_System.Entities; using Health_Care_System.BLL.Models;
namespace Health_Care_System.BLL.Interfaces;
public interface IPatientService { Task<Result<PaginatedList<Patient>>> GetAllAsync(string? searchValue,int pageIndex,int pageSize); Task<Result<Patient>> GetByIdAsync(int id); Task<Result<Patient>> CreateAsync(Patient entity); Task<Result<Patient>> UpdateAsync(Patient entity); Task<Result<Patient>> DeleteAsync(int id); }
