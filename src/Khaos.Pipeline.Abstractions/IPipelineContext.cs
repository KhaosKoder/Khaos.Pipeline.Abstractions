namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Provides per-batch shared state visible to every pipeline step.
/// </summary>
public interface IPipelineContext
{
    /// <summary>
    /// Gets a snapshot view of the stored items.
    /// </summary>
    IReadOnlyDictionary<string, object?> Items { get; }

    /// <summary>
    /// Retrieves a value previously stored in the context.
    /// </summary>
    /// <typeparam name="T">The expected type of the stored value.</typeparam>
    /// <param name="key">The lookup key.</param>
    /// <returns>The stored value.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key has not been stored.</exception>
    T Get<T>(string key);

    /// <summary>
    /// Attempts to retrieve a value from the shared context.
    /// </summary>
    /// <typeparam name="T">The expected type of the stored value.</typeparam>
    /// <param name="key">The lookup key.</param>
    /// <param name="value">When this method returns, contains the value if found.</param>
    /// <returns>True if the value was found; otherwise, false.</returns>
    bool TryGet<T>(string key, out T? value);

    /// <summary>
    /// Stores or replaces a value in the context.
    /// </summary>
    /// <param name="key">The lookup key.</param>
    /// <param name="value">The value to store.</param>
    void Set(string key, object? value);
}
