using System.ComponentModel;

namespace Vault.Core.Organizations
{
    /// <summary>
    /// Категория (размер) предприятия
    /// </summary>
    public enum OrgType
    {

        [Description("Физическое лицо")]
        Phisical = 2,

        [Description("Индивидуальный предприниматель")]
        IndividualEntrepreneur = 3,

        [Description("Юридическое лицо")]
        Juridical = 4
    }
}