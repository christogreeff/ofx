using System;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository
{
    /// <summary>
    /// Statement
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// Gets the currency
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Gets bank account details
        /// </summary>
        public BankAccount Account { get; private set; }

        /// <summary>
        /// Get ledger balance
        /// </summary>
        public LedgerBalance Ledger { get; private set; }

        /// <summary>
        /// Statement transaction
        /// </summary>
        public StatementTransaction Transaction { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Statement(XmlElement stmtrs)
        {
            this.BuildStatementInformation(stmtrs);
        }

        /// <summary>
        /// Build statement information
        /// </summary>
        /// <param name="stmtrs"></param>
        private void BuildStatementInformation(XmlElement stmtrs)
        {
            if (stmtrs != null)
            {
                this.Currency = Convert.ToString(stmtrs[Config.S_OFX_STATEMENT_CURRENCY].InnerText).Trim();
                this.Account = new BankAccount(stmtrs);
                this.Ledger = new LedgerBalance(stmtrs);
                this.Transaction = new StatementTransaction(stmtrs);
            }
        }
    }
}
