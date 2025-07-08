namespace Jlox.Tests;

public sealed class AstPrinterTests
{
    [Fact]
    public void Correct_output()
    {
        Expr expression = new Expr.Binary(
            new Expr.Unary(
                new Token(TokenType.Minus, "-", null, 1),
                new Expr.Literal(123)),
            new Token(TokenType.Star, "*", null, 1),
            new Expr.Grouping(
                new Expr.Literal(45.67f)));

        Assert.Equal("(* (- 123) (group 45.67))", new AstPrinter().Print(expression));
    }
}
