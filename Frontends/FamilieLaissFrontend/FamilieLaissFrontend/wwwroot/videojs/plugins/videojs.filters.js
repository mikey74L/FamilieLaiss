/**
 * Copyright (c) 2023 The Nuevodevel Team. All rights reserved.
 * Filters plugin for video.js
 * Version 1.2.0
 */
!function(e,t){"function"==typeof define&&define.amd?define([],t.bind(this,e,e.videojs)):"undefined"!=typeof module&&module.exports?module.exports=t(e,e.videojs):t(e,e.videojs)}(window,function(e,t){e.videojs_filters={version:"1.0"};var i=!1;var s=Object.defineProperty({},"passive",{get:function(){return i=!0,!0}});e.addEventListener("testPassive",null,s),e.removeEventListener("testPassive",null,s);var r=100,a=100,l=100;const n=(s,n)=>{const o={touchBrightness:!0};try{n=t.obj.merge(o,n)}catch(e){n=t.mergeOptions(o,n)}t.dom;let d=s.el();if(!0===n.touchBrightness){var v=t.dom.createEl("div",{className:"vjs-brightness"},{tabindex:0,"aria-live":"polite",role:"slider","aria-valuetext":"50%","aria-valuenow":50,"aria-valuemin":0,"aria-valuemax":100});return v.innerHTML='<span data-id="brightness" class="vjs-icon-placeholder"></span><div class="vjs-brightness-bar"><div class="bar"></div><div class="bar-level"></div></div>',d.appendChild(v),v.setAttribute("aria-label","Video brightness"),v.addEventListener("touchstart",function(i){i.stopImmediatePropagation(),t.dom.addClass(d,"vjs-touch-active"),f(i),e.addEventListener("touchmove",u,{passive:!1}),v.addEventListener("touchend",c)},!!i&&{passive:!0}),this}function u(e){e.preventDefault(),e.stopImmediatePropagation(),clearInterval(s.touchtimer),t.dom.addClass(d,"vjs-touch-acitve"),f(e)}function c(i){i.preventDefault(),s.touchtimer=setTimeout(function(){t.dom.removeClass(d,"vjs-touch-active")},1e3),e.removeEventListener("touchmove",u),v.removeEventListener("touchend",c)}function f(t){let i=null;if("mousemove"===t.type||"mousedown"===t.type?i=t.pageY||null:"touchmove"!==t.type&&"touchstart"!==t.type||(i=t.touches[0].pageY||null),null===i)return;var s=v.offsetHeight,n=i-(v.getBoundingClientRect().top+e.scrollY);n>s&&(n=s),n<0&&(n=0);var o=parseInt(100-n/s*100,10);o<0&&(o=0),o>100&&(o=100),v.querySelector(".bar-level").style.height=o+"%",v.setAttribute("aria-valuenow",o),v.setAttribute("aria-valuetext",o+"%");let u=d.querySelector(".vjs-filters");if(u){let e=u.querySelector(".vjs-filter-brightness");e&&(e.querySelector(".vjs-filter-level").style.width=o+"%",e.setAttribute("aria-valuenow",o),e.setAttribute("aria-valuetext",o+"%"))}100!==(r=100+2*(o-50))&&(filterss="brightness("+r+"%)"),100!==a&&(""!==filterss&&(filterss+=" "),filterss+="saturate("+a+"%)"),100!==l&&(""!==filterss&&(filterss+=" "),filterss+="contrast("+l+"%)"),d.querySelector(".vjs-tech").style.webkitFilter=filterss,d.querySelector(".vjs-tech").style.filter=filterss}},o=function(s){this.on("nuevoReady",function(){}),this.on("menusReady",function(){((s,r)=>{const a={filtersMenu:!0};let l=100,n=100,o=100,d=s.el();try{r=t.obj.merge(a,r)}catch(e){r=t.mergeOptions(a,r)}if(r.filtersMenu){let r=t.dom.createEl("li",{className:"vjs-settings-item vjs-filters-button"},{tabIndex:0,"aria-label":s.localize("Open filters container"),role:"menuitem","aria-disabled":"false"});r.innerHTML=s.localize("Filters")+'<span><span class="vjs-filters-icon vjs-icon-placeholder" data-id="filters" aria-hidden="true"></span></span>';let a=d.querySelector(".vjs-settings-list");a&&(a.children.length>0?a.insertBefore(r,a.firstChild.nextSibling):(a.appendChild(r),t.dom.removeClass(d.querySelector(".vjs-cog-menu-button"),"vjs-hidden")));let b=t.dom.createEl("div",{className:"vjs-filters vjs-hidden"},{"aria-label":s.localize("Video filters"),"aria-hidden":!0});b.style.zIndex=7;let y=t.dom.createEl("div",{className:"vjs-filter-body vjs-filter-brightness"},{tabindex:0,"aria-live":"polite",role:"slider","aria-valuetext":"50%","aria-valuenow":50,"aria-valuemin":0,"aria-valuemax":100});y.setAttribute("aria-label","video brigtness");let j=t.dom.createEl("div",{className:"vjs-filter-body vjs-filter-saturation"},{tabindex:0,"aria-live":"polite",role:"slider","aria-valuetext":"50%","aria-valuenow":50,"aria-valuemin":0,"aria-valuemax":100});j.setAttribute("aria-label","video saturation");let g=t.dom.createEl("div",{className:"vjs-filter-body vjs-filter-contrast"},{tabindex:0,"aria-live":"polite",role:"slider","aria-valuetext":"50%","aria-valuenow":50,"aria-valuemin":0,"aria-valuemax":100});g.setAttribute("aria-label","video contrast");let E='<div class="vjs-filter-bar"><div class="vjs-filter-level"></div><div class="filter-tip"></div></div>';y.innerHTML='<span aria-hidden="true" data-id="brightness" class="vjs-icon-placeholder"></span>'+E,j.innerHTML='<span aria-hidden="true" data-id="saturation" class="vjs-icon-placeholder"></span>'+E,g.innerHTML='<span aria-hidden="true" data-id="contrast" class="vjs-icon-placeholder"></span>'+E;let L=t.dom.createEl("button",{className:"filter-btn filter-close"},{tabindex:0,"aria-disabled":!1,"aria-label":s.localize("Close filters")},s.localize("Close")),w=t.dom.createEl("button",{className:"filter-btn filter-reset"},{tabindex:0,"aria-disabled":!1,"aria-label":s.localize("Reset filters")},s.localize("Reset"));b.appendChild(y),b.appendChild(j),b.appendChild(g),b.appendChild(w),b.appendChild(L),d.appendChild(b),b.addEventListener("keydown",function(e){let t=e.which;if(27===t&&L.click(),37===t||39===t){var i=document.activeElement;if(i===y||i===j||i===g){let e=i.querySelector(".vjs-filter-level"),d=parseInt(e.style.width);e.style.width||(d=50),39===t&&(d+=5)>99&&(d=100),37===t&&(d-=5)<0&&(d=0),e.style.width=d+"%";var r=100+2*(d-50);i===y&&(l=r),i===j&&(n=r),i===g&&(o=r);var a="";100!==l&&(a+="brightness("+l+"%)"),100!==n&&(""!==a&&(a+=" "),a+="saturate("+n+"%)"),100!==o&&(""!==a&&(a+=" "),a+="contrast("+o+"%)"),i.querySelector(".filter-tip").innerText=d,i.setAttribute("aria-valuetext",d+"%"),i.setAttribute("aria-valuenow",d);let v=s.el_.querySelector(".vjs-tech");v.style.webkitFilter=a,v.style.filter=a}}if(9===t){let t=document.activeElement;L==t?(y.focus(),e.preventDefault()):t!==y&&t!==j&&t!==g&&t!==w&&e.preventDefault()}}),r.onkeydown=function(e){setTimeout(function(){y.focus()},150)};var v=function(i){let s=q,r=s.querySelector(".vjs-filter-bar"),a=null;if("mousemove"===i.type||"mousedown"===i.type?a=i.clientX||null:"touchmove"!==i.type&&"touchstart"!==i.type||(a=i.touches[0].pageX||null),null===a)return;a=parseInt(a,10);var v=r.getBoundingClientRect(),u=r.offsetWidth,c=a-(v.left-e.scrollX);c>u&&(c=u),c<0&&(c=0);var f=parseInt(c/u*100,10);f<0&&(f=0),f>100&&(f=100),s.querySelector(".vjs-filter-level").style.width=f+"%";let h=s.querySelector(".filter-tip");t.dom.addClass(h,"tip-show"),h.style.left=u*(f/100)-h.offsetWidth/2+"px";let p=d.querySelector(".vjs-brightness-bar");p&&(p.querySelector(".bar-level").style.height=f+"%"),vv=2*(f-50);var m=2*(f-50);m<-100&&(m=-100),m>100&&(m=100);var E=100+2*(f-50);h.innerText=f,s.setAttribute("aria-valuetext",f+"%"),s.setAttribute("aria-valuenow",f),q===y&&(l=E),q===j&&(n=E),q===g&&(o=E);var L="";100!==l&&(L="brightness("+l+"%)"),100!==n&&(""!==L&&(L+=" "),L+="saturate("+n+"%)"),100!==o&&(""!==L&&(L+=" "),L+="contrast("+o+"%)");let w=d.querySelector(".vjs-tech");w.style.webkitFilter=b,w.style.filter=L},u=function(e){e.preventDefault(),e.stopPropagation();let t=s.el_.querySelector(".vjs-filters");t.setAttribute("aria-hidden","true"),t.classList.add("vjs-hidden"),s.$(".vjs-tech").style.removeProperty("pointer-events"),s.el_.classList.remove("vjs-filters-on"),setTimeout(function(){s.el_.focus(),e.preventDefault(),e.stopPropagation()},100),s.userActive(!1)},c=function(e){e.preventDefault(),e.stopPropagation(),t.dom.removeClass(b,"vjs-hidden"),b.setAttribute("aria-hidden","false"),s.trigger("filters"),s.$(".vjs-tech").style.setProperty("pointer-events","none"),t.dom.addClass(d,"vjs-filters-on");let i=d.querySelector(".vjs-sharing-overlay"),r=d.querySelector(".vjs-grid");i&&t.dom.addClass(i,"vjs-hidden"),r&&t.dom.addClass(r,"vjs-hidden");let a=d.querySelector(".vjs-menu-settings");setTimeout(function(){t.dom.removeClass(a,"vjs-lock-showing"),t.dom.addClass(a,"vjs-hidden"),b.focus()},500)},f=function(e){e.preventDefault(),e.stopPropagation(),l=100,o=100,n=100,d.querySelector(".vjs-tech").removeAttribute("style"),y.querySelector(".vjs-filter-level").style.width="50%",j.querySelector(".vjs-filter-level").style.width="50%",g.querySelector(".vjs-filter-level").style.width="50%"},h=function(t){t.stopPropagation();let i=t.target;q=i,v(t),e.addEventListener("mousemove",p,!1),e.addEventListener("mouseup",m,!1),e.addEventListener("touchmove",p,{passive:!1}),q.addEventListener("touchend",m)},p=function(e){e.preventDefault(),v(e)},m=function(i){i.preventDefault(),t.dom.removeClass(d,"vjs-touch-active"),t.dom.removeClass(q.querySelector(".filter-tip"),"tip-show"),e.removeEventListener("mousemove",p),e.removeEventListener("mouseup",m),e.removeEventListener("touchmove",p),q.removeEventListener("touchend",m)};w.addEventListener("click",f,!1),w.addEventListener("touchend",f,!1),L.addEventListener("click",u,!1),L.addEventListener("touchend",u,!1);let S=d.querySelector(".vjs-filters-button");S.addEventListener("click",c,!1),S.addEventListener("touchstart",c,!!i&&{passive:!1});let q=null;y.addEventListener("mousedown",h,!1),y.addEventListener("touchstart",h,!!i&&{passive:!0}),j.addEventListener("mousedown",h,!1),j.addEventListener("touchstart",h,!!i&&{passive:!0}),g.addEventListener("mousedown",h,!1),g.addEventListener("touchstart",h,!!i&&{passive:!0})}})(this,s),n(this,s)})};if(void 0!==e){t.registerPlugin||t.plugin;t.registerPlugin("filters",o)}});