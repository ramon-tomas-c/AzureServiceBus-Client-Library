namespace SB.WebAPI.Infrastructure.ServiceBus.Dtos
{
    public class RuleDescriptionDto
    {
        public string Name { get; set; }

        public CorrelationFilterDto Filter { get; set; }
    }
}
