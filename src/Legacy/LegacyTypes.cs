namespace PharmacyStock.Legacy;

/// <summary>
/// Represents the known patient pricing categories.
/// </summary>
public enum ClientKind
{
    Regular,
    Vip,
    Staff,
}

/// <summary>
/// Represents the known prescription states.
/// </summary>
public enum DocState
{
    Draft,
    New,
    Paid,
}

/// <summary>
/// Interprets legacy strings without changing case-sensitive behavior.
/// </summary>
internal static class LegacyTypeParser
{
    public static ClientKind KindOf(string? value)
    {
        return value switch
        {
            "vip" => ClientKind.Vip,
            "staff" => ClientKind.Staff,
            _ => ClientKind.Regular,
        };
    }

    public static DocState StateOf(string? value)
    {
        return value switch
        {
            "new" => DocState.New,
            "paid" => DocState.Paid,
            _ => DocState.Draft,
        };
    }
}
