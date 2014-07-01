using System;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository
{
    /// <summary>
    /// Bank account class
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// Gets the Bank Id
        /// </summary>
        public string BankId { get; private set; }

        /// <summary>
        /// Gets the account id
        /// </summary>
        public string AccountId { get; private set; }

        /// <summary>
        /// Gets the account type
        /// </summary>
        public string AccountType { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="stmtrs"></param>
        public BankAccount(XmlElement stmtrs)
        {
            this.GetBankAccountDetails(stmtrs);
        }

        /// <summary>
        /// Get bank account details
        /// </summary>
        /// <param name="stmtrs"></param>
        private void GetBankAccountDetails(XmlElement stmtrs)
        {
            XmlElement bankacctfrom = stmtrs[Config.S_OFX_BANK_ACCOUNT_FROM];

            if (bankacctfrom != null)
            {
                this.BankId = Convert.ToString(bankacctfrom[Config.S_OFX_BANK_ACCOUNT_BANK_ID].InnerText).Trim();
                this.AccountId = Convert.ToString(bankacctfrom[Config.S_OFX_BANK_ACCOUNT_ID].InnerText).Trim();
                this.AccountType = Convert.ToString(bankacctfrom[Config.S_OFX_BANK_ACCOUNT_TYPE].InnerText).Trim();
            }
        }
    }
}
