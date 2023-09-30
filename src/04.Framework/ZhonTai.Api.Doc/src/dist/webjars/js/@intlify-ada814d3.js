/*!
  * shared v9.3.0-beta.14
  * (c) 2023 kazuya kawaguchi
  * Released under the MIT License.
  */const cn=typeof window<"u",on=(e,t=!1)=>t?Symbol.for(e):Symbol(e),ze=(e,t,a)=>et({l:e,k:t,s:a}),et=e=>JSON.stringify(e).replace(/\u2028/g,"\\u2028").replace(/\u2029/g,"\\u2029").replace(/\u0027/g,"\\u0027"),v=e=>typeof e=="number"&&isFinite(e),tt=e=>ue(e)==="[object Date]",Ne=e=>ue(e)==="[object RegExp]",ie=e=>M(e)&&Object.keys(e).length===0;function nt(e,t){typeof console<"u"&&(console.warn("[intlify] "+e),t&&console.warn(t.stack))}const q=Object.assign;let ge;const rt=()=>ge||(ge=typeof globalThis<"u"?globalThis:typeof self<"u"?self:typeof window<"u"?window:typeof global<"u"?global:{});function Te(e){return e.replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/"/g,"&quot;").replace(/'/g,"&apos;")}const at=Object.prototype.hasOwnProperty;function un(e,t){return at.call(e,t)}const G=Array.isArray,U=e=>typeof e=="function",b=e=>typeof e=="string",W=e=>typeof e=="boolean",w=e=>e!==null&&typeof e=="object",Pe=Object.prototype.toString,ue=e=>Pe.call(e),M=e=>ue(e)==="[object Object]",st=e=>e==null?"":G(e)||M(e)&&e.toString===Pe?JSON.stringify(e,null,2):String(e);/*!
  * message-compiler v9.3.0-beta.14
  * (c) 2023 kazuya kawaguchi
  * Released under the MIT License.
  */const D={EXPECTED_TOKEN:1,INVALID_TOKEN_IN_PLACEHOLDER:2,UNTERMINATED_SINGLE_QUOTE_IN_PLACEHOLDER:3,UNKNOWN_ESCAPE_SEQUENCE:4,INVALID_UNICODE_ESCAPE_SEQUENCE:5,UNBALANCED_CLOSING_BRACE:6,UNTERMINATED_CLOSING_BRACE:7,EMPTY_PLACEHOLDER:8,NOT_ALLOW_NEST_PLACEHOLDER:9,INVALID_LINKED_FORMAT:10,MUST_HAVE_MESSAGES_IN_PLURAL:11,UNEXPECTED_EMPTY_LINKED_MODIFIER:12,UNEXPECTED_EMPTY_LINKED_KEY:13,UNEXPECTED_LEXICAL_ANALYSIS:14,__EXTEND_POINT__:15};function fe(e,t,a={}){const{domain:s,messages:o,args:c}=a,m=e,g=new SyntaxError(String(m));return g.code=e,t&&(g.location=t),g.domain=s,g}function lt(e){throw e}function ct(e,t,a){return{line:e,column:t,offset:a}}function oe(e,t,a){const s={start:e,end:t};return a!=null&&(s.source=a),s}const Y=" ",ot="\r",F=`
`,it=String.fromCharCode(8232),ut=String.fromCharCode(8233);function ft(e){const t=e;let a=0,s=1,o=1,c=0;const m=C=>t[C]===ot&&t[C+1]===F,g=C=>t[C]===F,f=C=>t[C]===ut,_=C=>t[C]===it,S=C=>m(C)||g(C)||f(C)||_(C),T=()=>a,N=()=>s,O=()=>o,y=()=>c,A=C=>m(C)||f(C)||_(C)?F:t[C],I=()=>A(a),l=()=>A(a+c);function d(){return c=0,S(a)&&(s++,o=0),m(a)&&a++,a++,o++,t[a]}function E(){return m(a+c)&&c++,c++,t[a+c]}function u(){a=0,s=1,o=1,c=0}function h(C=0){c=C}function p(){const C=a+c;for(;C!==a;)d();c=0}return{index:T,line:N,column:O,peekOffset:y,charAt:A,currentChar:I,currentPeek:l,next:d,peek:E,reset:u,resetPeek:h,skipToPeek:p}}const B=void 0,Ce="'",dt="tokenizer";function mt(e,t={}){const a=t.location!==!1,s=ft(e),o=()=>s.index(),c=()=>ct(s.line(),s.column(),s.index()),m=c(),g=o(),f={currentType:14,offset:g,startLoc:m,endLoc:m,lastType:14,lastOffset:g,lastStartLoc:m,lastEndLoc:m,braceNest:0,inLinked:!1,text:""},_=()=>f,{onError:S}=t;function T(n,r,i,...L){const k=_();if(r.column+=i,r.offset+=i,S){const P=oe(k.startLoc,r),j=fe(n,P,{domain:dt,args:L});S(j)}}function N(n,r,i){n.endLoc=c(),n.currentType=r;const L={type:r};return a&&(L.loc=oe(n.startLoc,n.endLoc)),i!=null&&(L.value=i),L}const O=n=>N(n,14);function y(n,r){return n.currentChar()===r?(n.next(),r):(T(D.EXPECTED_TOKEN,c(),0,r),"")}function A(n){let r="";for(;n.currentPeek()===Y||n.currentPeek()===F;)r+=n.currentPeek(),n.peek();return r}function I(n){const r=A(n);return n.skipToPeek(),r}function l(n){if(n===B)return!1;const r=n.charCodeAt(0);return r>=97&&r<=122||r>=65&&r<=90||r===95}function d(n){if(n===B)return!1;const r=n.charCodeAt(0);return r>=48&&r<=57}function E(n,r){const{currentType:i}=r;if(i!==2)return!1;A(n);const L=l(n.currentPeek());return n.resetPeek(),L}function u(n,r){const{currentType:i}=r;if(i!==2)return!1;A(n);const L=n.currentPeek()==="-"?n.peek():n.currentPeek(),k=d(L);return n.resetPeek(),k}function h(n,r){const{currentType:i}=r;if(i!==2)return!1;A(n);const L=n.currentPeek()===Ce;return n.resetPeek(),L}function p(n,r){const{currentType:i}=r;if(i!==8)return!1;A(n);const L=n.currentPeek()===".";return n.resetPeek(),L}function C(n,r){const{currentType:i}=r;if(i!==9)return!1;A(n);const L=l(n.currentPeek());return n.resetPeek(),L}function x(n,r){const{currentType:i}=r;if(!(i===8||i===12))return!1;A(n);const L=n.currentPeek()===":";return n.resetPeek(),L}function R(n,r){const{currentType:i}=r;if(i!==10)return!1;const L=()=>{const P=n.currentPeek();return P==="{"?l(n.peek()):P==="@"||P==="%"||P==="|"||P===":"||P==="."||P===Y||!P?!1:P===F?(n.peek(),L()):l(P)},k=L();return n.resetPeek(),k}function $(n){A(n);const r=n.currentPeek()==="|";return n.resetPeek(),r}function V(n){const r=A(n),i=n.currentPeek()==="%"&&n.peek()==="{";return n.resetPeek(),{isModulo:i,hasSpace:r.length>0}}function Z(n,r=!0){const i=(k=!1,P="",j=!1)=>{const Q=n.currentPeek();return Q==="{"?P==="%"?!1:k:Q==="@"||!Q?P==="%"?!0:k:Q==="%"?(n.peek(),i(k,"%",!0)):Q==="|"?P==="%"||j?!0:!(P===Y||P===F):Q===Y?(n.peek(),i(!0,Y,j)):Q===F?(n.peek(),i(!0,F,j)):!0},L=i();return r&&n.resetPeek(),L}function H(n,r){const i=n.currentChar();return i===B?B:r(i)?(n.next(),i):null}function Ee(n){return H(n,i=>{const L=i.charCodeAt(0);return L>=97&&L<=122||L>=65&&L<=90||L>=48&&L<=57||L===95||L===36})}function Xe(n){return H(n,i=>{const L=i.charCodeAt(0);return L>=48&&L<=57})}function Ke(n){return H(n,i=>{const L=i.charCodeAt(0);return L>=48&&L<=57||L>=65&&L<=70||L>=97&&L<=102})}function Le(n){let r="",i="";for(;r=Xe(n);)i+=r;return i}function Ge(n){I(n);const r=n.currentChar();return r!=="%"&&T(D.EXPECTED_TOKEN,c(),0,r),n.next(),"%"}function he(n){let r="";for(;;){const i=n.currentChar();if(i==="{"||i==="}"||i==="@"||i==="|"||!i)break;if(i==="%")if(Z(n))r+=i,n.next();else break;else if(i===Y||i===F)if(Z(n))r+=i,n.next();else{if($(n))break;r+=i,n.next()}else r+=i,n.next()}return r}function Ye(n){I(n);let r="",i="";for(;r=Ee(n);)i+=r;return n.currentChar()===B&&T(D.UNTERMINATED_CLOSING_BRACE,c(),0),i}function He(n){I(n);let r="";return n.currentChar()==="-"?(n.next(),r+=`-${Le(n)}`):r+=Le(n),n.currentChar()===B&&T(D.UNTERMINATED_CLOSING_BRACE,c(),0),r}function je(n){I(n),y(n,"'");let r="",i="";const L=P=>P!==Ce&&P!==F;for(;r=H(n,L);)r==="\\"?i+=Be(n):i+=r;const k=n.currentChar();return k===F||k===B?(T(D.UNTERMINATED_SINGLE_QUOTE_IN_PLACEHOLDER,c(),0),k===F&&(n.next(),y(n,"'")),i):(y(n,"'"),i)}function Be(n){const r=n.currentChar();switch(r){case"\\":case"'":return n.next(),`\\${r}`;case"u":return pe(n,r,4);case"U":return pe(n,r,6);default:return T(D.UNKNOWN_ESCAPE_SEQUENCE,c(),0,r),""}}function pe(n,r,i){y(n,r);let L="";for(let k=0;k<i;k++){const P=Ke(n);if(!P){T(D.INVALID_UNICODE_ESCAPE_SEQUENCE,c(),0,`\\${r}${L}${n.currentChar()}`);break}L+=P}return`\\${r}${L}`}function Je(n){I(n);let r="",i="";const L=k=>k!=="{"&&k!=="}"&&k!==Y&&k!==F;for(;r=H(n,L);)i+=r;return i}function Qe(n){let r="",i="";for(;r=Ee(n);)i+=r;return i}function qe(n){const r=(i=!1,L)=>{const k=n.currentChar();return k==="{"||k==="%"||k==="@"||k==="|"||!k||k===Y?L:k===F?(L+=k,n.next(),r(i,L)):(L+=k,n.next(),r(!0,L))};return r(!1,"")}function ae(n){I(n);const r=y(n,"|");return I(n),r}function se(n,r){let i=null;switch(n.currentChar()){case"{":return r.braceNest>=1&&T(D.NOT_ALLOW_NEST_PLACEHOLDER,c(),0),n.next(),i=N(r,2,"{"),I(n),r.braceNest++,i;case"}":return r.braceNest>0&&r.currentType===2&&T(D.EMPTY_PLACEHOLDER,c(),0),n.next(),i=N(r,3,"}"),r.braceNest--,r.braceNest>0&&I(n),r.inLinked&&r.braceNest===0&&(r.inLinked=!1),i;case"@":return r.braceNest>0&&T(D.UNTERMINATED_CLOSING_BRACE,c(),0),i=re(n,r)||O(r),r.braceNest=0,i;default:let k=!0,P=!0,j=!0;if($(n))return r.braceNest>0&&T(D.UNTERMINATED_CLOSING_BRACE,c(),0),i=N(r,1,ae(n)),r.braceNest=0,r.inLinked=!1,i;if(r.braceNest>0&&(r.currentType===5||r.currentType===6||r.currentType===7))return T(D.UNTERMINATED_CLOSING_BRACE,c(),0),r.braceNest=0,le(n,r);if(k=E(n,r))return i=N(r,5,Ye(n)),I(n),i;if(P=u(n,r))return i=N(r,6,He(n)),I(n),i;if(j=h(n,r))return i=N(r,7,je(n)),I(n),i;if(!k&&!P&&!j)return i=N(r,13,Je(n)),T(D.INVALID_TOKEN_IN_PLACEHOLDER,c(),0,i.value),I(n),i;break}return i}function re(n,r){const{currentType:i}=r;let L=null;const k=n.currentChar();switch((i===8||i===9||i===12||i===10)&&(k===F||k===Y)&&T(D.INVALID_LINKED_FORMAT,c(),0),k){case"@":return n.next(),L=N(r,8,"@"),r.inLinked=!0,L;case".":return I(n),n.next(),N(r,9,".");case":":return I(n),n.next(),N(r,10,":");default:return $(n)?(L=N(r,1,ae(n)),r.braceNest=0,r.inLinked=!1,L):p(n,r)||x(n,r)?(I(n),re(n,r)):C(n,r)?(I(n),N(r,12,Qe(n))):R(n,r)?(I(n),k==="{"?se(n,r)||L:N(r,11,qe(n))):(i===8&&T(D.INVALID_LINKED_FORMAT,c(),0),r.braceNest=0,r.inLinked=!1,le(n,r))}}function le(n,r){let i={type:14};if(r.braceNest>0)return se(n,r)||O(r);if(r.inLinked)return re(n,r)||O(r);switch(n.currentChar()){case"{":return se(n,r)||O(r);case"}":return T(D.UNBALANCED_CLOSING_BRACE,c(),0),n.next(),N(r,3,"}");case"@":return re(n,r)||O(r);default:if($(n))return i=N(r,1,ae(n)),r.braceNest=0,r.inLinked=!1,i;const{isModulo:k,hasSpace:P}=V(n);if(k)return P?N(r,0,he(n)):N(r,4,Ge(n));if(Z(n))return N(r,0,he(n));break}return i}function Ze(){const{currentType:n,offset:r,startLoc:i,endLoc:L}=f;return f.lastType=n,f.lastOffset=r,f.lastStartLoc=i,f.lastEndLoc=L,f.offset=o(),f.startLoc=c(),s.currentChar()===B?N(f,14):le(s,f)}return{nextToken:Ze,currentOffset:o,currentPosition:c,context:_}}const _t="parser",Et=/(?:\\\\|\\'|\\u([0-9a-fA-F]{4})|\\U([0-9a-fA-F]{6}))/g;function Lt(e,t,a){switch(e){case"\\\\":return"\\";case"\\'":return"'";default:{const s=parseInt(t||a,16);return s<=55295||s>=57344?String.fromCodePoint(s):"�"}}}function ht(e={}){const t=e.location!==!1,{onError:a}=e;function s(l,d,E,u,...h){const p=l.currentPosition();if(p.offset+=u,p.column+=u,a){const C=oe(E,p),x=fe(d,C,{domain:_t,args:h});a(x)}}function o(l,d,E){const u={type:l,start:d,end:d};return t&&(u.loc={start:E,end:E}),u}function c(l,d,E,u){l.end=d,u&&(l.type=u),t&&l.loc&&(l.loc.end=E)}function m(l,d){const E=l.context(),u=o(3,E.offset,E.startLoc);return u.value=d,c(u,l.currentOffset(),l.currentPosition()),u}function g(l,d){const E=l.context(),{lastOffset:u,lastStartLoc:h}=E,p=o(5,u,h);return p.index=parseInt(d,10),l.nextToken(),c(p,l.currentOffset(),l.currentPosition()),p}function f(l,d){const E=l.context(),{lastOffset:u,lastStartLoc:h}=E,p=o(4,u,h);return p.key=d,l.nextToken(),c(p,l.currentOffset(),l.currentPosition()),p}function _(l,d){const E=l.context(),{lastOffset:u,lastStartLoc:h}=E,p=o(9,u,h);return p.value=d.replace(Et,Lt),l.nextToken(),c(p,l.currentOffset(),l.currentPosition()),p}function S(l){const d=l.nextToken(),E=l.context(),{lastOffset:u,lastStartLoc:h}=E,p=o(8,u,h);return d.type!==12?(s(l,D.UNEXPECTED_EMPTY_LINKED_MODIFIER,E.lastStartLoc,0),p.value="",c(p,u,h),{nextConsumeToken:d,node:p}):(d.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,E.lastStartLoc,0,X(d)),p.value=d.value||"",c(p,l.currentOffset(),l.currentPosition()),{node:p})}function T(l,d){const E=l.context(),u=o(7,E.offset,E.startLoc);return u.value=d,c(u,l.currentOffset(),l.currentPosition()),u}function N(l){const d=l.context(),E=o(6,d.offset,d.startLoc);let u=l.nextToken();if(u.type===9){const h=S(l);E.modifier=h.node,u=h.nextConsumeToken||l.nextToken()}switch(u.type!==10&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(u)),u=l.nextToken(),u.type===2&&(u=l.nextToken()),u.type){case 11:u.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(u)),E.key=T(l,u.value||"");break;case 5:u.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(u)),E.key=f(l,u.value||"");break;case 6:u.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(u)),E.key=g(l,u.value||"");break;case 7:u.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(u)),E.key=_(l,u.value||"");break;default:s(l,D.UNEXPECTED_EMPTY_LINKED_KEY,d.lastStartLoc,0);const h=l.context(),p=o(7,h.offset,h.startLoc);return p.value="",c(p,h.offset,h.startLoc),E.key=p,c(E,h.offset,h.startLoc),{nextConsumeToken:u,node:E}}return c(E,l.currentOffset(),l.currentPosition()),{node:E}}function O(l){const d=l.context(),E=d.currentType===1?l.currentOffset():d.offset,u=d.currentType===1?d.endLoc:d.startLoc,h=o(2,E,u);h.items=[];let p=null;do{const R=p||l.nextToken();switch(p=null,R.type){case 0:R.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(R)),h.items.push(m(l,R.value||""));break;case 6:R.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(R)),h.items.push(g(l,R.value||""));break;case 5:R.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(R)),h.items.push(f(l,R.value||""));break;case 7:R.value==null&&s(l,D.UNEXPECTED_LEXICAL_ANALYSIS,d.lastStartLoc,0,X(R)),h.items.push(_(l,R.value||""));break;case 8:const $=N(l);h.items.push($.node),p=$.nextConsumeToken||null;break}}while(d.currentType!==14&&d.currentType!==1);const C=d.currentType===1?d.lastOffset:l.currentOffset(),x=d.currentType===1?d.lastEndLoc:l.currentPosition();return c(h,C,x),h}function y(l,d,E,u){const h=l.context();let p=u.items.length===0;const C=o(1,d,E);C.cases=[],C.cases.push(u);do{const x=O(l);p||(p=x.items.length===0),C.cases.push(x)}while(h.currentType!==14);return p&&s(l,D.MUST_HAVE_MESSAGES_IN_PLURAL,E,0),c(C,l.currentOffset(),l.currentPosition()),C}function A(l){const d=l.context(),{offset:E,startLoc:u}=d,h=O(l);return d.currentType===14?h:y(l,E,u,h)}function I(l){const d=mt(l,q({},e)),E=d.context(),u=o(0,E.offset,E.startLoc);return t&&u.loc&&(u.loc.source=l),u.body=A(d),E.currentType!==14&&s(d,D.UNEXPECTED_LEXICAL_ANALYSIS,E.lastStartLoc,0,l[E.offset]||""),c(u,d.currentOffset(),d.currentPosition()),u}return{parse:I}}function X(e){if(e.type===14)return"EOF";const t=(e.value||"").replace(/\r?\n/gu,"\\n");return t.length>10?t.slice(0,9)+"…":t}function pt(e,t={}){const a={ast:e,helpers:new Set};return{context:()=>a,helper:c=>(a.helpers.add(c),c)}}function Ie(e,t){for(let a=0;a<e.length;a++)de(e[a],t)}function de(e,t){switch(e.type){case 1:Ie(e.cases,t),t.helper("plural");break;case 2:Ie(e.items,t);break;case 6:de(e.key,t),t.helper("linked"),t.helper("type");break;case 5:t.helper("interpolate"),t.helper("list");break;case 4:t.helper("interpolate"),t.helper("named");break}}function Nt(e,t={}){const a=pt(e);a.helper("normalize"),e.body&&de(e.body,a);const s=a.context();e.helpers=Array.from(s.helpers)}function gt(e,t){const{sourceMap:a,filename:s,breakLineCode:o,needIndent:c}=t,m={source:e.loc.source,filename:s,code:"",column:1,line:1,offset:0,map:void 0,breakLineCode:o,needIndent:c,indentLevel:0},g=()=>m;function f(A,I){m.code+=A}function _(A,I=!0){const l=I?o:"";f(c?l+"  ".repeat(A):l)}function S(A=!0){const I=++m.indentLevel;A&&_(I)}function T(A=!0){const I=--m.indentLevel;A&&_(I)}function N(){_(m.indentLevel)}return{context:g,push:f,indent:S,deindent:T,newline:N,helper:A=>`_${A}`,needIndent:()=>m.needIndent}}function Tt(e,t){const{helper:a}=e;e.push(`${a("linked")}(`),te(e,t.key),t.modifier?(e.push(", "),te(e,t.modifier),e.push(", _type")):e.push(", undefined, _type"),e.push(")")}function Ct(e,t){const{helper:a,needIndent:s}=e;e.push(`${a("normalize")}([`),e.indent(s());const o=t.items.length;for(let c=0;c<o&&(te(e,t.items[c]),c!==o-1);c++)e.push(", ");e.deindent(s()),e.push("])")}function It(e,t){const{helper:a,needIndent:s}=e;if(t.cases.length>1){e.push(`${a("plural")}([`),e.indent(s());const o=t.cases.length;for(let c=0;c<o&&(te(e,t.cases[c]),c!==o-1);c++)e.push(", ");e.deindent(s()),e.push("])")}}function St(e,t){t.body?te(e,t.body):e.push("null")}function te(e,t){const{helper:a}=e;switch(t.type){case 0:St(e,t);break;case 1:It(e,t);break;case 2:Ct(e,t);break;case 6:Tt(e,t);break;case 8:e.push(JSON.stringify(t.value),t);break;case 7:e.push(JSON.stringify(t.value),t);break;case 5:e.push(`${a("interpolate")}(${a("list")}(${t.index}))`,t);break;case 4:e.push(`${a("interpolate")}(${a("named")}(${JSON.stringify(t.key)}))`,t);break;case 9:e.push(JSON.stringify(t.value),t);break;case 3:e.push(JSON.stringify(t.value),t);break}}const At=(e,t={})=>{const a=b(t.mode)?t.mode:"normal",s=b(t.filename)?t.filename:"message.intl",o=!!t.sourceMap,c=t.breakLineCode!=null?t.breakLineCode:a==="arrow"?";":`
`,m=t.needIndent?t.needIndent:a!=="arrow",g=e.helpers||[],f=gt(e,{mode:a,filename:s,sourceMap:o,breakLineCode:c,needIndent:m});f.push(a==="normal"?"function __msg__ (ctx) {":"(ctx) => {"),f.indent(m),g.length>0&&(f.push(`const { ${g.map(T=>`${T}: _${T}`).join(", ")} } = ctx`),f.newline()),f.push("return "),te(f,e),f.deindent(m),f.push("}");const{code:_,map:S}=f.context();return{ast:e,code:_,map:S?S.toJSON():void 0}};function bt(e,t={}){const a=q({},t),o=ht(a).parse(e);return Nt(o,a),At(o,a)}/*!
  * devtools-if v9.3.0-beta.14
  * (c) 2023 kazuya kawaguchi
  * Released under the MIT License.
  */const Me={I18nInit:"i18n:init",FunctionTranslate:"function:translate"};/*!
  * core-base v9.3.0-beta.14
  * (c) 2023 kazuya kawaguchi
  * Released under the MIT License.
  */const J=[];J[0]={w:[0],i:[3,0],"[":[4],o:[7]};J[1]={w:[1],".":[2],"[":[4],o:[7]};J[2]={w:[2],i:[3,0],0:[3,0]};J[3]={i:[3,0],0:[3,0],w:[1,1],".":[2,1],"[":[4,1],o:[7,1]};J[4]={"'":[5,0],'"':[6,0],"[":[4,2],"]":[1,3],o:8,l:[4,0]};J[5]={"'":[4,0],o:8,l:[5,0]};J[6]={'"':[4,0],o:8,l:[6,0]};const kt=/^\s?(?:true|false|-?[\d.]+|'[^']*'|"[^"]*")\s?$/;function Ot(e){return kt.test(e)}function yt(e){const t=e.charCodeAt(0),a=e.charCodeAt(e.length-1);return t===a&&(t===34||t===39)?e.slice(1,-1):e}function Dt(e){if(e==null)return"o";switch(e.charCodeAt(0)){case 91:case 93:case 46:case 34:case 39:return e;case 95:case 36:case 45:return"i";case 9:case 10:case 13:case 160:case 65279:case 8232:case 8233:return"w"}return"i"}function Pt(e){const t=e.trim();return e.charAt(0)==="0"&&isNaN(parseInt(e))?!1:Ot(t)?yt(t):"*"+t}function Mt(e){const t=[];let a=-1,s=0,o=0,c,m,g,f,_,S,T;const N=[];N[0]=()=>{m===void 0?m=g:m+=g},N[1]=()=>{m!==void 0&&(t.push(m),m=void 0)},N[2]=()=>{N[0](),o++},N[3]=()=>{if(o>0)o--,s=4,N[0]();else{if(o=0,m===void 0||(m=Pt(m),m===!1))return!1;N[1]()}};function O(){const y=e[a+1];if(s===5&&y==="'"||s===6&&y==='"')return a++,g="\\"+y,N[0](),!0}for(;s!==null;)if(a++,c=e[a],!(c==="\\"&&O())){if(f=Dt(c),T=J[s],_=T[f]||T.l||8,_===8||(s=_[0],_[1]!==void 0&&(S=N[_[1]],S&&(g=c,S()===!1))))return;if(s===7)return t}}const Se=new Map;function Rt(e,t){return w(e)?e[t]:null}function fn(e,t){if(!w(e))return null;let a=Se.get(t);if(a||(a=Mt(t),a&&Se.set(t,a)),!a)return null;const s=a.length;let o=e,c=0;for(;c<s;){const m=o[a[c]];if(m===void 0)return null;o=m,c++}return o}const Ft=e=>e,wt=e=>"",Ut="text",vt=e=>e.length===0?"":e.join(""),Wt=st;function Ae(e,t){return e=Math.abs(e),t===2?e?e>1?1:0:1:e?Math.min(e,2):0}function xt(e){const t=v(e.pluralIndex)?e.pluralIndex:-1;return e.named&&(v(e.named.count)||v(e.named.n))?v(e.named.count)?e.named.count:v(e.named.n)?e.named.n:t:t}function $t(e,t){t.count||(t.count=e),t.n||(t.n=e)}function Vt(e={}){const t=e.locale,a=xt(e),s=w(e.pluralRules)&&b(t)&&U(e.pluralRules[t])?e.pluralRules[t]:Ae,o=w(e.pluralRules)&&b(t)&&U(e.pluralRules[t])?Ae:void 0,c=l=>l[s(a,l.length,o)],m=e.list||[],g=l=>m[l],f=e.named||{};v(e.pluralIndex)&&$t(a,f);const _=l=>f[l];function S(l){const d=U(e.messages)?e.messages(l):w(e.messages)?e.messages[l]:!1;return d||(e.parent?e.parent.message(l):wt)}const T=l=>e.modifiers?e.modifiers[l]:Ft,N=M(e.processor)&&U(e.processor.normalize)?e.processor.normalize:vt,O=M(e.processor)&&U(e.processor.interpolate)?e.processor.interpolate:Wt,y=M(e.processor)&&b(e.processor.type)?e.processor.type:Ut,I={list:g,named:_,plural:c,linked:(l,...d)=>{const[E,u]=d;let h="text",p="";d.length===1?w(E)?(p=E.modifier||p,h=E.type||h):b(E)&&(p=E||p):d.length===2&&(b(E)&&(p=E||p),b(u)&&(h=u||h));let C=S(l)(I);return h==="vnode"&&G(C)&&p&&(C=C[0]),p?T(p)(C,h):C},message:S,type:y,interpolate:O,normalize:N};return I}let ne=null;function dn(e){ne=e}function Xt(e,t,a){ne&&ne.emit(Me.I18nInit,{timestamp:Date.now(),i18n:e,version:t,meta:a})}const Kt=Gt(Me.FunctionTranslate);function Gt(e){return t=>ne&&ne.emit(e,t)}function Yt(e,t,a){return[...new Set([a,...G(t)?t:w(t)?Object.keys(t):b(t)?[t]:[a]])]}function mn(e,t,a){const s=b(a)?a:Re,o=e;o.__localeChainCache||(o.__localeChainCache=new Map);let c=o.__localeChainCache.get(s);if(!c){c=[];let m=[a];for(;G(m);)m=be(c,m,t);const g=G(t)||!M(t)?t:t.default?t.default:null;m=b(g)?[g]:g,G(m)&&be(c,m,!1),o.__localeChainCache.set(s,c)}return c}function be(e,t,a){let s=!0;for(let o=0;o<t.length&&W(s);o++){const c=t[o];b(c)&&(s=Ht(e,t[o],a))}return s}function Ht(e,t,a){let s;const o=t.split("-");do{const c=o.join("-");s=jt(e,c,a),o.splice(-1,1)}while(o.length&&s===!0);return s}function jt(e,t,a){let s=!1;if(!e.includes(t)&&(s=!0,t)){s=t[t.length-1]!=="!";const o=t.replace(/!/g,"");e.push(o),(G(a)||M(a))&&a[o]&&(s=a[o])}return s}const Bt="9.3.0-beta.14",me=-1,Re="en-US",_n="",ke=e=>`${e.charAt(0).toLocaleUpperCase()}${e.substr(1)}`;function Jt(){return{upper:(e,t)=>t==="text"&&b(e)?e.toUpperCase():t==="vnode"&&w(e)&&"__v_isVNode"in e?e.children.toUpperCase():e,lower:(e,t)=>t==="text"&&b(e)?e.toLowerCase():t==="vnode"&&w(e)&&"__v_isVNode"in e?e.children.toLowerCase():e,capitalize:(e,t)=>t==="text"&&b(e)?ke(e):t==="vnode"&&w(e)&&"__v_isVNode"in e?ke(e.children):e}}let Fe;function En(e){Fe=e}let we;function Ln(e){we=e}let Ue;function hn(e){Ue=e}let ve=null;const pn=e=>{ve=e},Qt=()=>ve;let We=null;const Nn=e=>{We=e},gn=()=>We;let Oe=0;function Tn(e={}){const t=b(e.version)?e.version:Bt,a=b(e.locale)?e.locale:Re,s=G(e.fallbackLocale)||M(e.fallbackLocale)||b(e.fallbackLocale)||e.fallbackLocale===!1?e.fallbackLocale:a,o=M(e.messages)?e.messages:{[a]:{}},c=M(e.datetimeFormats)?e.datetimeFormats:{[a]:{}},m=M(e.numberFormats)?e.numberFormats:{[a]:{}},g=q({},e.modifiers||{},Jt()),f=e.pluralRules||{},_=U(e.missing)?e.missing:null,S=W(e.missingWarn)||Ne(e.missingWarn)?e.missingWarn:!0,T=W(e.fallbackWarn)||Ne(e.fallbackWarn)?e.fallbackWarn:!0,N=!!e.fallbackFormat,O=!!e.unresolving,y=U(e.postTranslation)?e.postTranslation:null,A=M(e.processor)?e.processor:null,I=W(e.warnHtmlMessage)?e.warnHtmlMessage:!0,l=!!e.escapeParameter,d=U(e.messageCompiler)?e.messageCompiler:Fe,E=U(e.messageResolver)?e.messageResolver:we||Rt,u=U(e.localeFallbacker)?e.localeFallbacker:Ue||Yt,h=w(e.fallbackContext)?e.fallbackContext:void 0,p=U(e.onWarn)?e.onWarn:nt,C=e,x=w(C.__datetimeFormatters)?C.__datetimeFormatters:new Map,R=w(C.__numberFormatters)?C.__numberFormatters:new Map,$=w(C.__meta)?C.__meta:{};Oe++;const V={version:t,cid:Oe,locale:a,fallbackLocale:s,messages:o,modifiers:g,pluralRules:f,missing:_,missingWarn:S,fallbackWarn:T,fallbackFormat:N,unresolving:O,postTranslation:y,processor:A,warnHtmlMessage:I,escapeParameter:l,messageCompiler:d,messageResolver:E,localeFallbacker:u,fallbackContext:h,onWarn:p,__meta:$};return V.datetimeFormats=c,V.numberFormats=m,V.__datetimeFormatters=x,V.__numberFormatters=R,__INTLIFY_PROD_DEVTOOLS__&&Xt(V,t,$),V}function _e(e,t,a,s,o){const{missing:c,onWarn:m}=e;if(c!==null){const g=c(e,a,t,o);return b(g)?g:t}else return t}function Cn(e,t,a){const s=e;s.__localeChainCache=new Map,e.localeFallbacker(e,a,t)}const qt=e=>e;let ye=Object.create(null);function In(e,t={}){{const s=(t.onCacheKey||qt)(e),o=ye[s];if(o)return o;let c=!1;const m=t.onError||lt;t.onError=_=>{c=!0,m(_)};const{code:g}=bt(e,t),f=new Function(`return ${g}`)();return c?f:ye[s]=f}}let xe=D.__EXTEND_POINT__;const ce=()=>++xe,z={INVALID_ARGUMENT:xe,INVALID_DATE_ARGUMENT:ce(),INVALID_ISO_DATE_ARGUMENT:ce(),__EXTEND_POINT__:ce()};function ee(e){return fe(e,null,void 0)}const De=()=>"",K=e=>U(e);function Sn(e,...t){const{fallbackFormat:a,postTranslation:s,unresolving:o,messageCompiler:c,fallbackLocale:m,messages:g}=e,[f,_]=en(...t),S=W(_.missingWarn)?_.missingWarn:e.missingWarn,T=W(_.fallbackWarn)?_.fallbackWarn:e.fallbackWarn,N=W(_.escapeParameter)?_.escapeParameter:e.escapeParameter,O=!!_.resolvedMessage,y=b(_.default)||W(_.default)?W(_.default)?c?f:()=>f:_.default:a?c?f:()=>f:"",A=a||y!=="",I=b(_.locale)?_.locale:e.locale;N&&Zt(_);let[l,d,E]=O?[f,I,g[I]||{}]:$e(e,f,I,m,T,S),u=l,h=f;if(!O&&!(b(u)||K(u))&&A&&(u=y,h=u),!O&&(!(b(u)||K(u))||!b(d)))return o?me:f;let p=!1;const C=()=>{p=!0},x=K(u)?u:Ve(e,f,d,u,h,C);if(p)return u;const R=nn(e,d,E,_),$=Vt(R),V=zt(e,x,$),Z=s?s(V,f):V;if(__INTLIFY_PROD_DEVTOOLS__){const H={timestamp:Date.now(),key:b(f)?f:K(u)?u.key:"",locale:d||(K(u)?u.locale:""),format:b(u)?u:K(u)?u.source:"",message:Z};H.meta=q({},e.__meta,Qt()||{}),Kt(H)}return Z}function Zt(e){G(e.list)?e.list=e.list.map(t=>b(t)?Te(t):t):w(e.named)&&Object.keys(e.named).forEach(t=>{b(e.named[t])&&(e.named[t]=Te(e.named[t]))})}function $e(e,t,a,s,o,c){const{messages:m,onWarn:g,messageResolver:f,localeFallbacker:_}=e,S=_(e,s,a);let T={},N,O=null;const y="translate";for(let A=0;A<S.length&&(N=S[A],T=m[N]||{},(O=f(T,t))===null&&(O=T[t]),!(b(O)||U(O)));A++){const I=_e(e,t,N,c,y);I!==t&&(O=I)}return[O,N,T]}function Ve(e,t,a,s,o,c){const{messageCompiler:m,warnHtmlMessage:g}=e;if(K(s)){const _=s;return _.locale=_.locale||a,_.key=_.key||t,_}if(m==null){const _=()=>s;return _.locale=a,_.key=t,_}const f=m(s,tn(e,a,o,s,g,c));return f.locale=a,f.key=t,f.source=s,f}function zt(e,t,a){return t(a)}function en(...e){const[t,a,s]=e,o={};if(!b(t)&&!v(t)&&!K(t))throw ee(z.INVALID_ARGUMENT);const c=v(t)?String(t):(K(t),t);return v(a)?o.plural=a:b(a)?o.default=a:M(a)&&!ie(a)?o.named=a:G(a)&&(o.list=a),v(s)?o.plural=s:b(s)?o.default=s:M(s)&&q(o,s),[c,o]}function tn(e,t,a,s,o,c){return{warnHtmlMessage:o,onError:m=>{throw c&&c(m),m},onCacheKey:m=>ze(t,a,m)}}function nn(e,t,a,s){const{modifiers:o,pluralRules:c,messageResolver:m,fallbackLocale:g,fallbackWarn:f,missingWarn:_,fallbackContext:S}=e,N={locale:t,modifiers:o,pluralRules:c,messages:O=>{let y=m(a,O);if(y==null&&S){const[,,A]=$e(S,O,t,g,f,_);y=m(A,O)}if(b(y)){let A=!1;const l=Ve(e,O,t,y,O,()=>{A=!0});return A?De:l}else return K(y)?y:De}};return e.processor&&(N.processor=e.processor),s.list&&(N.list=s.list),s.named&&(N.named=s.named),v(s.plural)&&(N.pluralIndex=s.plural),N}function An(e,...t){const{datetimeFormats:a,unresolving:s,fallbackLocale:o,onWarn:c,localeFallbacker:m}=e,{__datetimeFormatters:g}=e,[f,_,S,T]=an(...t),N=W(S.missingWarn)?S.missingWarn:e.missingWarn;W(S.fallbackWarn)?S.fallbackWarn:e.fallbackWarn;const O=!!S.part,y=b(S.locale)?S.locale:e.locale,A=m(e,o,y);if(!b(f)||f==="")return new Intl.DateTimeFormat(y,T).format(_);let I={},l,d=null;const E="datetime format";for(let p=0;p<A.length&&(l=A[p],I=a[l]||{},d=I[f],!M(d));p++)_e(e,f,l,N,E);if(!M(d)||!b(l))return s?me:f;let u=`${l}__${f}`;ie(T)||(u=`${u}__${JSON.stringify(T)}`);let h=g.get(u);return h||(h=new Intl.DateTimeFormat(l,q({},d,T)),g.set(u,h)),O?h.formatToParts(_):h.format(_)}const rn=["localeMatcher","weekday","era","year","month","day","hour","minute","second","timeZoneName","formatMatcher","hour12","timeZone","dateStyle","timeStyle","calendar","dayPeriod","numberingSystem","hourCycle","fractionalSecondDigits"];function an(...e){const[t,a,s,o]=e,c={};let m={},g;if(b(t)){const f=t.match(/(\d{4}-\d{2}-\d{2})(T|\s)?(.*)/);if(!f)throw ee(z.INVALID_ISO_DATE_ARGUMENT);const _=f[3]?f[3].trim().startsWith("T")?`${f[1].trim()}${f[3].trim()}`:`${f[1].trim()}T${f[3].trim()}`:f[1].trim();g=new Date(_);try{g.toISOString()}catch{throw ee(z.INVALID_ISO_DATE_ARGUMENT)}}else if(tt(t)){if(isNaN(t.getTime()))throw ee(z.INVALID_DATE_ARGUMENT);g=t}else if(v(t))g=t;else throw ee(z.INVALID_ARGUMENT);return b(a)?c.key=a:M(a)&&Object.keys(a).forEach(f=>{rn.includes(f)?m[f]=a[f]:c[f]=a[f]}),b(s)?c.locale=s:M(s)&&(m=s),M(o)&&(m=o),[c.key||"",g,c,m]}function bn(e,t,a){const s=e;for(const o in a){const c=`${t}__${o}`;s.__datetimeFormatters.has(c)&&s.__datetimeFormatters.delete(c)}}function kn(e,...t){const{numberFormats:a,unresolving:s,fallbackLocale:o,onWarn:c,localeFallbacker:m}=e,{__numberFormatters:g}=e,[f,_,S,T]=ln(...t),N=W(S.missingWarn)?S.missingWarn:e.missingWarn;W(S.fallbackWarn)?S.fallbackWarn:e.fallbackWarn;const O=!!S.part,y=b(S.locale)?S.locale:e.locale,A=m(e,o,y);if(!b(f)||f==="")return new Intl.NumberFormat(y,T).format(_);let I={},l,d=null;const E="number format";for(let p=0;p<A.length&&(l=A[p],I=a[l]||{},d=I[f],!M(d));p++)_e(e,f,l,N,E);if(!M(d)||!b(l))return s?me:f;let u=`${l}__${f}`;ie(T)||(u=`${u}__${JSON.stringify(T)}`);let h=g.get(u);return h||(h=new Intl.NumberFormat(l,q({},d,T)),g.set(u,h)),O?h.formatToParts(_):h.format(_)}const sn=["localeMatcher","style","currency","currencyDisplay","currencySign","useGrouping","minimumIntegerDigits","minimumFractionDigits","maximumFractionDigits","minimumSignificantDigits","maximumSignificantDigits","compactDisplay","notation","signDisplay","unit","unitDisplay","roundingMode","roundingPriority","roundingIncrement","trailingZeroDisplay"];function ln(...e){const[t,a,s,o]=e,c={};let m={};if(!v(t))throw ee(z.INVALID_ARGUMENT);const g=t;return b(a)?c.key=a:M(a)&&Object.keys(a).forEach(f=>{sn.includes(f)?m[f]=a[f]:c[f]=a[f]}),b(s)?c.locale=s:M(s)&&(m=s),M(o)&&(m=o),[c.key||"",g,c,m]}function On(e,t,a){const s=e;for(const o in a){const c=`${t}__${o}`;s.__numberFormatters.has(c)&&s.__numberFormatters.delete(c)}}typeof __INTLIFY_PROD_DEVTOOLS__!="boolean"&&(rt().__INTLIFY_PROD_DEVTOOLS__=!1);export{On as A,pn as B,D as C,Re as D,gn as E,me as F,en as G,Sn as H,an as I,An as J,ln as K,kn as L,_n as M,sn as N,Nn as O,q as a,b,w as c,W as d,ie as e,G as f,M as g,Ne as h,v as i,U as j,Ln as k,hn as l,rt as m,fe as n,on as o,un as p,cn as q,En as r,dn as s,rn as t,Cn as u,In as v,fn as w,mn as x,Tn as y,bn as z};