namespace Jlox;

public abstract class Expr
{
	public class Binary : Expr
	{
		Binary(Expr left, Token op, Expr right)
		{
			this._left = left;
			this._op = op;
			this._right = right;
		}

		private readonly Expr _left;
		private readonly Token _op;
		private readonly Expr _right;
	}

	public class Grouping : Expr
	{
		Grouping(Expr expression)
		{
			this._expression = expression;
		}

		private readonly Expr _expression;
	}

	public class Literal : Expr
	{
		Literal(object value)
		{
			this._value = value;
		}

		private readonly object _value;
	}

	public class Unary : Expr
	{
		Unary(Token op, Expr right)
		{
			this._op = op;
			this._right = right;
		}

		private readonly Token _op;
		private readonly Expr _right;
	}

}