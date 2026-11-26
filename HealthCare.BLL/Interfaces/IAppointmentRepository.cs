using Health_Care_System.Entities;
namespace Health_Care_System.BLL.Interfaces;
public interface IAppointmentRepository { Task<IEnumerable<Appointment>> GetAllAsync(); Task<Appointment?> GetAsync(int patientId,int doctorId); Task CreateAsync(Appointment entity); void Update(Appointment entity); void Delete(Appointment entity); }
