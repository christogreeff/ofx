using System.Xml;
using Ofx.Configuration;
using Ofx.Repository;

namespace Ofx
{
    /// <summary>
    /// Transaction parser
    /// </summary>
    public class TransactionParser
    {
        /// <summary>
        /// Gets the file name
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Xml document
        /// </summary>
        private XmlDocument _doc = null;

        /// <summary>
        /// Statement
        /// </summary>
        public Statement ElectronicStatement { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionParser(string filename)
        {
            this.ReadFile(filename);
        }

        /// <summary>
        /// Read ofx file
        /// </summary>
        /// <param name="filename">File name to read</param>
        private void ReadFile(string filename)
        {
            this.File = filename;
            this._doc = new XmlDocument();
            this._doc.Load(filename);

            // little to no checking here
            // to be updated later if I need to

            // check ofx
            XmlElement ofx = this._doc[Config.S_OFX_HEADER];
            if (ofx != null)
            {
                // check bank element
                XmlElement bankmsgsrsv1 = ofx[Config.S_OFX_HEADER_BANK];
                if (bankmsgsrsv1 != null)
                {
                    // check statement transactions element
                    XmlElement stmttrnrs = bankmsgsrsv1[Config.S_OFX_HEADER_STATEMENT];
                    if (stmttrnrs != null)
                    {
                        // check statement element
                        XmlElement stmtrs = stmttrnrs[Config.S_OFX_HEADER_STATEMENT_TRANSACTIONS];

                        // get the full statement
                        this.ElectronicStatement = new Statement(stmtrs);
                    }
                }
            }
        }
    }
}
