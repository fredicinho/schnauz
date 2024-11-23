using Schnauz.Shared.Interfaces;
using System;
using System.Reflection;

namespace Schnauz.CommandHandlers.Core
{
    public class CommandExecuter(IServiceProvider provider/*, IDbContextFactory<DbContext> contextFactory*/) : ICommandExecutor
    {
        private readonly IServiceProvider _provider = provider;
        //private readonly IDbContextFactory<tDbContext> _contextFactory = contextFactory;

        public async Task<bool> Send(ICommand cqrsCommand)
        {
            //var dbContext = await _contextFactory.CreateDbContextAsync();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(cqrsCommand.GetType());
            var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.Execute),
                BindingFlags.Public | BindingFlags.Instance) ?? throw new NotSupportedException();

            await (Task)method.Invoke(_provider.GetService(handlerType), [cqrsCommand/*, dbContext*/])!;
            //await dbContext.SaveChangesAsync();
            //dbContext.Dispose();
            return true;
        }
    }
}
