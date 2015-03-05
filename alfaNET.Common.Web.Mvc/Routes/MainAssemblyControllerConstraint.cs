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
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Routes
{
    // TODO : Unit tests
    /// <summary>
    /// <see cref="IRouteConstraint"/> that forces the controller value to be one of the controllers in the given assembly.
    /// </summary>
    public class MainAssemblyControllerConstraint : IRouteConstraint
    {
        private const string ControllerRouteValueName = "controller";
        private const string ControllerNameSuffix = "Controller";

        private readonly string[] _controllerNames;

        /// <summary>
        /// Constructs an instance of <see cref="MainAssemblyControllerConstraint"/>
        /// </summary>
        /// <param name="assembly">The assembly to search for valid controllers</param>
        /// <exception cref="ArgumentNullException">In case assembly is null</exception>
        public MainAssemblyControllerConstraint(Assembly assembly)
        {
            ExceptionUtil.ThrowIfNull(assembly, "assembly");
            var types = assembly.GetTypes().Where(IsControllerType);
            _controllerNames = types.Select(GetControllerName).ToArray();
        }

        private static string GetControllerName(Type type)
        {
            var typeName = type.Name;
            return typeName.Substring(0, typeName.Length - ControllerNameSuffix.Length);
        }

        private static bool IsControllerType(Type t)
        {
            var controllerType = typeof(IController);
            return
                t != null &&
                t.IsPublic &&
                t.Name.EndsWith(ControllerNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                !t.IsAbstract &&
                controllerType.IsAssignableFrom(t);
        }

        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param><param name="route">The object that this constraint belongs to.</param><param name="parameterName">The name of the parameter that is being checked.</param><param name="values">An object that contains the parameters for the URL.</param><param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(ControllerRouteValueName))
                return false;
            var controllerName = values[ControllerRouteValueName] as string;
            return IsControllerKnown(controllerName);
        }

        private bool IsControllerKnown(string controllerName)
        {
            return
                !string.IsNullOrWhiteSpace(controllerName) &&
                _controllerNames.Contains(controllerName, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}