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
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class FormsAuthSigninRedirectResult : ActionResult
    {
        public FormsAuthSigninRedirectResult(string username, DateTime issueDate, bool persistent = true, string userData = "")
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(username, "username");
            ExceptionUtil.ThrowIfDefault(issueDate, "issueDate");

            Username = username;
            IssueDate = issueDate;
            Persistent = persistent;
            UserData = userData;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Persistent { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string UserData { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime IssueDate { get; private set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            ExceptionUtil.ThrowIfNull(context, "context");

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: Username,
                issueDate: IssueDate,
                expiration: IssueDate.AddYears(20),
                isPersistent: Persistent,
                userData: UserData,
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encTicket = FormsAuthentication.Encrypt(ticket);
            var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            context.HttpContext.Response.Cookies.Add(httpCookie);
            var redirectUrl = FormsAuthentication.GetRedirectUrl(Username, Persistent);
            context.HttpContext.Response.Redirect(redirectUrl);
        }
    }
}