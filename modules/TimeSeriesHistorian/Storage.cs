/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using System.Threading.Tasks;
using Dolittle.Edge.Modules;
using Dolittle.Logging;
using Dolittle.Serialization.Json;

namespace Dolittle.Edge.TimeSeriesHistorian
{
    /// <summary>
    /// Represents an implementation <see cref="IStorage"/>
    /// </summary>
    public class Storage : IStorage
    {
        readonly ILogger _logger;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="Storage"/>
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="logger"><see cref="ILogger"/> used for logging</param>
        public Storage(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Append(DataPoint<object> dataPoint)
        {
            var minute = dataPoint.Timestamp / 60000;
            var path = Path.Join(Directory.GetCurrentDirectory(), "data");
            path = Path.Join(path,dataPoint.TimeSeries.ToString());
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _logger.Information($"Using path '{path}'");

            var file = Path.Join(path, $"{minute}.datapoints");
            _logger.Information($"Append to file '{file}'");

            var json = _serializer.ToJson(dataPoint);

            await File.AppendAllTextAsync(file, $"{json}\n");
        }
    }
}