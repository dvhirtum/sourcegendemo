using System.Runtime.CompilerServices;

namespace SourceGenDemo.Generators.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() => VerifySourceGenerators.Initialize();
}
