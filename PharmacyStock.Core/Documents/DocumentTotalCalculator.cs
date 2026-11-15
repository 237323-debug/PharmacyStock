namespace PharmacyStock.Core.Documents;

/// <summary>
/// Calculates prescription totals and discounts.
/// </summary>
public static class DocumentTotalCalculator
{
    /// <summary>
    /// Calculates a prescription using one request object.
    /// </summary>
    /// <param name="request">Prescription request.</param>
    /// <returns>Calculated amount and resulting status.</returns>
    public static TotalResult Calculate(TotalRequest request)
    {
        DocumentGuards.EnsureRequestValid(request);

        decimal total = SubtotalOf(request.Lines);

        total -= ClientDiscountOf(
            total, request.Client.IsRegular);

        total -= VolumeDiscountOf(total);

        total -= CouponDiscountOf(
            total, request.CouponCode);

        total -= WeekendDiscountOf(
            total, request.CreatedAt);

        total += DeliveryFeeOf(
            total, request.DeliveryPrice);

        total = request.Status == DocumentStatus.Cancelled
            ? 0m
            : Math.Round(Math.Max(total, 0m), 2);

        return new TotalResult(
            total, NextStatusOf(request.Status, total));
    }

    /// <summary>
    /// Calculates the subtotal of medicine lines.
    /// </summary>
    /// <param name="lines">Medicine lines.</param>
    /// <returns>The subtotal amount.</returns>
    public static decimal SubtotalOf(
        IReadOnlyList<MedicineLine> lines)
    {
        DocumentGuards.EnsureLinesValid(lines);

        decimal subtotal = 0m;

        foreach (MedicineLine line in lines)
        {
            decimal sum = line.Quantity * line.UnitPrice;

            if (line.Quantity >= PricingRules.BulkQuantity)
            {
                sum -= sum * PricingRules.BulkRate;
            }

            subtotal += sum;
        }

        return subtotal;
    }

    /// <summary>
    /// Calculates the regular patient discount.
    /// </summary>
    /// <param name="amount">Current amount.</param>
    /// <param name="isRegularPatient">Regular patient indicator.</param>
    /// <returns>The discount amount.</returns>
    public static decimal ClientDiscountOf(
        decimal amount, bool isRegularPatient)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        return isRegularPatient
            ? amount * PricingRules.RegularPatientRate
            : 0m;
    }

    /// <summary>
    /// Calculates the volume discount.
    /// </summary>
    /// <param name="amount">Current amount.</param>
    /// <returns>The discount amount.</returns>
    public static decimal VolumeDiscountOf(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        if (amount > PricingRules.LargeOrderFrom)
        {
            return amount * PricingRules.LargeOrderRate;
        }

        if (amount > PricingRules.MediumOrderFrom)
        {
            return amount * PricingRules.MediumOrderRate;
        }

        return 0m;
    }

    /// <summary>
    /// Calculates the coupon discount.
    /// </summary>
    /// <param name="amount">Current amount.</param>
    /// <param name="couponCode">Coupon code.</param>
    /// <returns>The discount amount.</returns>
    public static decimal CouponDiscountOf(
        decimal amount, string? couponCode)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        return couponCode switch
        {
            PricingRules.SaleCoupon
                => amount * PricingRules.CouponSaleRate,

            PricingRules.FixedCoupon
                when amount > PricingRules.CouponFixedFrom
                => PricingRules.CouponFixed,

            _ => 0m,
        };
    }

    /// <summary>
    /// Calculates the Sunday discount.
    /// </summary>
    /// <param name="amount">Current amount.</param>
    /// <param name="createdAt">Prescription creation date.</param>
    /// <returns>The discount amount.</returns>
    public static decimal WeekendDiscountOf(
        decimal amount, DateOnly createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        return createdAt.DayOfWeek == DayOfWeek.Sunday
            ? amount * PricingRules.WeekendRate
            : 0m;
    }

    /// <summary>
    /// Calculates the delivery fee.
    /// </summary>
    /// <param name="amount">Current amount.</param>
    /// <param name="deliveryPrice">Delivery price.</param>
    /// <returns>The delivery fee.</returns>
    public static decimal DeliveryFeeOf(
        decimal amount, decimal deliveryPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentOutOfRangeException.ThrowIfNegative(deliveryPrice);

        return amount < PricingRules.FreeDeliveryFrom
            ? deliveryPrice
            : 0m;
    }

    /// <summary>
    /// Determines the next prescription status.
    /// </summary>
    /// <param name="current">Current prescription status.</param>
    /// <param name="total">Calculated amount.</param>
    /// <returns>The next prescription status.</returns>
    public static DocumentStatus NextStatusOf(
        DocumentStatus current, decimal total)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(total);

        return current switch
        {
            DocumentStatus.New when total > 0m
                => DocumentStatus.Approved,

            DocumentStatus.Approved
                => DocumentStatus.Dispensed,

            _ => current,
        };
    }
}
