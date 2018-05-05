using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceWebsite.Models
{
    public class ErrorViewModel : System.Web.Mvc.HandleErrorInfo
    {
        public string ErrorMessage { get; set; }

        public ErrorViewModel(Exception exception, string controllerName, string actionName) : base(exception, controllerName, actionName) { }
    }
}