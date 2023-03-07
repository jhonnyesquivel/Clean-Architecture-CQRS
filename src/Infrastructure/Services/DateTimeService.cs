using Zemoga_Test.Application.Common.Interfaces;

namespace Zemoga_Test.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
