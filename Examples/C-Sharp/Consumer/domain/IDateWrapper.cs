using System;

namespace Asos.Customer.Update.Tool.Api.PactTests.domain
{
    public interface IDateTimeWrapper
    {
        DateTime UtcNow { get; }
    }
}