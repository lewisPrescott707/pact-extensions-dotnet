# The Example Provider Project

System Preferences / Update Preferences - Customer Preferences API. Consumed by Identity Web / Customer Update Tool.

## Running the tests

#### Before test run
- Set environment "appsettings.json" (Environments can be found in `config/environments.json`)

#### Running tests
1. `dotnet test .\Asos.Customer.Preference.PactTests.csproj`
    - _note: customer creation can fail on test setup, pending polly implementation_

### Pact Broker

Pacts stored: https://asos.pact.dius.com.au/
