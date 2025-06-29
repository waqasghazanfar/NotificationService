namespace NotificationService.Application.Mappings
{
    using AutoMapper;
    using NotificationService.Application.DTOs;
    using NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting;
    using NotificationService.Application.Features.SmtpSettings.Commands.UpdateSmtpSetting;
    using NotificationService.Application.Features.SmtpSettings.Queries;
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

            // SmtpSetting Mappings
            CreateMap<SmtpSetting, SmtpSettingsListVm>().ReverseMap();
            CreateMap<SmtpSetting, SmtpSettingDto>().ReverseMap();
            CreateMap<SmtpSetting, CreateSmtpSettingCommand>().ReverseMap();
            CreateMap<SmtpSetting, UpdateSmtpSettingCommand>().ReverseMap();

            // NotificationLog Mappings
            CreateMap<NotificationLog, NotificationLogDto>();
        }
    }
}