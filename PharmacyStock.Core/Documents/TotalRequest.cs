namespace PharmacyStock.Core.Documents;

/// <summary>
/// Contains data required to calculate a prescription total.
/// </summary>
/// <param name="DocumentId">Prescription identifier.</param>
/// <param name="Client">Patient information.</param>
/// <param name="Lines">Medicine lines.</param>
/// <param name="CreatedAt">Prescription creation date.</param>
/// <param name="CouponCode">Discount coupon.</param>
/// <param name="Status">Current prescription status.</param>
/// <param name="DeliveryPrice">Delivery price.</param>
public sealed record TotalRequest(
    int DocumentId,
    ClientInfo Client,
    IReadOnlyList<MedicineLine> Lines,
    DateOnly CreatedAt,
    string? CouponCode,
    DocumentStatus Status,
    decimal DeliveryPrice);
