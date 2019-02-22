# Introduction 
A wrapper around Pact .Net Implementation. To offer extension methods in order to run Pact tests easily.

1. [Installation](#Installation)
2. [Implementation](#Implementation)
3. [Using The Package](#Using-Asos.Core.Testing.Pact)
    - [Start Mock Service](#Start-Mock-Service)
    - [Create Pact File](#Create-Pact-File)
4. [Cheat Methods](#Cheat-Methods)
    - [Construct Response Body](#Construct-Response-Body)
    - [Construct Request Body](#Construct-Request-Body)
5. [Provider State Methods](#Provider-State)
6. [Contribute](#Contribute)

# Installation
Package stored in [Asos Progret](https://proget.services.kingsway.asos.com/feeds/ASOS/Asos.Core.Testing.Pact)
1.	Install package
2.	Create Config folder with `pact.json` file. E.g.
```
{
  "pactBroker": {
    "url": "https://asos.pact.dius.com.au",
    "username": "",
    "password": ""
  },
  "providerService": {
    "port": 1234
  },
  "pactConfig": {
    "pactDir": "C:\\Pact",
    "specificationVersion": "2.0.0"
  }
}
```

# Implementation
Run Pact as usual or utilise the useful methods we have provided

See the [Examples](https://asos.visualstudio.com/ASOS%20Core/_git/asos-core-testing-pact?path=%2FExamples&version=GBmaster) section of the repo to see implementation details

## Using Asos.Core.Testing.Pact

### Start Mock Service
Create a new instance of the Provider Service in your Setup and Start the Mock Service ready for setting up your mock

```
_providerService = new ProviderService("Consumer", "Provider","./Config");
_providerService.Initialize();
```

### Create Pact File
Once your contract has been verified with `VerifyInteractions`. Then you can create your pact file using
```
_providerService.Build();
```

## Cheat Methods
Often you can find yourself repeating the process of adding pact matchers to fields in your response. Why not pass a C# object to these cheat methods to automatically add the matchers on conversion to a JSON Object.

### Construct Response Body
Example:
```
_providerService.ConstructResponseBody(new ResponseBody())
```

With the request we simply pass the C# object to the JSON request body.
### Construct Request Body
Example:
```
_providerService.ConstructRequestBody(new RequestBody())
```

## Provider State
TO DO: How to use these methods

# Contribute
Clone the repo yourself and create a PR against master. #innersource

Once it is merged, the package will be automatically incremented and created via the pipeline and available in proget:
https://teamcity.services.kingsway.asos.com/viewType.html?buildTypeId=AsosCore_TestingPact