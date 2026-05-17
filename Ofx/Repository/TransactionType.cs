namespace Ofx.Repository;

/// <summary>
/// OFX transaction type values as defined in OFX Banking Specification v2.3 (STMTTRN/TRNTYPE).
/// </summary>
public enum TransactionType
{
    /// <summary>Generic credit.</summary>
    Credit,

    /// <summary>Generic debit.</summary>
    Debit,

    /// <summary>Interest earned or paid.</summary>
    Int,

    /// <summary>Dividend.</summary>
    Div,

    /// <summary>Financial institution fee.</summary>
    Fee,

    /// <summary>Service charge.</summary>
    SrvChg,

    /// <summary>Deposit.</summary>
    Dep,

    /// <summary>ATM debit or credit.</summary>
    Atm,

    /// <summary>Point-of-sale debit or credit.</summary>
    Pos,

    /// <summary>Transfer.</summary>
    Xfer,

    /// <summary>Cheque.</summary>
    Check,

    /// <summary>Electronic payment.</summary>
    Payment,

    /// <summary>Cash withdrawal.</summary>
    Cash,

    /// <summary>Direct deposit.</summary>
    DirectDep,

    /// <summary>Merchant-initiated debit.</summary>
    DirectDebit,

    /// <summary>Repeating payment / standing order.</summary>
    RepeatPmt,

    /// <summary>Funds hold (valid only in pending transactions).</summary>
    Hold,

    /// <summary>Other / unrecognised type.</summary>
    Other,
}
