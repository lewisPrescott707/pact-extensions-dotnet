namespace Core.Testing.Pact.Provider.PactVerifier
{
    public interface IProviderState
    {
        int GetIndex(string providerState);

        void SerializeJsonToFile(string localDirectory);
    }
}
