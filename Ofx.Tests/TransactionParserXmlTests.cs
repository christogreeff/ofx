using Ofx;
using Ofx.Repository;
using Xunit;

namespace Ofx.Tests;

/// <summary>
/// Parses <c>docs/samples/ofx-sample-2.3.xml</c> and asserts the values the library extracts
/// from the first STMTRS (checking account, USD, account 9876543210).
/// </summary>
public class TransactionParserXmlTests
{
    // The parser navigates OFX → BANKMSGSRSV1 → first STMTTRNRS → STMTRS.
    // All assertions target that checking-account response aggregate.
    private static readonly Statement _s = ParseSample("ofx-sample-2.3.xml");
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

    [Fact] public void LedgerBalance_Amount() => Assert.Equal(736.78m, _s.Ledger!.Balance);
    [Fact] public void LedgerBalance_Date() => Assert.Equal(new DateTime(2024, 10, 15), _s.Ledger!.Date);
    [Fact] public void AvailableBalance_Amount() => Assert.Equal(236.78m, _s.AvailableBalance!.Balance);
    [Fact] public void AvailableBalance_Date() => Assert.Equal(new DateTime(2024, 10, 15), _s.AvailableBalance!.Date);

    // ── Transaction list ─────────────────────────────────────────────────────

    [Fact] public void Transactions_Count_Is13() => Assert.Equal(13, _txns.Count);

    // TXN-01: DIRECTDEP — payroll; full datetime YYYYMMDDHHMMSS.XXX[offset:TZ]
    [Fact]
    public void Txn00_DirectDep()
    {
        var t = _txns[0];
        Assert.Equal(TransactionType.DirectDep, t.TransType);
        Assert.Equal(3250.00m, t.Amount);
        Assert.Equal("2024100100001", t.Id);
        Assert.Equal("ACME CORP PAYROLL", t.Name);
        Assert.Equal(new DateTime(2024, 10, 1), t.Date);
        Assert.Equal("Payroll deposit - period ending 2024-09-30", t.Memo);
    }

    // TXN-02: CHECK — CHECKNUM and REFNUM populated
    [Fact]
    public void Txn01_Check_ChecknumAndRefnum()
    {
        var t = _txns[1];
        Assert.Equal(TransactionType.Check, t.TransType);
        Assert.Equal(-1842.57m, t.Amount);
        Assert.Equal("1042", t.CheckNum);
        Assert.Equal("MTG-2024100200001", t.RefNum);
        Assert.Equal(new DateTime(2024, 10, 2), t.Date);
    }

    // TXN-05: SRVCHG — no MEMO, no CHECKNUM
    [Fact]
    public void Txn04_SrvChg_OptionalFieldsAbsent()
    {
        var t = _txns[4];
        Assert.Equal(TransactionType.SrvChg, t.TransType);
        Assert.Equal(-12.00m, t.Amount);
        Assert.Null(t.Memo);
        Assert.Null(t.CheckNum);
    }

    // TXN-06: PAYMENT — NAME contains &amp; → must decode to '&'
    [Fact]
    public void Txn05_Payment_XmlEntityInName_Decoded()
    {
        var t = _txns[5];
        Assert.Equal(TransactionType.Payment, t.TransType);
        Assert.Equal("CITY ELECTRIC & GAS", t.Name);
    }

    // TXN-08: XFER — nested BANKACCTTO must not disturb core field parsing
    [Fact]
    public void Txn07_Xfer_WithNestedBankAcctTo()
    {
        var t = _txns[7];
        Assert.Equal(TransactionType.Xfer, t.TransType);
        Assert.Equal(-500.00m, t.Amount);
        Assert.Equal("2024100900001", t.Id);
    }

    // TXN-09: POS with ORIGCURRENCY (EUR converted) — parser uses TRNAMT
    [Fact]
    public void Txn08_Pos_OrigCurrency_UsesTrnAmt()
    {
        var t = _txns[8];
        Assert.Equal(TransactionType.Pos, t.TransType);
        Assert.Equal(-54.32m, t.Amount);
        Assert.Equal("2024101000001", t.Id);
    }

    // TXN-10: CORRECTACTION=REPLACE — parser treats as a normal transaction
    [Fact]
    public void Txn09_CorrectionReplace_ParsedAsTransaction()
    {
        var t = _txns[9];
        Assert.Equal(TransactionType.Pos, t.TransType);
        Assert.Equal(-52.17m, t.Amount);
        Assert.Equal("2024100300001-COR", t.Id);
    }

    // All 11 distinct TRNTYPE values present in the checking account BANKTRANLIST
    [Fact]
    public void AllCheckingAccountTransactionTypes_Present()
    {
        var types = _txns.Select(t => t.TransType).ToHashSet();
        Assert.Contains(TransactionType.DirectDep, types);
        Assert.Contains(TransactionType.Check, types);
        Assert.Contains(TransactionType.Pos, types);
        Assert.Contains(TransactionType.Atm, types);
        Assert.Contains(TransactionType.SrvChg, types);
        Assert.Contains(TransactionType.Payment, types);
        Assert.Contains(TransactionType.Int, types);
        Assert.Contains(TransactionType.Xfer, types);
        Assert.Contains(TransactionType.Debit, types);
        Assert.Contains(TransactionType.DirectDebit, types);
        Assert.Contains(TransactionType.Credit, types);
    }
}
