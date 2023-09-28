import{r as A,c as k,h as Y}from"./d3-color-568b2243.js";const X=r=>()=>r;function D(r,n){return function(t){return r+t*n}}function E(r,n,t){return r=Math.pow(r,t),n=Math.pow(n,t)-r,t=1/t,function(i){return Math.pow(r+i*n,t)}}function O(r,n){var t=n-r;return t?D(r,t>180||t<-180?t-360*Math.round(t/360):t):X(isNaN(r)?n:r)}function R(r){return(r=+r)==1?v:function(n,t){return t-n?E(n,t,r):X(isNaN(n)?t:n)}}function v(r,n){var t=n-r;return t?D(r,t):X(isNaN(r)?n:r)}const S=function r(n){var t=R(n);function i(e,l){var o=t((e=A(e)).r,(l=A(l)).r),f=t(e.g,l.g),p=t(e.b,l.b),c=v(e.opacity,l.opacity);return function(u){return e.r=o(u),e.g=f(u),e.b=p(u),e.opacity=c(u),e+""}}return i.gamma=r,i}(1);function V(r,n){n||(n=[]);var t=r?Math.min(n.length,r.length):0,i=n.slice(),e;return function(l){for(e=0;e<t;++e)i[e]=r[e]*(1-l)+n[e]*l;return i}}function B(r){return ArrayBuffer.isView(r)&&!(r instanceof DataView)}function C(r,n){var t=n?n.length:0,i=r?Math.min(t,r.length):0,e=new Array(i),l=new Array(t),o;for(o=0;o<i;++o)e[o]=I(r[o],n[o]);for(;o<t;++o)l[o]=n[o];return function(f){for(o=0;o<i;++o)l[o]=e[o](f);return l}}function $(r,n){var t=new Date;return r=+r,n=+n,function(i){return t.setTime(r*(1-i)+n*i),t}}function g(r,n){return r=+r,n=+n,function(t){return r*(1-t)+n*t}}function z(r,n){var t={},i={},e;(r===null||typeof r!="object")&&(r={}),(n===null||typeof n!="object")&&(n={});for(e in n)e in r?t[e]=I(r[e],n[e]):i[e]=n[e];return function(l){for(e in t)i[e]=t[e](l);return i}}var N=/[-+]?(?:\d+\.?\d*|\.?\d+)(?:[eE][-+]?\d+)?/g,x=new RegExp(N.source,"g");function G(r){return function(){return r}}function H(r){return function(n){return r(n)+""}}function K(r,n){var t=N.lastIndex=x.lastIndex=0,i,e,l,o=-1,f=[],p=[];for(r=r+"",n=n+"";(i=N.exec(r))&&(e=x.exec(n));)(l=e.index)>t&&(l=n.slice(t,l),f[o]?f[o]+=l:f[++o]=l),(i=i[0])===(e=e[0])?f[o]?f[o]+=e:f[++o]=e:(f[++o]=null,p.push({i:o,x:g(i,e)})),t=x.lastIndex;return t<n.length&&(l=n.slice(t),f[o]?f[o]+=l:f[++o]=l),f.length<2?p[0]?H(p[0].x):G(n):(n=p.length,function(c){for(var u=0,s;u<n;++u)f[(s=p[u]).i]=s.x(c);return f.join("")})}function I(r,n){var t=typeof n,i;return n==null||t==="boolean"?X(n):(t==="number"?g:t==="string"?(i=k(n))?(n=i,S):K:n instanceof k?S:n instanceof Date?$:B(n)?V:Array.isArray(n)?C:typeof n.valueOf!="function"&&typeof n.toString!="function"||isNaN(n)?z:g)(r,n)}function Q(r,n){return r=+r,n=+n,function(t){return Math.round(r*(1-t)+n*t)}}var j=180/Math.PI,d={translateX:0,translateY:0,rotate:0,skewX:0,scaleX:1,scaleY:1};function q(r,n,t,i,e,l){var o,f,p;return(o=Math.sqrt(r*r+n*n))&&(r/=o,n/=o),(p=r*t+n*i)&&(t-=r*p,i-=n*p),(f=Math.sqrt(t*t+i*i))&&(t/=f,i/=f,p/=f),r*i<n*t&&(r=-r,n=-n,p=-p,o=-o),{translateX:e,translateY:l,rotate:Math.atan2(n,r)*j,skewX:Math.atan(p)*j,scaleX:o,scaleY:f}}var M;function W(r){const n=new(typeof DOMMatrix=="function"?DOMMatrix:WebKitCSSMatrix)(r+"");return n.isIdentity?d:q(n.a,n.b,n.c,n.d,n.e,n.f)}function F(r){return r==null||(M||(M=document.createElementNS("http://www.w3.org/2000/svg","g")),M.setAttribute("transform",r),!(r=M.transform.baseVal.consolidate()))?d:(r=r.matrix,q(r.a,r.b,r.c,r.d,r.e,r.f))}function T(r,n,t,i){function e(c){return c.length?c.pop()+" ":""}function l(c,u,s,a,h,w){if(c!==s||u!==a){var m=h.push("translate(",null,n,null,t);w.push({i:m-4,x:g(c,s)},{i:m-2,x:g(u,a)})}else(s||a)&&h.push("translate("+s+n+a+t)}function o(c,u,s,a){c!==u?(c-u>180?u+=360:u-c>180&&(c+=360),a.push({i:s.push(e(s)+"rotate(",null,i)-2,x:g(c,u)})):u&&s.push(e(s)+"rotate("+u+i)}function f(c,u,s,a){c!==u?a.push({i:s.push(e(s)+"skewX(",null,i)-2,x:g(c,u)}):u&&s.push(e(s)+"skewX("+u+i)}function p(c,u,s,a,h,w){if(c!==s||u!==a){var m=h.push(e(h)+"scale(",null,",",null,")");w.push({i:m-4,x:g(c,s)},{i:m-2,x:g(u,a)})}else(s!==1||a!==1)&&h.push(e(h)+"scale("+s+","+a+")")}return function(c,u){var s=[],a=[];return c=r(c),u=r(u),l(c.translateX,c.translateY,u.translateX,u.translateY,s,a),o(c.rotate,u.rotate,s,a),f(c.skewX,u.skewX,s,a),p(c.scaleX,c.scaleY,u.scaleX,u.scaleY,s,a),c=u=null,function(h){for(var w=-1,m=a.length,y;++w<m;)s[(y=a[w]).i]=y.x(h);return s.join("")}}}var U=T(W,"px, ","px)","deg)"),Z=T(F,", ",")",")");function J(r){return function(n,t){var i=r((n=Y(n)).h,(t=Y(t)).h),e=v(n.c,t.c),l=v(n.l,t.l),o=v(n.opacity,t.opacity);return function(f){return n.h=i(f),n.c=e(f),n.l=l(f),n.opacity=o(f),n+""}}}const _=J(O);export{I as a,Q as b,S as c,K as d,Z as e,U as f,_ as g,g as i};
