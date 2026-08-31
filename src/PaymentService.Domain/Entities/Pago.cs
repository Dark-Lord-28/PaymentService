namespace PaymentService.Domain.Entities;

public class Pago
{
    public int Id { get; private set; }
    public string OrderId { get; private set; } = string.Empty;
    public decimal Monto { get; private set; }
    public string Estado { get; private set; } = string.Empty; // Approved | Rejected
    public string TransactionId { get; private set; } = string.Empty;
    public DateTime FechaProceso { get; private set; }

    private Pago() { }

    public Pago(string orderId, decimal monto, decimal limiteAprobacion)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ArgumentException("El OrderId no puede estar vacío.", nameof(orderId));

        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0.", nameof(monto));

        OrderId = orderId;
        Monto = monto;
        FechaProceso = DateTime.UtcNow;
        TransactionId = $"TX-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        // Regla de negocio explícita: Aprobado si el monto es <= limiteAprobacion
        Estado = monto <= limiteAprobacion ? "Approved" : "Rejected";
    }
}