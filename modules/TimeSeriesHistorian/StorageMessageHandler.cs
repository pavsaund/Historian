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
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance ofr <see cref="StorageMessageHandler"/>
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer">JSON serializer</see></param>
        /// <param name="logger"><see cref="ILogger"/> used for logging</param>
        public StorageMessageHandler(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Input Input => "events";

        /// <inheritdoc/>
        public async Task<MessageResponse> Handle(Message message)
        {
            _logger.Information($"Handle incoming message");
            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);
            _logger.Information($"Event received '{messageString}'");
            var dataPoint = _serializer.FromJson<TimeSeriesDataPoint>(messageString);
            var minute = dataPoint.Timestamp / 60000;

            try
            {
                var path = Path.Join(Directory.GetCurrentDirectory(), "data", message.ConnectionModuleId);
                _logger.Information($"Using path '{path}'");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var file = Path.Join(path, $"{minute}.datapoints");
                _logger.Information($"Append to file '{file}'");
                await File.AppendAllTextAsync(file, $"{messageString}\n");

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
