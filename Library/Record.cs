// #error version

public record Local(int Amount);

public static class Program
{
    public static void Main()
    {
        // Fake consuming the referenced type, to bring it into the GetUsedAssemblyReferences
        Console.WriteLine(new External(42).Amount);
    }
}