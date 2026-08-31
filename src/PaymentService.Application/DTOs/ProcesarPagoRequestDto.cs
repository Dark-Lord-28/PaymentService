namespace PaymentService.Application.DTOs;

public class ProcesarPagoRequestDto
{
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}