/*

@license
dhtmlxScheduler v.5.3.5 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) XB Software Ltd.

*/
Scheduler.plugin(function(e){e.expand=function(){if(e.callEvent("onBeforeExpand",[])){var t=e._obj;do{t._position=t.style.position||"",t.style.position="static"}while((t=t.parentNode)&&t.style);t=e._obj,t.style.position="absolute",t._width=t.style.width,t._height=t.style.height,t.style.width=t.style.height="100%",t.style.top=t.style.left="0px";var a=document.body;a.scrollTop=0,a=a.parentNode,a&&(a.scrollTop=0),document.body._overflow=document.body.style.overflow||"",
document.body.style.overflow="hidden",e._maximize(),e.callEvent("onExpand",[])}},e.collapse=function(){if(e.callEvent("onBeforeCollapse",[])){var t=e._obj;do{t.style.position=t._position}while((t=t.parentNode)&&t.style);t=e._obj,t.style.width=t._width,t.style.height=t._height,document.body.style.overflow=document.body._overflow,e._maximize(),e.callEvent("onCollapse",[])}},e.attachEvent("onTemplatesReady",function(){var t=document.createElement("div");t.className="dhx_expand_icon",e.toggleIcon=t,
e._obj.appendChild(t),t.onclick=function(){e.expanded?e.collapse():e.expand()}}),e._maximize=function(){this.expanded=!this.expanded,this.toggleIcon.style.backgroundPosition="0 "+(this.expanded?"0":"18")+"px";for(var t=["left","top"],a=0;a<t.length;a++){var n=(e.xy["margin_"+t[a]],e["_prev_margin_"+t[a]]);e.xy["margin_"+t[a]]?(e["_prev_margin_"+t[a]]=e.xy["margin_"+t[a]],e.xy["margin_"+t[a]]=0):n&&(e.xy["margin_"+t[a]]=e["_prev_margin_"+t[a]],delete e["_prev_margin_"+t[a]])}e.setCurrentView()}
});