# Repro for missing backing field in referenced project record with primary constructor

See [reported issue](https://github.com/dotnet/roslyn/issues/74634).

## Steps to reproduce

Run the project setting a breakpoint in the generator.
Alternatively, just compile the solution and inspect the generated 
code in `\Library\obj\Debug\net8.0\generated\Analyzer\Generator\RecordMembers.cs`
which looks like the following and showcases the missing field symbol:

```
// Local                                   External
// --------------------------------------------------------------------------------
// .ctor                                   .ctor
// .ctor                                   .ctor
// <Amount>k__BackingField                 
// <Clone>$                                <Clone>$
// Amount                                  Amount
// Deconstruct                             Deconstruct
// EqualityContract                        EqualityContract
// Equals                                  Equals
// Equals                                  Equals
// get_Amount                              get_Amount
// get_EqualityContract                    get_EqualityContract
// GetHashCode                             GetHashCode
// op_Equality                             op_Equality
// op_Inequality                           op_Inequality
// PrintMembers                            PrintMembers
// set_Amount                              set_Amount
// ToString                                ToString
```

Both record definitions are the same except for the name:

```csharp
// Library
public record Local(int Amount);

// External
public record External(int Amount);
```
