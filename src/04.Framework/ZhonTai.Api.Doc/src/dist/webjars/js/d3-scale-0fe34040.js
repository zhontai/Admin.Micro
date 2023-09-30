import{I as M}from"./internmap-7949acc8.js";import{b as P,t as T,a as $,c as j}from"./d3-array-f46ca9a0.js";import{f as z,p as B,a as L,b as W,c as q,d as C}from"./d3-format-ffdb8652.js";import{i as E,a as G,b as J}from"./d3-interpolate-23596b47.js";import{t as K}from"./d3-time-format-8d6e99ce.js";import{s as O,t as Q,a as U,b as V,c as X,d as Z,e as _,f as nn,g as rn}from"./d3-time-e3072704.js";function y(n,t){switch(arguments.length){case 0:break;case 1:this.range(n);break;default:this.range(t).domain(n);break}return this}const w=Symbol("implicit");function tn(){var n=new M,t=[],u=[],r=w;function a(i){let e=n.get(i);if(e===void 0){if(r!==w)return r;n.set(i,e=t.push(i)-1)}return u[e%u.length]}return a.domain=function(i){if(!arguments.length)return t.slice();t=[],n=new M;for(const e of i)n.has(e)||n.set(e,t.push(e)-1);return a},a.range=function(i){return arguments.length?(u=Array.from(i),a):u.slice()},a.unknown=function(i){return arguments.length?(r=i,a):r},a.copy=function(){return tn(t,u).unknown(r)},y.apply(a,arguments),a}function en(n){return function(){return n}}function un(n){return+n}var b=[0,1];function g(n){return n}function k(n,t){return(t-=n=+n)?function(u){return(u-n)/t}:en(isNaN(t)?NaN:.5)}function an(n,t){var u;return n>t&&(u=n,n=t,t=u),function(r){return Math.max(n,Math.min(t,r))}}function on(n,t,u){var r=n[0],a=n[1],i=t[0],e=t[1];return a<r?(r=k(a,r),i=u(e,i)):(r=k(r,a),i=u(i,e)),function(f){return i(r(f))}}function cn(n,t,u){var r=Math.min(n.length,t.length)-1,a=new Array(r),i=new Array(r),e=-1;for(n[r]<n[0]&&(n=n.slice().reverse(),t=t.slice().reverse());++e<r;)a[e]=k(n[e],n[e+1]),i[e]=u(t[e],t[e+1]);return function(f){var m=P(n,f,1,r)-1;return i[m](a[m](f))}}function N(n,t){return t.domain(n.domain()).range(n.range()).interpolate(n.interpolate()).clamp(n.clamp()).unknown(n.unknown())}function sn(){var n=b,t=b,u=G,r,a,i,e=g,f,m,o;function l(){var s=Math.min(n.length,t.length);return e!==g&&(e=an(n[0],n[s-1])),f=s>2?cn:on,m=o=null,p}function p(s){return s==null||isNaN(s=+s)?i:(m||(m=f(n.map(r),t,u)))(r(e(s)))}return p.invert=function(s){return e(a((o||(o=f(t,n.map(r),E)))(s)))},p.domain=function(s){return arguments.length?(n=Array.from(s,un),l()):n.slice()},p.range=function(s){return arguments.length?(t=Array.from(s),l()):t.slice()},p.rangeRound=function(s){return t=Array.from(s),u=J,l()},p.clamp=function(s){return arguments.length?(e=s?!0:g,l()):e!==g},p.interpolate=function(s){return arguments.length?(u=s,l()):u},p.unknown=function(s){return arguments.length?(i=s,p):i},function(s,v){return r=s,a=v,l()}}function D(){return sn()(g,g)}function fn(n,t,u,r){var a=T(n,t,u),i;switch(r=z(r??",f"),r.type){case"s":{var e=Math.max(Math.abs(n),Math.abs(t));return r.precision==null&&!isNaN(i=W(a,e))&&(r.precision=i),q(r,e)}case"":case"e":case"g":case"p":case"r":{r.precision==null&&!isNaN(i=L(a,Math.max(Math.abs(n),Math.abs(t))))&&(r.precision=i-(r.type==="e"));break}case"f":case"%":{r.precision==null&&!isNaN(i=B(a))&&(r.precision=i-(r.type==="%")*2);break}}return C(r)}function ln(n){var t=n.domain;return n.ticks=function(u){var r=t();return $(r[0],r[r.length-1],u??10)},n.tickFormat=function(u,r){var a=t();return fn(a[0],a[a.length-1],u??10,r)},n.nice=function(u){u==null&&(u=10);var r=t(),a=0,i=r.length-1,e=r[a],f=r[i],m,o,l=10;for(f<e&&(o=e,e=f,f=o,o=a,a=i,i=o);l-- >0;){if(o=j(e,f,u),o===m)return r[a]=e,r[i]=f,t(r);if(o>0)e=Math.floor(e/o)*o,f=Math.ceil(f/o)*o;else if(o<0)e=Math.ceil(e*o)/o,f=Math.floor(f*o)/o;else break;m=o}return n},n}function mn(){var n=D();return n.copy=function(){return N(n,mn())},y.apply(n,arguments),ln(n)}function pn(n,t){n=n.slice();var u=0,r=n.length-1,a=n[u],i=n[r],e;return i<a&&(e=u,u=r,r=e,e=a,a=i,i=e),n[u]=t.floor(a),n[r]=t.ceil(i),n}function hn(n){return new Date(n)}function gn(n){return n instanceof Date?+n:+new Date(+n)}function d(n,t,u,r,a,i,e,f,m,o){var l=D(),p=l.invert,s=l.domain,v=o(".%L"),x=o(":%S"),A=o("%I:%M"),F=o("%I %p"),I=o("%a %d"),S=o("%b %d"),R=o("%B"),Y=o("%Y");function H(c){return(m(c)<c?v:f(c)<c?x:e(c)<c?A:i(c)<c?F:r(c)<c?a(c)<c?I:S:u(c)<c?R:Y)(c)}return l.invert=function(c){return new Date(p(c))},l.domain=function(c){return arguments.length?s(Array.from(c,gn)):s().map(hn)},l.ticks=function(c){var h=s();return n(h[0],h[h.length-1],c??10)},l.tickFormat=function(c,h){return h==null?H:o(h)},l.nice=function(c){var h=s();return(!c||typeof c.range!="function")&&(c=t(h[0],h[h.length-1],c??10)),c?s(pn(h,c)):l},l.copy=function(){return N(l,d(n,t,u,r,a,i,e,f,m,o))},l}function Nn(){return y.apply(d(rn,nn,_,Z,X,V,U,Q,O,K).domain([new Date(2e3,0,1),new Date(2e3,0,2)]),arguments)}export{mn as l,tn as o,Nn as t};