using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Cissy.APIs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ApiTagAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        public string GroupName { get; set; }

        public string Description { get; set; }

        public ApiTagAttribute(string version, string description = null)
            : base("/api/" + version.ToString() + "/[controller]/[action]")
        {
            GroupName = version.ToString();
            Description = description;
        }
    }
}
