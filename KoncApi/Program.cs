using KoncApi;
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
builder.Services.AddGraphQLServer()
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    SeedData.Initialize(dbContext); // Вызов метода инициализации данных
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

// Setup middleware pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL();
});

app.Run();