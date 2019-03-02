using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Core
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the specified input is null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///     <c>true</c> if the specified input is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this object input)
        {
            return (input == null);
        }


        /// <summary>
        /// Determines whether [is not null] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///     <c>true</c> if [is not null] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNull(this object input)
        {
            return (input != null);
        }

    }
}
