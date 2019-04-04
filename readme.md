# DDD Core

A project with base classes and interfaces for building projects in a DDD approach.

## Building aggregates

Use `Entity` and `ValueObject` to build your domain classes.

Add events to entities to later have them published and produce side effects.

``` csharp
namespace Domain
{
    public class TodoItem : Entity
    {
        // ...

        public void MarkCompleted()
        {
            if (Completed) throw new InvalidOperationException("Already completed");

            Completed = true;
            AddDomainEvent(new TodoItemCompleted(this));
        }
    }
}
```

## CQRS

Create **Commands** by implementing the `ICommand` interface, and handle them by implementing `INotificationHandler<TCommand>` (MediatR).

``` csharp
namespace Application
{
    public class CompleteTodoItem : ICommand
    {
        public TodoItemId Id { get; set; }
    }

    public class CompleteTodoItemHandler : INotificationHandler<CompleteTodoItem>
    {
        public async Task Handle(CompleteTodoItem command, CancellationToken cancellationToken)
        {
            var item = await repository.GetAsync(command.Id);
            item.MarkCompleted();
            await repository.UpdateAsync(item);
        }
    }
}
```

Create **Queries** by implementing the `IQuery<>` interface, and handle them by implementing `IRequestHandler<TQuery, TResponse>` (MediatR).

``` csharp
namespace Application
{
    public class ListPendingTodoItems : IQuery<ListPendingTodoItemsResponse>
    {
    }

    public class ListPendingTodoItemsResponse : IQueryResponse
    {
        public IEnumerable<TodoItemData> TodoItems { get; set; }
    }
}

namespace Infra
{
    public class ListPendingTodoItemsHandler : IRequestHandler<ListPendingTodoItems, ListPendingTodoItemsResponse>
    {
        public async Task<ListPendingTodoItemsResponse> Handle(ListPendingTodoItems query, CancellationToken cancellationToken)
        {
            // infrastructure code to query the database
        }
    }
}
```