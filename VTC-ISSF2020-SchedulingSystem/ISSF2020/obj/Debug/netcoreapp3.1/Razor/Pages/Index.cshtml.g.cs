#pragma checksum "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f09efd9a04ad70c09c699f480533b6c2d35f642d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ISSF2020.Pages.Pages_Index), @"mvc.1.0.razor-page", @"/Pages/Index.cshtml")]
namespace ISSF2020.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/_ViewImports.cshtml"
using ISSF2020;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f09efd9a04ad70c09c699f480533b6c2d35f642d", @"/Pages/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7316d72c68264b60edfb05b9701bbf3276724f27", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
  
    ViewData["Title"] = "Home page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    <div class=\"text-center\">\n        <h1 class=\"display-4\">Welcome</h1>\n\n        <!--if (ViewData[\"User\"] != null)\n        {\n        <h3> Current user: ViewData[\"User\"]</h3>\n        <a href=\"/account/logout\">Logout</a>\n        }-->\n        <hr />\n\n");
#nullable restore
#line 17 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
         if (ViewData["WeatherData"] != null)
        {
            var weatherData = (ISSF2020.Models.RegionalWeatherModel)ViewData["WeatherData"];


#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""list-group-item list-group-item-action flex-column align-items-start mb-3"">
                <div class=""d-flex w-100 justify-content-center mb-2"">
                    <h5 class=""mb-1 text-info"">Current Regional Weather</h5>
                </div>

                <p class=""d-flex w-100 justify-content-around mb-1"">

                    <span><small class=""text-muted"">Temperature:</small> ");
#nullable restore
#line 28 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
                                                                    Write(weatherData.Temperature);

#line default
#line hidden
#nullable disable
            WriteLiteral(" °C</span>\n\n                    <span><small class=\"text-muted\">Humidity:</small> ");
#nullable restore
#line 30 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
                                                                 Write(weatherData.Humidity);

#line default
#line hidden
#nullable disable
            WriteLiteral(" %</span>\n\n                    <span><small class=\"text-muted\">Description:</small> ");
#nullable restore
#line 32 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
                                                                    Write(weatherData.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\n\n                </p>\n\n                <small class=\"text-info\">\n                    <span class=\"text-muted\">Last update:</span>\n                    ");
#nullable restore
#line 38 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
               Write(weatherData.LastUpdate);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <span class=\"text-muted\">");
#nullable restore
#line 39 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
                                        Write(weatherData.Location);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Time</span>\n                </small>\n\n            </div>\n");
#nullable restore
#line 43 "/Users/shingwaichan/Desktop/INB/VTC-ISSF2020-SchedulingSystem/ISSF2020/Pages/Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        <br />
        <hr />

        <h3>Progress List</h3>
        <div class=""list-group"">
            <div class=""list-group-item list-group-item-action flex-column align-items-start"">
                <div class=""d-flex w-100 justify-content-between"">
                    <h5 class=""mb-1"">User Authentication and Authorization</h5>
                    <small class=""text-muted"">1</small>
                </div>

                <h6 class=""mb-1 mt-4 text-center"">Done</h6>
                <p class=""mb-1"">Simple registration and login system. Users can create an account and then log-in with the credentials.</p>
                <p class=""mb-1"">Field validation for both forms; usernames checked with DB to be unique.</p>

                <h6 class=""mb-1 mt-4 text-center"">To do</h6>
                <p class=""mb-1"">
                    Use <a href=""https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio"">ASP.NET Core Identity</a>
                    or ot");
            WriteLiteral(@"her similar systems to properly develop this system.
                </p>
                <p class=""mb-1"">Password hashing and secure storage</p>
                <p class=""mb-1"">Role based access control</p>

                <small>Note: This system needs to be properly developed since, at the moment, logged in user is only saved as Session key.</small>
            </div>
            <div class=""list-group-item list-group-item-action flex-column align-items-start"">
                <div class=""d-flex w-100 justify-content-between"">
                    <h5 class=""mb-1"">Schedule system</h5>
                    <small class=""text-muted"">2</small>
                </div>

                <h6 class=""mb-1 mt-4 text-center"">Done</h6>
                <p class=""mb-1"">Can now schedule event. Bookings are saved in the DB.</p>
                <p class=""mb-1"">List of bookings can be viewed in the Schedules page.</p>

                <h6 class=""mb-1 mt-4 text-center"">To do</h6>
                <p class=""mb-1"">Prevent overlap");
            WriteLiteral(@"s by adding booking validation. </p>
                <p class=""mb-1"">User management of own bookings (cancel, update, edit)</p>
                <p class=""mb-1"">Add schedules access control once user system is more stable.</p>

            </div>
            <div class=""list-group-item list-group-item-action flex-column align-items-start"">
                <div class=""d-flex w-100 justify-content-between"">
                    <h5 class=""mb-1"">Other features</h5>
                    <small class=""text-muted"">3</small>
                </div>

                <h6 class=""mb-1 mt-4 text-center"">Done</h6>
                <p class=""mb-1"">Messages for user as action results through TempData.</p>
                <p class=""mb-1"">Regular expression patterns as validation for some form fields.</p>
                <p class=""mb-1"">Logout page and form to clear session data (cookie remains).</p>


                <h6 class=""mb-1 mt-4 text-center"">To do</h6>
                <p class=""mb-1"">Option to save schedule list as .CSV,");
            WriteLiteral(@" ICS or Excel</p>
                <p class=""mb-1"">Display schedule list through graphs/timetable etc. (and not just raw json)</p>
                <p class=""mb-1"">Add sort-by and filters for the schedules viewing page</p>

                <small class=""text-muted""></small>
            </div>
        </div>
        <hr />
        <p>Learn about <a href=""https://docs.microsoft.com/aspnet/core"">building Web apps with ASP.NET Core</a>.</p>
    </div>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
