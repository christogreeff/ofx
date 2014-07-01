using System;
using System.Xml;
using Ofx.Configuration;

namespace Ofx.Repository
{
    /// <summary>
    /// Transaction class
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction Type Text 
        /// </summary>
        public string TransText { get; private set; }

        /// <summary>
        /// Transaction Type enumerator
        /// </summary>
        public TransactionType TransType { get; private set; }

        /// <summary>
        /// Gets Transaction Date
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets Transaction Amount
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets Transaction Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets Transaction Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets Transaction Memo
        /// </summary>
        public string Memo { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="element">Xml Element</param>
        public Transaction(XmlNode node)
        {
            this.ParseNode(node);
        }

        /// <summary>
        /// Parse the Transaction xml node
        /// </summary>
        /// <param name="node"></param>
        private void ParseNode(XmlNode node)
        {
            this.TransText = Convert.ToString(node[Config.S_OFX_TRANSACTION_TYPE].InnerText).Trim();

            // set credit, debit or other
            switch (this.TransText.ToLower())
            {
                case Config.S_OFX_TEXT_CREDIT: this.TransType = TransactionType.Credit;
                    break;
                case Config.S_OFX_TEXT_DEBIT: this.TransType = TransactionType.Debit;
                    break;
                default:
                    this.TransType = TransactionType.Other;
                    break;
            }

            // set values
            this.Date = DateTime.ParseExact(Convert.ToString(node[Config.S_OFX_TRANSACTION_DATE].InnerText).Trim(), Config.S_OFX_DATE_FORMAT, null);
            this.Amount = Convert.ToDecimal(Convert.ToString(node[Config.S_OFX_TRANSACTION_AMOUNT].InnerText).Trim());
            this.Id = Convert.ToString(node[Config.S_OFX_TRANSACTION_ID].InnerText).Trim();
            this.Name = Convert.ToString(node[Config.S_OFX_TRANSACTION_NAME].InnerText).Trim();
            this.Memo = Convert.ToString(node[Config.S_OFX_TRANSACTION_MEMO].InnerText).Trim();
        }
    }
}
