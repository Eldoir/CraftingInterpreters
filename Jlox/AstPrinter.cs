using System.Globalization;
using System.Text;

namespace Jlox;

public class AstPrinter : Expr.IVisitor<string>
{
    #region IVisitor
    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
        if (expr.Value is null) return "nil";

        // This is an addition to the Crafting Interpreters book (chapter 5)
        if (expr.Value is IFormattable formattable)
        {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }

        return expr.Value.ToString() ?? "<null>";
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return Parenthesize(expr.Op.Lexeme, expr.Right);
    }
    #endregion

    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    private string Parenthesize(string name, params Expr[] exprs)
    {
        StringBuilder builder = new();

        builder.Append('(').Append(name);
        foreach (Expr expr in exprs)
        {
            builder.Append(' ');
            builder.Append(expr.Accept(this));
        }
        builder.Append(')');

        return builder.ToString();
    }
}
