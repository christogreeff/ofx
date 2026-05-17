using System.Globalization;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository;

/// <summary>
/// Account balance, corresponding to the OFX <c>LEDGERBAL</c> or <c>AVAILBAL</c> aggregate.
/// </summary>
public class LedgerBalance
{
    /// <summary>Balance amount (BALAMT).</summary>
    public decimal Balance { get; private set; }

    /// <summary>Date the balance was calculated (DTASOF).</summary>
    public DateTime Date { get; private set; }

    /// <param name="balElement">The <c>LEDGERBAL</c> or <c>AVAILBAL</c> XML element.</param>
    public LedgerBalance(XmlElement balElement)
    {
        GetBalance(balElement);
    }

    private void GetBalance(XmlElement balElement)
    {
        Balance = decimal.Parse(
                      balElement[Config.S_OFX_LEDGER_BALANCE_AMOUNT]?.InnerText.Trim() ?? "0",
                      CultureInfo.InvariantCulture);
        Date = Transaction.ParseOfxDate(balElement[Config.S_OFX_LEDGER_BALANCE_DATE]?.InnerText.Trim());
    }
}
