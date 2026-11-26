using Health_Care_System.Entities; using Health_Care_System.BLL.Models;
namespace Health_Care_System.BLL.Interfaces;
public interface IAppointmentService { Task<Result<PaginatedList<Appointment>>> GetAllAsync(string? searchValue,int pageIndex,int pageSize); Task<Result<Appointment>> GetByIdAsync(int patientId,int doctorId); Task<Result<Appointment>> CreateAsync(Appointment entity); Task<Result<Appointment>> UpdateAsync(Appointment entity); Task<Result<Appointment>> DeleteAsync(int patientId,int doctorId); }
