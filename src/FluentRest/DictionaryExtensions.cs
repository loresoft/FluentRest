using System.Diagnostics.CodeAnalysis;

namespace FluentRest;

/// <summary>
/// Extension methods for dictionary
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Adds a key/value pair to the dictionary if the key does not already exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <returns>
    /// The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value for the key as returned by valueFactory if the key was not in the dictionary.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <see langword="null" /></exception>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (dictionary.TryGetValue(key, out var value))
            return value;

        var factoryValue = valueFactory(key);
        dictionary.Add(key, factoryValue);

        return factoryValue;
    }

    /// <summary>
    /// Attempts to add the specified key and value to the Dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to perform an action upon.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <returns>
    ///   <c>true</c> if the key/value pair was added to the Dictionary successfully. If the key already exists, this method returns <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <see langword="null" /></exception>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (dictionary.ContainsKey(key))
            return false;

        dictionary.Add(key, value);
        return true;
    }

    /// <summary>
    /// Attempts to remove and return the value with the specified key from the Dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The Dictionary to perform an action upon.</param>
    /// <param name="key">The key of the element to remove and return.</param>
    /// <param name="value">When this method returns, value contains the object removed from the Dictionary or the default value if the operation failed.</param>
    /// <returns><c>true</c> if an object was removed successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <see langword="null" /></exception>
    public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue? value)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (!dictionary.TryGetValue(key, out value))
            return false;

        return dictionary.Remove(key);
    }

    /// <summary>
    /// Compares the existing value for the specified key with a specified value, and if they are equal, updates the key with a third value.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to perform an action upon.</param>
    /// <param name="key">The key whose value is compared with comparisonValue and possibly replaced.</param>
    /// <param name="newValue">The value that replaces the value of the element with key if the comparison results in equality.</param>
    /// <param name="comparisonValue">The value that is compared to the value of the element with key.</param>
    /// <returns><c>true</c> if the value with key was equal to comparisonValue and replaced with newValue; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if key is a <c>null</c> reference </exception>
    public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue, TValue comparisonValue)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (!dictionary.TryGetValue(key, out var value))
            return false;

        if (!Equals(value, comparisonValue))
            return false;

        dictionary[key] = newValue;
        return true;
    }
}
