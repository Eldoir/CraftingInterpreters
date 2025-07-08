namespace Jlox;

public abstract class Expr
{
	public interface IVisitor<T>
	{
		T VisitBinaryExpr(Binary expr);
		T VisitGroupingExpr(Grouping expr);
		T VisitLiteralExpr(Literal expr);
		T VisitUnaryExpr(Unary expr);
	}

	public abstract T Accept<T>(IVisitor<T> visitor);

	public class Binary : Expr
	{
		Binary(Expr left, Token op, Expr right)
		{
			this._left = left;
			this._op = op;
			this._right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitBinaryExpr(this);
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

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitGroupingExpr(this);
		}

		private readonly Expr _expression;
	}

	public class Literal : Expr
	{
		Literal(object value)
		{
			this._value = value;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitLiteralExpr(this);
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

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitUnaryExpr(this);
		}

		private readonly Token _op;
		private readonly Expr _right;
	}

}