# PseudoMediatR

The repository contains the implementation of the popular library **MediatR**, which is widely used in backend application development.  

## 📌 Credits
- The author of the library **MediatR**: [Jimmy Bogard](https://github.com/jbogard)  
- The repository of the original library: [MediatR on GitHub](https://github.com/jbogard/MediatR)

## 📂 Structure
- `DependencyInjection/` — ready extensions for injection of the services that are used in the library.
- `Implementations/` — ready implementations of contracts (Sender).
- `Interfaces/` — contracts for further usage (ISender, IRequest, IRequestHandler, IAsyncRequestHandler).

## 🚀 Usage Example

```csharp
app.MapPost("/one", ([FromServices] ISender sender, [FromBody] OneRequest request) =>
{
    var response = sender.Send<OneRequest, OneResponse>(request);
    return Results.Ok(response);
});
```

## ⚙️ DI Registration

Apply default configuration for DI:

```csharp
builder.Services.AddPseudoMediatR();
```

Or setup your own configuration for DI:

```csharp
builder.Services.AddPseudoMediatR(config=>
{
    config
    .SetLifetime(ServiceLifetime.Transient)
    .SetAssembly(Assembly.GetExecutingAssembly())
    .InjectSender()
    .InjectHandlers();
});
```



