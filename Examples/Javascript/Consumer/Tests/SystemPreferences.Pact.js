const path = require('path')
const chai = require('chai')
const chaiAsPromised = require('chai-as-promised')
const expect = chai.expect
const { Pact } = require('@pact-foundation/pact')
const { get } = require('axios')
const MOCK_SERVER_PORT = 2202
const LOG_LEVEL = process.env.LOG_LEVEL || 'INFO'
var ServiceConsumerName = "Asos.Identity.Web"
var ProviderName = "Asos.Customer.Preference.Api"

chai.use(chaiAsPromised)

describe('Pact', () => {

  const EXPECTED_BODY = {
    "services": [
      {
        "serviceId": "marketing",
        "serviceVersion": "1.0",
        "publishedFrom": "2017-09-12T00:00:01",
        "privacyPolicyUrlSegment": "/privacy-policy/",
        "termsConditionsUrlSegment": "/terms-and-conditions/",
        "serviceEligibility": [
          
        ],
        "preferences": [
          {
            "preferenceId": "promos",
            "serviceVersion": "1.0",
            "channels": [
              "emailAddress",
              "sms"
            ],
            "title": "Discounts and sales",
            "actionText": "Discounts and sales",
            "summary": "Be first in line to nab the stuff you love for less.",
            "imageUrl": "https://images.asos-media.com/navigation/gdpr_132x170_promos_high_res",
            "preSelected": false
          },
          {
            "preferenceId": "newness",
            "serviceVersion": "1.0",
            "channels": [
              "emailAddress",
              "sms"
            ],
            "title": "New stuff",
            "actionText": "New stuff",
            "summary": "Fashion drops, news and style advice: hear it first, wear it first.",
            "imageUrl": "https://images.asos-media.com/navigation/gdpr_132x170_new_high_res",
            "preSelected": false
          },
          {
            "preferenceId": "lifestyle",
            "serviceVersion": "1.0",
            "channels": [
              "emailAddress",
              "sms"
            ],
            "title": "Your exclusives",
            "actionText": "Your exclusives",
            "summary": "Enjoy a birthday treat, as well as tailored rewards and account updates.",
            "imageUrl": "https://images.asos-media.com/navigation/gdpr_132x170_exclusives_high_res",
            "preSelected": false
          },
          {
            "preferenceId": "partner",
            "serviceVersion": "1.0",
            "channels": [
              "emailAddress",
              "sms"
            ],
            "title": "ASOS partners",
            "actionText": "ASOS partners",
            "summary": "Stay in the know with exclusive collabs and handpicked offers.",
            "imageUrl": "https://images.asos-media.com/navigation/gdpr_132x170_partners_high_res",
            "preSelected": false
          }
        ]
      }
    ]
  }

  const provider = new Pact({
    consumer: ServiceConsumerName,
    provider: ProviderName,
    port: MOCK_SERVER_PORT,
    log: path.resolve(process.cwd(), 'logs', 'mockserver-integration.log'),
    dir: path.resolve(process.cwd(), 'pacts'),
    logLevel: LOG_LEVEL,
    spec: 2
  })

  before((done) => {
    provider.setup()
      .then(() => {
        return provider.addInteraction({
          state: "System Preferences with Marketing Service in British English",
          uponReceiving: "a request to retrieve the service preferences for marketing",
          withRequest: {
            method: "GET",
            path: "/customer/preference/v1/preferences",
            query: "country=GB&lang=en-GB&services=marketing",
            headers: { Accept: "application/json, text/plain, */*" }
          },
          willRespondWith: {
            status: 200,
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: EXPECTED_BODY
          }
        })
      })
      .then(() => done())
  })

  it("should display preferences for english-gb language and marketing service", (done) => {
      get(`http://localhost:${MOCK_SERVER_PORT}/customer/preference/v1/preferences?country=GB&lang=en-GB&services=marketing`)
      .then((response) => {
          expect(response).not.to.be.null;
      })
      .then(() => done())
  })

  it('successfully verifies', (done) => {
    provider.verify()
    .then(() => done())
  })

  // Write pact files
  after((done) => {
    provider.finalize()
    .then(() => done())
  })
})
