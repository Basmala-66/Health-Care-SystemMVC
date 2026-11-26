using Health_Care_System.Entities;
namespace Health_Care_System.BLL.Interfaces;
public interface IUnitOfWork { IGenericRepository<Patient> Patients{get;} IGenericRepository<Doctor> Doctors{get;} IAppointmentRepository Appointments{get;} Task<int> SaveChangesAsync(); }
