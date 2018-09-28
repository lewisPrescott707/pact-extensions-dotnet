using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Asos.Customer.Contracts.Preferences;
using Asos.Customer.Contracts.Profile.V2.Rest.Customer;
using Asos.Customer.Contracts.Profile.V2.Rest.FindCustomer;
using Asos.Customer.Preference.PactTests.config;
using Asos.Customer.Profile.Api.Client;
using Asos.Customer.Profile.Api.Client.Communication.Results;
using NUnit.Framework;

namespace Asos.Customer.Preference.PactTests.helpers
{
    public class Customer
    {
        private readonly CustomerProfileApi _profileApiClient;

        public Customer()
        {
            _profileApiClient = TokenGenerator.Create();
        }

        public async Task<ProfileApiResult<CustomerResponseV2>> Get(int customerId)
        {
            return await _profileApiClient.Customers(customerId).GetAsync();
        }

        public async Task<ProfileApiResult<FindCustomerResponseV2>> Find(string customerGuid)
        {
            return await _profileApiClient.Search.GetAsync(new FindCustomerRequestV2()
            {
                CustomerGuid = customerGuid
            });
        }

        public async Task<ProfileApiResult<CustomerResponseV2>> Update(CustomerResponseV2 customer, string customerGuid)
        {
            return await _profileApiClient.Customers(customer.CustomerId).PutAsync(new UpdateCustomerRequestV2()
            {
                CustomerId = customer.CustomerId,
                CustomerGuid = customerGuid,
                IsActive = true,
                IsFirstTimeBuyer = true,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.Email,
                TypeId = customer.TypeId,
                Addresses = new List<AddressRequestV2>()
                {
                    new AddressRequestV2()
                    {
                        FirstName = customer.Addresses.First().FirstName,
                        LastName = customer.Addresses.First().LastName,
                        Address1 = customer.Addresses.First().Address1,
                        Address2 = customer.Addresses.First().Address2,
                        AddressId = customer.Addresses.First().AddressId,
                        DefaultDelivery = customer.Addresses.First().DefaultDelivery,
                        CountryCode = customer.Addresses.First().CountryCode,
                        CountyStateProvinceOrArea = customer.Addresses.First().CountyStateProvinceOrArea,
                        CountyStateProvinceOrAreaCode = customer.Addresses.First().CountyStateProvinceOrAreaCode,
                        DefaultBilling = customer.Addresses.First().DefaultBilling,
                        Locality = customer.Addresses.First().Locality,
                        PostalCode = customer.Addresses.First().PostalCode,
                        TelephoneMobile = customer.Addresses.First().TelephoneMobile
                    }
                },
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                HomeCountry = customer.HomeCountry,
                HomeStore = customer.HomeStore,
                Language = customer.Language
            });
        }

        public async Task<CustomerResponseV2> Create()
        {
            var randomString = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var randomEmail = "TEST" + randomString.Substring(0, 10 < randomString.Length ? 10 : randomString.Length) + "@asos.com";
            var result = await _profileApiClient.Customers().PostAsync(new CreateCustomerRequestV2()
            {
                FirstName = "Erasure",
                LastName = "Test",
                Password = "Password1",
                EmailAddress = randomEmail,
                Gender = "F",
                DateOfBirth = new DateTime(1900, 01, 01),
                TypeId = "registeredCustomer",
                HomeStore = "RU",
                HomeCountry = "GB",
                Language = "en-GB",
                KeyStoreDataVersion = null,
                Addresses = new List<AddressRequestV2>()
                {
                    new AddressRequestV2()
                    {
                        AddressId = 2954,
                        Address1 = "address 1",
                        FirstName = "Erasure",
                        LastName = "Test",
                        Address2 = "Fnqohcbnoitqxvjbtqetpwwqwrsch",
                        Locality = "Wxdxkolebmgbyufmwwygioqpswsedwepeipevshf",
                        CountyStateProvinceOrArea = "Wyoming",
                        CountyStateProvinceOrAreaCode = null,
                        PostalCode = "NW1 7AW",
                        CountryCode = "GB",
                        TelephoneMobile = "01234567891",
                        DefaultBilling = true,
                        DefaultDelivery = true
                    }
                },

                Preferences = new List<CustomerPreferenceServiceData>()
                {
                    new CustomerPreferenceServiceData()
                    {
                        ServiceId = "marketing",
                        Referrer = "Erasure test",
                        Source = "Erasure",
                        Preferences = new List<CustomerPreferenceDataRequest>()
                        {
                            new CustomerPreferenceDataRequest()
                            {
                                PreferenceId = "newness",
                                DateRecorded = new DateTime(2018, 02, 01),
                                CustomerChannels = new List<Channel>(){ Channel.EmailAddress }
                            },
                            new CustomerPreferenceDataRequest()
                            {
                                PreferenceId = "promos",
                                DateRecorded = new DateTime(2018, 02, 01),
                                CustomerChannels = new List<Channel>(){ Channel.EmailAddress }
                            },
                            new CustomerPreferenceDataRequest()
                            {
                                PreferenceId = "lifestyle",
                                DateRecorded = new DateTime(2018, 02, 01),
                                CustomerChannels = new List<Channel>(){ Channel.EmailAddress }
                            },
                            new CustomerPreferenceDataRequest()
                            {
                                PreferenceId = "partner",
                                DateRecorded = new DateTime(2018, 02, 01),
                                CustomerChannels = new List<Channel>(){ Channel.EmailAddress }
                            },
                        }
                    }
                }
            });
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            return result.Data;
        }
    }
}
