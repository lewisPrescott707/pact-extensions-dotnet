using System;
using System.Collections.Generic;
using Asos.Customer.Contracts.Profile.V2.Rest.Customer;
using Asos.Customer.Preference.Contracts.Customer;
using Asos.Customer.Preference.Contracts.Enum;
using Asos.Customer.Preference.Contracts.Requests;
using Asos.Customer.Update.Tool.Api.PactTests.domain;
using Asos.Customer.Update.Tool.Api.PactTests.helpers;
using NUnit.Framework;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService.Models;
using RestSharp;

namespace Asos.Customer.Update.Tool.Api.PactTests.Tests
{
    public class PreferencesTests
    {
        private const string ServiceConsumerName = "Asos.Customer.Update.Tool";
        private const string ProviderName = "Asos.Customer.Preference.Api";
        private UpdateCustomerPreferencesRequest _customerPreferencesUpdateRequest;
        private ProviderService _providerService;
        private string PreferencesApiUrl { get; } = "/customer/preference/v1/preferences/";

        [OneTimeSetUp]
        public void SetUp()
        {
            _providerService = new ProviderService(ServiceConsumerName, ProviderName);

            _customerPreferencesUpdateRequest = new PreferenceRequestBuilder(new DateTimeWrapper())
                .BuildUpdateRequestWithErasureRules(new CustomerData()
                {
                    Uuid = "1234",
                    CustomerType = CustomerTypeEnum.Registered,
                    Gender = null,
                    ChannelIdentifiers = null,
                    DateModified = DateTime.Today,
                    Services = new List<CustomerServiceData>()
                    {
                        new CustomerServiceData()
                        {
                            ServiceId = ServiceIdEnum.Marketing,
                            Preferences = new List<CustomerPreferenceData>()
                            {
                                new CustomerPreferenceData()
                                {
                                    PreferenceId = PreferenceIdEnum.Newness,
                                    SourceReason = "pact test",
                                    ConsentStatus = ConsentStatusEnum.OptedIn,
                                    CustomerChannels = new List<ChannelEnum>(){ ChannelEnum.EmailAddress },
                                    Referrer = "pact test"
                                }
                            }
                        }
                    }
                });
        }

        [Category("Local")]
        [Test]
        public void When_Patch_Opt_Out_Customer_Then_Response_OK()
        {
            //Arrange
            const string guid = "47ea713e1c35435597d4fba40e1f3050";
            var url = $"{PreferencesApiUrl}{guid}";
            const string regex = "[0-9a-zA-Z]{32}";
            var urlRegex = Match.Regex($"{PreferencesApiUrl}{guid}", $"^\\/customer\\/preference\\/v1\\/preferences\\/{regex}$");

            _providerService
                .SetupRequest("A Customer With Preferences Exists",
                    "A request to opt out customer from all preferences for Customer Update Tool",
                    HttpVerb.Patch,
                    urlRegex,
                    new Dictionary<string, object>
                    {
                        { "Accept", "application/json" },
                        { "Content-Type", "application/json; charset=utf-8" },
                        { "asos-c-name", "https://www.asos.com" }
                    },
                    _providerService.ConstructRequestBody(_customerPreferencesUpdateRequest));

            _providerService.MockProviderService.WillRespondWith(new ProviderServiceResponse()
            {
                Status = 200
            });

            //Act
            RestRequestHelper.Get<UpdateCustomerPreferencesRequest, CustomerResponseV2>(url, Method.PATCH, _customerPreferencesUpdateRequest);

            //Assert
            _providerService.MockProviderService.VerifyInteractions();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _providerService.PactBuilder.Build();
        }
    }
}

