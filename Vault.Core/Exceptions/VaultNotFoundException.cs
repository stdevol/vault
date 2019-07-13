namespace Vault.Core.Exceptions
{
    public class VaultNotFoundException : VaultException
    {
        public VaultNotFoundException()
        {
        }

        public VaultNotFoundException(string message) : base(message)
        {
        }
    }
}
