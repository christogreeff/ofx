namespace Ofx.Repository;

/// <summary>
/// OFX bank account type values as defined in OFX Banking Specification v2.3 (BANKACCTFROM/ACCTTYPE).
/// </summary>
public enum AccountType
{
    /// <summary>Cheque / transaction account.</summary>
    Checking,

    /// <summary>Savings account.</summary>
    Savings,

    /// <summary>Money market account.</summary>
    MoneyMrkt,

    /// <summary>Line of credit.</summary>
    CreditLine,

    /// <summary>Certificate of deposit.</summary>
    CD,

    /// <summary>Unrecognised or absent account type.</summary>
    Unknown,
}
