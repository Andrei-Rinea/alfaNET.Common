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
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using alfaNET.Common.Time;
using alfaNET.Common.Web.Mvc.Results;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.Web.Mvc.Tests.Results
{
    public class FormsAuthSigninRedirectResultTests
    {
        private const string Username = "me";
        private const string UserData = "asdf";
        private const string ReturnUrl = "/";

        private static readonly DateTime IssueDate = new DateTime(2015, 2, 6);

        private HttpCookieCollection _cookies;
        private FormsAuthenticationTicket _ticket;
        private HttpResponseBase _response;

        [Fact]
        public void ExecuteResult_SetsAuthCookie()
        {
            Init();
            var authCookie = _cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(authCookie);
        }

        [Fact]
        public void ExecuteResult_AuthCookieContainsTicket()
        {
            Init(extractTicket: true);
            Assert.NotNull(_ticket);
        }

        [Fact]
        public void ExecuteResult_TicketNameIsCorrect()
        {
            Init(extractTicket: true);
            Assert.Equal(Username, _ticket.Name);
        }

        [Fact]
        public void ExecuteResult_TicketUserDataIsCorrect()
        {
            Init(extractTicket: true);
            Assert.Equal(UserData, _ticket.UserData);
        }

        [Fact]
        public void ExecuteResult_TicketIssueDateIsCorrect()
        {
            Init(extractTicket: true);
            Assert.Equal(IssueDate, _ticket.IssueDate);
        }

        [Fact]
        public void ExecuteResult_TicketExpirationIsFarEnough()
        {
            Init(extractTicket: true);
            Assert.True(_ticket.Expiration > IssueDate.AddMonths(1));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ExecuteResult_PersistenIsCorrect(bool persistent)
        {
            Init(persistent: persistent, extractTicket: true);
            Assert.Equal(persistent, _ticket.IsPersistent);
        }

        [Fact]
        public void ExecuteResult_Redirects()
        {
            Init();
            _response.Received().Redirect(Arg.Any<string>());
        }

        [Fact]
        public void ExecuteResult_RedirectsCorrectly()
        {
            Init();
            _response.Received().Redirect(ReturnUrl);
        }

        private void Init(bool persistent = true, bool extractTicket = false)
        {
            var timeService = Substitute.For<ITimeService>();
            timeService.UtcNow.Returns(IssueDate);
            var systemUnderTest = new FormsAuthSigninRedirectResult(Username, timeService.UtcNow.UtcDateTime, persistent, UserData);

            var httpContext = Substitute.For<HttpContextBase>();
            var textWriter = Substitute.For<TextWriter>();
            var httpRequest = new HttpRequest("a.txt", "http://localhost/a.txt", "ReturnUrl=" + ReturnUrl);
            var httpResponse = new HttpResponse(textWriter);
            HttpContext.Current = new HttpContext(httpRequest, httpResponse);
            _response = Substitute.For<HttpResponseBase>();
            _cookies = new HttpCookieCollection();
            _response.Cookies.Returns(_cookies);
            httpContext.Response.Returns(_response);
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            systemUnderTest.ExecuteResult(controllerContext);

            if (!extractTicket) return;
            var authCookie = _cookies[FormsAuthentication.FormsCookieName];
            // ReSharper disable once PossibleNullReferenceException
            _ticket = FormsAuthentication.Decrypt(authCookie.Value);
        }
    }
}