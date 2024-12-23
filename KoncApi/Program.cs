using KoncApi;
using KoncAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();    
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<VenueQuery>();
builder.Services.AddScoped<VenueMutation>();
builder.Services.AddScoped<BookingQuery>();
builder.Services.AddScoped<BookingMutation>();
builder.Services.AddScoped<EventQuery>();
builder.Services.AddScoped<EventMutation>();
builder.Services.AddScoped<UserQuery>();
builder.Services.AddScoped<UserMutation>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddTypeExtension<VenueQuery>()
    .AddTypeExtension<EventQuery>()
    .AddTypeExtension<BookingQuery>()
    .AddTypeExtension<UserQuery>()
    .AddMutationType(d => d.Name("Mutation"))  
    .AddTypeExtension<VenueMutation>()
    .AddTypeExtension<BookingMutation>()
    .AddTypeExtension<EventMutation>()
    .AddTypeExtension<UserMutation>();

builder.Services.AddControllers();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    SeedData.Initialize(dbContext); 
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KoncAPI v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapHub<BookingHub>("/bookinghub");

app.MapGet("/", () => "Hello from KoncAPI with SignalR!");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL();
});

app.Run();