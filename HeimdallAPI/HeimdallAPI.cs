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

using System.Reflection;
using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using com.GraphDefined.SMSApi.API;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SMTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using social.OpenData.UsersAPI;

#endregion

namespace org.GraphDefined.Vanaheimr.Heimdall.API
{

    /// <summary>
    /// Extention methods for the Heimdall API.
    /// </summary>
    public static class HeimdallAPIExtensions
    {

    }


    /// <summary>
    /// The Heimdall API.
    /// </summary>
    public class HeimdallAPI : UsersAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const       String               DefaultHTTPServerName                 = "Heimdall Test Gateway";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const       String               DefaultHTTPServiceName                = "Heimdall Test Gateway";

        /// <summary>
        /// The HTTP root for embedded ressources.
        /// </summary>
        public new const       String               HTTPRoot                              = "org.GraphDefined.Vanaheimr.Heimdall.API.HTTPRoot.";

        public const           String               DefaultHeimdallAPI_DatabaseFileName   = "HeimdallAPI.db";
        public const           String               DefaultHeimdallAPI_LogfileName        = "HeimdallAPI.log";

        public static readonly HTTPEventSource_Id   DebugLogId                            = HTTPEventSource_Id.Parse("DebugLog");


        public                 IEnumerable<String>  WWWAuthenticateDefaults               = new[] {
                                                                                                @"Basic realm=""Heimdall"", charset =""UTF-8""",
                                                                                                @"Bearer realm=""Heimdall"", error=""invalid_token"", error_description=""The access token is invalid!""",
                                                                                                "Token",
                                                                                                "API-Key"
                                                                                            };

        #endregion

        #region Properties

        /// <summary>
        /// The API version hash (git commit hash value).
        /// </summary>
        public new String                                   APIVersionHash                { get; }

        public String                                       HeimdallAPIPath      { get; }

        //public String                                       ChargingReservationsPath      { get; }
        //public String                                       ChargingSessionsPath          { get; }
        //public String                                       ChargeDetailRecordsPath       { get; }


        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>                     DebugLog                      { get; }

        #endregion

        #region Events

        #region (protected internal) CreateRoamingNetworkRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnCreateRoamingNetworkRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task CreateRoamingNetworkRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnCreateRoamingNetworkRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) CreateRoamingNetworkResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnCreateRoamingNetworkResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task CreateRoamingNetworkResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnCreateRoamingNetworkResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) DeleteRoamingNetworkRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnDeleteRoamingNetworkRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task DeleteRoamingNetworkRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnDeleteRoamingNetworkRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) DeleteRoamingNetworkResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnDeleteRoamingNetworkResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task DeleteRoamingNetworkResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnDeleteRoamingNetworkResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion



        #region (protected internal) CreateChargingPoolRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnCreateChargingPoolRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task CreateChargingPoolRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnCreateChargingPoolRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) CreateChargingPoolResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnCreateChargingPoolResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task CreateChargingPoolResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnCreateChargingPoolResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) DeleteChargingPoolRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnDeleteChargingPoolRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task DeleteChargingPoolRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnDeleteChargingPoolRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) DeleteChargingPoolResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnDeleteChargingPoolResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task DeleteChargingPoolResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnDeleteChargingPoolResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion



        #region (protected internal) CreateChargingStationRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnCreateChargingStationRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task CreateChargingStationRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnCreateChargingStationRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) CreateChargingStationResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnCreateChargingStationResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task CreateChargingStationResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnCreateChargingStationResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) DeleteChargingStationRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnDeleteChargingStationRequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task DeleteChargingStationRequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnDeleteChargingStationRequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) DeleteChargingStationResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnDeleteChargingStationResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task DeleteChargingStationResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnDeleteChargingStationResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion



        #region (protected internal) SendGetEVSEsStatusRequest (Request)

        /// <summary>
        /// An event sent whenever an EVSEs->Status request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetEVSEsStatusRequest = new ();

        /// <summary>
        /// An event sent whenever an EVSEs->Status request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendGetEVSEsStatusRequest(DateTime     Timestamp,
                                                          HTTPAPI      API,
                                                          HTTPRequest  Request)

            => OnGetEVSEsStatusRequest?.WhenAll(Timestamp,
                                                API ?? this,
                                                Request);

        #endregion

        #region (protected internal) SendGetEVSEsStatusResponse(Response)

        /// <summary>
        /// An event sent whenever an EVSEs->Status response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnGetEVSEsStatusResponse = new ();

        /// <summary>
        /// An event sent whenever an EVSEs->Status response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendGetEVSEsStatusResponse(DateTime      Timestamp,
                                                           HTTPAPI       API,
                                                           HTTPRequest   Request,
                                                           HTTPResponse  Response)

            => OnGetEVSEsStatusResponse?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request,
                                                 Response);

        #endregion



        #region (protected internal) SendRemoteStartEVSERequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnSendRemoteStartEVSERequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendRemoteStartEVSERequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnSendRemoteStartEVSERequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) SendRemoteStartEVSEResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSendRemoteStartEVSEResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendRemoteStartEVSEResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnSendRemoteStartEVSEResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) SendRemoteStopEVSERequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnSendRemoteStopEVSERequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendRemoteStopEVSERequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnSendRemoteStopEVSERequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) SendRemoteStopEVSEResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSendRemoteStopEVSEResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendRemoteStopEVSEResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnSendRemoteStopEVSEResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion






        #region (protected internal) SendReserveEVSERequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnSendReserveEVSERequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendReserveEVSERequest(DateTime     Timestamp,
                                                       HTTPAPI      API,
                                                       HTTPRequest  Request)

            => OnSendReserveEVSERequest?.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (protected internal) SendReserveEVSEResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSendReserveEVSEResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendReserveEVSEResponse(DateTime      Timestamp,
                                                        HTTPAPI       API,
                                                        HTTPRequest   Request,
                                                        HTTPResponse  Response)

            => OnSendReserveEVSEResponse?.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) SendAuthStartEVSERequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthStartEVSERequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendAuthStartEVSERequest(DateTime     Timestamp,
                                                         HTTPAPI      API,
                                                         HTTPRequest  Request)

            => OnAuthStartEVSERequest?.WhenAll(Timestamp,
                                               API ?? this,
                                               Request);

        #endregion

        #region (protected internal) SendAuthStartEVSEResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthStartEVSEResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendAuthStartEVSEResponse(DateTime      Timestamp,
                                                          HTTPAPI       API,
                                                          HTTPRequest   Request,
                                                          HTTPResponse  Response)

            => OnAuthStartEVSEResponse?.WhenAll(Timestamp,
                                                API ?? this,
                                                Request,
                                                Response);

        #endregion


        #region (protected internal) SendAuthStopEVSERequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthStopEVSERequest = new ();

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendAuthStopEVSERequest(DateTime     Timestamp,
                                                        HTTPAPI      API,
                                                        HTTPRequest  Request)

            => OnAuthStopEVSERequest?.WhenAll(Timestamp,
                                              API ?? this,
                                              Request);

        #endregion

        #region (protected internal) SendAuthStopEVSEResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthStopEVSEResponse = new ();

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendAuthStopEVSEResponse(DateTime      Timestamp,
                                                         HTTPAPI       API,
                                                         HTTPRequest   Request,
                                                         HTTPResponse  Response)

            => OnAuthStopEVSEResponse?.WhenAll(Timestamp,
                                               API ?? this,
                                               Request,
                                               Response);

        #endregion


        #region (protected internal) SendCDRsRequest(Request)

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public HTTPRequestLogEvent OnSendCDRsRequest = new ();

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task SendCDRsRequest(DateTime     Timestamp,
                                                HTTPAPI      API,
                                                HTTPRequest  Request)

            => OnSendCDRsRequest?.WhenAll(Timestamp,
                                          API ?? this,
                                          Request);

        #endregion

        #region (protected internal) SendCDRsResponse(Response)

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSendCDRsResponse = new ();

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task SendCDRsResponse(DateTime      Timestamp,
                                                 HTTPAPI       API,
                                                 HTTPRequest   Request,
                                                 HTTPResponse  Response)

            => OnSendCDRsResponse?.WhenAll(Timestamp,
                                           API ?? this,
                                           Request,
                                           Response);

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an instance of the Heimdall API.
        /// </summary>
        /// <param name="HTTPHostname">The HTTP hostname for all URLs within this API.</param>
        /// <param name="ExternalDNSName">The offical URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP servername, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="TCPPort"></param>
        /// <param name="UDPPort"></param>
        /// 
        /// <param name="APIRobotEMailAddress">An e-mail address for this API.</param>
        /// <param name="APIRobotGPGPassphrase">A GPG passphrase for this API.</param>
        /// <param name="SMTPClient">A SMTP client for sending e-mails.</param>
        /// <param name="SMSClient">A SMS client for sending SMS.</param>
        /// <param name="SMSSenderName">The (default) SMS sender name.</param>
        /// <param name="TelegramClient">A Telegram client for sendind and receiving Telegrams.</param>
        /// 
        /// <param name="PasswordQualityCheck">A delegate to ensure a minimal password quality.</param>
        /// <param name="CookieName">The name of the HTTP Cookie for authentication.</param>
        /// <param name="UseSecureCookies">Force the web browser to send cookies only via HTTPS.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance intervall.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden intervall.</param>
        /// 
        /// <param name="RemoteAuthServers">Servers for remote authorization.</param>
        /// <param name="RemoteAuthAPIKeys">API keys for incoming remote authorizations.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="SkipURLTemplates">Skip URL templates.</param>
        /// <param name="DatabaseFileName">The name of the database file for this API.</param>
        /// <param name="DisableNotifications">Disable external notifications.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LogfileName">The name of the logfile.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        public HeimdallAPI(HTTPHostname?                         HTTPHostname                       = null,
                           String?                               ExternalDNSName                    = null,
                           IPPort?                               HTTPServerPort                     = null,
                           HTTPPath?                             BasePath                           = null,
                           String                                HTTPServerName                     = DefaultHTTPServerName,

                           HTTPPath?                             URLPathPrefix                      = null,
                           String                                HTTPServiceName                    = DefaultHTTPServiceName,
                           String?                               HTMLTemplate                       = null,
                           JObject?                              APIVersionHashes                   = null,

                           ServerCertificateSelectorDelegate?    ServerCertificateSelector          = null,
                           RemoteCertificateValidationHandler?  ClientCertificateValidator         = null,
                           LocalCertificateSelectionHandler?    ClientCertificateSelector          = null,
                           SslProtocols?                         AllowedTLSProtocols                = null,
                           Boolean?                              ClientCertificateRequired          = null,
                           Boolean?                              CheckCertificateRevocation         = null,

                           IPPort?                               TCPPort                            = null,
                           IPPort?                               UDPPort                            = null,

                           Organization_Id?                      AdminOrganizationId                = null,
                           EMailAddress?                         APIRobotEMailAddress               = null,
                           String?                               APIRobotGPGPassphrase              = null,
                           ISMTPClient?                          SMTPClient                         = null,
                           ISMSClient?                           SMSClient                          = null,
                           String?                               SMSSenderName                      = null,
                           ITelegramStore?                       TelegramClient                     = null,

                           PasswordQualityCheckDelegate?         PasswordQualityCheck               = null,
                           HTTPCookieName?                       CookieName                         = null,
                           Boolean                               UseSecureCookies                   = true,
                           Languages?                            DefaultLanguage                    = null,

                           String?                               ServerThreadName                   = null,
                           ThreadPriority?                       ServerThreadPriority               = null,
                           Boolean?                              ServerThreadIsBackground           = null,
                           ConnectionIdBuilder?                  ConnectionIdBuilder                = null,
                           TimeSpan?                             ConnectionTimeout                  = null,
                           UInt32?                               MaxClientConnections               = null,

                           Boolean?                              DisableMaintenanceTasks            = null,
                           TimeSpan?                             MaintenanceInitialDelay            = null,
                           TimeSpan?                             MaintenanceEvery                   = null,

                           Boolean?                              DisableWardenTasks                 = null,
                           TimeSpan?                             WardenInitialDelay                 = null,
                           TimeSpan?                             WardenCheckEvery                   = null,

                           IEnumerable<URLWithAPIKey>?           RemoteAuthServers                  = null,
                           IEnumerable<APIKey_Id>?               RemoteAuthAPIKeys                  = null,

                           Boolean?                              AllowsAnonymousReadAccesss         = true,

                           Boolean?                              IsDevelopment                      = null,
                           IEnumerable<String>?                  DevelopmentServers                 = null,
                           Boolean                               SkipURLTemplates                   = false,
                           String                                DatabaseFileName                   = DefaultHeimdallAPI_DatabaseFileName,
                           Boolean                               DisableNotifications               = false,
                           Boolean                               DisableLogging                     = false,
                           String?                               LoggingPath                        = null,
                           String                                LogfileName                        = DefaultHeimdallAPI_LogfileName,
                           LogfileCreatorDelegate?               LogfileCreator                     = null,
                           DNSClient?                            DNSClient                          = null,
                           Boolean                               AutoStart                          = false)

            : base(HTTPHostname,
                   ExternalDNSName,
                   HTTPServerPort,
                   BasePath,
                   HTTPServerName,

                   URLPathPrefix,
                   HTTPServiceName,
                   HTMLTemplate,
                   APIVersionHashes,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ServerThreadName,
                   ServerThreadPriority,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionTimeout,
                   MaxClientConnections,

                   AdminOrganizationId,
                   APIRobotEMailAddress ?? new EMailAddress(
                                               "heimdall",
                                               SimpleEMailAddress.Parse("robot@heimdall.vanaheimr")
                                           ),
                   APIRobotGPGPassphrase,
                   SMTPClient           ?? new NullMailer(),
                   SMSClient,
                   SMSSenderName,
                   TelegramClient,

                   PasswordQualityCheck,
                   CookieName           ?? HTTPCookieName.Parse(nameof(HeimdallAPI)),
                   UseSecureCookies,
                   TimeSpan.FromDays(30),
                   DefaultLanguage      ?? Languages.en,
                   4,
                   4,
                   4,
                   5,
                   20,
                   8,
                   4,
                   4,
                   8,
                   8,
                   8,
                   8,

                   DisableMaintenanceTasks,
                   MaintenanceInitialDelay,
                   MaintenanceEvery,

                   DisableWardenTasks,
                   WardenInitialDelay,
                   WardenCheckEvery,

                   RemoteAuthServers,
                   RemoteAuthAPIKeys,

                   IsDevelopment,
                   DevelopmentServers,
                   SkipURLTemplates,
                   DatabaseFileName     ?? DefaultHeimdallAPI_DatabaseFileName,
                   DisableNotifications,
                   DisableLogging,
                   LoggingPath,
                   LogfileName          ?? DefaultHeimdallAPI_LogfileName,
                   LogfileCreator,
                   DNSClient,
                   AutoStart)

        {

            this.APIVersionHash              = APIVersionHashes?[nameof(HeimdallAPI)]?.Value<String>()?.Trim() ?? "";

            this.HeimdallAPIPath             = Path.Combine(this.LoggingPath, "HeimdallAPI");

            if (!DisableLogging)
            {
                Directory.CreateDirectory(HeimdallAPIPath);
            }

            DebugLog     = HTTPServer.AddJSONEventSource(EventIdentification:          DebugLogId,
                                                         HTTPAPI:                      this,
                                                         URLTemplate:                  this.URLPathPrefix + "/" + DebugLogId.ToString(),
                                                         MaxNumberOfCachedEvents:      10000,
                                                         RetryIntervall:               TimeSpan.FromSeconds(5),
                                                         EnableLogging:                true,
                                                         LogfilePath:                  this.HeimdallAPIPath);

            //RegisterNotifications().Wait();
            RegisterURLTemplates();

            this.HTMLTemplate = HTMLTemplate ?? GetResourceString("template.html");

            DebugX.Log(nameof(HeimdallAPI) + " version '" + APIVersionHash + "' initialized...");

        }

        #endregion


        #region (private) RegisterURLTemplates()

        #region Manage HTTP Resources

        #region (protected override) GetResourceStream      (ResourceName)

        protected override Stream? GetResourceStream(String ResourceName)

            => GetResourceStream(ResourceName,
                                 new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                 new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                 new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly));

        #endregion

        #region (protected override) GetResourceMemoryStream(ResourceName)

        protected override MemoryStream? GetResourceMemoryStream(String ResourceName)

            => GetResourceMemoryStream(ResourceName,
                                       new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                       new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                       new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly));

        #endregion

        #region (protected override) GetResourceString      (ResourceName)

        protected override String GetResourceString(String ResourceName)

            => GetResourceString(ResourceName,
                                 new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                 new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                 new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly));

        #endregion

        #region (protected override) GetResourceBytes       (ResourceName)

        protected override Byte[] GetResourceBytes(String ResourceName)

            => GetResourceBytes(ResourceName,
                                new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly));

        #endregion

        #region (protected override) MixWithHTMLTemplate    (ResourceName)

        protected override String MixWithHTMLTemplate(String ResourceName)

            => MixWithHTMLTemplate(ResourceName,
                                   new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                   new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                   new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly));

        #endregion

        #region (protected override) MixWithHTMLTemplate    (Template, ResourceName, Content = null)

        protected override String MixWithHTMLTemplate(String   Template,
                                                      String   ResourceName,
                                                      String?  Content   = null)

            => MixWithHTMLTemplate(Template,
                                   ResourceName,
                                   new Tuple<String, Assembly>[] {
                                       new Tuple<String, Assembly>(HeimdallAPI.HTTPRoot, typeof(HeimdallAPI).Assembly),
                                       new Tuple<String, Assembly>(UsersAPI.   HTTPRoot, typeof(UsersAPI).   Assembly),
                                       new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
                                   },
                                   Content);

        #endregion

        #endregion

        private void RegisterURLTemplates()
        {

            HTTPServer.AddAuth(request => {

                #region Allow some URLs for anonymous access...

                if (request.Path.StartsWith(URLPathPrefix))
                    return Anonymous;

                #endregion

                return null;

            });

            //HTTPServer.AddFilter(request => {
            //    return null;
            //});

            HTTPServer.Rewrite  (request => {

                #region /               => /dashboard/index.shtml

                //if ((request.Path == URLPathPrefix || request.Path == (URLPathPrefix + "/")) &&
                //    request.HTTPMethod == HTTPMethod.GET &&
                //    TryGetSecurityTokenFromCookie(request, out SecurityToken_Id SecurityToken) &&
                //    _HTTPCookies.ContainsKey(SecurityToken))
                //{

                //    return new HTTPRequest.Builder(request) {
                //        Path = URLPathPrefix + HTTPPath.Parse("/dashboard/index.shtml")
                //    };

                //}

                #endregion

                #region /profile        => /profile/profile.shtml

                //if ((request.Path == URLPathPrefix + "/profile" ||
                //     request.Path == URLPathPrefix + "/profile/") &&
                //     request.HTTPMethod == HTTPMethod.GET)
                //{

                //    return new HTTPRequest.Builder(request) {
                //        Path = URLPathPrefix + HTTPPath.Parse("/profile/profile.shtml")
                //    };

                //}

                #endregion

                #region /admin          => /admin/index.shtml

                //if (request.Path == URLPathPrefix + "/admin" &&
                //    request.HTTPMethod == HTTPMethod.GET)
                //{

                //    return new HTTPRequest.Builder(request) {
                //        Path = URLPathPrefix + HTTPPath.Parse("/admin/index.shtml")
                //    };

                //}

                #endregion

                return request;

            });



            #region / (HTTPRoot)

            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.GET,
                              new HTTPPath[] {
                                  URLPathPrefix + HTTPPath.Parse("/index.html"),
                                  URLPathPrefix + HTTPPath.Parse("/"),
                                  URLPathPrefix + HTTPPath.Parse("/{FileName}")
                              },
                              HTTPDelegate: Request => {

                                  #region Get file path

                                  var filePath = (Request.ParsedURLParameters is not null && Request.ParsedURLParameters.Length > 0)
                                                     ? Request.ParsedURLParameters.Last().Replace("/", ".")
                                                     : "index.html";

                                  if (filePath.EndsWith(".", StringComparison.Ordinal))
                                      filePath += "index.shtml";

                                  #endregion

                                  #region The resource is a templated HTML file...

                                  if (filePath.EndsWith(".shtml", StringComparison.Ordinal))
                                  {

                                      var file = MixWithHTMLTemplate(filePath);

                                      if (file.IsNullOrEmpty())
                                          return Task.FromResult(
                                              new HTTPResponse.Builder(Request) {
                                                  HTTPStatusCode  = HTTPStatusCode.NotFound,
                                                  Server          = HTTPServer.DefaultServerName,
                                                  Date            = Timestamp.Now,
                                                  CacheControl    = "public, max-age=300",
                                                  Connection      = "close"
                                              }.AsImmutable);

                                      else
                                          return Task.FromResult(
                                              new HTTPResponse.Builder(Request) {
                                                  HTTPStatusCode  = HTTPStatusCode.OK,
                                                  ContentType     = HTTPContentType.HTML_UTF8,
                                                  Content         = file.ToUTF8Bytes(),
                                                  CacheControl    = "public, max-age=300",
                                                  Connection      = "close"
                                              }.AsImmutable);

                                  }

                                  #endregion

                                  else
                                  {

                                      var resourceStream = GetResourceStream(filePath);

                                      #region File not found!

                                      if (resourceStream is null)
                                          return Task.FromResult(
                                              new HTTPResponse.Builder(Request) {
                                                  HTTPStatusCode  = HTTPStatusCode.NotFound,
                                                  Server          = HTTPServer.DefaultServerName,
                                                  Date            = Timestamp.Now,
                                                  CacheControl    = "public, max-age=300",
                                                  Connection      = "close"
                                              }.AsImmutable);

                                      #endregion

                                      #region Choose HTTP content type based on the file name extention of the requested resource...

                                      var fileName             = filePath[(filePath.LastIndexOf("/") + 1)..];

                                      var responseContentType  = fileName.Remove(0, fileName.LastIndexOf(".") + 1) switch {

                                          "htm"   => HTTPContentType.HTML_UTF8,
                                          "html"  => HTTPContentType.HTML_UTF8,
                                          "css"   => HTTPContentType.CSS_UTF8,
                                          "gif"   => HTTPContentType.GIF,
                                          "jpg"   => HTTPContentType.JPEG,
                                          "jpeg"  => HTTPContentType.JPEG,
                                          "svg"   => HTTPContentType.SVG,
                                          "png"   => HTTPContentType.PNG,
                                          "ico"   => HTTPContentType.ICO,
                                          "swf"   => HTTPContentType.SWF,
                                          "js"    => HTTPContentType.JAVASCRIPT_UTF8,
                                          "txt"   => HTTPContentType.TEXT_UTF8,
                                          "xml"   => HTTPContentType.XMLTEXT_UTF8,

                                          _       => HTTPContentType.OCTETSTREAM,

                                      };

                                      #endregion

                                      #region Create HTTP response

                                      return Task.FromResult(
                                          new HTTPResponse.Builder(Request) {
                                              HTTPStatusCode  = HTTPStatusCode.OK,
                                              Server          = HTTPServer.DefaultServerName,
                                              Date            = Timestamp.Now,
                                              ContentType     = responseContentType,
                                              ContentStream   = resourceStream,
                                              CacheControl    = "public, max-age=300",
                                              //Expires          = "Mon, 25 Jun 2015 21:31:12 GMT",
//                                              KeepAlive       = new KeepAliveType(TimeSpan.FromMinutes(5), 500),
//                                              Connection      = "Keep-Alive",
                                              Connection      = "close"
                                          }.AsImmutable);

                                      #endregion

                                  }

                              }, AllowReplacement: URLReplacement.Allow);

            #endregion


        }

        #endregion




        /// <summary>
        /// Add a method callback for the given URL template.
        /// </summary>
        /// <param name="Hostname">A HTTP hostname.</param>
        /// <param name="HTTPMethod">A HTTP method.</param>
        /// <param name="URLTemplate">An URL templates.</param>
        /// <param name="HTTPContentType">The HTTP content type.</param>
        /// <param name="URLAuthentication">Whether this method needs explicit uri authentication or not.</param>
        /// <param name="HTTPMethodAuthentication">Whether this method needs explicit HTTP method authentication or not.</param>
        /// <param name="ContentTypeAuthentication">Whether this method needs explicit HTTP content type authentication or not.</param>
        /// <param name="HTTPRequestLogger">A HTTP request logger.</param>
        /// <param name="HTTPResponseLogger">A HTTP response logger.</param>
        /// <param name="DefaultErrorHandler">The default error handler.</param>
        /// <param name="HTTPDelegate">The method to call.</param>
        public void AddForwarding(HTTPHostname             Hostname,
                                  HTTPMethod               HTTPMethod,
                                  HTTPPath                 URLTemplate,
                                  URL                      Target,

                                  Boolean                  RemovePath                  = true,

                                  HTTPContentType?         HTTPContentType             = null,
                                  HTTPAuthentication?      URLAuthentication           = null,
                                  HTTPAuthentication?      HTTPMethodAuthentication    = null,
                                  HTTPAuthentication?      ContentTypeAuthentication   = null,
                                  HTTPRequestLogHandler?   HTTPRequestLogger           = null,
                                  HTTPResponseLogHandler?  HTTPResponseLogger          = null,
                                  HTTPDelegate?            DefaultErrorHandler         = null,
                                  HTTPDelegate?            HTTPDelegate                = null,
                                  URLReplacement           AllowReplacement            = URLReplacement.Fail)
        {

            AddMethodCallback(Hostname,
                              HTTPMethod,
                              URLTemplate,
                              HTTPDelegate: async Request => {

                                  var requestPath   = Request.Path;

                                  if (RemovePath)
                                  {

                                      if (requestPath.ToString().StartsWith(URLTemplate.ToString(), StringComparison.Ordinal))
                                          requestPath = HTTPPath.Parse(requestPath.ToString()[URLTemplate.ToString().Length..]);

                                  }


                                  var httpClient    = HTTPClientFactory.Create(Target);
                                  var httpResponse  = await httpClient.GET(Target.Path + requestPath).
                                                                       ConfigureAwait(false);

                                  return httpResponse;

                              });

        }


    }

}
