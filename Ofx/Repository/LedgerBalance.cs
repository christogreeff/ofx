using System;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository
{
    /// <summary>
    /// Ledger balance
    /// </summary>
    public class LedgerBalance
    {
        /// <summary>
        /// Gets the balance
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Gets balance date
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="stmtrs">XmlElement object</param>
        public LedgerBalance(XmlElement stmtrs)
        {
            this.GetBalance(stmtrs);
        }

        /// <summary>
        /// Get balance
        /// </summary>
        /// <param name="stmtrs">XmlElement object</param>
        private void GetBalance(XmlElement stmtrs)
        {
            if (stmtrs != null)
            {
                XmlElement ledgerbal = stmtrs[Config.S_OFX_LEDGER_BALANCE];

                if (ledgerbal != null)
                {
                    this.Balance = Convert.ToDecimal(ledgerbal[Config.S_OFX_LEDGER_BALANCE_AMOUNT].InnerText);
                    this.Date = DateTime.ParseExact(Convert.ToString(ledgerbal[Config.S_OFX_LEDGER_BALANCE_DATE].InnerText).Trim(), Config.S_OFX_DATE_FORMAT, null);
                }
            }
        }
    }
}
