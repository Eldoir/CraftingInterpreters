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

         static List<string> DefineType(
             string baseName,
             string className,
             string fieldList)
        {
            // Construcctor
            List<string> result = [
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
                result.Add($"\t\t\tthis._{name} = {name};");
            }

            result.Add("\t\t}");
            result.Add(string.Empty);

            // Fields
            foreach (string field in fields)
            {
                string[] typeAndName = field.Split(" ");
                result.Add($"\t\tprivate readonly {typeAndName[0]} _{typeAndName[1]};");
            }

            result.Add("\t}");

            return result;
        }
        #endregion
    }
}