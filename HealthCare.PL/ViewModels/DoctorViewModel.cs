using System.ComponentModel.DataAnnotations;
namespace Health_Care_System.PL.ViewModels;
public class DoctorViewModel { public int Id{get;set;} [Required,StringLength(100,MinimumLength=3)] public string Name{get;set;}=string.Empty; [Required,StringLength(100)] public string Specialization{get;set;}=string.Empty; }
