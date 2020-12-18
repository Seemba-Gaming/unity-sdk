using SeembaSDK.AppleAuth.Enums;

namespace SeembaSDK.AppleAuth.Interfaces
{
    public interface ICredentialStateResponse
    {
        bool Success { get; }
        CredentialState CredentialState { get; }
        IAppleError Error { get; }
    }
}
