/*

@license
dhtmlxScheduler v.5.3.5 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) XB Software Ltd.

*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){for(var t=document.body.getElementsByTagName("DIV"),a=0;a<t.length;a++){var i=t[a].className||"";if(i=i.split(":"),2==i.length&&"template"==i[0]){var n='return "'+(t[a].innerHTML||"").replace(/\"/g,'\\"').replace(/[\n\r]+/g,"")+'";';n=unescape(n).replace(/\{event\.([a-z]+)\}/g,function(e,t){return'"+ev.'+t+'+"'}),e.templates[i[1]]=Function("start","end","ev",n),t[a].style.display="none"}}})});