/*
 * Copyright (c) 2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of Heimdall API <https://github.com/Vanaheimr/HeimdallAPI>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace org.GraphDefined.Vanaheimr.Heimdall.API
{

    /// <summary>
    /// The Heimdall API HTTP logger.
    /// </summary>
    public class HeimdallAPILogger : HTTPServerLogger
    {

        #region Data

        /// <summary>
        /// The default context of this logger.
        /// </summary>
        public const String  DefaultContext   = "HeimdallAPI";

        #endregion

        #region Properties

        /// <summary>
        /// The linked Heimdall API.
        /// </summary>
        public HeimdallAPI   HeimdallAPI    { get; }

        #endregion

        #region Constructor(s)

        #region HeimdallAPILogger(HeimdallAPI, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new WWCP HTTP API logger using the default logging delegates.
        /// </summary>
        /// <param name="HeimdallAPI">A WWCP API.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public HeimdallAPILogger(HeimdallAPI              HeimdallAPI,
                                 String                   LoggingPath,
                                 String                   Context         = DefaultContext,
                                 LogfileCreatorDelegate?  LogfileCreator  = null)

            : this(HeimdallAPI,
                   LoggingPath,
                   Context,
                   null,
                   null,
                   null,
                   null,
                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region HeimdallAPILogger(HeimdallAPI, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new WWCP HTTP API logger using the given logging delegates.
        /// </summary>
        /// <param name="HeimdallAPI">A WWCP API.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// 
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public HeimdallAPILogger(HeimdallAPI                  HeimdallAPI,
                                 String                       LoggingPath,
                                 String                       Context,

                                 HTTPRequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                                 HTTPRequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                                 HTTPRequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                                 HTTPRequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                                 HTTPResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                                 HTTPResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                                 LogfileCreatorDelegate?      LogfileCreator              = null)

            : base(HeimdallAPI.HTTPServer,//.InternalHTTPServer,
                   LoggingPath,
                   Context,

                   LogHTTPRequest_toConsole,
                   LogHTTPResponse_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPResponse_toDisc,

                   LogHTTPRequest_toNetwork,
                   LogHTTPResponse_toNetwork,
                   LogHTTPRequest_toHTTPSSE,
                   LogHTTPResponse_toHTTPSSE,

                   LogHTTPError_toConsole,
                   LogHTTPError_toDisc,
                   LogHTTPError_toNetwork,
                   LogHTTPError_toHTTPSSE,

                   LogfileCreator)

        {

            this.HeimdallAPI = HeimdallAPI ?? throw new ArgumentNullException(nameof(HeimdallAPI), "The given WWCP HTTP API must not be null!");

            #region Register auth start/stop log events

            RegisterEvent2("AuthEVSEStart",
                          handler => HeimdallAPI.OnAuthStartEVSERequest += handler,
                          handler => HeimdallAPI.OnAuthStartEVSERequest -= handler,
                          "Auth", "AuthEVSE", "AuthStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent2("AuthEVSEStarted",
                          handler => HeimdallAPI.OnAuthStartEVSEResponse += handler,
                          handler => HeimdallAPI.OnAuthStartEVSEResponse -= handler,
                          "Auth", "AuthEVSE", "AuthStarted", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
