# Sample OFX Files

[ofx-sample-2.3.xml](samples/ofx-sample-2.3.xml) — OFX 2.3 XML (primary)

[ofx-sample-1x.ofx](samples/ofx-sample-1x.ofx) — OFX 1.x SGML (legacy compat)

**Validation results:** XML well-formed · 27 unique FITIDs · all 18 TRNTYPE values covered · correct element ordering on all 24 `<STMTTRN>` elements · valid ISO 4217 currency codes.

**What the 2.3 XML file exercises:**

- Three account types: checking, savings, credit card
- All 18 `TRNTYPE` values across posted and pending transactions
- `CORRECTACTION=REPLACE` correcting a wrong amount (TXN-03 → TXN-10)
- `CORRECTACTION=DELETE` removing a duplicate (CC-TXN-04)
- Full `<PAYEE>` aggregate with all 9 fields (NAME through PHONE)
- `<ORIGCURRENCY>` — EUR amount already converted to USD (parser should use TRNAMT)
- `<CURRENCY>` — GBP amount not converted (parser should use the CURSYM amount)
- `<BANKACCTTO>` on a XFER transaction
- `<CCACCTTO>` on a credit card payment
- `<BALLIST>` with named `<BAL>` aggregates
- `<BANKTRANLISTP>` with `HOLD` and pending `POS`
- Error `<STMTTRNRS>` (STATUS 2003, no nested `<STMTRS>`) — parser must not crash on absent response aggregate
- XML special characters escaped in `<MEMO>` and `<NAME>` (`&amp;`, `&lt;`, `&gt;`)
- Both full datetime (`YYYYMMDDHHMMSS.XXX[offset:TZ]`) and date-only (`YYYYMMDD`) forms

The 1.x SGML file covers the same checking account with the key structural difference: leaf element values follow the opening tag with no closing tag, and the header is a key:value block separated from the body by a blank line.
