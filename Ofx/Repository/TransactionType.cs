using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ofx.Repository
{
    /// <summary>
    /// Transaction Type
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Debit
        /// </summary>
        Debit = 0,

        /// <summary>
        /// Credit
        /// </summary>
        Credit = 1,

        /// <summary>
        /// Other
        /// </summary>
        Other = 2,
    }
}
