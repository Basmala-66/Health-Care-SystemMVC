using System.ComponentModel.DataAnnotations;
namespace Health_Care_System.PL.ViewModels;
public class AppointmentViewModel { [Required] public int PatientId{get;set;} [Required] public int DoctorId{get;set;} [Required,DataType(DataType.DateTime)] public DateTime AppointmentDate{get;set;} public string? PatientName{get;set;} public string? DoctorName{get;set;} }
