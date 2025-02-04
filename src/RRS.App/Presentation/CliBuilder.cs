using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace RRS.App.Presentation;

internal class CliBuilder
{
    private readonly RootCommand _rootCommand;
    private readonly IServiceProvider _serviceProvider;

    public CliBuilder(IServiceProvider serviceProvider)
    {
        _rootCommand = new RootCommand("Room Reservation System");
        _serviceProvider = serviceProvider;
    }

    public CliCommandBuilder CreateCommand(string name, string description)
    {
        var command = new Command(name, description);
        _rootCommand.AddCommand(command);

        return new CliCommandBuilder(command, this, _serviceProvider);
    }

    public RootCommand Build() => _rootCommand;

    internal class CliCommandBuilder
    {
        private readonly Command _command;
        private readonly CliBuilder? _cliBuilder;
        private readonly CliCommandBuilder? _parentCommandBuilder;
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<Option> _options = new List<Option>();

        public CliCommandBuilder(Command command, CliBuilder parent, IServiceProvider serviceProvider) : this(command, serviceProvider)
        {
            _cliBuilder = parent;
        }

        public CliCommandBuilder(Command command, CliCommandBuilder parent, IServiceProvider serviceProvider) : this(command, serviceProvider)
        {
            _parentCommandBuilder = parent;
        }

        private CliCommandBuilder(Command command, IServiceProvider serviceProvider)
        {
            _command = command;
            _serviceProvider = serviceProvider;
        }


        public CliCommandBuilder AddOption<T>(string name, string? description = null)
        {
            var option = new Option<T>(name, description);

            _options.Add(option);
            _command.AddOption(option);

            return this;
        }

        public CliCommandBuilder CreateSubCommand(string name, string description)
        {
            var subCommand = new Command(name, description);
            _command.AddCommand(subCommand);

            return new CliCommandBuilder(subCommand, this, _serviceProvider);
        }

        public CliCommandBuilder SetHandler<T>() where T : ICliCommandHandler
        {
            _command.SetHandler(async (context) =>
            {
                var commandHandler = _serviceProvider.GetRequiredService<T>();

                var optionValues = new Dictionary<string, object?>(_options.Count);

                foreach (var item in _options)
                {
                    var value = context.ParseResult.GetValueForOption(item);
                    optionValues.Add(item.Name, value);
                }

                await commandHandler.Handle(optionValues);
            });

            return this;
        }

        public CliBuilder BuildCommand()
        {
            if (_cliBuilder == null)
            {
                throw new InvalidOperationException();
            }

            return _cliBuilder;
        }

        public CliCommandBuilder BuildSubcommand()
        {
            if (_parentCommandBuilder == null)
            {
                throw new InvalidOperationException();
            }

            return _parentCommandBuilder;
        }
    }
}

