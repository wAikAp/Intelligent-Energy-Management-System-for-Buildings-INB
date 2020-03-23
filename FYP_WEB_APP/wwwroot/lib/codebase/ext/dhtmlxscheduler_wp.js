/*

@license
dhtmlxScheduler v.5.3.5 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) XB Software Ltd.

*/
Scheduler.plugin(function(e){e.attachEvent("onLightBox",function(){if(this._cover)try{this._cover.style.height=this.expanded?"100%":(document.body.parentNode||document.body).scrollHeight+"px"}catch(e){}}),e.form_blocks.select.set_value=function(e,t,a){void 0!==t&&""!==t||(t=(e.firstChild.options[0]||{}).value),e.firstChild.value=t||""}});