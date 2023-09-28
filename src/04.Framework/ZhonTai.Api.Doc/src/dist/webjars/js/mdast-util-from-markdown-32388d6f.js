import{p as Ce,a as Re,b as He}from"./micromark-24282579.js";import{d as Le}from"./micromark-util-decode-numeric-character-reference-bd25dc10.js";import{d as ze}from"./micromark-util-decode-string-02bf9559.js";import{n as F}from"./micromark-util-normalize-identifier-dfdf0387.js";import{d as De}from"./decode-named-character-reference-29ded5ae.js";import{s as T}from"./unist-util-stringify-position-7b16d8bf.js";import{t as Oe}from"./mdast-util-to-string-bfd41500.js";const Q={}.hasOwnProperty,Ae=function(d,r,a){return typeof r!="string"&&(a=r,r=void 0),Pe(a)(Ce(Re(a).document().write(He()(d,r,!0))))};function Pe(d){const r={transforms:[],canContainEols:["emphasis","fragment","heading","paragraph","strong"],enter:{autolink:c(P),autolinkProtocol:k,autolinkEmail:k,atxHeading:c(z),blockQuote:c(xe),characterEscape:k,characterReference:k,codeFenced:c(L),codeFencedFenceInfo:f,codeFencedFenceMeta:f,codeIndented:c(L,f),codeText:c(ye,f),codeTextData:k,data:k,codeFlowValue:k,definition:c(Se),definitionDestinationString:f,definitionLabelString:f,definitionTitleString:f,emphasis:c(be),hardBreakEscape:c(D),hardBreakTrailing:c(D),htmlFlow:c(O,f),htmlFlowData:k,htmlText:c(O,f),htmlTextData:k,image:c(we),label:f,link:c(P),listItem:c(Ie),listItemValue:j,listOrdered:c(M,_),listUnordered:c(M),paragraph:c(Te),reference:fe,referenceString:f,resourceDestinationString:f,resourceTitleString:f,setextHeading:c(z),strong:c(Ee),thematicBreak:c(Be)},exit:{atxHeading:l(),atxHeadingSequence:Z,autolink:l(),autolinkEmail:me,autolinkProtocol:ke,blockQuote:l(),characterEscapeValue:m,characterReferenceMarkerHexadecimal:H,characterReferenceMarkerNumeric:H,characterReferenceValue:ge,codeFenced:l(G),codeFencedFence:$,codeFencedFenceInfo:A,codeFencedFenceMeta:W,codeFlowValue:m,codeIndented:l(J),codeText:l(se),codeTextData:m,data:m,definition:l(),definitionDestinationString:Y,definitionLabelString:K,definitionTitleString:X,emphasis:l(),hardBreakEscape:l(R),hardBreakTrailing:l(R),htmlFlow:l(ie),htmlFlowData:m,htmlText:l(re),htmlTextData:m,image:l(ce),label:oe,labelText:le,lineEnding:ne,link:l(ae),listItem:l(),listOrdered:l(),listUnordered:l(),paragraph:l(),referenceString:pe,resourceDestinationString:de,resourceTitleString:he,resource:ue,setextHeading:l(te),setextHeadingLineSequence:ee,setextHeadingText:v,strong:l(),thematicBreak:l()}};q(r,(d||{}).mdastExtensions||[]);const a={};return u;function u(e){let t={type:"root",children:[]};const n={stack:[t],tokenStack:[],config:r,enter:B,exit:C,buffer:f,resume:U,setData:h,getData:g},i=[];let s=-1;for(;++s<e.length;)if(e[s][1].type==="listOrdered"||e[s][1].type==="listUnordered")if(e[s][0]==="enter")i.push(s);else{const p=i.pop();s=N(e,p,s)}for(s=-1;++s<e.length;){const p=r[e[s][0]];Q.call(p,e[s][1].type)&&p[e[s][1].type].call(Object.assign({sliceSerialize:e[s][2].sliceSerialize},n),e[s][1])}if(n.tokenStack.length>0){const p=n.tokenStack[n.tokenStack.length-1];(p[1]||V).call(n,void 0,p[0])}for(t.position={start:b(e.length>0?e[0][1].start:{line:1,column:1,offset:0}),end:b(e.length>0?e[e.length-2][1].end:{line:1,column:1,offset:0})},s=-1;++s<r.transforms.length;)t=r.transforms[s](t)||t;return t}function N(e,t,n){let i=t-1,s=-1,p=!1,S,x,w,I;for(;++i<=n;){const o=e[i];if(o[1].type==="listUnordered"||o[1].type==="listOrdered"||o[1].type==="blockQuote"?(o[0]==="enter"?s++:s--,I=void 0):o[1].type==="lineEndingBlank"?o[0]==="enter"&&(S&&!I&&!s&&!w&&(w=i),I=void 0):o[1].type==="linePrefix"||o[1].type==="listItemValue"||o[1].type==="listItemMarker"||o[1].type==="listItemPrefix"||o[1].type==="listItemPrefixWhitespace"||(I=void 0),!s&&o[0]==="enter"&&o[1].type==="listItemPrefix"||s===-1&&o[0]==="exit"&&(o[1].type==="listUnordered"||o[1].type==="listOrdered")){if(S){let E=i;for(x=void 0;E--;){const y=e[E];if(y[1].type==="lineEnding"||y[1].type==="lineEndingBlank"){if(y[0]==="exit")continue;x&&(e[x][1].type="lineEndingBlank",p=!0),y[1].type="lineEnding",x=E}else if(!(y[1].type==="linePrefix"||y[1].type==="blockQuotePrefix"||y[1].type==="blockQuotePrefixWhitespace"||y[1].type==="blockQuoteMarker"||y[1].type==="listItemIndent"))break}w&&(!x||w<x)&&(S._spread=!0),S.end=Object.assign({},x?e[x][1].start:o[1].end),e.splice(x||i,0,["exit",S,o[2]]),i++,n++}o[1].type==="listItemPrefix"&&(S={type:"listItem",_spread:!1,start:Object.assign({},o[1].start),end:void 0},e.splice(i,0,["enter",S,o[2]]),i++,n++,w=void 0,I=!0)}}return e[t][1]._spread=p,n}function h(e,t){a[e]=t}function g(e){return a[e]}function c(e,t){return n;function n(i){B.call(this,e(i),i),t&&t.call(this,i)}}function f(){this.stack.push({type:"fragment",children:[]})}function B(e,t,n){return this.stack[this.stack.length-1].children.push(e),this.stack.push(e),this.tokenStack.push([t,n]),e.position={start:b(t.start)},e}function l(e){return t;function t(n){e&&e.call(this,n),C.call(this,n)}}function C(e,t){const n=this.stack.pop(),i=this.tokenStack.pop();if(i)i[0].type!==e.type&&(t?t.call(this,e,i[0]):(i[1]||V).call(this,e,i[0]));else throw new Error("Cannot close `"+e.type+"` ("+T({start:e.start,end:e.end})+"): it’s not open");return n.position.end=b(e.end),n}function U(){return Oe(this.stack.pop())}function _(){h("expectingFirstListItemValue",!0)}function j(e){if(g("expectingFirstListItemValue")){const t=this.stack[this.stack.length-2];t.start=Number.parseInt(this.sliceSerialize(e),10),h("expectingFirstListItemValue")}}function A(){const e=this.resume(),t=this.stack[this.stack.length-1];t.lang=e}function W(){const e=this.resume(),t=this.stack[this.stack.length-1];t.meta=e}function $(){g("flowCodeInside")||(this.buffer(),h("flowCodeInside",!0))}function G(){const e=this.resume(),t=this.stack[this.stack.length-1];t.value=e.replace(/^(\r?\n|\r)|(\r?\n|\r)$/g,""),h("flowCodeInside")}function J(){const e=this.resume(),t=this.stack[this.stack.length-1];t.value=e.replace(/(\r?\n|\r)$/g,"")}function K(e){const t=this.resume(),n=this.stack[this.stack.length-1];n.label=t,n.identifier=F(this.sliceSerialize(e)).toLowerCase()}function X(){const e=this.resume(),t=this.stack[this.stack.length-1];t.title=e}function Y(){const e=this.resume(),t=this.stack[this.stack.length-1];t.url=e}function Z(e){const t=this.stack[this.stack.length-1];if(!t.depth){const n=this.sliceSerialize(e).length;t.depth=n}}function v(){h("setextHeadingSlurpLineEnding",!0)}function ee(e){const t=this.stack[this.stack.length-1];t.depth=this.sliceSerialize(e).charCodeAt(0)===61?1:2}function te(){h("setextHeadingSlurpLineEnding")}function k(e){const t=this.stack[this.stack.length-1];let n=t.children[t.children.length-1];(!n||n.type!=="text")&&(n=Fe(),n.position={start:b(e.start)},t.children.push(n)),this.stack.push(n)}function m(e){const t=this.stack.pop();t.value+=this.sliceSerialize(e),t.position.end=b(e.end)}function ne(e){const t=this.stack[this.stack.length-1];if(g("atHardBreak")){const n=t.children[t.children.length-1];n.position.end=b(e.end),h("atHardBreak");return}!g("setextHeadingSlurpLineEnding")&&r.canContainEols.includes(t.type)&&(k.call(this,e),m.call(this,e))}function R(){h("atHardBreak",!0)}function ie(){const e=this.resume(),t=this.stack[this.stack.length-1];t.value=e}function re(){const e=this.resume(),t=this.stack[this.stack.length-1];t.value=e}function se(){const e=this.resume(),t=this.stack[this.stack.length-1];t.value=e}function ae(){const e=this.stack[this.stack.length-1];if(g("inReference")){const t=g("referenceType")||"shortcut";e.type+="Reference",e.referenceType=t,delete e.url,delete e.title}else delete e.identifier,delete e.label;h("referenceType")}function ce(){const e=this.stack[this.stack.length-1];if(g("inReference")){const t=g("referenceType")||"shortcut";e.type+="Reference",e.referenceType=t,delete e.url,delete e.title}else delete e.identifier,delete e.label;h("referenceType")}function le(e){const t=this.sliceSerialize(e),n=this.stack[this.stack.length-2];n.label=ze(t),n.identifier=F(t).toLowerCase()}function oe(){const e=this.stack[this.stack.length-1],t=this.resume(),n=this.stack[this.stack.length-1];if(h("inReference",!0),n.type==="link"){const i=e.children;n.children=i}else n.alt=t}function de(){const e=this.resume(),t=this.stack[this.stack.length-1];t.url=e}function he(){const e=this.resume(),t=this.stack[this.stack.length-1];t.title=e}function ue(){h("inReference")}function fe(){h("referenceType","collapsed")}function pe(e){const t=this.resume(),n=this.stack[this.stack.length-1];n.label=t,n.identifier=F(this.sliceSerialize(e)).toLowerCase(),h("referenceType","full")}function H(e){h("characterReferenceType",e.type)}function ge(e){const t=this.sliceSerialize(e),n=g("characterReferenceType");let i;n?(i=Le(t,n==="characterReferenceMarkerNumeric"?10:16),h("characterReferenceType")):i=De(t);const s=this.stack.pop();s.value+=i,s.position.end=b(e.end)}function ke(e){m.call(this,e);const t=this.stack[this.stack.length-1];t.url=this.sliceSerialize(e)}function me(e){m.call(this,e);const t=this.stack[this.stack.length-1];t.url="mailto:"+this.sliceSerialize(e)}function xe(){return{type:"blockquote",children:[]}}function L(){return{type:"code",lang:null,meta:null,value:""}}function ye(){return{type:"inlineCode",value:""}}function Se(){return{type:"definition",identifier:"",label:null,title:null,url:""}}function be(){return{type:"emphasis",children:[]}}function z(){return{type:"heading",depth:void 0,children:[]}}function D(){return{type:"break"}}function O(){return{type:"html",value:""}}function we(){return{type:"image",title:null,url:"",alt:null}}function P(){return{type:"link",title:null,url:"",children:[]}}function M(e){return{type:"list",ordered:e.type==="listOrdered",start:null,spread:e._spread,children:[]}}function Ie(e){return{type:"listItem",spread:e._spread,checked:null,children:[]}}function Te(){return{type:"paragraph",children:[]}}function Ee(){return{type:"strong",children:[]}}function Fe(){return{type:"text",value:""}}function Be(){return{type:"thematicBreak"}}}function b(d){return{line:d.line,column:d.column,offset:d.offset}}function q(d,r){let a=-1;for(;++a<r.length;){const u=r[a];Array.isArray(u)?q(d,u):Me(d,u)}}function Me(d,r){let a;for(a in r)if(Q.call(r,a)){if(a==="canContainEols"){const u=r[a];u&&d[a].push(...u)}else if(a==="transforms"){const u=r[a];u&&d[a].push(...u)}else if(a==="enter"||a==="exit"){const u=r[a];u&&Object.assign(d[a],u)}}}function V(d,r){throw d?new Error("Cannot close `"+d.type+"` ("+T({start:d.start,end:d.end})+"): a different token (`"+r.type+"`, "+T({start:r.start,end:r.end})+") is open"):new Error("Cannot close document, a token (`"+r.type+"`, "+T({start:r.start,end:r.end})+") is still open")}export{Ae as f};
