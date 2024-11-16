using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () => "Welcome to Simple REST API with dotnet");


RouteGroupBuilder groupRoute = app.MapGroup("/todoitems");

groupRoute.MapPost("/", CreateTodo);
groupRoute.MapGet("/", GetAllTodos);
groupRoute.MapGet("/complete", GetCompleteTodos);
groupRoute.MapGet("/{id}", GetTodosById);
groupRoute.MapPut("/{id}", UpdateTodo);
groupRoute.MapDelete("/{id}", DeleteTodo);

app.Run();



static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
}



static async Task<IResult> GetAllTodos(TodoDb db)
{
    List<Todo> todos = await db.Todos.ToListAsync();

    return TypedResults.Ok(todos);
}



static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    List<Todo> completeTodos = await db.Todos.Where(item => item.IsComplete).ToListAsync();

    return TypedResults.Ok(completeTodos);
}



static async Task<IResult> GetTodosById(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id) 
        is Todo todo ? TypedResults.Ok(todo) 
        : TypedResults.NotFound();
}



static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if(todo is null) return TypedResults.NotFound();

    if(inputTodo.Name != null) todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete == true ? true : false;
 
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}



static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    var delTodo = await db.Todos.FindAsync(id);

    if(delTodo is Todo todo){
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}