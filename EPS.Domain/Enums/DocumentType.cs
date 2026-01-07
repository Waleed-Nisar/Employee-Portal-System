namespace EPS.Domain.Enums;

/// <summary>
/// Represents different types of documents that can be uploaded
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// Resume or CV
    /// </summary>
    Resume = 1,

    /// <summary>
    /// Government issued ID (Passport, Driver's License, etc.)
    /// </summary>
    IdentityProof = 2,

    /// <summary>
    /// Address proof documents
    /// </summary>
    AddressProof = 3,

    /// <summary>
    /// Educational certificates and degrees
    /// </summary>
    Certificate = 4,

    /// <summary>
    /// Experience letters from previous employers
    /// </summary>
    ExperienceLetter = 5,

    /// <summary>
    /// Salary slips
    /// </summary>
    SalarySlip = 6,

    /// <summary>
    /// Offer letter
    /// </summary>
    OfferLetter = 7,

    /// <summary>
    /// Contract documents
    /// </summary>
    Contract = 8,

    /// <summary>
    /// Other miscellaneous documents
    /// </summary>
    Other = 9
}