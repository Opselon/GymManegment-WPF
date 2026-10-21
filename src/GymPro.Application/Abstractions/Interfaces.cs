using GymPro.Domain.Common;
using GymPro.Domain.Memberships;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Unit-of-work scope. Saver of aggregate roots and committer of the
/// transaction that produced their domain events.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persists changes and dispatches domain events after commit.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Reads aggregate roots. Query side of CQRS: never tracks changes, never
/// throws for a missing row.
/// </summary>
/// <typeparam name="TAggregate">Type of the aggregate root.</typeparam>
public interface IRepository<TAggregate> where TAggregate : AggregateRoot
{
    Task<TAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TAggregate>> ListAsync(CancellationToken cancellationToken = default);
}

/// <summary>Writes aggregate roots.</summary>
public interface IWriteRepository<TAggregate> where TAggregate : AggregateRoot
{
    void Add(TAggregate aggregate);
    void Remove(TAggregate aggregate);
}

/// <summary>
/// Marker for query models projected for the UI. These are read models: shaped
/// for screens, not warehouse-normalized, and rebuilt from aggregates.
/// </summary>
public interface IReadModel;

/// <summary>Filters and sorts a query, abbreviated as `spec`.</summary>
public interface ISpecification<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}

/// <summary>
/// Renders a payload as a QR image. Infrastructure owns the imaging library;
/// the application only knows it gets back bytes for a screen or a printout.
/// </summary>
public interface IQrCodeRenderer : IDisposable
{
    /// <summary>Renders <paramref name="payload"/> as a PNG byte array.</summary>
    byte[] RenderPng(string payload, int pixelsPerModule = 4);
}

/// <summary>
/// Exports tabular data to a spreadsheet. Used by the reporting module for
/// member and revenue extracts.
/// </summary>
public interface IExcelExporter
{
    /// <summary>
    /// Writes rows to a new worksheet named <paramref name="sheetName"/> and
    /// returns the workbook as a byte array.
    /// </summary>
    byte[] Export(string sheetName, string[] headers, IReadOnlyList<string[]> rows);
}
