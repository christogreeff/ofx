namespace Ofx.Configuration;

/// <summary>
/// OFX XML element name constants, keyed to OFX Banking Specification v2.3.
/// </summary>
public static class Config
{
    // OFX date format. OFX datetime values start with yyyyMMdd and may be followed by
    // time and timezone offset (e.g. 20201231120000[+2:SAST]). Only the date portion
    // is used by this library; callers should pass raw[..8] before parsing.
    public const string S_OFX_DATE_FORMAT = "yyyyMMdd";

    // ── Transaction element names (STMTTRN) ─────────────────────────────────
    public const string S_OFX_TRANSACTION_TYPE   = "TRNTYPE";
    public const string S_OFX_TRANSACTION_DATE   = "DTPOSTED";
    public const string S_OFX_TRANSACTION_AMOUNT = "TRNAMT";
    public const string S_OFX_TRANSACTION_ID     = "FITID";
    public const string S_OFX_TRANSACTION_NAME   = "NAME";
    public const string S_OFX_TRANSACTION_MEMO   = "MEMO";
    public const string S_OFX_TRANSACTION_CHECKNUM = "CHECKNUM";
    public const string S_OFX_TRANSACTION_REFNUM   = "REFNUM";

    // ── Statement / BANKTRANLIST element names ───────────────────────────────
    public const string S_OFX_STATEMENT_TRANSACTIONS  = "STMTTRN";
    public const string S_OFX_STATEMENT_START         = "DTSTART";
    public const string S_OFX_STATEMENT_END           = "DTEND";
    public const string S_OFX_STATEMENT_BANKTRANSLIST = "BANKTRANLIST";
    public const string S_OFX_STATEMENT_CURRENCY      = "CURDEF";

    // ── Bank account element names (BANKACCTFROM) ────────────────────────────
    public const string S_OFX_BANK_ACCOUNT_FROM    = "BANKACCTFROM";
    public const string S_OFX_BANK_ACCOUNT_BANK_ID = "BANKID";
    public const string S_OFX_BANK_ACCOUNT_ID      = "ACCTID";
    public const string S_OFX_BANK_ACCOUNT_TYPE    = "ACCTTYPE";

    // ── Balance element names ────────────────────────────────────────────────
    public const string S_OFX_LEDGER_BALANCE        = "LEDGERBAL";
    public const string S_OFX_AVAIL_BALANCE         = "AVAILBAL";
    public const string S_OFX_LEDGER_BALANCE_AMOUNT = "BALAMT";
    public const string S_OFX_LEDGER_BALANCE_DATE   = "DTASOF";

    // ── OFX envelope element names ───────────────────────────────────────────
    public const string S_OFX_HEADER                        = "OFX";
    public const string S_OFX_HEADER_BANK                   = "BANKMSGSRSV1";
    public const string S_OFX_HEADER_STATEMENT              = "STMTTRNRS";
    public const string S_OFX_HEADER_STATEMENT_TRANSACTIONS = "STMTRS";
}
