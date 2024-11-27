using AutoMapper;
using PatientRegistartionService.Data;
using PatientRegistartionService.Models;

namespace PatientRegistartionService.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {            
            CreateMap<Patients, PatientModel>().ReverseMap();
        }
    }
}
