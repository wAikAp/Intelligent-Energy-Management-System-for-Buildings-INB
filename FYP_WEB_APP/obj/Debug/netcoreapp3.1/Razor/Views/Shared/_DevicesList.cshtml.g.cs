#pragma checksum "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__DevicesList), @"mvc.1.0.view", @"/Views/Shared/_DevicesList.cshtml")]
namespace AspNetCore
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
#line 1 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/_ViewImports.cshtml"
using FYP_APP;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/_ViewImports.cshtml"
using FYP_APP.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f6", @"/Views/Shared/_DevicesList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1103289fd338f5238efba9485e51b05468496a5f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__DevicesList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/dragButton.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width: 24px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("m-0"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Devices/turnOnOffDevice"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/rightArrow.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("mr-2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width: 16px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/efficiency96.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/lighting32.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
   List<FYP_WEB_APP.Models.DevicesListModel> DevicesList = ViewData["MongoDevicesListModel"] as List<FYP_WEB_APP.Models.DevicesListModel>;

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f67667", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<div class=""modal fade"" id=""DevicesForm"" tabindex=""-1"" role=""dialog"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-dialog-centered"" role=""document"">
        <div class=""modal-content"" id=""DevicesFormBody"">

        </div>
    </div>
</div>
<div class=""card d-flex flex-column mb-2 mx-2 p-2"" >
    <div class=""row justify-content-between mb-2"">
        <div class=""col"">
            <div class=""col h4"">
                Device List
            </div>

        </div>
        <div class=""d-flex justify-content-end"">
");
            WriteLiteral("            <div class=\"d-flex\">\n\n\n");
#nullable restore
#line 35 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                 if (ViewData["roomID"] != null)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a href=\"#\" class=\"btn btn-outline-success px-2 mr-3\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1389, "\"", 1433, 3);
            WriteAttributeValue("", 1399, "AddtDevices(\'", 1399, 13, true);
#nullable restore
#line 37 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("", 1412, ViewData["roomID"], 1412, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1431, "\')", 1431, 2, true);
            EndWriteAttribute();
            WriteLiteral(">\n                        &#43; Add\n                    </a>\n");
#nullable restore
#line 40 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(" <a href=\"#\" class=\"btn btn-outline-success px-2 mr-3\" onclick=\"AddtDevices()\">\n                        &#43; Add\n                    </a>\n");
#nullable restore
#line 45 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"

                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\n        </div>\n    </div>\n\n");
#nullable restore
#line 51 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
     if (DevicesList.Count() == 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"alert alert-warning text-center font-weight-bold\" role=\"alert\">\n            <h3>Please try again, no corresponding conditions.  </h3>\n        </div>");
#nullable restore
#line 55 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
              }
    else
    {


#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"overflow-auto \">\n            <div class=\"card-deck shadow-sm rounded pt-1 \">\n");
#nullable restore
#line 61 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                 foreach (var deviceModel in DevicesList)
                {
                    string unit = "";
                    string deviceImg = "air-conditioner96.png";
                    string arrowInv = deviceModel.status ? "filter: invert(100%);" : "";
                    string devImgInv = deviceModel.status ? "filter: invert(100%);" : "";
                    ViewData["cardType"] = deviceModel.status ? "device_on " : "device_off ";
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 68 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                     switch (deviceModel.devicesId.Substring(0, 2))
                    {
                        case "AC":
                            
                            ViewData["devicesType"] = "Air Conditioner";
                            unit = "ºC";
                            deviceImg = "air-conditioner96.png";
                            break;
                        case "LT":
                            ViewData["cardType"] = deviceModel.status ? "LightDevice_on " : "LightDevice_off ";
                            ViewData["devicesType"] = "Light";
                            devImgInv = "";
                            unit = "lm";
                            deviceImg = deviceModel.status ? "light-on64.png " : "light-off96.png ";

                            break;
                        case "HD":
                            ViewData["devicesType"] = "Humidifier";
                            deviceImg = "humidifier96.png";
                            break;
                        case "EF":
                            ViewData["devicesType"] = "Fan";
                            deviceImg = "fan96.png";
                            break;
                        case "CA":
                            ViewData["devicesType"] = "Camera";
                            deviceImg = "camera96.png";
                            break;
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 96 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                     


                    string turnOFstr = deviceModel.status ? "Turning On" : "Turning Off";
                    string turnOFClr = deviceModel.status ? "badge-success" : "badge-danger";
                    string turnOFimg = deviceModel.status ? "power-button-on36.png" : "power-button-off36.png";
                    ViewData["text"] = deviceModel.status ? "text-dark" : "text-white";
                    


#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div");
            BeginWriteAttribute("class", " class=\"", 4356, "\"", 4414, 4);
            WriteAttributeValue("", 4364, "card", 4364, 4, true);
#nullable restore
#line 105 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("  ", 4368, ViewData["cardType"], 4370, 21, false);

#line default
#line hidden
#nullable disable
#nullable restore
#line 105 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue(" ", 4391, ViewData["text"], 4392, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 4409, "mb-3", 4410, 5, true);
            EndWriteAttribute();
            WriteLiteral(" style=\"max-width: 16rem; min-width: 16rem\">\n                        ");
#nullable restore
#line 106 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                   Write(Html.Partial("_loader", "deviceLoader"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                        <div class=\"card-header pr-1\">\n                            <div class=\"d-flex bd-highlight align-items-center\">\n                                <div class=\"mr-auto bd-highlight\">\n                                    <a");
            BeginWriteAttribute("class", " class=\"", 4766, "\"", 4811, 3);
#nullable restore
#line 110 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue(" ", 4774, ViewData["text"], 4775, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 4792, "align-middle", 4793, 13, true);
            WriteAttributeValue(" ", 4805, "h-100", 4806, 6, true);
            EndWriteAttribute();
            WriteLiteral(" href=\"#\">\n                                        ");
#nullable restore
#line 111 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                   Write(ViewData["devicesType"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                                    </a>\n                                </div>\n                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f617463", async() => {
                WriteLiteral("\n                                    <input type=\"hidden\" name=\"deviceID\"");
                BeginWriteAttribute("value", " value=\"", 5140, "\"", 5170, 1);
#nullable restore
#line 115 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("", 5148, deviceModel.devicesId, 5148, 22, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\n                                    <input type=\"hidden\" name=\"deviceCurrentStatus\"");
                BeginWriteAttribute("value", " value=\"", 5258, "\"", 5276, 1);
#nullable restore
#line 116 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("", 5266, turnOFstr, 5266, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\n\n                                    <button class=\"btn align-middle p-0\" type=\"submit\" >\n\n                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f618877", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 5443, "~/img/icon/", 5443, 11, true);
#nullable restore
#line 120 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
AddHtmlAttributeValue("", 5454, turnOFimg, 5454, 10, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\n\n                                    </button>\n                                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                                <div class=""dropdown  bd-highlight"">

                                    <button role=""button"" class=""btn  float-right ml-2"" type=""button"" id=""dropdownMenuButton0"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">

                                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f622545", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "style", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 5873, "width:", 5873, 6, true);
            AddHtmlAttributeValue(" ", 5879, "16px;", 5880, 6, true);
#nullable restore
#line 128 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
AddHtmlAttributeValue(" ", 5885, arrowInv, 5886, 9, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n                                    </button>\n                                    <div class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuButton0\">\n                                        <a href=\"#\" class=\"dropdown-item text-success \"");
            BeginWriteAttribute("onclick", " onclick=\"", 6167, "\"", 6214, 3);
            WriteAttributeValue("", 6177, "EditDevices(\'", 6177, 13, true);
#nullable restore
#line 131 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("", 6190, deviceModel.devicesId, 6190, 22, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 6212, "\')", 6212, 2, true);
            EndWriteAttribute();
            WriteLiteral(">Edit </a>\n                                        <a href=\"#\" class=\"dropdown-item text-danger \"");
            BeginWriteAttribute("onclick", " onclick=\"", 6312, "\"", 6359, 3);
            WriteAttributeValue("", 6322, "DropDevices(\'", 6322, 13, true);
#nullable restore
#line 132 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("", 6335, deviceModel.devicesId, 6335, 22, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 6357, "\')", 6357, 2, true);
            EndWriteAttribute();
            WriteLiteral(@">Delete </a>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class=""card-body"">

                            <p class=""ard-text p-0 m-0"">");
#nullable restore
#line 141 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                   Write(deviceModel.roomId);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - <b>");
#nullable restore
#line 141 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                            Write(deviceModel.devicesId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> - ");
#nullable restore
#line 141 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                         Write(deviceModel.devices_Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\n\n                            <div class=\"card-title\">\n                                <h3 class=\"card-text\">\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f627157", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "style", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 6882, "width:", 6882, 6, true);
            AddHtmlAttributeValue(" ", 6888, "32px;", 6889, 6, true);
#nullable restore
#line 145 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
AddHtmlAttributeValue(" ", 6894, devImgInv, 6895, 10, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 6912, "~/img/icon/", 6912, 11, true);
#nullable restore
#line 145 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
AddHtmlAttributeValue("", 6923, deviceImg, 6923, 10, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n                                    ");
#nullable restore
#line 146 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                               Write(deviceModel.set_value);

#line default
#line hidden
#nullable disable
#nullable restore
#line 146 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                     Write(unit);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                                    <snap class=\"float-right\">\n\n                                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f630112", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n                                        <b style=\"font-size:10px\">");
#nullable restore
#line 150 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                             Write(deviceModel.power);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" kWh</b>

                                    </snap>
                                </h3>

                            </div>
                            <!--this month usage stauts-->
                            <p class=""card-text"" style=""margin-bottom: 1px; margin-top: 5px""> ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d125fe3ee316dca6f17b6ba74d9cd2f4bba59f631888", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<b> ");
#nullable restore
#line 157 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                                                                         Write(deviceModel.currentMonthUsage);

#line default
#line hidden
#nullable disable
            WriteLiteral(" kW </b> <small>/M - <b>");
#nullable restore
#line 157 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                                                                                                                               Write(deviceModel.currentMonthTotalUseTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> /hr</small></p>\n                            <p class=\"card-text\" style=\"margin-top: 3px\"><small> 📊 Avg <b>");
#nullable restore
#line 158 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                      Write(deviceModel.avgPower);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> kW /day</small></p>\n                            <a");
            BeginWriteAttribute("class", " class=\"", 7877, "\"", 7920, 4);
            WriteAttributeValue("", 7885, "shadow", 7885, 6, true);
            WriteAttributeValue(" ", 7891, "badge", 7892, 6, true);
            WriteAttributeValue(" ", 7897, "badge-pill", 7898, 11, true);
#nullable restore
#line 159 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
WriteAttributeValue("  ", 7908, turnOFClr, 7910, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"margin-left:-2px; font-size: 14px; margin-top:-5px; margin-bottom:5px\" href=\"#\">");
#nullable restore
#line 159 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                                                                             Write(turnOFstr);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\n                            <p class=\"card-text\" style=\"margin-bottom:0px; margin-top:5px\"><small> <b style=\"font-size:14px;\"> 🔄 ");
#nullable restore
#line 160 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                                             Write(deviceModel.lastest_checking_time);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></small></p>\n                            <p class=\"card-text\" style=\"margin-top: 1px; margin-bottom:0px;\"><b style=\"font-size:14px;\"> 📅 ");
#nullable restore
#line 161 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                                       Write(deviceModel.turn_on_time);

#line default
#line hidden
#nullable disable
            WriteLiteral("  </b><small>Turned on</small></p>\n                            <p class=\"card-text\" style=\"margin-top: 1px\"><small> 🕝 Turned on</small><b> ");
#nullable restore
#line 162 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
                                                                                                    Write(deviceModel.turnedOnTime);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </b><small>hrs ago</small></p>\n\n\n                        </div>\n                    </div>\n");
#nullable restore
#line 167 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"

                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\n        </div>\n");
#nullable restore
#line 171 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Shared/_DevicesList.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>

<script>
    function turnOnOffDevice(deviceID) {
        alert(deviceID);
    }


    var EditDevices = function (Id) {

        var url = ""/Devices/EditDevices/"" + Id;

        $(""#DevicesFormBody"").load(url, function () {
            $(""#DevicesForm"").modal(""show"");

        })

    }
    var DropDevices = function (Id) {

        var url = ""/Devices/DropDevices/"" + Id;

        $(""#DevicesFormBody"").load(url, function () {
            $(""#DevicesForm"").modal(""show"");

        })

    }
    var AddtDevices = function () {

        var url = ""/Devices/AddDevices"";

        $(""#DevicesFormBody"").load(url, function () {
            $(""#DevicesForm"").modal(""show"");

        })

    }
    var AddtDevices = function (roomid) {

        var url = ""/Devices/AddDevices/"" + roomid;

        $(""#DevicesFormBody"").load(url, function () {
            $(""#DevicesForm"").modal(""show"");

        })

    }
   
</script>
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
