using System.Text;
using Microsoft.CodeAnalysis;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, (ctx, compilation) =>
        {
            var builder = new StringBuilder();
            var local = compilation.GetAllTypes().Single(x => x.Name == "Local");
            var localMembers = local.GetMembers().Select(x => x.Name).OrderBy(x => x).ToList();

            var referenced = compilation.GetUsedAssemblyReferences()
                .Select(x => compilation.GetAssemblyOrModuleSymbol(x))
                .Where(x => x is IAssemblySymbol)
                .Select(x => (IAssemblySymbol)x)
                .SelectMany(x => x.GetAllTypes())
                .Single(x => x.Name == "External");

            var referencedMembers = referenced.GetMembers().Select(x => x.Name).OrderBy(x => x).ToList();
            var left = 0;
            var right = 0;

            builder.Append("// ").Append(local.Name).Append(new string(' ', 40 - local.Name.Length)).AppendLine(referenced.Name);
            builder.Append("// ").AppendLine(new string('-', 80));

            while (left < localMembers.Count)
            {
                var x = localMembers[left];
                var y = referencedMembers[right];
                // print both members if they match
                // print only the local member if it's not in the referenced type
                // increment variables accordingly
                if (x == y)
                {
                    builder.Append("// ").Append(x).Append(new string(' ', 40 - x.Length)).AppendLine(y);
                    left++;
                    right++;
                }
                else
                {
                    builder.Append("// ").Append(x).Append(new string(' ', 40 - x.Length)).AppendLine();
                    left++;
                }
            }

            ctx.AddSource("RecordMembers.cs", builder.ToString());
        });
    }
}