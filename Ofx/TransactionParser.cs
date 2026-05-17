using System.Text.RegularExpressions;
using System.Xml;
using Ofx.Configuration;
using Ofx.Repository;

namespace Ofx;

/// <summary>
/// Loads an OFX file and parses it into a <see cref="Statement"/>.
/// Supports both OFX 2.x XML format and OFX 1.x SGML format (used by FNB and ABSA).
/// Navigates the bank statement envelope: <c>OFX → BANKMSGSRSV1 → STMTTRNRS → STMTRS</c>.
/// </summary>
public class TransactionParser : ITransactionParser
{
    /// <inheritdoc/>
    public string File { get; private set; } = string.Empty;

    /// <inheritdoc/>
    public Statement? ElectronicStatement { get; private set; }

    /// <param name="filename">Path to the OFX file to parse.</param>
    public TransactionParser(string filename)
    {
        ReadFile(filename);
    }

    private void ReadFile(string filename)
    {
        File = filename;

        var raw = System.IO.File.ReadAllText(filename);
        var xml = IsOfxSgml(raw) ? ConvertSgmlToXml(raw) : raw;

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var ofx = doc[Config.S_OFX_HEADER];
        if (ofx is null)
            return;

        var bankmsgsrsv1 = ofx[Config.S_OFX_HEADER_BANK];
        if (bankmsgsrsv1 is null)
            return;

        var stmttrnrs = bankmsgsrsv1[Config.S_OFX_HEADER_STATEMENT];
        if (stmttrnrs is null)
            return;

        var stmtrs = stmttrnrs[Config.S_OFX_HEADER_STATEMENT_TRANSACTIONS];
        if (stmtrs is null)
            return;

        ElectronicStatement = new Statement(stmtrs);
    }

    // OFX 1.x SGML files start with a plain-text header block whose first line is "OFXHEADER:".
    private static bool IsOfxSgml(string content) =>
        content.TrimStart().StartsWith("OFXHEADER:", StringComparison.OrdinalIgnoreCase);

    // Converts an OFX 1.x SGML document to well-formed XML so that XmlDocument can parse it.
    //
    // Two transformations are applied:
    //   1. The SGML header block (all plain-text lines before the first '<') is stripped.
    //   2. Closing tags are injected for leaf elements. OFX 1.x SGML omits them:
    //        <BANKID>FNB          (SGML — no closing tag)
    //        <BANKID>FNB</BANKID> (XML — closing tag required)
    //      Aggregate elements already carry closing tags in FNB/ABSA files and are unaffected.
    private static string ConvertSgmlToXml(string content)
    {
        var xmlStart = content.IndexOf('<');
        if (xmlStart < 0)
            throw new FormatException("OFX content contains no XML elements.");

        // Inject </TAG> after text values that are not followed by a child element.
        // The pattern matches <TAGNAME> followed by non-empty text on the same line.
        // Text values are XML-escaped because OFX 1.x SGML allows raw characters such as
        // '&' that are illegal in XML (e.g. <NAME>CITY ELECTRIC & GAS).
        return Regex.Replace(
            content[xmlStart..],
            @"<([A-Z][A-Z0-9]*)>([^<\r\n]+)",
            m =>
            {
                var text = m.Groups[2].Value.TrimEnd()
                             .Replace("&", "&amp;")
                             .Replace(">", "&gt;");
                return $"<{m.Groups[1].Value}>{text}</{m.Groups[1].Value}>";
            });
    }
}
