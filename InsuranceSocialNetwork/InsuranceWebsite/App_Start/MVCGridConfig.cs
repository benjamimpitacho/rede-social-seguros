[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(InsuranceWebsite.MVCGridConfig), "RegisterGrids")]

namespace InsuranceWebsite
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using Models;
    using InsuranceSocialNetworkBusiness;
    using Utils;

    public static class MVCGridConfig
    {
        public static string GetActivateCommandCode(string val)
        {
            return "<script language='JavaScript'>('" + val + "' == 'False') ? document.write('<span class=\"glyphicon glyphicon-ok-circle\"></span>') : document.write('');</script>";
        }

        public static string GetBlockCommandCode(string val)
        {
            return "<script language='JavaScript'>('" + val + "' == 'True') ? document.write('<span class=\"glyphicon glyphicon-ban-circle\"></span>') : document.write('');</script>";
        }

        public static void RegisterGrids()
        {
        }
    }
}