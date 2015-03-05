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
using System.Net;
using System.Web.Mvc;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Results
{
    // TODO : Unit tests
    /// <summary>
    /// A <see cref="ViewResult"/>-derived result that besides setting a view also sets a status code
    /// </summary>
    public class ViewAndStatusResult : ViewResult
    {
        /// <summary>
        /// Status code to be send in the response to the client
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            ExceptionUtil.ThrowIfNull(context, "context");
            if (StatusCode != null)
                context.HttpContext.Response.StatusCode = (int)StatusCode;
            base.ExecuteResult(context);
        }
    }
}