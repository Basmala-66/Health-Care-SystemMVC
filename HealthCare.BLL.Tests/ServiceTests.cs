using Health_Care_System.BLL.Interfaces;
using Health_Care_System.BLL.Services;
using Health_Care_System.Entities;
using Xunit;

namespace HealthCare.BLL.Tests;

public sealed class ServiceTests
{
    [Fact]
    public async Task PatientGetByIdReturnsMatchingPatient()
    {
        var patient = new Patient { Id = 1, Name = "Alex" };
        var unitOfWork = new FakeUnitOfWork();
        unitOfWork.Patients.Items.Add(patient);

        var result = await new PatientService(unitOfWork).GetByIdAsync(patient.Id);

        Assert.True(result.Successeded);
        Assert.Same(patient, result.Data);
    }

    [Fact]
    public async Task PatientGetByIdReturnsFailureWhenPatientIsMissing()
    {
        var result = await new PatientService(new FakeUnitOfWork()).GetByIdAsync(42);

        Assert.False(result.Successeded);
        Assert.Equal("Patient not found.", result.Error);
    }

    [Fact]
    public async Task PatientCreateAddsPatientAndSavesChanges()
    {
        var unitOfWork = new FakeUnitOfWork();
        var patient = new Patient { Name = "Alex" };

        var result = await new PatientService(unitOfWork).CreateAsync(patient);

        Assert.True(result.Successeded);
        Assert.Same(patient, Assert.Single(unitOfWork.Patients.Items));
        Assert.Equal(1, unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task PatientUpdateCopiesFieldsAndSavesChanges()
    {
        var unitOfWork = new FakeUnitOfWork();
        var existing = new Patient { Id = 1, Name = "Old name", BirthDate = new DateTime(2000, 1, 1) };
        var updated = new Patient { Id = 1, Name = "New name", BirthDate = new DateTime(2001, 2, 3) };
        unitOfWork.Patients.Items.Add(existing);

        var result = await new PatientService(unitOfWork).UpdateAsync(updated);

        Assert.True(result.Successeded);
        Assert.Same(existing, result.Data);
        Assert.Equal("New name", existing.Name);
        Assert.Equal(updated.BirthDate, existing.BirthDate);
        Assert.Same(existing, unitOfWork.Patients.UpdatedEntity);
        Assert.Equal(1, unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task PatientUpdateDoesNotSaveWhenPatientIsMissing()
    {
        var unitOfWork = new FakeUnitOfWork();

        var result = await new PatientService(unitOfWork).UpdateAsync(new Patient { Id = 42 });

        Assert.False(result.Successeded);
        Assert.Equal("Patient not found.", result.Error);
        Assert.Equal(0, unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task DoctorGetAllUsesSearchAndReturnsPaginatedResults()
    {
        var unitOfWork = new FakeUnitOfWork();
        var doctor = new Doctor { Id = 1, Name = "Dr. Lin", Specialization = "Cardiology" };
        unitOfWork.Doctors.SearchResults = new[] { doctor };

        var result = await new DoctorService(unitOfWork).GetAllAsync("Lin", 1, 10);

        Assert.True(result.Successeded);
        Assert.Same(doctor, Assert.Single(result.Data!));
        Assert.Equal("Lin", unitOfWork.Doctors.LastSearchValue);
    }

    [Fact]
    public async Task DoctorDeleteRemovesDoctorAndSavesChanges()
    {
        var unitOfWork = new FakeUnitOfWork();
        var doctor = new Doctor { Id = 7, Name = "Dr. Lin", Specialization = "Cardiology" };
        unitOfWork.Doctors.Items.Add(doctor);

        var result = await new DoctorService(unitOfWork).DeleteAsync(doctor.Id);

        Assert.True(result.Successeded);
        Assert.Same(doctor, unitOfWork.Doctors.DeletedEntity);
        Assert.Equal(1, unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task AppointmentGetAllSearchesNamesWithoutCaseSensitivity()
    {
        var matchingAppointment = new Appointment
        {
            Patient = new Patient { Name = "Morgan Smith" },
            Doctor = new Doctor { Name = "Dr. Lee" }
        };
        var otherAppointment = new Appointment
        {
            Patient = new Patient { Name = "Taylor Jones" },
            Doctor = new Doctor { Name = "Dr. Patel" }
        };
        var unitOfWork = new FakeUnitOfWork();
        unitOfWork.Appointments.Items.AddRange([matchingAppointment, otherAppointment]);

        var result = await new AppointmentService(unitOfWork).GetAllAsync("SMITH", 1, 10);

        Assert.True(result.Successeded);
        Assert.Same(matchingAppointment, Assert.Single(result.Data!));
    }

    [Fact]
    public async Task AppointmentUpdateReturnsFailureWithoutSavingWhenMissing()
    {
        var unitOfWork = new FakeUnitOfWork();

        var result = await new AppointmentService(unitOfWork).UpdateAsync(
            new Appointment { PatientId = 1, DoctorId = 2 });

        Assert.False(result.Successeded);
        Assert.Equal("Appointment not found.", result.Error);
        Assert.Equal(0, unitOfWork.SaveChangesCount);
    }

    [Fact]
    public async Task AppointmentDeleteRemovesExistingAppointmentAndSavesChanges()
    {
        var unitOfWork = new FakeUnitOfWork();
        var appointment = new Appointment { PatientId = 1, DoctorId = 2 };
        unitOfWork.Appointments.Items.Add(appointment);

        var result = await new AppointmentService(unitOfWork).DeleteAsync(1, 2);

        Assert.True(result.Successeded);
        Assert.Same(appointment, unitOfWork.Appointments.DeletedEntity);
        Assert.Equal(1, unitOfWork.SaveChangesCount);
    }
}

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public FakeRepository<Patient> Patients { get; } = new();
    public FakeRepository<Doctor> Doctors { get; } = new();
    public FakeAppointmentRepository Appointments { get; } = new();
    public int SaveChangesCount { get; private set; }

    IGenericRepository<Patient> IUnitOfWork.Patients => Patients;
    IGenericRepository<Doctor> IUnitOfWork.Doctors => Doctors;
    IAppointmentRepository IUnitOfWork.Appointments => Appointments;

    public Task<int> SaveChangesAsync()
    {
        SaveChangesCount++;
        return Task.FromResult(1);
    }
}

internal sealed class FakeRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    public List<TEntity> Items { get; } = [];
    public IEnumerable<TEntity>? SearchResults { get; set; }
    public string? LastSearchValue { get; private set; }
    public TEntity? UpdatedEntity { get; private set; }
    public TEntity? DeletedEntity { get; private set; }

    public Task CreateAsync(TEntity entity)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(TEntity entity) => UpdatedEntity = entity;

    public void Delete(TEntity entity) => DeletedEntity = entity;

    public Task<TEntity?> GetAsync(int id) =>
        Task.FromResult(Items.SingleOrDefault(item => item.Id == id));

    public Task<IEnumerable<TEntity>> GetAllAsync() =>
        Task.FromResult<IEnumerable<TEntity>>(Items);

    public Task<IEnumerable<TEntity>> SearchAsync(string searchValue)
    {
        LastSearchValue = searchValue;
        return Task.FromResult(SearchResults ?? Items);
    }
}

internal sealed class FakeAppointmentRepository : IAppointmentRepository
{
    public List<Appointment> Items { get; } = [];
    public Appointment? DeletedEntity { get; private set; }

    public Task<IEnumerable<Appointment>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Appointment>>(Items);

    public Task<Appointment?> GetAsync(int patientId, int doctorId) =>
        Task.FromResult(Items.SingleOrDefault(item =>
            item.PatientId == patientId && item.DoctorId == doctorId));

    public Task CreateAsync(Appointment entity)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(Appointment entity) { }

    public void Delete(Appointment entity) => DeletedEntity = entity;
}
