/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using Dolittle.TimeSeries.Modules;

namespace Dolittle.TimeSeries.Historian
{
    /// <summary>
    /// Defines the storage system
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Append text to a file at a path
        /// </summary>
        /// <param name="dataPoint"><see cref="DataPoint{T}"/> to append</param>
        /// <returns>Awaitable task</returns>
        Task Append(DataPoint<object> dataPoint);
    }
}
