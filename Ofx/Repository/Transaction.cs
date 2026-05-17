using System.Globalization;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository;

/// <summary>
/// A single statement transaction record, corresponding to the OFX <c>STMTTRN</c> aggregate.
/// </summary>
public class Transaction
{
    /// <summary>Raw TRNTYPE string as it appears in the OFX file.</summary>
    public string TransText { get; private set; } = string.Empty;

    /// <summary>Parsed transaction type.</summary>
    public TransactionType TransType { get; private set; }

    /// <summary>Date the transaction was posted to the account (DTPOSTED).</summary>
    public DateTime Date { get; private set; }

    /// <summary>Transaction amount — negative values are debits per OFX spec.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Financial institution transaction identifier (FITID).</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Payee name or transaction description (NAME).</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Additional memo text (MEMO), if present.</summary>
    public string? Memo { get; private set; }

    /// <summary>Cheque number (CHECKNUM), if present.</summary>
    public string? CheckNum { get; private set; }

    /// <summary>Reference number (REFNUM), if present.</summary>
    public string? RefNum { get; private set; }

    /// <param name="node">The <c>STMTTRN</c> XML node.</param>
    public Transaction(XmlNode node)
    {
        ParseNode(node);
    }

    private void ParseNode(XmlNode node)
    {
        TransText = node[Config.S_OFX_TRANSACTION_TYPE]?.InnerText.Trim() ?? string.Empty;
        TransType = ParseTransactionType(TransText);
        Date      = ParseOfxDate(node[Config.S_OFX_TRANSACTION_DATE]?.InnerText.Trim());
        Amount    = decimal.Parse(
                        node[Config.S_OFX_TRANSACTION_AMOUNT]?.InnerText.Trim() ?? "0",
                        CultureInfo.InvariantCulture);
        Id       = node[Config.S_OFX_TRANSACTION_ID]?.InnerText.Trim()   ?? string.Empty;
        Name     = node[Config.S_OFX_TRANSACTION_NAME]?.InnerText.Trim() ?? string.Empty;
        Memo     = node[Config.S_OFX_TRANSACTION_MEMO]?.InnerText.Trim();
        CheckNum = node[Config.S_OFX_TRANSACTION_CHECKNUM]?.InnerText.Trim();
        RefNum   = node[Config.S_OFX_TRANSACTION_REFNUM]?.InnerText.Trim();
    }

    private static TransactionType ParseTransactionType(string text) =>
        text.ToUpperInvariant() switch
        {
            "CREDIT"      => TransactionType.Credit,
            "DEBIT"       => TransactionType.Debit,
            "INT"         => TransactionType.Int,
            "DIV"         => TransactionType.Div,
            "FEE"         => TransactionType.Fee,
            "SRVCHG"      => TransactionType.SrvChg,
            "DEP"         => TransactionType.Dep,
            "ATM"         => TransactionType.Atm,
            "POS"         => TransactionType.Pos,
            "XFER"        => TransactionType.Xfer,
            "CHECK"       => TransactionType.Check,
            "PAYMENT"     => TransactionType.Payment,
            "CASH"        => TransactionType.Cash,
            "DIRECTDEP"   => TransactionType.DirectDep,
            "DIRECTDEBIT" => TransactionType.DirectDebit,
            "REPEATPMT"   => TransactionType.RepeatPmt,
            "HOLD"        => TransactionType.Hold,
            _             => TransactionType.Other,
        };

    // OFX datetime values start with yyyyMMdd and may include time and a timezone
    // offset (e.g. "20201231120000[+2:SAST]"). Only the date portion is used here.
    internal static DateTime ParseOfxDate(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return default;

        var datePart = raw.Length >= 8 ? raw[..8] : raw;
        return DateTime.ParseExact(datePart, Config.S_OFX_DATE_FORMAT, CultureInfo.InvariantCulture);
    }
}
