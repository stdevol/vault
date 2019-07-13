using System;
using Vault.Helpers;

namespace Vault.Core.Exceptions
{
    [PublicContract]
    public class VaultException : Exception
    {
        public VaultException()
        { }

        public VaultException(string message)
            : base(message)
        { }

        public VaultException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
