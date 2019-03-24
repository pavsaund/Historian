/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dolittle.Edge.Modules;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Microsoft.Azure.Devices.Client;

namespace Dolittle.Edge.TimeSeriesHistorian
{
    /// <summary>
    /// Represents a <see cref="ICanHandleMessages"/> for storing messages offline
    /// </summary>
    public class StorageMessageHandler : ICanHandleMessages
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
        public async Task<MessageResponse> Handle(Message message)
        {
            try
            {
                _logger.Information($"Handle incoming message");
                var messageBytes = message.GetBytes();
                var messageString = Encoding.UTF8.GetString(messageBytes);
                _logger.Information($"Event received '{messageString}'");
                var dataPoint = _serializer.FromJson<TimeSeriesDataPoint>(messageString);
                await _storage.Append(message.ConnectionModuleId, dataPoint.TimeSeriesId, dataPoint.Timestamp, messageString);
                _logger.Information("Datapoint appended");

                return MessageResponse.Completed;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Issues writing datapoint : '{ex.Message}'");
                return MessageResponse.Abandoned;
            }
        }
    }
}