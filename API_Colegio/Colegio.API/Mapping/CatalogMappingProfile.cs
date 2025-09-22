using AutoMapper;
using Colegio.Domain.Entities;
using Colegio.Application.DTOs;

namespace Colegio.API.Mapping;

public class CatalogMappingProfile : Profile
{
    public CatalogMappingProfile()
    {
        CreateMap<Student, StudentResponseDTO>()
            .ForCtorParam("FullName", opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));

        CreateMap<Teacher, TeacherResponseDTO>()
            .ForCtorParam("FullName", opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));

        CreateMap<Subject, SubjectResponseDTO>();
        CreateMap<Period, PeriodResponseDTO>();

        CreateMap<StudentRequestDTO, Student>();
        CreateMap<TeacherRequestDTO, Teacher>();
        CreateMap<SubjectRequestDTO, Subject>();
        CreateMap<PeriodRequestDTO, Period>();
    }
}
