# Introduction

How to create new Consumer Pact Json

# Steps
1. Construct C# Request / Response Object
2. Verify it against Pact Mock Service

### Run locally:
1. dotnet test .\Asos.Customer.Update.Tool.Api.PactTests.csproj

#### Pact Files

Local Pact File Location: `C:\Pact` (Change in test)

### Run in Docker:
1. docker build -t pact-test . --build-arg nugetPassword={password} nugetUsername="{username}"
2. docker run -t pact-test ./Asos.Customer.Update.Tool.Api.PactTests.csproj

