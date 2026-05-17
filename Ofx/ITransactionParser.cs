using Ofx.Repository;

namespace Ofx;

/// <summary>
/// Parses an OFX XML file and exposes the resulting electronic statement.
/// </summary>
public interface ITransactionParser
{
    /// <summary>Path to the OFX file that was loaded.</summary>
    string File { get; }

    /// <summary>
    /// The parsed statement, or <see langword="null"/> if the file did not contain
    /// a recognisable <c>OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS</c> structure.
    /// </summary>
    Statement? ElectronicStatement { get; }
}
