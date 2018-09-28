using System;

namespace Asos.Customer.Update.Tool.Api.PactTests.domain
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
