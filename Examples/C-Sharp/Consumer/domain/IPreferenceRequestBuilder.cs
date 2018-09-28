using Asos.Customer.Preference.Contracts.Customer;
using Asos.Customer.Preference.Contracts.Requests;

namespace Asos.Customer.Update.Tool.Api.PactTests.domain
{
    public interface IPreferenceRequestBuilder
    {
        UpdateCustomerPreferencesRequest BuildUpdateRequestWithErasureRules(CustomerData data);
    }
}