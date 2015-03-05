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
    public class MainAssemblyControllerConstraint : IRouteConstraint
    {
        private const string ControllerRouteValueName = "controller";
        private const string ControllerNameSuffix = "Controller";

        private readonly string[] _controllerNames;

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