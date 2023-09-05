
namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// An extension helper class for numbers
    /// </summary>
    public static class NumericUtils
    {
        /// <summary>
        /// Returns -1 if the given float value is smaller than 0, otherwise 1
        /// </summary>
        /// <param name="value">The given float value to check</param>
        /// <returns>
        /// Returns -1 if the given float value is smaller than 0, otherwise 1
        /// </returns>
        public static int PositiveOrNegative(this float value)
        {
            if (value < 0)
                return -1;

            return 1;
        }
    }
}
