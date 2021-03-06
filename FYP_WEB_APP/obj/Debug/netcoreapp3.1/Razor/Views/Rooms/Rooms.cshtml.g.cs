#pragma checksum "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "46f2510b0f4e7a0b42cfeabb1a1608a5ee368630"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Rooms_Rooms), @"mvc.1.0.view", @"/Views/Rooms/Rooms.cshtml")]
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
#nullable restore
#line 2 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
using FYP_APP.Models.MongoModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"46f2510b0f4e7a0b42cfeabb1a1608a5ee368630", @"/Views/Rooms/Rooms.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1103289fd338f5238efba9485e51b05468496a5f", @"/Views/_ViewImports.cshtml")]
    public class Views_Rooms_Rooms : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Rooms/searchRoom"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/edit32_white.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("width", new global::Microsoft.AspNetCore.Html.HtmlString("24px"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/trash96.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Rooms/dropRoom"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
            WriteLiteral("\n");
#nullable restore
#line 4 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
  
    ViewData["Title"] = "Rooms";
    Layout = "_Layout";

    var roomsDatalist = (List<MongoRoomModel>)ViewData["roomsDatalist"];//get room list 
  

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "46f2510b0f4e7a0b42cfeabb1a1608a5ee3686306445", async() => {
                WriteLiteral("\n\n        <title>");
#nullable restore
#line 14 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
          Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</title>
        <script>
        $(document).ready(function () {
            //navbar ui
            $('.navbar-nav .nav-link').removeClass('active');
            $(""#rooms"").addClass('active');


            $("".clickable"").click(function () {
                var roomID = $(this).data(""href"");
                var url = '");
#nullable restore
#line 24 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                      Write(Url.Action("RoomDetail", "RoomDetail"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\';\n                url += \"?roomID=\" + escape(roomID);\n                window.location.href = url;\n\n            });\n        });\n        </script>\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n<!--body-->\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "46f2510b0f4e7a0b42cfeabb1a1608a5ee3686308491", async() => {
                WriteLiteral("\n        ");
#nullable restore
#line 35 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
   Write(Html.Partial("_addRoom"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
        <div class=""container-fluid p-4 "">
            <div class=""row mb-2"">
                <h1 class=""col-sm-9""><b>Room List</b></h1>
                <div class=""d-flex col-sm-3 btn-sm justify-content-end"">
                    <button class=""btn btn-primary"" type=""button"" data-toggle=""modal"" data-target=""#addRoomModal"">+Add Room</button>
                </div>
            </div>

            <!--searchbox-->
            <div class=""card"">
                <div class=""row"">
                    <div class=""col-sm-12 "">
                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "46f2510b0f4e7a0b42cfeabb1a1608a5ee3686309595", async() => {
                    WriteLiteral(@"
                            <div class=""input-group"">

                                <span class=""input-group-text"" style=""height:39px"">🔍 </span>
                                <input class=""form-control "" type=""text"" placeholder=""Search Room ID"" name=""searchRoomID""");
                    BeginWriteAttribute("value", " value=\"", 1685, "\"", 1716, 1);
#nullable restore
#line 52 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
WriteAttributeValue("", 1693, ViewData["searchedID"], 1693, 23, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(">\n                                <button class=\"btn-sm btn-outline-secondary float-right m-2 \" type=\"submit\">search</button>\n                            </div>\n                            \n                        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                    </div>

                </div>
                <!--table-->
                <div class=""overflow-auto"">
                    <table class=""table table-striped table-hover"" id=""RoomsTable"">
                        <thead>
                            <tr>
                                <th class=""th-sm"" scope=""col"">Room name</th>
                                <th scope=""col"">Room type</th>
                                <th scope=""col"">Room ID</th>
                                <th scope=""col"">Floor</th>
                                <th scope=""col"">Today Usage</th>
                                <th scope=""col"">Humidity</th>
                                <th scope=""col"">Luminosity</th>
                                <th scope=""col"">Temperature</th>
                                <th scope=""col"">Description</th>
                                <th scope=""col""></th>
                            </tr>
                        </thead>

                        <tbody>
               ");
                WriteLiteral("             <!--load data from ViewData:List<MongoRoomModel> -->\n");
#nullable restore
#line 80 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                             foreach (MongoRoomModel roomModel in roomsDatalist)
                            {
                                

#line default
#line hidden
#nullable disable
#nullable restore
#line 82 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                           Write(Html.Partial("_editRoom", roomModel));

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <tr style=\"cursor: pointer;\">\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 84 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 84 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.roomName);

#line default
#line hidden
#nullable disable
                WriteLiteral(" </td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 85 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 85 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.type);

#line default
#line hidden
#nullable disable
                WriteLiteral(" </td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 86 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 86 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 87 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 87 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.floor);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n");
#nullable restore
#line 88 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                      
                                        double power = Convert.ToDouble(roomModel.power.ToString("0.00"));
                                    

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 91 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 91 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(power);

#line default
#line hidden
#nullable disable
                WriteLiteral(" kW</td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 92 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 92 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.hum);

#line default
#line hidden
#nullable disable
                WriteLiteral(" %</td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 93 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 93 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.lig);

#line default
#line hidden
#nullable disable
                WriteLiteral(" lm</td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 94 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 94 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.temp);

#line default
#line hidden
#nullable disable
                WriteLiteral(" &#8451;</td>\n                                    <td class=\'clickable\' data-href=\'");
#nullable restore
#line 95 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                Write(roomModel.roomId);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'>");
#nullable restore
#line 95 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                   Write(roomModel.desc);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n");
#nullable restore
#line 96 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                      
                                        var modalViewID = "#editRoomModal" + roomModel.roomId;//for modal view
                                        var formID = "form" + roomModel.roomId;//formid for drop room
                                    

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    <td>\n                                        <button class=\"btn btn-primary\" type=\"button\" data-toggle=\"modal\" data-target=\"");
#nullable restore
#line 101 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
                                                                                                                  Write(modalViewID);

#line default
#line hidden
#nullable disable
                WriteLiteral("\">");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "46f2510b0f4e7a0b42cfeabb1a1608a5ee36863022440", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(" Edit</button>\n                                        <button class=\"btn btn-danger\" type=\"button\"");
                BeginWriteAttribute("onclick", " onclick=\"", 5065, "\"", 5104, 3);
                WriteAttributeValue("", 5075, "dropRoom(\'", 5075, 10, true);
#nullable restore
#line 102 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
WriteAttributeValue("", 5085, roomModel.roomId, 5085, 17, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 5102, "\')", 5102, 2, true);
                EndWriteAttribute();
                WriteLiteral(">");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "46f2510b0f4e7a0b42cfeabb1a1608a5ee36863024269", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(" Drop</button>\n                                    </td>\n\n                                </tr>\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "46f2510b0f4e7a0b42cfeabb1a1608a5ee36863025557", async() => {
                    WriteLiteral("\n                                    <input type=\"hidden\"");
                    BeginWriteAttribute("value", " value=\"", 5397, "\"", 5422, 1);
#nullable restore
#line 107 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
WriteAttributeValue("", 5405, roomModel.roomId, 5405, 17, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(" name=\"roomID\" />\n                                ");
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "id", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 106 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"
AddHtmlAttributeValue("", 5291, formID, 5291, 7, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\n");
#nullable restore
#line 109 "/Users/shingwaichan/Desktop/INB/Intelligent-Energy-Management-System-for-Buildings-INB/FYP_WEB_APP/Views/Rooms/Rooms.cshtml"


                            }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                        </tbody>
                        </table>
                </div>
            </div>
            <script>

                function dropRoom(roomid) {
                    //alert(""roomid = "" + roomid);
                    var r = confirm(""Are you sure to drop room "" + roomid+""?"");
                    if (r == true) {
                        var formID = ""#form"" + roomid;
                        $(formID).submit();
                    } 
                    
                }

            </script>


        </div>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n\n");
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
