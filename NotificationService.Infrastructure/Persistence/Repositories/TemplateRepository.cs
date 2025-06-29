namespace NotificationService.Infrastructure.Persistence.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class TemplateRepository : BaseRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Template?> GetTemplateAsync(string name, string channel, string locale)
        {
            return await _dbContext.Templates
                .FirstOrDefaultAsync(t => t.Name == name && t.Channel == channel && t.Locale == locale && t.IsActive);
        }
    }
}