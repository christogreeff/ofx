using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository;

/// <summary>
/// Bank account identification details, corresponding to the OFX <c>BANKACCTFROM</c> aggregate.
/// </summary>
public class BankAccount
{
    /// <summary>Bank routing / transit identifier (BANKID).</summary>
    public string BankId { get; private set; } = string.Empty;

    /// <summary>Account number (ACCTID).</summary>
    public string AccountId { get; private set; } = string.Empty;

    /// <summary>Account type (ACCTTYPE).</summary>
    public AccountType AccountType { get; private set; }

    /// <param name="stmtrs">The <c>STMTRS</c> XML element.</param>
    public BankAccount(XmlElement stmtrs)
    {
        GetBankAccountDetails(stmtrs);
    }

    private void GetBankAccountDetails(XmlElement stmtrs)
    {
        var bankacctfrom = stmtrs[Config.S_OFX_BANK_ACCOUNT_FROM];
        if (bankacctfrom is null)
            return;

        BankId    = bankacctfrom[Config.S_OFX_BANK_ACCOUNT_BANK_ID]?.InnerText.Trim() ?? string.Empty;
        AccountId = bankacctfrom[Config.S_OFX_BANK_ACCOUNT_ID]?.InnerText.Trim()      ?? string.Empty;
        AccountType = ParseAccountType(bankacctfrom[Config.S_OFX_BANK_ACCOUNT_TYPE]?.InnerText.Trim());
    }

    private static AccountType ParseAccountType(string? raw) =>
        raw?.ToUpperInvariant() switch
        {
            "CHECKING"   => AccountType.Checking,
            "SAVINGS"    => AccountType.Savings,
            "MONEYMRKT"  => AccountType.MoneyMrkt,
            "CREDITLINE" => AccountType.CreditLine,
            "CD"         => AccountType.CD,
            _            => AccountType.Unknown,
        };
}
