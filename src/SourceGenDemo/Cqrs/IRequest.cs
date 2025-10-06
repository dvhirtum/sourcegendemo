namespace SourceGenDemo.Cqrs;

public interface IRequest;

public interface IRequest<out TResponse> : IRequest;
