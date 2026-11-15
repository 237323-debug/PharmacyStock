namespace PharmacyStock.Core.Documents;

/// <summary>
/// Contains pricing rules for pharmacy prescriptions.
/// </summary>
public static class PricingRules
{
    /// <summary>Minimum quantity for bulk discount.</summary>
    public const int BulkQuantity = 10;

    /// <summary>Bulk discount rate.</summary>
    public const decimal BulkRate = 0.05m;

    /// <summary>Regular patient discount rate.</summary>
    public const decimal RegularPatientRate = 0.07m;

    /// <summary>Large prescription threshold.</summary>
    public const decimal LargeOrderFrom = 5000m;

    /// <summary>Large prescription discount rate.</summary>
    public const decimal LargeOrderRate = 0.10m;

    /// <summary>Medium prescription threshold.</summary>
    public const decimal MediumOrderFrom = 2000m;

    /// <summary>Medium prescription discount rate.</summary>
    public const decimal MediumOrderRate = 0.05m;

    /// <summary>Sunday discount rate.</summary>
    public const decimal WeekendRate = 0.02m;

    /// <summary>Coupon percentage discount rate.</summary>
    public const decimal CouponSaleRate = 0.10m;

    /// <summary>Fixed coupon discount amount.</summary>
    public const decimal CouponFixed = 200m;

    /// <summary>Minimum amount for the fixed coupon.</summary>
    public const decimal CouponFixedFrom = 1000m;

    /// <summary>Minimum amount for free delivery.</summary>
    public const decimal FreeDeliveryFrom = 1000m;

    /// <summary>Percentage discount coupon code.</summary>
    public const string SaleCoupon = "SALE10";

    /// <summary>Fixed discount coupon code.</summary>
    public const string FixedCoupon = "MINUS200";
}
