/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;

namespace Dolittle.Edge.TimeSeriesHistorian
{
    /// <summary>
    /// Defines the storage system
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Append text to a file at a path
        /// </summary>
        /// <param name="sourceOutput">The name of the source output in which the information to store came from</param>
        /// <param name="timeSeries"><see cref="TimeSeriesId"/> to store for</param>
        /// <param name="timestamp">Timestamp of the information to store</param>
        /// <param name="text">Text to append</param>
        /// <returns>Awaitable task</returns>
        Task Append(string sourceOutput, TimeSeriesId timeSeries, long timestamp, string text);
    }
}
