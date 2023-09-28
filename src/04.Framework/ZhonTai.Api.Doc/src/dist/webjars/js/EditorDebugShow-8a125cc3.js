import{V as u}from"./vue3-ace-editor-f0da59bd.js";import"./ace-builds-a464a6c2.js";import{_ as h}from"./index-d84fb973.js";import{l as g,w as f,W as v,X as s,Y as n,c as a,a5 as l}from"./@vue-963d0241.js";import"./resize-observer-polyfill-0f9f8adb.js";import"./@babel-f22c9f32.js";import"./ant-design-vue-7939988e.js";import"./dayjs-594ccbfa.js";import"./@ant-design-54a0d12d.js";import"./@ctrl-fb5a5473.js";import"./lodash-es-027f92d5.js";import"./async-validator-dee29e8b.js";import"./scroll-into-view-if-needed-6b992d05.js";import"./compute-scroll-into-view-183f845a.js";import"./vue-types-ef06c517.js";import"./dom-align-ecb06dd6.js";import"./pinia-c4c0f489.js";import"./mermaid-cefb411a.js";import"./@braintree-9b6e9674.js";import"./d3-transition-dcb2c4ce.js";import"./d3-dispatch-4199cc96.js";import"./d3-timer-8bbb3adf.js";import"./d3-interpolate-23596b47.js";import"./d3-color-568b2243.js";import"./d3-selection-de98f9a2.js";import"./d3-ease-6a302484.js";import"./d3-zoom-a2c53521.js";import"./dompurify-0c4e1f83.js";import"./dagre-d3-es-ecbf9480.js";import"./d3-shape-8fda5547.js";import"./d3-path-41c4cb36.js";import"./d3-fetch-5dc2c9dc.js";import"./khroma-dc7cc5cc.js";import"./uuid-80707d31.js";import"./d3-scale-0fe34040.js";import"./internmap-7949acc8.js";import"./d3-array-f46ca9a0.js";import"./d3-format-ffdb8652.js";import"./d3-time-format-8d6e99ce.js";import"./d3-time-e3072704.js";import"./d3-axis-8d489beb.js";import"./elkjs-86b86e03.js";import"./ts-dedent-356e4a49.js";import"./stylis-fad5b415.js";import"./mdast-util-from-markdown-32388d6f.js";import"./micromark-24282579.js";import"./micromark-util-combine-extensions-34ef5cdc.js";import"./micromark-util-chunked-6f054bfc.js";import"./micromark-factory-space-62b287c8.js";import"./micromark-util-character-b4c2c3b7.js";import"./micromark-core-commonmark-c7408300.js";import"./micromark-util-classify-character-b0dd44a1.js";import"./micromark-util-resolve-all-8a85a8df.js";import"./decode-named-character-reference-29ded5ae.js";import"./micromark-util-subtokenize-d9357da7.js";import"./micromark-factory-destination-cef6acfb.js";import"./micromark-factory-label-01414396.js";import"./micromark-factory-title-1a18636a.js";import"./micromark-factory-whitespace-7a4e1cef.js";import"./micromark-util-normalize-identifier-dfdf0387.js";import"./micromark-util-html-tag-name-eaa6d7c0.js";import"./micromark-util-decode-numeric-character-reference-bd25dc10.js";import"./micromark-util-decode-string-02bf9559.js";import"./unist-util-stringify-position-7b16d8bf.js";import"./mdast-util-to-string-bfd41500.js";import"./cytoscape-9984022b.js";import"./cytoscape-cose-bilkent-7a02157b.js";import"./cose-base-062de851.js";import"./layout-base-089b2bf3.js";import"./vue-router-ea0bd7e7.js";import"./vue-i18n-48149f47.js";import"./@intlify-ada814d3.js";import"./localforage-5e17476c.js";import"./marked-c4f1ef09.js";import"./lodash-bf2603fc.js";import"./js-md5-774453ff.js";import"./async-ba739671.js";import"./axios-f38c850f.js";const c={name:"EditorShow",components:{editor:u},props:{value:{type:String,required:!0,default:""},mode:{type:String,required:!0,default:"json"},debugResponse:{type:Boolean,default:!1}},emits:["update:value","debugEditorChange","showDescription"],setup(e){const t=g(e.value);return f(()=>e.value,()=>{t.value=e.value}),{valueText:t}},data(){return{editor:null,editorHeight:200,debugOptions:{readOnly:!1,autoScrollEditorIntoView:!0,displayIndentGuides:!1,fixedWidthGutter:!0},commonOptions:{readOnly:!1}}},methods:{resetEditorHeight(){var e=this;setTimeout(()=>{var t=e.editor.session.getLength();t==1&&(t=15),t<15&&(e.debugResponse?t=30:t=15),t>20&&(e.debugResponse||(t=20));var i=t*16;i>2e3&&(i=2e3),e.editorHeight=i},10)},change(){this.$emit("update:value",this.valueText),this.debugResponse||this.resetEditorHeight()},editorInit(e){var t=this;this.editor=e,this.debugResponse?(this.editor.getSession().setUseWrapMode(!0),this.editor.setOptions(this.debugOptions),this.mode=="text"&&this.editor.getSession().setUseWrapMode(!0)):this.editor.setOptions(this.commonOptions),this.resetEditorHeight(),this.editor.renderer.on("afterRender",function(){var i=t.editor.session.getLength();t.$emit("showDescription",i)})}}},_={key:0},x={key:1};function y(e,t,i,o,p,r){const d=v("editor");return s(),n("div",null,[i.debugResponse?(s(),n("div",_,[a(d,{class:"knife4j-debug-ace-editor",onInput:r.change,options:p.debugOptions,value:o.valueText,"onUpdate:value":t[0]||(t[0]=m=>o.valueText=m),onInit:r.editorInit,lang:i.mode,theme:"eclipse",width:"100%",style:l({height:p.editorHeight+"px"})},null,8,["onInput","options","value","onInit","lang","style"])])):(s(),n("div",x,[a(d,{value:o.valueText,"onUpdate:value":t[1]||(t[1]=m=>o.valueText=m),onInit:r.editorInit,onInput:r.change,lang:i.mode,theme:"eclipse",width:"100%",style:l({height:p.editorHeight+"px"})},null,8,["value","onInit","onInput","lang","style"])]))])}const Xt=h(c,[["render",y]]);export{Xt as default};
