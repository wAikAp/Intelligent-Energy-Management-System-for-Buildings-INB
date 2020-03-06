/*

@license
dhtmlxScheduler v.5.3.5 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) XB Software Ltd.

*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){e.xy.scroll_width=0;var t=e.render_view_data;e.render_view_data=function(){var a=this._els.dhx_cal_data[0];a.firstChild._h_fix=!0,t.apply(e,arguments);var n=parseInt(a.style.height);a.style.height="1px",a.style.height=a.scrollHeight+"px",this._obj.style.height=this._obj.clientHeight+a.scrollHeight-n+"px"};var a=e._reset_month_scale;e._reset_month_scale=function(t,n,i,r){var o={clientHeight:100};a.apply(e,[o,n,i,r]),
t.innerHTML=o.innerHTML}})});