# Introduction 
A wrapper around Pact .Net Implementation. To offer extension methods in order to run Pact tests easily.

1. [Installation](#Installation)
2. [Implementation](#Implementation)
3. [Using The Package](#Using-Core.Testing.Pact)
    - [Start Mock Service](#Start-Mock-Service)
    - [Create Pact File](#Create-Pact-File)
4. [Cheat Methods](#Cheat-Methods)
    - [Construct Response Body](#Construct-Response-Body)
    - [Construct Request Body](#Construct-Request-Body)
5. [Provider State Methods](#Provider-State)
6. [Contribute](#Contribute)

# Implementation
Run Pact as usual or utilise the useful methods we have provided

See the [Examples] section of the repo to see implementation details

## Using Core.Testing.Pact

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
