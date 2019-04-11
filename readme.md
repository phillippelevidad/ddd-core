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

namespace Application
{
    public class DoSomethingWhenTodoItemCompleted : IDomainEventHandler<TodoItemCompleted>
    {
        public async Task Handle(TodoItemCompleted domainEvent, CancellationToken cancellationToken)
        {
            // Something cool...
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

    public class CompleteTodoItemHandler : ICommandHandler<CompleteTodoItem>
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
    public class ListPendingTodoItemsHandler : IQueryHandler<ListPendingTodoItems, ListPendingTodoItemsResponse>
    {
        public async Task<ListPendingTodoItemsResponse> Handle(ListPendingTodoItems query, CancellationToken cancellationToken)
        {
            // infrastructure code to query the database
        }
    }
}
```

Finally, to send commands and run queries in your application:

``` csharp
public class MyController
{
    private readonly ICommandDispatcher commands;
    private readonly IQueryDispatcher queries;

    public MyController(ICommandDispatcher commands, IQueryDispatcher queries)
    {
        this.commands = commands;
        this.queries = queries;
    }

    public async Task OnGetAsync()
    {
        var pending = await queries.QueryAsync(new ListPendingTodoItems());
    }

    public async Task OnPostCompleteTodoAsync(Guid id)
    {
        await commands.DispatchAsync(new CompleteTodoItem(id));
    }
}
```

As for dispatching domain events, a google place to call the dispatcher might be your repository's' SaveChanges or similar, right before or right after the fact:

``` csharp
public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher dispatcher;

    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher) : base(options)
    {
        this.dispatcher = dispatcher;
    }

    public async override Task<int> SaveChangesAsync()
    {
        var result = await base.SaveChangesAsync();
        await DispatchDomainEventsAsync();
        return result;
    }

    private async Task DispatchDomainEventsAsync()
    {
        var events = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.HasDomainEvents)
            .SelectMany(e => e.Entity.ConsumeDomainEvents());

        foreach (var evt in events)
            await dispatcher.DispatchAsync(evt);
    }
}
```

## Dependency Injection and the use of MediatR

MediatR is used to bind Commands, DomainEvents and Queries to their respective handlers at runtime, so you need to register them.

**Long-story short**: In your `ConfigureServices` method, do:

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    // using Core.Infra
    services.AddCore();
}
```

This will automatically scan all assemblies at startup, which can be costly depending on how many references you have. Alternatively, you can specify which assemblies to look:

``` csharp
    services.AddCore(
        typeof(SomeClass).Assembly,
        typeof(AnotherClass).Assembly);
```
