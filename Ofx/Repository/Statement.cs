using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository;

/// <summary>
/// A complete bank statement, corresponding to the OFX <c>STMTRS</c> aggregate.
/// </summary>
public class Statement
{
    /// <summary>Default currency for the statement (CURDEF), e.g. <c>ZAR</c>.</summary>
    public string Currency { get; private set; } = string.Empty;

    /// <summary>Bank account details (BANKACCTFROM).</summary>
    public BankAccount? Account { get; private set; }

    /// <summary>Ledger (book) balance (LEDGERBAL).</summary>
    public LedgerBalance? Ledger { get; private set; }

    /// <summary>
    /// Available balance (AVAILBAL), if the financial institution provides it.
    /// <see langword="null"/> when not present in the OFX file.
    /// </summary>
    public LedgerBalance? AvailableBalance { get; private set; }

    /// <summary>Transaction list and date range (BANKTRANLIST).</summary>
    public StatementTransaction? Transaction { get; private set; }

    /// <param name="stmtrs">The <c>STMTRS</c> XML element.</param>
    public Statement(XmlElement stmtrs)
    {
        BuildStatementInformation(stmtrs);
    }

    private void BuildStatementInformation(XmlElement stmtrs)
    {
        Currency         = stmtrs[Config.S_OFX_STATEMENT_CURRENCY]?.InnerText.Trim() ?? string.Empty;
        Account          = new BankAccount(stmtrs);
        Transaction      = new StatementTransaction(stmtrs);

        var ledgerbal = stmtrs[Config.S_OFX_LEDGER_BALANCE];
        if (ledgerbal is not null)
            Ledger = new LedgerBalance(ledgerbal);

        var availbal = stmtrs[Config.S_OFX_AVAIL_BALANCE];
        if (availbal is not null)
            AvailableBalance = new LedgerBalance(availbal);
    }
}
