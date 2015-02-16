using Schloss.CQRS;
using Milkshake.Contracts.Commands;
using Milkshake.Domain.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milkshake.Domain
{
    public class DomainEntry
    {
        private readonly CommandDispatcher _commandDispatcher;

        public DomainEntry(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe = null, IEnumerable<Action<object>> postExecutionPipe = null)
        {
            preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            postExecutionPipe = CreatePostExecutionPipe(postExecutionPipe);
            _commandDispatcher = CreateCommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandDispatcher.ExecuteCommand(command);
        }

        private CommandDispatcher CreateCommandDispatcher(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            var commandDispatcher = new CommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);

            // Product Command Handler
            var productCommandHandler = new ProductCommandHandler(domainRepository);
            
            commandDispatcher.RegisterHandler<CreateProduct>(productCommandHandler);

            // Store Command Handler

            // Brand Command Handler

            // Manufacturer Command Handler

            /*var customerCommandHandler = new CustomerCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler<CreateCustomer>(customerCommandHandler);
            commandDispatcher.RegisterHandler<MarkCustomerAsPreferred>(customerCommandHandler);
                        

            var basketCommandHandler = new BasketCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler<CreateBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<AddItemToBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<ProceedToCheckout>(basketCommandHandler);
            commandDispatcher.RegisterHandler<CheckoutBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<MakePayment>(basketCommandHandler);

            var orderCommandHanler = new OrderHandler(domainRepository);
            commandDispatcher.RegisterHandler<ApproveOrder>(orderCommandHanler);
            commandDispatcher.RegisterHandler<StartShippingProcess>(orderCommandHanler);
            commandDispatcher.RegisterHandler<CancelOrder>(orderCommandHanler);
            commandDispatcher.RegisterHandler<ShipOrder>(orderCommandHanler);*/

            return commandDispatcher;
        }

        private IEnumerable<Action<object>> CreatePostExecutionPipe(IEnumerable<Action<object>> postExecutionPipe)
        {
            if (postExecutionPipe != null)
            {
                foreach (var action in postExecutionPipe)
                {
                    yield return action;
                }
            }
        }
    }
}
