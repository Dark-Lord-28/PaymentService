namespace PaymentService.Application.Payments.Commands;

using MediatR;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

public record ProcesarPagoCommand(string OrderId, decimal Amount) : IRequest<ProcesarPagoResponseDto>;

public class ProcesarPagoCommandHandler : IRequestHandler<ProcesarPagoCommand, ProcesarPagoResponseDto>
{
    private readonly IConfiguration _config;

    public ProcesarPagoCommandHandler(IConfiguration config)
    {
        _config = config;
    }

    public Task<ProcesarPagoResponseDto> Handle(ProcesarPagoCommand request, CancellationToken cancellationToken)
    {
        var limite = _config.GetValue<decimal>("PaymentRules:MaxApprovedAmount", 100000m);
        var pago = new Pago(request.OrderId, request.Amount, limite);

        var response = new ProcesarPagoResponseDto
        {
            Status = pago.Estado,
            TransactionId = pago.TransactionId
        };

        return Task.FromResult(response);
    }
}