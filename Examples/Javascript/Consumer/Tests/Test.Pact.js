const path = require('path')
const chai = require('chai')
const chaiAsPromised = require('chai-as-promised')
const expect = chai.expect
const { Pact } = require('@pact-foundation/pact')
const { get } = require('axios')
const MOCK_SERVER_PORT = 2202
const LOG_LEVEL = process.env.LOG_LEVEL || 'INFO'
var ServiceConsumerName = "Web"
var ProviderName = "Api"

chai.use(chaiAsPromised)

describe('Pact', () => {

  const EXPECTED_BODY = { "pact": "1" }

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
          state: "200 Response",
          uponReceiving: "a request to get response",
          withRequest: {
            method: "GET",
            path: "/pact/1",
            headers: { Accept: "application/json, text/plain, */*" }
          },
          willRespondWith: {
            status: 200,
            headers: { "Content-Type": "application/json" },
            body: EXPECTED_BODY
          }
        })
      })
      .then(() => done())
  })

  it("should display 200 OK", (done) => {
      get(`http://localhost:${MOCK_SERVER_PORT}/pact/1`)
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
