using System.Collections.Generic;

namespace PharmacyStock.Legacy;

/// <summary>
/// Combines the data needed to process a legacy pharmacy document.
/// </summary>
public sealed record PrescriptionRequest(
    int DocId,
    PatientRecord Patient,
    List<PrescriptionLine>? Items,
    DocState State,
    string Currency,
    bool SendMail);
