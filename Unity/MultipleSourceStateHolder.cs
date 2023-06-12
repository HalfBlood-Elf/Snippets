using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Support class for managing up booleans from multiple sources.
/// </summary>
public class MultipleSourceStateHolder
{
    public event System.Action<bool> IsActiveChanged;
    private readonly HashSet<string> _sources = new();

    public bool IsActive => _sources.Count > 0;

    /// <summary>
    /// Add (if not present) or remove (if present) the source.
    /// </summary>
    /// <param name="targetActive"></param>
    /// <param name="source">Source description</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public void SetSourceIsActive(bool targetActive, string source)
    {
        if (source is null) throw new System.ArgumentNullException(nameof(source), "Source must not be null");
        bool wasActive = IsActive;

        if (targetActive && !_sources.Contains(source))
        {
            _sources.Add(source);
        }
        else if(!targetActive && _sources.Contains(source))
        {
            _sources.Remove(source);
        }

        if (wasActive != IsActive) IsActiveChanged?.Invoke(IsActive);
    }

    public bool IsSourceActive(string source) => _sources.Contains(source);

    /// <summary>
    /// Clears list of sources
    /// </summary>
    public void ResetIsActive()
    {
        //Debug.Log("Reset: " + ToString());
        _sources.Clear(); 
        IsActiveChanged?.Invoke(false);
    }

    /// <summary>
    /// Return list of sources. Useful for debug.
    /// </summary>
    /// <returns>List of sources</returns>
    public IEnumerable<string> GetSources() => _sources;

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        var sourcesList = GetSources();
        stringBuilder.Append("Blocked by: [");
        stringBuilder.AppendJoin(", ", sourcesList);
        stringBuilder.Append(']');

        return stringBuilder.ToString();
    }


    public static implicit operator bool(MultipleSourceStateHolder holder) => holder.IsActive;
}
