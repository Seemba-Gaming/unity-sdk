#if FRAMEWORK
namespace RestSharp
{
	/// <summary>
	/// Tries to Authenticate with the credentials of the currently logged in user, or impersonate a user
	/// </summary>
	public class NtlmAuthenticator : IAuthenticator
	{
		private readonly ICredentials credentials;
		/// <summary>
		/// Authenticate with the credentials of the currently logged in user
		/// </summary>
		public NtlmAuthenticator()
			: this(CredentialCache.DefaultCredentials)
		{
		}
		/// <summary>
		/// Authenticate by impersonation
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public NtlmAuthenticator(string username, string password) : this(new NetworkCredential(username, password))
		{
		}	
		/// <summary>
		/// Authenticate by impersonation, using an existing <c>ICredentials</c> instance
		/// </summary>
		/// <param name="credentials"></param>
		public NtlmAuthenticator(ICredentials credentials)
		{
			if (credentials == null) throw new ArgumentNullException("credentials");
				this.credentials = credentials;
		}
		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.Credentials = credentials;
		}
	}
}
#endif