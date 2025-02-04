namespace RRS.App.Presentation;
internal interface ICliCommandHandler
{
    Task Handle(Dictionary<string, object?> options);
}
