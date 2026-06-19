using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Exceptions;


namespace Dsw2026Ej15.Api.Controllers
{
    
    public class DoctorsController : AppController
    {
        private readonly IPersistence _persistence;

        public DoctorsController (IPersistence persistence)
        {
            _persistence = persistence;
        }



        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y matricula son requeridos. ");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);

            if(speciality is null)
            {
                throw new ValidationException("Especialidad no existe");
            }

            var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
            _persistence.SaveDoctor(doctor);
            return Created();
        }
        [HttpGet]
        public async Task<IActionResult> GetActiveDoctors()
        {
            var allDoctors = _persistence.GetAllDoctors();

            var activeDoctors = allDoctors.Where(d => d.IsActive).Select(d => new DoctorModel.Response(d.Id,d.Name,d.LicenseNumber,
        new SpecialityDto(d.Speciality!.Id, d.Speciality!.Name, d.Speciality!.Description))).ToList();

            return Ok(activeDoctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetAllDoctors().FirstOrDefault(d => d.Id == id);
            if(doctor is null || !doctor.IsActive)
            {
                return NotFound("El medico no existe o no esta activo");
            }
            var response = new DoctorModel.GetByIdResponse(doctor.Name, doctor.LicenseNumber,doctor.Speciality != null ? doctor.Speciality.Name : string.Empty);
            
            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = _persistence.GetAllDoctors().FirstOrDefault(d => d.Id == id);
            if(doctor is null || !doctor.IsActive)
            {
                return NotFound("El medico no existe o no esta activo");
            }
            doctor.Deactivate();
            _persistence.SaveDoctor(doctor);

            return NoContent();
             
        }
    }
}
