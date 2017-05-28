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
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("UsersManagementGrid", new MVCGridBuilder<UserProfileModelObject>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("Username")
                        .WithHeaderText(Resources.Resources.Username)
                        .WithValueExpression(i => i.User.UserName)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText(Resources.Resources.Name)
                        .WithValueExpression(i => i.FirstName)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("UrlExample")
                        .WithHeaderText("Edit")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("detail", "demo", new { id = i.ID }))
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true);
                })
                .WithSorting(true, "FirstName")
                .WithAdditionalQueryOptionNames("Search")
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>

                    var options = context.QueryOptions;
                    var result = new QueryResult<UserProfileModelObject>();
                    //result.Items = BusinessItemsLists.GetUsers();
                    var query = BusinessItemsLists.GetUsers();
                    if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                    {
                        switch (options.SortColumnName.ToLower())
                        {
                            case "firstname":
                                if(options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.FirstName).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.FirstName).ToList();
                                }
                                
                                break;
                            case "lastname":
                                //query = query.OrderBy(p => p.LastName, options.SortDirection);
                                break;
                        }
                    }

                    string globalSearch = options.GetAdditionalQueryOptionString("Search");
                    query = query.Where(i => 
                        i.FirstName.Contains(globalSearch)
                        || i.LastName.Contains(globalSearch)
                        || i.User.UserName.Contains(globalSearch)
                        || i.ContactEmail.Contains(globalSearch)
                    ).ToList();

                    return new QueryResult<UserProfileModelObject>()
                    {
                        Items = query,
                        TotalRecords = query.Count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
        }
    }
}