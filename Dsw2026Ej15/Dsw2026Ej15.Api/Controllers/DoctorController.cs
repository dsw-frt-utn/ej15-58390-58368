using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data.Dtos;


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
                return BadRequest("Nombre y matricula son requeridos. ");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);

            if(speciality is null)
            {
                return BadRequest("Especialidad no existe");
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
    }
}
