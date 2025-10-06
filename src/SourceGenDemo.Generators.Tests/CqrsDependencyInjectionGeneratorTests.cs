using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenDemo.Generators.Tests;

public class CqrsDependencyInjectionGeneratorTests
{
    [Fact]
    public async Task Should_Create_An_Empty_Extension_Method_If_There_Are_No_Handlers()
    {
        var source =
            """
            public interface IRequest;

            public interface IRequest<out TResponse> : IRequest;

            public interface IRequestHandler<in TRequest, TResponse>
                where TRequest : IRequest<TResponse>
            {
                Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
            }
            """;
        
        await Verify(source);
    }
    
    [Fact]
    public async Task Should_Create_A_Scoped_Service_In_The_Extension_Method_If_There_Is_A_Handler()
    {
        var source =
            """
            public interface IRequest;

            public interface IRequest<out TResponse> : IRequest;

            public interface IRequestHandler<in TRequest, TResponse>
                where TRequest : IRequest<TResponse>
            {
                Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
            }
            
            public record TestQuery : IRequest<string>;
            
            public class TestQueryHandler : IRequestHandler<TestQuery, string>
            {
                public Task<string> Handle(TestQuery request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            }
            """;
        
        await Verify(source);
    }

    private async Task Verify(string source)
    {
        // Arrange
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree]);
        
        var generator = new CqrsDependencyInjectionGenerator();
        
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        
        // Act
        driver = driver.RunGenerators(compilation);
        
        // Assert
        await Verifier.Verify(driver);
    }
}
