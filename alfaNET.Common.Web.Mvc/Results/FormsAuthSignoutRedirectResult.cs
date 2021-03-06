﻿// Copyright 2015 Andrei Rînea
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
using System.Web.Mvc;
using System.Web.Security;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Results
{
    /// <summary>
    /// Forms Authentication sign-out MVC result
    /// </summary>
    public class FormsAuthSignoutRedirectResult : ActionResult
    {
        /// <summary>
        /// Constructs an instance of <see cref="FormsAuthSignoutRedirectResult"/>
        /// </summary>
        /// <param name="redirectUrl">The redirect URL. This may be null.</param>
        /// <remarks>If the redirect URL is null the execution will try to redirect to the referrer and if that is missing, will redirect to root.</remarks>
        public FormsAuthSignoutRedirectResult(string redirectUrl = null)
        {
            RedirectUrl = redirectUrl;
        }

        /// <summary>
        /// The post-signout redirect URL
        /// </summary>
        public string RedirectUrl { get; private set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <exception cref="ArgumentNullException">In case context is null</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            ExceptionUtil.ThrowIfNull(context, "context");

            var url = RedirectUrl;
            if (url == null)
            {
                var referrer = context.RequestContext.HttpContext.Request.UrlReferrer;
                url = referrer == null ? "/" : referrer.ToString();
            }
            FormsAuthentication.SignOut();
            context.RequestContext.HttpContext.Response.Redirect(url);
        }
    }
}