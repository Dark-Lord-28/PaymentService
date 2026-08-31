using PaymentService.Application.Payments.Commands;

var builder = WebApplication.CreateBuilder(args);

// Registrar MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<ProcesarPagoCommand>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();