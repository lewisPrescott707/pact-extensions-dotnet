using System;
using System.Collections.Generic;
using System.IO;
using Asos.Core.Testing.Pact.Consumer.MockProviderService;
using Asos.Customer.Preference.Contracts.Customer;
using Asos.Customer.Update.Tool.Api.PactTests.helpers;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using RestSharp;

namespace Asos.Customer.Update.Tool.Api.PactTests.Tests
{
    public class GetPreferencesTests
    {
        private const string ServiceConsumerName = "Asos.Customer.Comms";
        private const string ProviderName = "Asos.Customer.Preference.Api";
        private CustomerPreferenceWithDisplayContent _customerPreferencesGetResponse;
        private ProviderService _providerService;
        private const string Url = "/customer/preference/v1/preferences/47ea713e1c35435597d4fba40e1f3050";

        [OneTimeSetUp]
        public void SetUp()
        {
            _providerService = new ProviderService(ServiceConsumerName, ProviderName, $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Config");
            _providerService.StartMockService();

            _customerPreferencesGetResponse = new CustomerPreferenceWithDisplayContent();
        }

        [Test]
        public void When_Get_Customer_Preferences__Then_Customer_Preference_With_Display_Content_Response()
        {
            //Arrange
            _providerService.MockProviderService
                .UponReceiving("A request to get customer preferences")
                .With(new ProviderServiceRequest()
                {
                    Headers = new Dictionary<string, object>(),
                    Path = Url,
                    Method = HttpVerb.Get
                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                   Body = _providerService.ConstructResponseBody(_customerPreferencesGetResponse),
                   Headers = new Dictionary<string, object>
                   {
                       { "Accept", "application/json" },
                       { "Content-Type", "application/json; charset=utf-8" },
                       { "asos-c-name", "https://www.asos.com" }
                   },
                   Status = 200
                });

            //Act
            RestRequestHelper.Get<CustomerPreferenceWithDisplayContent>(Url, Method.GET, 1234);

            //Assert
            _providerService.MockProviderService.VerifyInteractions();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _providerService.CreatePact();
        }
    }
}

