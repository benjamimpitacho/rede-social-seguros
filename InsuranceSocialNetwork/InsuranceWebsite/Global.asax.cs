using InsuranceWebsite.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InsuranceWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var waitHandle = new AutoResetEvent(false);
            ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                // Method to execute
                (state, timeout) =>
                {
                    //InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.Log(InsuranceSocialNetworkCore.Enums.SystemLogLevelEnum.INFO, "WORKER", "TIMER JOB TASK", string.Format("Run at {0}", DateTime.Now.ToString()));

                    WorkerUtils.CheckPendingPaymentsToConfirm();

                    //InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.CheckUsersToDisableDueToPaymentFail();

                    //InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.CheckUsersForRenewal();
                },
                // optional state object to pass to the method
                null,
                // Execute the method after 5 seconds
                //TimeSpan.FromSeconds(5),
                TimeSpan.FromMinutes(15),
                // Set this to false to execute it repeatedly every 5 seconds
                false
            );
        }
        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            string culture = null;
            if (context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            {
                culture = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // For example a cookie, but better extract it from the url
            //string culture = HttpContext.Current.Request.Cookies["culture"].Value;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }
}
