# OFX

Lightweight .NET 10 library for parsing OFX bank statement files. Handles both OFX 2.x XML and
OFX 1.x SGML formats (including files exported by South African banks such as ABSA and FNB).
Extracts statements, accounts, transactions, and balances — without the overhead of a full OFX stack.

## Specifications

The implementation is aligned with two specifications:

- **OFX Banking Specification v2.3** (October 2020, Financial Data Exchange) — used as the primary
  reference for element names, data types, transaction type values, and account type values.
- **FNB File Specification: Statement Type OFX** (July 2012, First National Bank) — used as the
  bank-specific reference for the exact fields, field lengths, and data formats produced by South
  African banks.

## Requirements

- .NET 10

## File Format Support

The parser handles both OFX file formats:

| Format | Version | Detection |
| ------ | ------- | --------- |
| OFX 2.x XML | Standard XML | File starts with `<OFX>` |
| OFX 1.x SGML | Used by FNB / ABSA | File starts with `OFXHEADER:100` |

OFX 1.x SGML files are automatically converted to well-formed XML before parsing.

## Supported OFX Elements

| Element | Description |
| ------- | ----------- |
| `STMTRS` | Statement currency and account details |
| `BANKACCTFROM` | Bank account identifier (BANKID, ACCTID, ACCTTYPE) |
| `BANKTRANLIST` | Transaction list with DTSTART / DTEND |
| `STMTTRN` | Individual transactions — all 18 OFX TRNTYPE values, FITID, NAME, MEMO, CHECKNUM, REFNUM |
| `LEDGERBAL` | Ledger balance and date |
| `AVAILBAL` | Available balance and date (optional) |

## Solution Structure

```text
Ofx.slnx                   ← solution file
Ofx/                       ← library project
  Ofx.csproj
  ITransactionParser.cs
  TransactionParser.cs
  Configuration/
  Repository/
Ofx.Tests/                 ← xUnit test project
  Ofx.Tests.csproj
  TransactionParserXmlTests.cs
  TransactionParserSgmlTests.cs
docs/                      ← specification docs and sample OFX files
README.md
```

## Usage

```csharp
ITransactionParser parser = new TransactionParser("statement.ofx");
Statement? statement = parser.ElectronicStatement;
```

## Testing

The `Ofx.Tests` project validates the parser against two sample files in [`docs/samples/`](docs/samples/):

| Sample file | Format | What it covers |
| ----------- | ------ | -------------- |
| [`ofx-sample-2.3.xml`](docs/samples/ofx-sample-2.3.xml) | OFX 2.3 XML | Checking account with 13 transactions; full datetime format; XML entity decoding (`&amp;` → `&`); `CORRECTACTION=REPLACE`; `ORIGCURRENCY`; nested `BANKACCTTO`; ledger and available balances |
| [`ofx-sample-1x.ofx`](docs/samples/ofx-sample-1x.ofx) | OFX 1.x SGML | Checking account with 9 transactions; date-only format (`YYYYMMDD`); raw `&` in leaf values (escaped during SGML-to-XML conversion); nested `BANKACCTTO` aggregate |

Run via the solution (builds both projects):

```shell
dotnet test
```

Or target the test project directly:

```shell
dotnet test Ofx.Tests/Ofx.Tests.csproj
```
