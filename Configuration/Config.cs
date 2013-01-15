using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ofx.Configuration
{
    /// <summary>
    /// Static configuration class
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Ofx "Credit" string
        /// </summary>
        public const string S_OFX_TEXT_CREDIT = "credit";

        /// <summary>
        /// Ofx "Debit" string
        /// </summary>
        public const string S_OFX_TEXT_DEBIT = "debit";

        /// <summary>
        /// Ofx date format
        /// </summary>
        public const string S_OFX_DATE_FORMAT = "yyyyMMdd";

        /// <summary>
        /// Ofx transaction date
        /// </summary>
        public const string S_OFX_TRANSACTION_DATE = "DTPOSTED";

        /// <summary>
        /// Ofx transaction amount
        /// </summary>
        public const string S_OFX_TRANSACTION_AMOUNT = "TRNAMT";

        /// <summary>
        /// Ofx transaction id
        /// </summary>
        public const string S_OFX_TRANSACTION_ID = "FITID";

        /// <summary>
        /// Ofx transaction name
        /// </summary>
        public const string S_OFX_TRANSACTION_NAME = "NAME";

        /// <summary>
        /// Ofx transaction memo
        /// </summary>
        public const string S_OFX_TRANSACTION_MEMO = "MEMO";

        /// <summary>
        /// Ofx transaction type
        /// </summary>
        public const string S_OFX_TRANSACTION_TYPE = "TRNTYPE";

        /// <summary>
        /// Ofx statement transactions
        /// </summary>
        public const string S_OFX_STATEMENT_TRANSACTIONS = "STMTTRN";

        /// <summary>
        /// Ofx statement start
        /// </summary>
        public const string S_OFX_STATEMENT_START = "DTSTART";

        /// <summary>
        /// Ofx statement end
        /// </summary>
        public const string S_OFX_STATEMENT_END = "DTEND";

        /// <summary>
        /// Ofx bank transaction list
        /// </summary>
        public const string S_OFX_STATEMENT_BANKTRANSLIST = "BANKTRANLIST";

        /// <summary>
        /// Ofx statement currency
        /// </summary>
        public const string S_OFX_STATEMENT_CURRENCY = "CURDEF";

        /// <summary>
        /// Ofx bank account bank id
        /// </summary>
        public const string S_OFX_BANK_ACCOUNT_BANK_ID = "BANKID";

        /// <summary>
        /// Ofx bank account id
        /// </summary>
        public const string S_OFX_BANK_ACCOUNT_ID = "ACCTID";

        /// <summary>
        /// Ofx bank account type
        /// </summary>
        public const string S_OFX_BANK_ACCOUNT_TYPE = "ACCTTYPE";

        /// <summary>
        /// Ofx bank account from
        /// </summary>
        public const string S_OFX_BANK_ACCOUNT_FROM = "BANKACCTFROM";

        /// <summary>
        /// Ofx ledger balance
        /// </summary>
        public const string S_OFX_LEDGER_BALANCE = "LEDGERBAL";
        
        /// <summary>
        /// Ofx ledger balance
        /// </summary>
        public const string S_OFX_LEDGER_BALANCE_AMOUNT = "BALAMT";

        /// <summary>
        /// Ofx ledger balance
        /// </summary>
        public const string S_OFX_LEDGER_BALANCE_DATE = "DTASOF";

        /// <summary>
        /// Ofx header
        /// </summary>
        public const string S_OFX_HEADER = "OFX";

        /// <summary>
        /// Ofx header bank
        /// </summary>
        public const string S_OFX_HEADER_BANK = "BANKMSGSRSV1";

        /// <summary>
        /// Ofx header statement
        /// </summary>
        public const string S_OFX_HEADER_STATEMENT = "STMTTRNRS";

        /// <summary>
        /// Ofx header statement transactions
        /// </summary>
        public const string S_OFX_HEADER_STATEMENT_TRANSACTIONS = "STMTRS";
    }
}
