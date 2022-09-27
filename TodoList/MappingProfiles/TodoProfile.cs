using AutoMapper;
using System;
using TodoListAPI.Dtos;
using TodoListAPI.Models;

namespace TodoListAPI.MappingProfiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<DateTime, DueTo>()
                .ForMember(dest => dest.Date,
                            opt => opt.MapFrom(src => src.ToString("d")))
                .ForMember(dest => dest.Time,
                            opt => opt.MapFrom(src => src.ToString("HH:mm")))
                .ReverseMap();

            MapDto<TodoDto>();
            MapDto<TodoDtoCreate>();
            MapDto<TodoDtoUpdate>();
        }

        public void MapDto<T>() where T : ITodoDto
        {
            CreateMap<Todo, T>()
                .ReverseMap()
                .ForPath(dest => dest.DueTo, opt => opt.MapFrom(src => ParseDueTo(src)));
        }
        public DateTime ParseDueTo(ITodoDto dto) => DateTime.Parse($"{dto.DueTo.Date} {dto.DueTo.Time}");
    }
}
