/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using System.Threading.Tasks;
using Dolittle.Logging;

namespace Dolittle.Edge.TimeSeriesHistorian
{
    /// <summary>
    /// Represents an implementation <see cref="IStorage"/>
    /// </summary>
    public class Storage : IStorage
    {
        readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> used for logging</param>
        public Storage(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Append(string sourceOutput, TimeSeriesId timeSeries, long timestamp, string text)
        {
            var minute = timestamp / 60000;
            var path = Path.Join(Directory.GetCurrentDirectory(), "data", sourceOutput);
            path = Path.Join(path,timeSeries.ToString());
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _logger.Information($"Using path '{path}'");

            var file = Path.Join(path, $"{minute}.datapoints");
            _logger.Information($"Append to file '{file}'");

            await File.AppendAllTextAsync(file, $"{text}\n");
        }
    }
}