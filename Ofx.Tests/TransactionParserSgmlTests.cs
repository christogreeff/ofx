using Ofx;
using Ofx.Repository;
using Xunit;

namespace Ofx.Tests;

/// <summary>
/// Parses <c>docs/samples/ofx-sample-1x.ofx</c> and asserts the values the library extracts
/// from the single STMTRS (checking account, USD, account 9876543210).
///
/// Key differences from the XML sample exercised here:
///   - OFX 1.x SGML format (OFXHEADER:100) — triggers ConvertSgmlToXml
///   - Date-only format YYYYMMDD (no time or timezone offset)
///   - Raw '&amp;' character in a leaf value (must be XML-escaped before parsing)
///   - Nested BANKACCTTO aggregate inside a XFER transaction
/// </summary>
public class TransactionParserSgmlTests
{
    private static readonly Statement _s = ParseSample("ofx-sample-1x.ofx");
    private static readonly IReadOnlyList<Transaction> _txns = _s.Transaction!.Transactions;

    private static Statement ParseSample(string filename)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", filename);
        return new TransactionParser(path).ElectronicStatement!;
    }

    // ── Statement-level ──────────────────────────────────────────────────────

    [Fact] public void Statement_NotNull() => Assert.NotNull(_s);
    [Fact] public void Currency_IsUsd() => Assert.Equal("USD", _s.Currency);

    // ── Account ──────────────────────────────────────────────────────────────

    [Fact] public void Account_BankId() => Assert.Equal("000111222", _s.Account!.BankId);
    [Fact] public void Account_AccountId() => Assert.Equal("9876543210", _s.Account!.AccountId);
    [Fact] public void Account_Type_Checking() => Assert.Equal(AccountType.Checking, _s.Account!.AccountType);

    // ── Transaction period ───────────────────────────────────────────────────

    [Fact] public void Period_StartDate() => Assert.Equal(new DateTime(2024, 10, 1), _s.Transaction!.StartDate);
    [Fact] public void Period_EndDate() => Assert.Equal(new DateTime(2024, 10, 15), _s.Transaction!.EndDate);

    // ── Balances ─────────────────────────────────────────────────────────────

    [Fact] public void LedgerBalance_Amount() => Assert.Equal(831.16m, _s.Ledger!.Balance);
    [Fact] public void LedgerBalance_Date() => Assert.Equal(new DateTime(2024, 10, 15), _s.Ledger!.Date);
    [Fact] public void AvailableBalance_Amount() => Assert.Equal(831.16m, _s.AvailableBalance!.Balance);
    [Fact] public void AvailableBalance_Date() => Assert.Equal(new DateTime(2024, 10, 15), _s.AvailableBalance!.Date);

    // ── Transaction list ─────────────────────────────────────────────────────

    [Fact] public void Transactions_Count_Is9() => Assert.Equal(9, _txns.Count);

    // First transaction: date-only YYYYMMDD format
    [Fact]
    public void Txn00_DirectDep_DateOnlyFormat()
    {
        var t = _txns[0];
        Assert.Equal(TransactionType.DirectDep, t.TransType);
        Assert.Equal(3250.00m, t.Amount);
        Assert.Equal("SGML-2024100100001", t.Id);
        Assert.Equal("ACME CORP PAYROLL", t.Name);
        Assert.Equal(new DateTime(2024, 10, 1), t.Date);
    }

    [Fact]
    public void Txn01_Check_WithChecknum()
    {
        var t = _txns[1];
        Assert.Equal(TransactionType.Check, t.TransType);
        Assert.Equal(-1842.57m, t.Amount);
        Assert.Equal("1042", t.CheckNum);
        Assert.Equal(new DateTime(2024, 10, 2), t.Date);
    }

    // PAYMENT: raw '&' in SGML must be escaped to '&amp;' before XML parsing,
    // then decoded back to '&' by XmlDocument — result is the plain text character.
    [Fact]
    public void Txn05_Payment_RawAmpersand_EscapedAndDecoded()
    {
        var t = _txns[5];
        Assert.Equal(TransactionType.Payment, t.TransType);
        Assert.Equal("CITY ELECTRIC & GAS", t.Name);
    }

    // XFER with nested <BANKACCTTO> aggregate must not break core field parsing
    [Fact]
    public void Txn07_Xfer_WithNestedBankAcctTo()
    {
        var t = _txns[7];
        Assert.Equal(TransactionType.Xfer, t.TransType);
        Assert.Equal(-500.00m, t.Amount);
        Assert.Equal("SGML-2024100900001", t.Id);
        Assert.Equal("ONLINE TRANSFER", t.Name);
    }

    // Last transaction verifies date-only parsing across the full range
    [Fact]
    public void Txn08_Credit_LastTransaction()
    {
        var t = _txns[8];
        Assert.Equal(TransactionType.Credit, t.TransType);
        Assert.Equal(318.40m, t.Amount);
        Assert.Equal(new DateTime(2024, 10, 14), t.Date);
    }
}
