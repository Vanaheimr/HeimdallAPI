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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.Vanaheimr.Heimdall.API.Tests
{

    /// <summary>
    /// Tests between Hermod HTTP clients and Hermod HTTP servers.
    /// </summary>
    [TestFixture]
    public class HTTPClientTests : AHeimdallAPITests
    {

        #region Data

        public static readonly IPPort HTTPAPI_TCPPort1      = IPPort.Parse(81);
        public static readonly IPPort HTTPAPI_TCPPort2      = IPPort.Parse(82);

        public static readonly IPPort HeimdallAPI_TCPPort3  = IPPort.Parse(90);

        #endregion

        #region Constructor(s)

        public HTTPClientTests()

            : base(HTTPAPI_TCPPort1,
                   HTTPAPI_TCPPort2,
                   HeimdallAPI_TCPPort3)

        { }

        #endregion


        #region Test_GET_api1()

        [Test]
        public async Task Test_GET_api1()
        {

            var httpClient    = new HTTPClient(URL.Parse($"http://127.0.0.1:{HeimdallAPI_TCPPort3}"));
            var httpResponse  = await httpClient.GET(HTTPPath.Root + "api1").
                                                 ConfigureAwait(false);



            var request       = httpResponse.HTTPRequest?.EntirePDU ?? "";

            // GET /api1 HTTP/1.1
            // Host: 127.0.0.1:90

            Assert.IsTrue (request.Contains("GET /api1 HTTP/1.1"),                        request);
            Assert.IsTrue (request.Contains($"Host: 127.0.0.1:{HeimdallAPI_TCPPort3}"),   request);



            var response      = httpResponse.EntirePDU;
            var httpBody      = httpResponse.HTTPBodyAsUTF8String;

            // HTTP/1.1 200 OK
            // Date:                            Sun, 30 Jul 2023 22:03:29 GMT
            // Server:                          Heimdall Test Server 1
            // Access-Control-Allow-Origin:     *
            // Access-Control-Allow-Methods:    GET
            // Content-Type:                    text/plain; charset=utf-8
            // Content-Length:                  15
            // Connection:                      close
            // X-Environment-ManagedThreadId:   20
            // 
            // Hello World v1!

            Assert.IsTrue  (response.Contains("HTTP/1.1 200 OK"),   response);
            Assert.IsTrue  (response.Contains("Hello World v1!"),   response);

            Assert.AreEqual("Hello World v1!",                      httpBody);

            Assert.AreEqual("Heimdall Test Server 1",               httpResponse.Server);
            Assert.AreEqual("Hello World v1!".Length,               httpResponse.ContentLength);

        }

        #endregion

        #region Test_GET_api2()

        [Test]
        public async Task Test_GET_api2()
        {

            var httpClient    = new HTTPClient(URL.Parse($"http://127.0.0.1:{HeimdallAPI_TCPPort3}"));
            var httpResponse  = await httpClient.GET(HTTPPath.Root + "api2").
                                                 ConfigureAwait(false);



            var request       = httpResponse.HTTPRequest?.EntirePDU ?? "";

            // GET /api2 HTTP/1.1
            // Host: 127.0.0.1:90

            Assert.IsTrue (request.Contains("GET /api2 HTTP/1.1"),                        request);
            Assert.IsTrue (request.Contains($"Host: 127.0.0.1:{HeimdallAPI_TCPPort3}"),   request);



            var response      = httpResponse.EntirePDU;
            var httpBody      = httpResponse.HTTPBodyAsUTF8String;

            // HTTP/1.1 200 OK
            // Date:                            Sun, 30 Jul 2023 22:04:45 GMT
            // Server:                          Heimdall Test Server 2
            // Access-Control-Allow-Origin:     *
            // Access-Control-Allow-Methods:    GET
            // Content-Type:                    text/plain; charset=utf-8
            // Content-Length:                  15
            // Connection:                      close
            // X-Environment-ManagedThreadId:   10
            //
            // Hello World v2!

            Assert.IsTrue  (response.Contains("HTTP/1.1 200 OK"),   response);
            Assert.IsTrue  (response.Contains("Hello World v2!"),   response);

            Assert.AreEqual("Hello World v2!",                      httpBody);

            Assert.AreEqual("Heimdall Test Server 2",               httpResponse.Server);
            Assert.AreEqual("Hello World v2!".Length,               httpResponse.ContentLength);

        }

        #endregion


    }

}
