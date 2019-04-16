using System;
using System.Collections.Generic;
using System.IO;
using Asos.Core.Testing.Pact.Consumer.MockProviderService;
using Asos.Customer.Contracts.Profile.V2.Rest.Customer;
using Asos.Customer.Contracts.Profile.V2.Rest.LinkedAccounts;
using Asos.Customer.Contracts.Profile.V2.Rest.Subscription;
using Asos.Customer.Preference.Contracts.Customer;
using Asos.Customer.Update.Tool.Api.PactTests.helpers;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using RestSharp;
using CustomerPreference = Asos.Customer.Contracts.Preferences.CustomerPreference;

namespace Asos.Customer.Update.Tool.Api.PactTests.Tests
{
    public class GetPreferencesTests
    {
        private const string ServiceConsumerName = "Asos.Customer.Comms";
        private const string ProviderName = "Asos.Customer.Api";
        private CustomerResponseV2 _customerGetResponse;
        private ProviderService _providerService;
        private const string Url = "/customer/profile/v2/customers/5695794";

        [OneTimeSetUp]
        public void SetUp()
        {
            _providerService = new ProviderService(ServiceConsumerName, ProviderName, $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Config");
            _providerService.StartMockService();

            _customerGetResponse = new CustomerResponseV2()
            {
                Preferences = new List<CustomerPreference>(),
                Account = new AccountDetailsResponseV2(),
                Addresses = new List<AddressResponseV2>(),
                Created = DateTime.Now,
                CustomerGuid = "dsh12414",
                CustomerId = 1234,
                DateOfBirth = DateTime.Now,
                Email = "email@asos.com",
                EmailLastUpdated = DateTime.Now,
                FirstName = "first name",
                Gender = "F",
                HomeCountry = "GB",
                HomeSiteId = 1,
                HomeStore = "GB",
                Subscriptions = new List<SubscriptionResponseV2>(),
                IsActive = true,
                IsFirstTimeBuyer = true,
                IsReconsentRequired = true,
                Language = "EN-gb",
                LastName = "last name",
                LastUpdated = DateTime.Now,
                LinkedAccounts = new List<LinkedAccountV2>(),
                LoginProvider = "site",
                Rewards = new CustomerRewardsStatusV2(),
                SessionInfo = null,
                ThumbnailUrl = "thumbnail",
                TypeId = "registeredCustomer"
            };
        }

        [Test]
        public void When_Get_Customer_Preferences__Then_Customer_Preference_With_Display_Content_Response()
        {
            //Arrange
            _providerService.MockProviderService
                .UponReceiving("A request to get customer with preferences")
                .With(new ProviderServiceRequest()
                {
                    Headers = new Dictionary<string, object>(),
                    Path = Url,
                    Query = "expand=preferences",
                    Method = HttpVerb.Get
                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                   Body = _providerService.ConstructResponseBody(_customerGetResponse),
                   Headers = new Dictionary<string, object>
                   {
                       { "Accept", "application/json" },
                       { "Content-Type", "application/json; charset=utf-8" },
                       { "asos-c-name", "https://www.asos.com" }
                   },
                   Status = 200
                });

            //Act
            RestRequestHelper.Get<CustomerResponseV2>($"{Url}?expand=preferences", Method.GET, 1234);

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

