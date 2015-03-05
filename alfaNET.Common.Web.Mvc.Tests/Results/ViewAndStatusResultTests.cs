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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using alfaNET.Common.Web.Mvc.Results;
using NSubstitute;
using Xunit;

namespace alfaNET.Common.Web.Mvc.Tests.Results
{
    public class ViewAndStatusResultTests
    {
        [Fact]
        public void ExecuteResult_SetsStatusCode()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;

            var view = Substitute.For<IView>();
            var systemUnderTest = new ViewAndStatusResult { ViewName = "ViewName", StatusCode = httpStatusCode, View = view };
            var controllerContext = new ControllerContext();
            var httpContext = Substitute.For<HttpContextBase>();
            var itemsDictionary = new Dictionary<string, string>();
            httpContext.Items.Returns(itemsDictionary);
            controllerContext.HttpContext = httpContext;
            var response = Substitute.For<HttpResponseBase>();
            TextWriter stringWriter = new StringWriter();
            var responseStatusCode = -1;
            response.When(x => x.StatusCode = Arg.Any<int>()).Do(x =>
            {
                responseStatusCode = (int)x[0];
            });
            response.Output.Returns(stringWriter);
            httpContext.Response.Returns(response);
            controllerContext.RouteData.Values.Add("controller", "controller");
            controllerContext.RouteData.Values.Add("action", "action");

            systemUnderTest.ExecuteResult(controllerContext);

            Assert.Equal((int)httpStatusCode, responseStatusCode);
        }
    }
}