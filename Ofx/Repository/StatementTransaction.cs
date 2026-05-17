using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository;

/// <summary>
/// Transaction list and date range, corresponding to the OFX <c>BANKTRANLIST</c> aggregate.
/// </summary>
public class StatementTransaction
{
    /// <summary>Start of the statement period (DTSTART).</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>End of the statement period (DTEND).</summary>
    public DateTime EndDate { get; private set; }

    /// <summary>All transactions in the statement period.</summary>
    public IReadOnlyList<Transaction> Transactions => _transactions;

    private readonly List<Transaction> _transactions = [];

    /// <param name="stmtrs">The <c>STMTRS</c> XML element.</param>
    public StatementTransaction(XmlElement stmtrs)
    {
        BuildTransactionList(stmtrs);
    }

    private void BuildTransactionList(XmlElement stmtrs)
    {
        var banktranlist = stmtrs[Config.S_OFX_STATEMENT_BANKTRANSLIST];
        if (banktranlist is null)
            return;

        StartDate = Transaction.ParseOfxDate(banktranlist[Config.S_OFX_STATEMENT_START]?.InnerText.Trim());
        EndDate   = Transaction.ParseOfxDate(banktranlist[Config.S_OFX_STATEMENT_END]?.InnerText.Trim());

        foreach (XmlNode node in banktranlist.GetElementsByTagName(Config.S_OFX_STATEMENT_TRANSACTIONS))
            _transactions.Add(new Transaction(node));
    }
}
