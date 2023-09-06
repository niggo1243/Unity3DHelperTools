using System.Collections.Generic;

namespace NikosAssets.Helpers.Interfaces
{
    /// <summary>
    /// For custom multiple Id implementations for various use cases
    /// </summary>
    public interface IIdList
    {
        List<int> IDs { get; }
    }
}
