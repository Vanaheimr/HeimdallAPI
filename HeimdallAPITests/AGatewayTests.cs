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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.Vanaheimr.Heimdall.API.Tests
{

    public abstract class AHeimdallAPITests
    {

        #region Data

        protected readonly HTTPAPI      httpAPI1;
        protected readonly HTTPAPI      httpAPI2;

        protected readonly HeimdallAPI  heimdallAPI;

        public AHeimdallAPITests(IPPort HTTPAPI1_TCPPort,
                                 IPPort HTTPAPI2_TCPPort,
                                 IPPort HeimdallAPI_TCPPort)
        {

            httpAPI1     = new HTTPAPI(
                               HTTPServerPort:  HTTPAPI1_TCPPort,
                               AutoStart:       true
                           );

            httpAPI2     = new HTTPAPI(
                               HTTPServerPort:  HTTPAPI2_TCPPort,
                               AutoStart:       true
                           );

            heimdallAPI  = new HeimdallAPI(
                               HTTPServerPort:  HeimdallAPI_TCPPort,
                               AutoStart:       true
                           );

        }

        #endregion

        #region Init_HTTPServer()

        [OneTimeSetUp]
        public void Init_HTTPServer()
        {

            #region GET     /

            httpAPI1.AddMethodCallback(HTTPHostname.Any,
                                       HTTPMethod.GET,
                                       HTTPPath.Root,
                                       HTTPDelegate: request => Task.FromResult(
                                                                     new HTTPResponse.Builder(request) {
                                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                                         Server                     = "Heimdall Test Server 1",
                                                                         Date                       = Timestamp.Now,
                                                                         AccessControlAllowOrigin   = "*",
                                                                         AccessControlAllowMethods  = new[] { "GET" },
                                                                         ContentType                = HTTPContentType.TEXT_UTF8,
                                                                         Content                    = "Hello World v1!".ToUTF8Bytes(),
                                                                         Connection                 = "close"
                                                                     }.SetHeaderField("X-Environment-ManagedThreadId", Environment.CurrentManagedThreadId).
                                                                       AsImmutable));

            httpAPI2.AddMethodCallback(HTTPHostname.Any,
                                       HTTPMethod.GET,
                                       HTTPPath.Root,
                                       HTTPDelegate: request => Task.FromResult(
                                                                     new HTTPResponse.Builder(request) {
                                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                                         Server                     = "Heimdall Test Server 2",
                                                                         Date                       = Timestamp.Now,
                                                                         AccessControlAllowOrigin   = "*",
                                                                         AccessControlAllowMethods  = new[] { "GET" },
                                                                         ContentType                = HTTPContentType.TEXT_UTF8,
                                                                         Content                    = "Hello World v2!".ToUTF8Bytes(),
                                                                         Connection                 = "close"
                                                                     }.SetHeaderField("X-Environment-ManagedThreadId", Environment.CurrentManagedThreadId).
                                                                       AsImmutable));

            #endregion


            heimdallAPI.AddForwarding(HTTPHostname.Any,
                            HTTPMethod.GET,
                            HTTPPath.Root + "api1",
                            URL.Parse($"https://localhost:{httpAPI1.HTTPServer.IPPorts.First()}"));

            heimdallAPI.AddForwarding(HTTPHostname.Any,
                            HTTPMethod.GET,
                            HTTPPath.Root + "api2",
                            URL.Parse($"https://localhost:{httpAPI2.HTTPServer.IPPorts.First()}"));


            #region MIRROR  /mirror

            httpAPI1.AddMethodCallback(HTTPHostname.Any,
                                       HTTPMethod.MIRROR,
                                       HTTPPath.Root + "mirror",
                                       HTTPDelegate: request => Task.FromResult(
                                                                     new HTTPResponse.Builder(request) {
                                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                                         Server                     = "Heimdall Test Server 1",
                                                                         Date                       = Timestamp.Now,
                                                                         AccessControlAllowOrigin   = "*",
                                                                         AccessControlAllowMethods  = new[] { "MIRROR" },
                                                                         ContentType                = HTTPContentType.TEXT_UTF8,
                                                                         Content                    = (request.HTTPBodyAsUTF8String ?? "").Reverse().ToUTF8Bytes(),
                                                                         Connection                 = "close"
                                                                     }.AsImmutable));

            httpAPI2.AddMethodCallback(HTTPHostname.Any,
                                       HTTPMethod.MIRROR,
                                       HTTPPath.Root + "mirror",
                                       HTTPDelegate: request => Task.FromResult(
                                                                     new HTTPResponse.Builder(request) {
                                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                                         Server                     = "Heimdall Test Server 2",
                                                                         Date                       = Timestamp.Now,
                                                                         AccessControlAllowOrigin   = "*",
                                                                         AccessControlAllowMethods  = new[] { "MIRROR" },
                                                                         ContentType                = HTTPContentType.TEXT_UTF8,
                                                                         Content                    = (request.HTTPBodyAsUTF8String ?? "").Reverse().ToUTF8Bytes(),
                                                                         Connection                 = "close"
                                                                     }.AsImmutable));

            #endregion

            #region GET     /chunked

            httpAPI1.AddMethodCallback(HTTPHostname.Any,
                                       HTTPMethod.GET,
                                       HTTPPath.Root + "chunked",
                                       HTTPDelegate: request => Task.FromResult(
                                                                     new HTTPResponse.Builder(request) {
                                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                                         Server                     = "Heimdall Test Server",
                                                                         Date                       = Timestamp.Now,
                                                                         AccessControlAllowOrigin   = "*",
                                                                         AccessControlAllowMethods  = new[] { "GET" },
                                                                         TransferEncoding           = "chunked",
                                                                         Trailer                    = "X-Message-Length, X-Protocol-Version",
                                                                         ContentType                = HTTPContentType.TEXT_UTF8,
                                                                         Content                    = (new[] { "5", "Hello", "1", " ", "6", "World!", "0" }.AggregateWith("\r\n") + "\r\nX-Message-Length: 13\r\nX-Protocol-Version: 1.0\r\n\r\n").ToUTF8Bytes(),
                                                                         Connection                 = "close"
                                                                     }.SetHeaderField("X-Environment-ManagedThreadId", Environment.CurrentManagedThreadId).
                                                                       AsImmutable));

            #endregion


        }

        #endregion

        #region Shutdown_HTTPServer()

        [OneTimeTearDown]
        public void Shutdown_HTTPServer()
        {
            httpAPI1?.Shutdown();
        }

        #endregion


    }

}
