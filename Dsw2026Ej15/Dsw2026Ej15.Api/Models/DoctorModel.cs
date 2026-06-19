using Dsw2026Ej15.Data.Dtos;

namespace Dsw2026Ej15.Api.Models
{
    public record DoctorModel
    {
        public record Request(string Name, string LicenseNumber, Guid SpecialityId);


        public record Response(Guid Id, string Name, string LicenseNumber, SpecialityDto Speciality);

        public record GetByIdResponse(string Name, string LicenseNumber, string SpecialityName);
    }

}