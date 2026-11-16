namespace PharmacyStock.Core.Errors;

/// <summary>
/// Represents a violated pharmacy business rule.
/// </summary>
public sealed class DomainRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DomainRuleException"/> class.
    /// </summary>
    public DomainRuleException()
        : base("Порушено правило предметної області.")
    {
        Rule = "domain.unknown";
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public DomainRuleException(string message)
        : base(message)
    {
        Rule = "domain.unknown";
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Original exception.</param>
    public DomainRuleException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        Rule = "domain.unknown";
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="rule">Business rule identifier.</param>
    /// <param name="message">Error message.</param>
    public DomainRuleException(string rule, string message)
        : base(message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rule);

        Rule = rule;
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="rule">Business rule identifier.</param>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Original exception.</param>
    public DomainRuleException(
        string rule,
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rule);

        Rule = rule;
    }

    /// <summary>Gets the violated business rule identifier.</summary>
    public string Rule { get; }
}
