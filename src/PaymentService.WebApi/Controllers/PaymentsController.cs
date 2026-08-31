namespace PaymentService.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Payments.Commands;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcesarPagoRequestDto dto)
    {
        var command = new ProcesarPagoCommand(dto.OrderId, dto.Amount);
        var resultado = await _mediator.Send(command);

        return Ok(resultado);
    }
}