/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dolittle.TimeSeries.Modules;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Microsoft.Azure.Devices.Client;

namespace Dolittle.TimeSeries.Historian
{
    /// <summary>
    /// Represents a <see cref="ICanHandleDataPoint{T}"/> for storing messages offline
    /// </summary>
    public class StorageMessageHandler : ICanHandleDataPoint<object>
    {
        readonly ISerializer _serializer;
        readonly ILogger _logger;
        readonly IStorage _storage;

        /// <summary>
        /// Initializes a new instance ofr <see cref="StorageMessageHandler"/>
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer">JSON serializer</see></param>
        /// <param name="logger"><see cref="ILogger"/> used for logging</param>
        /// <param name="storage"><see cref="IStorage"/> for dealing with storage</param>
        public StorageMessageHandler(ISerializer serializer, ILogger logger, IStorage storage)
        {
            _serializer = serializer;
            _logger = logger;
            _storage = storage;
        }

        /// <inheritdoc/>
        public Input Input => "events";

        /// <inheritdoc/>
        public async Task Handle(DataPoint<object> dataPoint)
        {
            await _storage.Append(dataPoint);
        }

    }
}