using System.Collections.Generic;
using System.Linq;
using Asos.Customer.Preference.Contracts.Customer;
using Asos.Customer.Preference.Contracts.Enum;
using Asos.Customer.Preference.Contracts.Requests;

namespace Asos.Customer.Update.Tool.Api.PactTests.domain
{
    public class PreferenceRequestBuilder : IPreferenceRequestBuilder
    {
        private readonly IDateTimeWrapper _dateTimeWrapper;

        public PreferenceRequestBuilder(IDateTimeWrapper dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public UpdateCustomerPreferencesRequest BuildUpdateRequestWithErasureRules(CustomerData data)
        {
            return new UpdateCustomerPreferencesRequest()
            {
                SourceReason = "Customer erasure request",
                Services = data.Services.ToDictionary(
                    service => service.ServiceId,
                    MapServicePreferencesWithOptOut)
            };
        }

        private IList<CustomerPreferenceDataRequest> MapServicePreferencesWithOptOut(CustomerServiceData service)
        {
            return service.Preferences.Select(preference => new CustomerPreferenceDataRequest()
            {
                PreferenceId = preference.PreferenceId,
                CustomerChannels = new List<ChannelEnum>(),
                DateRecorded = _dateTimeWrapper.UtcNow
            }).ToList();
        }
    }
}