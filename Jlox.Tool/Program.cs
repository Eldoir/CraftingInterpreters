namespace Jlox.Tool;

public partial class GenerateAst
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            args = ["D:\\Fork_Repos\\CraftingInterpreters\\Jlox\\Expr"]; // default value
        }

        if (args.Length != 1)
        {
            Console.WriteLine("Usage: Jlox.Tool <output_directory>");
            return;
        }

        string outputDir = args[0];
        DefineAst(outputDir, "Expr", [
            "Binary   : Expr left, Token op, Expr right",
            "Grouping : Expr expression",
            "Literal  : object value",
            "Unary    : Token op, Expr right"
        ]);
    }

    private static void DefineAst(
        string outputDir,
        string baseName,
        List<string> types)
    {
        string path = Path.Join(outputDir, baseName) + ".cs";

        List<string> lines = [
            "namespace Jlox;",
            string.Empty,
            $"public abstract class {baseName}",
            "{"
        ];

        lines.AddRange(DefineVisitor(baseName, types));
        lines.Add(string.Empty);

        // The AST classes.
        foreach (string type in types)
        {
            string[] split = type.Split(":");
            string className = split[0].Trim();
            string fields = split[1].Trim();
            lines.AddRange(DefineType(baseName, className, fields));
            lines.Add(string.Empty);
        }

        lines.Add("}");
        File.WriteAllText(path, string.Join(Environment.NewLine, lines));

        #region Local methods

        static List<string> DefineVisitor(string baseName, List<string> types)
        {
            List<string> lines = [
                "\tpublic interface IVisitor<T>",
                "\t{"
            ];

            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                lines.Add($"\t\tT Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            lines.Add("\t}");
            lines.Add(string.Empty);
            lines.Add("\tpublic abstract T Accept<T>(IVisitor<T> visitor);");

            return lines;
        }

         static List<string> DefineType(
             string baseName,
             string className,
             string fieldList)
        {
            // Construcctor
            List<string> lines = [
                $"\tpublic class {className} : {baseName}",
                "\t{",
                $"\t\t{className}({fieldList})",
                "\t\t{"
            ];

            // Store parameters in fields
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                lines.Add($"\t\t\tthis._{name} = {name};");
            }

            lines.Add("\t\t}");
            lines.Add(string.Empty);

            // Visitor pattern
            lines.Add("\t\tpublic override T Accept<T>(IVisitor<T> visitor)");
            lines.Add("\t\t{");
            lines.Add($"\t\t\treturn visitor.Visit{className}{baseName}(this);");
            lines.Add("\t\t}");
            lines.Add(string.Empty);

            // Fields
            foreach (string field in fields)
            {
                string[] typeAndName = field.Split(" ");
                lines.Add($"\t\tprivate readonly {typeAndName[0]} _{typeAndName[1]};");
            }

            lines.Add("\t}");

            return lines;
        }
        #endregion
    }
}