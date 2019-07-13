using System;
using JetBrains.Annotations;

namespace Vault.Helpers
{
    [MeansImplicitUse(ImplicitUseTargetFlags.Members)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class PublicContractAttribute : Attribute
    { }
}