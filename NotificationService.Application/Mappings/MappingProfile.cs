namespace NotificationService.Application.Mappings
{
    using AutoMapper;
    using NotificationService.Application.DTOs;
    using NotificationService.Application.Features.Templates.Commands.CreateTemplate;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;
    using NotificationService.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Template, TemplateListVm>().ReverseMap();
            CreateMap<Template, TemplateDto>().ReverseMap();
            CreateMap<Template, CreateTemplateCommand>().ReverseMap();
        }
    }
}