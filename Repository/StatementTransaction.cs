using System;
using System.Collections.Generic;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository
{
    /// <summary>
    /// Statement Transaction
    /// </summary>
    public class StatementTransaction
    {
        /// <summary>
        /// Gets the Transactions
        /// </summary>
        public List<Transaction> Transactions
        {
            get
            {
                return this._transactions;
            }
        }

        /// <summary>
        /// Gets the statement start date
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the statement end date
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Internal transactions list
        /// </summary>
        private List<Transaction> _transactions;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="stmtrs">XmlElement object</param>
        public StatementTransaction(XmlElement stmtrs)
        {
            BuildTransactionList(stmtrs);
        }

        /// <summary>
        /// Build transaction list
        /// </summary>
        private void BuildTransactionList(XmlElement stmtrs)
        {
            this._transactions = new List<Transaction>();

            if (stmtrs != null)
            {
                XmlElement banktranlist = stmtrs[Config.S_OFX_STATEMENT_BANKTRANSLIST];

                if (banktranlist != null)
                {
                    // set dates
                    this.StartDate = DateTime.ParseExact(Convert.ToString(banktranlist[Config.S_OFX_STATEMENT_START].InnerText).Trim(), Config.S_OFX_DATE_FORMAT, null);
                    this.EndDate = DateTime.ParseExact(Convert.ToString(banktranlist[Config.S_OFX_STATEMENT_END].InnerText).Trim(), Config.S_OFX_DATE_FORMAT, null);

                    // build transactions
                    XmlNodeList transactions = banktranlist.GetElementsByTagName(Config.S_OFX_STATEMENT_TRANSACTIONS);

                    if (transactions != null)
                        foreach (XmlNode node in transactions)
                            this._transactions.Add(new Transaction(node));
                }
            }
        }
    }
}
