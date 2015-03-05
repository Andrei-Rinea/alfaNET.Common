// Copyright 2015 Andrei Rînea
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using alfaNET.Common.Web.Mvc.Results;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace alfaNET.Common.Web.Mvc.Tests.Results
{
    public class FormsAuthSignoutRedirectResultTests
    {
        private HttpResponse _httpResponse;
        private int _redirectCallsCount;
        private string _redirectUrl;

        public void Init(string redirectUrl = null, Uri httpReferrer = null)
        {
            var httpContext = Substitute.For<HttpContextBase>();
            var systemUnderTest = new FormsAuthSignoutRedirectResult(redirectUrl);
            var request = Substitute.For<HttpRequestBase>();
            request.UrlReferrer.Returns(httpReferrer);
            var response = Substitute.For<HttpResponseBase>();
            response.When(r => r.Redirect(Arg.Any<string>())).Do(MockRedirect);
            response.When(r => r.Redirect(Arg.Any<string>(), Arg.Any<bool>())).Do(MockRedirect);
            httpContext.Request.Returns(request);
            httpContext.Response.Returns(response);
            var controllerContext = new ControllerContext { HttpContext = httpContext };

            var textWriter = Substitute.For<TextWriter>();
            var capabilities = new Dictionary<string, string> { { "cookies", "true" } };
            var httpBrowserCapabilities = new HttpBrowserCapabilities { Capabilities = capabilities };
            var httpRequest = new HttpRequest("a.txt", "http://localhost/a.txt", "") { Browser = httpBrowserCapabilities };
            _httpResponse = new HttpResponse(textWriter);
            HttpContext.Current = new HttpContext(httpRequest, _httpResponse);
            systemUnderTest.ExecuteResult(controllerContext);
        }

        private void MockRedirect(CallInfo x)
        {
            _redirectUrl = (string)x[0];
            _redirectCallsCount++;
        }

        [Fact]
        public void Execute_CallsRedirect()
        {
            Init();
            Assert.True(_redirectCallsCount > 0);
        }

        [Fact]
        public void Execute_EmitsCookie()
        {
            Init();
            var cookie = _httpResponse.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
        }

        [Fact]
        public void Execute_ExpiresCookie()
        {
            Init();
            var cookie = _httpResponse.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.True(cookie.Expires < DateTime.Now);
        }

        [Fact]
        public void Execute_RedirectsToRootForNullUrlAndNoReferrer()
        {
            Init();
            Assert.Equal("/", _redirectUrl);
        }

        [Fact]
        public void Execute_RedirectsToReferrerIfPresent()
        {
            const string referrer = "/some/page";
            Init(httpReferrer: new Uri(referrer, UriKind.Relative));
            Assert.Equal(referrer, _redirectUrl);
        }

        [Fact]
        public void Execute_RedirectsToGivenUrl()
        {
            const string url = "/some/other/page";
            Init(redirectUrl: url);
            Assert.Equal(url, _redirectUrl);
        }
    }
}