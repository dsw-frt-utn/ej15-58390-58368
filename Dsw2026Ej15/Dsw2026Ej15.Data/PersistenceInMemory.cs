using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Data.Dtos;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private  List<Speciality> _speciatilities = [];
        private  List<Doctor> _doctors = [];

        public PersistenceInMemory() 
        {
           

            LoadSpecialities();
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _doctors;
        }

        public void SaveDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

        Speciality? IPersistence.GetSpecialityById(Guid id)
        {
            return _speciatilities.SingleOrDefault(e => e.Id == id);
        }

        private void LoadSpecialities()
        {

            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");

                var json = File.ReadAllText(jsonPath);
                var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                }) ?? [];
                _speciatilities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];

            }
            catch(Exception)
            {
                
            }
            
            }

        

    }
}
