using Health_Care_System.Entities; using Health_Care_System.BLL.Models;
namespace Health_Care_System.BLL.Interfaces;
public interface IDoctorService { Task<Result<PaginatedList<Doctor>>> GetAllAsync(string? searchValue,int pageIndex,int pageSize); Task<Result<Doctor>> GetByIdAsync(int id); Task<Result<Doctor>> CreateAsync(Doctor entity); Task<Result<Doctor>> UpdateAsync(Doctor entity); Task<Result<Doctor>> DeleteAsync(int id); }
