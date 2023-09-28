import{o as J}from"./@babel-f22c9f32.js";import{r as $}from"./layout-base-089b2bf3.js";var S={exports:{}},X;function z(){return X||(X=1,function(W,k){(function(y,w){W.exports=w($())})(J,function(x){return function(y){var w={};function l(n){if(w[n])return w[n].exports;var g=w[n]={i:n,l:!1,exports:{}};return y[n].call(g.exports,g,g.exports,l),g.l=!0,g.exports}return l.m=y,l.c=w,l.i=function(n){return n},l.d=function(n,g,p){l.o(n,g)||Object.defineProperty(n,g,{configurable:!1,enumerable:!0,get:p})},l.n=function(n){var g=n&&n.__esModule?function(){return n.default}:function(){return n};return l.d(g,"a",g),g},l.o=function(n,g){return Object.prototype.hasOwnProperty.call(n,g)},l.p="",l(l.s=7)}([function(y,w){y.exports=x},function(y,w,l){var n=l(0).FDLayoutConstants;function g(){}for(var p in n)g[p]=n[p];g.DEFAULT_USE_MULTI_LEVEL_SCALING=!1,g.DEFAULT_RADIAL_SEPARATION=n.DEFAULT_EDGE_LENGTH,g.DEFAULT_COMPONENT_SEPERATION=60,g.TILE=!0,g.TILING_PADDING_VERTICAL=10,g.TILING_PADDING_HORIZONTAL=10,g.TREE_REDUCTION_ON_INCREMENTAL=!1,y.exports=g},function(y,w,l){var n=l(0).FDLayoutEdge;function g(G,u,E){n.call(this,G,u,E)}g.prototype=Object.create(n.prototype);for(var p in n)g[p]=n[p];y.exports=g},function(y,w,l){var n=l(0).LGraph;function g(G,u,E){n.call(this,G,u,E)}g.prototype=Object.create(n.prototype);for(var p in n)g[p]=n[p];y.exports=g},function(y,w,l){var n=l(0).LGraphManager;function g(G){n.call(this,G)}g.prototype=Object.create(n.prototype);for(var p in n)g[p]=n[p];y.exports=g},function(y,w,l){var n=l(0).FDLayoutNode,g=l(0).IMath;function p(u,E,T,A){n.call(this,u,E,T,A)}p.prototype=Object.create(n.prototype);for(var G in n)p[G]=n[G];p.prototype.move=function(){var u=this.graphManager.getLayout();this.displacementX=u.coolingFactor*(this.springForceX+this.repulsionForceX+this.gravitationForceX)/this.noOfChildren,this.displacementY=u.coolingFactor*(this.springForceY+this.repulsionForceY+this.gravitationForceY)/this.noOfChildren,Math.abs(this.displacementX)>u.coolingFactor*u.maxNodeDisplacement&&(this.displacementX=u.coolingFactor*u.maxNodeDisplacement*g.sign(this.displacementX)),Math.abs(this.displacementY)>u.coolingFactor*u.maxNodeDisplacement&&(this.displacementY=u.coolingFactor*u.maxNodeDisplacement*g.sign(this.displacementY)),this.child==null?this.moveBy(this.displacementX,this.displacementY):this.child.getNodes().length==0?this.moveBy(this.displacementX,this.displacementY):this.propogateDisplacementToChildren(this.displacementX,this.displacementY),u.totalDisplacement+=Math.abs(this.displacementX)+Math.abs(this.displacementY),this.springForceX=0,this.springForceY=0,this.repulsionForceX=0,this.repulsionForceY=0,this.gravitationForceX=0,this.gravitationForceY=0,this.displacementX=0,this.displacementY=0},p.prototype.propogateDisplacementToChildren=function(u,E){for(var T=this.getChild().getNodes(),A,P=0;P<T.length;P++)A=T[P],A.getChild()==null?(A.moveBy(u,E),A.displacementX+=u,A.displacementY+=E):A.propogateDisplacementToChildren(u,E)},p.prototype.setPred1=function(u){this.pred1=u},p.prototype.getPred1=function(){return pred1},p.prototype.getPred2=function(){return pred2},p.prototype.setNext=function(u){this.next=u},p.prototype.getNext=function(){return next},p.prototype.setProcessed=function(u){this.processed=u},p.prototype.isProcessed=function(){return processed},y.exports=p},function(y,w,l){var n=l(0).FDLayout,g=l(4),p=l(3),G=l(5),u=l(2),E=l(1),T=l(0).FDLayoutConstants,A=l(0).LayoutConstants,P=l(0).Point,R=l(0).PointD,V=l(0).Layout,B=l(0).Integer,Z=l(0).IGeometry,j=l(0).LGraph,K=l(0).Transform;function d(){n.call(this),this.toBeTiled={}}d.prototype=Object.create(n.prototype);for(var U in n)d[U]=n[U];d.prototype.newGraphManager=function(){var t=new g(this);return this.graphManager=t,t},d.prototype.newGraph=function(t){return new p(null,this.graphManager,t)},d.prototype.newNode=function(t){return new G(this.graphManager,t)},d.prototype.newEdge=function(t){return new u(null,null,t)},d.prototype.initParameters=function(){n.prototype.initParameters.call(this,arguments),this.isSubLayout||(E.DEFAULT_EDGE_LENGTH<10?this.idealEdgeLength=10:this.idealEdgeLength=E.DEFAULT_EDGE_LENGTH,this.useSmartIdealEdgeLengthCalculation=E.DEFAULT_USE_SMART_IDEAL_EDGE_LENGTH_CALCULATION,this.springConstant=T.DEFAULT_SPRING_STRENGTH,this.repulsionConstant=T.DEFAULT_REPULSION_STRENGTH,this.gravityConstant=T.DEFAULT_GRAVITY_STRENGTH,this.compoundGravityConstant=T.DEFAULT_COMPOUND_GRAVITY_STRENGTH,this.gravityRangeFactor=T.DEFAULT_GRAVITY_RANGE_FACTOR,this.compoundGravityRangeFactor=T.DEFAULT_COMPOUND_GRAVITY_RANGE_FACTOR,this.prunedNodesAll=[],this.growTreeIterations=0,this.afterGrowthIterations=0,this.isTreeGrowing=!1,this.isGrowthFinished=!1,this.coolingCycle=0,this.maxCoolingCycle=this.maxIterations/T.CONVERGENCE_CHECK_PERIOD,this.finalTemperature=T.CONVERGENCE_CHECK_PERIOD/this.maxIterations,this.coolingAdjuster=1)},d.prototype.layout=function(){var t=A.DEFAULT_CREATE_BENDS_AS_NEEDED;return t&&(this.createBendpoints(),this.graphManager.resetAllEdges()),this.level=0,this.classicLayout()},d.prototype.classicLayout=function(){if(this.nodesWithGravity=this.calculateNodesToApplyGravitationTo(),this.graphManager.setAllNodesToApplyGravitation(this.nodesWithGravity),this.calcNoOfChildrenForAllNodes(),this.graphManager.calcLowestCommonAncestors(),this.graphManager.calcInclusionTreeDepths(),this.graphManager.getRoot().calcEstimatedSize(),this.calcIdealEdgeLengths(),this.incremental){if(E.TREE_REDUCTION_ON_INCREMENTAL){this.reduceTrees(),this.graphManager.resetAllNodesToApplyGravitation();var e=new Set(this.getAllNodes()),r=this.nodesWithGravity.filter(function(a){return e.has(a)});this.graphManager.setAllNodesToApplyGravitation(r)}}else{var t=this.getFlatForest();if(t.length>0)this.positionNodesRadially(t);else{this.reduceTrees(),this.graphManager.resetAllNodesToApplyGravitation();var e=new Set(this.getAllNodes()),r=this.nodesWithGravity.filter(function(i){return e.has(i)});this.graphManager.setAllNodesToApplyGravitation(r),this.positionNodesRandomly()}}return this.initSpringEmbedder(),this.runSpringEmbedder(),!0},d.prototype.tick=function(){if(this.totalIterations++,this.totalIterations===this.maxIterations&&!this.isTreeGrowing&&!this.isGrowthFinished)if(this.prunedNodesAll.length>0)this.isTreeGrowing=!0;else return!0;if(this.totalIterations%T.CONVERGENCE_CHECK_PERIOD==0&&!this.isTreeGrowing&&!this.isGrowthFinished){if(this.isConverged())if(this.prunedNodesAll.length>0)this.isTreeGrowing=!0;else return!0;this.coolingCycle++,this.layoutQuality==0?this.coolingAdjuster=this.coolingCycle:this.layoutQuality==1&&(this.coolingAdjuster=this.coolingCycle/3),this.coolingFactor=Math.max(this.initialCoolingFactor-Math.pow(this.coolingCycle,Math.log(100*(this.initialCoolingFactor-this.finalTemperature))/Math.log(this.maxCoolingCycle))/100*this.coolingAdjuster,this.finalTemperature),this.animationPeriod=Math.ceil(this.initialAnimationPeriod*Math.sqrt(this.coolingFactor))}if(this.isTreeGrowing){if(this.growTreeIterations%10==0)if(this.prunedNodesAll.length>0){this.graphManager.updateBounds(),this.updateGrid(),this.growTree(this.prunedNodesAll),this.graphManager.resetAllNodesToApplyGravitation();var t=new Set(this.getAllNodes()),e=this.nodesWithGravity.filter(function(o){return t.has(o)});this.graphManager.setAllNodesToApplyGravitation(e),this.graphManager.updateBounds(),this.updateGrid(),this.coolingFactor=T.DEFAULT_COOLING_FACTOR_INCREMENTAL}else this.isTreeGrowing=!1,this.isGrowthFinished=!0;this.growTreeIterations++}if(this.isGrowthFinished){if(this.isConverged())return!0;this.afterGrowthIterations%10==0&&(this.graphManager.updateBounds(),this.updateGrid()),this.coolingFactor=T.DEFAULT_COOLING_FACTOR_INCREMENTAL*((100-this.afterGrowthIterations)/100),this.afterGrowthIterations++}var r=!this.isTreeGrowing&&!this.isGrowthFinished,i=this.growTreeIterations%10==1&&this.isTreeGrowing||this.afterGrowthIterations%10==1&&this.isGrowthFinished;return this.totalDisplacement=0,this.graphManager.updateBounds(),this.calcSpringForces(),this.calcRepulsionForces(r,i),this.calcGravitationalForces(),this.moveNodes(),this.animate(),!1},d.prototype.getPositionsData=function(){for(var t=this.graphManager.getAllNodes(),e={},r=0;r<t.length;r++){var i=t[r].rect,o=t[r].id;e[o]={id:o,x:i.getCenterX(),y:i.getCenterY(),w:i.width,h:i.height}}return e},d.prototype.runSpringEmbedder=function(){this.initialAnimationPeriod=25,this.animationPeriod=this.initialAnimationPeriod;var t=!1;if(T.ANIMATE==="during")this.emit("layoutstarted");else{for(;!t;)t=this.tick();this.graphManager.updateBounds()}},d.prototype.calculateNodesToApplyGravitationTo=function(){var t=[],e,r=this.graphManager.getGraphs(),i=r.length,o;for(o=0;o<i;o++)e=r[o],e.updateConnected(),e.isConnected||(t=t.concat(e.getNodes()));return t},d.prototype.createBendpoints=function(){var t=[];t=t.concat(this.graphManager.getAllEdges());var e=new Set,r;for(r=0;r<t.length;r++){var i=t[r];if(!e.has(i)){var o=i.getSource(),a=i.getTarget();if(o==a)i.getBendpoints().push(new R),i.getBendpoints().push(new R),this.createDummyNodesForBendpoints(i),e.add(i);else{var s=[];if(s=s.concat(o.getEdgeListToNode(a)),s=s.concat(a.getEdgeListToNode(o)),!e.has(s[0])){if(s.length>1){var h;for(h=0;h<s.length;h++){var f=s[h];f.getBendpoints().push(new R),this.createDummyNodesForBendpoints(f)}}s.forEach(function(C){e.add(C)})}}}if(e.size==t.length)break}},d.prototype.positionNodesRadially=function(t){for(var e=new P(0,0),r=Math.ceil(Math.sqrt(t.length)),i=0,o=0,a=0,s=new R(0,0),h=0;h<t.length;h++){h%r==0&&(a=0,o=i,h!=0&&(o+=E.DEFAULT_COMPONENT_SEPERATION),i=0);var f=t[h],C=V.findCenterOfTree(f);e.x=a,e.y=o,s=d.radialLayout(f,C,e),s.y>i&&(i=Math.floor(s.y)),a=Math.floor(s.x+E.DEFAULT_COMPONENT_SEPERATION)}this.transform(new R(A.WORLD_CENTER_X-s.x/2,A.WORLD_CENTER_Y-s.y/2))},d.radialLayout=function(t,e,r){var i=Math.max(this.maxDiagonalInTree(t),E.DEFAULT_RADIAL_SEPARATION);d.branchRadialLayout(e,null,0,359,0,i);var o=j.calculateBounds(t),a=new K;a.setDeviceOrgX(o.getMinX()),a.setDeviceOrgY(o.getMinY()),a.setWorldOrgX(r.x),a.setWorldOrgY(r.y);for(var s=0;s<t.length;s++){var h=t[s];h.transform(a)}var f=new R(o.getMaxX(),o.getMaxY());return a.inverseTransformPoint(f)},d.branchRadialLayout=function(t,e,r,i,o,a){var s=(i-r+1)/2;s<0&&(s+=180);var h=(s+r)%360,f=h*Z.TWO_PI/360,C=o*Math.cos(f),m=o*Math.sin(f);t.setCenter(C,m);var N=[];N=N.concat(t.getEdges());var c=N.length;e!=null&&c--;for(var v=0,L=N.length,D,O=t.getEdgesBetween(e);O.length>1;){var F=O[0];O.splice(0,1);var M=N.indexOf(F);M>=0&&N.splice(M,1),L--,c--}e!=null?D=(N.indexOf(O[0])+1)%L:D=0;for(var b=Math.abs(i-r)/c,I=D;v!=c;I=++I%L){var Y=N[I].getOtherEnd(t);if(Y!=e){var H=(r+v*b)%360,Q=(H+b)%360;d.branchRadialLayout(Y,t,H,Q,o+a,a),v++}}},d.maxDiagonalInTree=function(t){for(var e=B.MIN_VALUE,r=0;r<t.length;r++){var i=t[r],o=i.getDiagonal();o>e&&(e=o)}return e},d.prototype.calcRepulsionRange=function(){return 2*(this.level+1)*this.idealEdgeLength},d.prototype.groupZeroDegreeMembers=function(){var t=this,e={};this.memberGroups={},this.idToDummyNode={};for(var r=[],i=this.graphManager.getAllNodes(),o=0;o<i.length;o++){var a=i[o],s=a.getParent();this.getNodeDegreeWithChildren(a)===0&&(s.id==null||!this.getToBeTiled(s))&&r.push(a)}for(var o=0;o<r.length;o++){var a=r[o],h=a.getParent().id;typeof e[h]>"u"&&(e[h]=[]),e[h]=e[h].concat(a)}Object.keys(e).forEach(function(f){if(e[f].length>1){var C="DummyCompound_"+f;t.memberGroups[C]=e[f];var m=e[f][0].getParent(),N=new G(t.graphManager);N.id=C,N.paddingLeft=m.paddingLeft||0,N.paddingRight=m.paddingRight||0,N.paddingBottom=m.paddingBottom||0,N.paddingTop=m.paddingTop||0,t.idToDummyNode[C]=N;var c=t.getGraphManager().add(t.newGraph(),N),v=m.getChild();v.add(N);for(var L=0;L<e[f].length;L++){var D=e[f][L];v.remove(D),c.add(D)}}})},d.prototype.clearCompounds=function(){var t={},e={};this.performDFSOnCompounds();for(var r=0;r<this.compoundOrder.length;r++)e[this.compoundOrder[r].id]=this.compoundOrder[r],t[this.compoundOrder[r].id]=[].concat(this.compoundOrder[r].getChild().getNodes()),this.graphManager.remove(this.compoundOrder[r].getChild()),this.compoundOrder[r].child=null;this.graphManager.resetAllNodes(),this.tileCompoundMembers(t,e)},d.prototype.clearZeroDegreeMembers=function(){var t=this,e=this.tiledZeroDegreePack=[];Object.keys(this.memberGroups).forEach(function(r){var i=t.idToDummyNode[r];e[r]=t.tileNodes(t.memberGroups[r],i.paddingLeft+i.paddingRight),i.rect.width=e[r].width,i.rect.height=e[r].height})},d.prototype.repopulateCompounds=function(){for(var t=this.compoundOrder.length-1;t>=0;t--){var e=this.compoundOrder[t],r=e.id,i=e.paddingLeft,o=e.paddingTop;this.adjustLocations(this.tiledMemberPack[r],e.rect.x,e.rect.y,i,o)}},d.prototype.repopulateZeroDegreeMembers=function(){var t=this,e=this.tiledZeroDegreePack;Object.keys(e).forEach(function(r){var i=t.idToDummyNode[r],o=i.paddingLeft,a=i.paddingTop;t.adjustLocations(e[r],i.rect.x,i.rect.y,o,a)})},d.prototype.getToBeTiled=function(t){var e=t.id;if(this.toBeTiled[e]!=null)return this.toBeTiled[e];var r=t.getChild();if(r==null)return this.toBeTiled[e]=!1,!1;for(var i=r.getNodes(),o=0;o<i.length;o++){var a=i[o];if(this.getNodeDegree(a)>0)return this.toBeTiled[e]=!1,!1;if(a.getChild()==null){this.toBeTiled[a.id]=!1;continue}if(!this.getToBeTiled(a))return this.toBeTiled[e]=!1,!1}return this.toBeTiled[e]=!0,!0},d.prototype.getNodeDegree=function(t){t.id;for(var e=t.getEdges(),r=0,i=0;i<e.length;i++){var o=e[i];o.getSource().id!==o.getTarget().id&&(r=r+1)}return r},d.prototype.getNodeDegreeWithChildren=function(t){var e=this.getNodeDegree(t);if(t.getChild()==null)return e;for(var r=t.getChild().getNodes(),i=0;i<r.length;i++){var o=r[i];e+=this.getNodeDegreeWithChildren(o)}return e},d.prototype.performDFSOnCompounds=function(){this.compoundOrder=[],this.fillCompexOrderByDFS(this.graphManager.getRoot().getNodes())},d.prototype.fillCompexOrderByDFS=function(t){for(var e=0;e<t.length;e++){var r=t[e];r.getChild()!=null&&this.fillCompexOrderByDFS(r.getChild().getNodes()),this.getToBeTiled(r)&&this.compoundOrder.push(r)}},d.prototype.adjustLocations=function(t,e,r,i,o){e+=i,r+=o;for(var a=e,s=0;s<t.rows.length;s++){var h=t.rows[s];e=a;for(var f=0,C=0;C<h.length;C++){var m=h[C];m.rect.x=e,m.rect.y=r,e+=m.rect.width+t.horizontalPadding,m.rect.height>f&&(f=m.rect.height)}r+=f+t.verticalPadding}},d.prototype.tileCompoundMembers=function(t,e){var r=this;this.tiledMemberPack=[],Object.keys(t).forEach(function(i){var o=e[i];r.tiledMemberPack[i]=r.tileNodes(t[i],o.paddingLeft+o.paddingRight),o.rect.width=r.tiledMemberPack[i].width,o.rect.height=r.tiledMemberPack[i].height})},d.prototype.tileNodes=function(t,e){var r=E.TILING_PADDING_VERTICAL,i=E.TILING_PADDING_HORIZONTAL,o={rows:[],rowWidth:[],rowHeight:[],width:0,height:e,verticalPadding:r,horizontalPadding:i};t.sort(function(h,f){return h.rect.width*h.rect.height>f.rect.width*f.rect.height?-1:h.rect.width*h.rect.height<f.rect.width*f.rect.height?1:0});for(var a=0;a<t.length;a++){var s=t[a];o.rows.length==0?this.insertNodeToRow(o,s,0,e):this.canAddHorizontal(o,s.rect.width,s.rect.height)?this.insertNodeToRow(o,s,this.getShortestRowIndex(o),e):this.insertNodeToRow(o,s,o.rows.length,e),this.shiftToLastRow(o)}return o},d.prototype.insertNodeToRow=function(t,e,r,i){var o=i;if(r==t.rows.length){var a=[];t.rows.push(a),t.rowWidth.push(o),t.rowHeight.push(0)}var s=t.rowWidth[r]+e.rect.width;t.rows[r].length>0&&(s+=t.horizontalPadding),t.rowWidth[r]=s,t.width<s&&(t.width=s);var h=e.rect.height;r>0&&(h+=t.verticalPadding);var f=0;h>t.rowHeight[r]&&(f=t.rowHeight[r],t.rowHeight[r]=h,f=t.rowHeight[r]-f),t.height+=f,t.rows[r].push(e)},d.prototype.getShortestRowIndex=function(t){for(var e=-1,r=Number.MAX_VALUE,i=0;i<t.rows.length;i++)t.rowWidth[i]<r&&(e=i,r=t.rowWidth[i]);return e},d.prototype.getLongestRowIndex=function(t){for(var e=-1,r=Number.MIN_VALUE,i=0;i<t.rows.length;i++)t.rowWidth[i]>r&&(e=i,r=t.rowWidth[i]);return e},d.prototype.canAddHorizontal=function(t,e,r){var i=this.getShortestRowIndex(t);if(i<0)return!0;var o=t.rowWidth[i];if(o+t.horizontalPadding+e<=t.width)return!0;var a=0;t.rowHeight[i]<r&&i>0&&(a=r+t.verticalPadding-t.rowHeight[i]);var s;t.width-o>=e+t.horizontalPadding?s=(t.height+a)/(o+e+t.horizontalPadding):s=(t.height+a)/t.width,a=r+t.verticalPadding;var h;return t.width<e?h=(t.height+a)/e:h=(t.height+a)/t.width,h<1&&(h=1/h),s<1&&(s=1/s),s<h},d.prototype.shiftToLastRow=function(t){var e=this.getLongestRowIndex(t),r=t.rowWidth.length-1,i=t.rows[e],o=i[i.length-1],a=o.width+t.horizontalPadding;if(t.width-t.rowWidth[r]>a&&e!=r){i.splice(-1,1),t.rows[r].push(o),t.rowWidth[e]=t.rowWidth[e]-a,t.rowWidth[r]=t.rowWidth[r]+a,t.width=t.rowWidth[instance.getLongestRowIndex(t)];for(var s=Number.MIN_VALUE,h=0;h<i.length;h++)i[h].height>s&&(s=i[h].height);e>0&&(s+=t.verticalPadding);var f=t.rowHeight[e]+t.rowHeight[r];t.rowHeight[e]=s,t.rowHeight[r]<o.height+t.verticalPadding&&(t.rowHeight[r]=o.height+t.verticalPadding);var C=t.rowHeight[e]+t.rowHeight[r];t.height+=C-f,this.shiftToLastRow(t)}},d.prototype.tilingPreLayout=function(){E.TILE&&(this.groupZeroDegreeMembers(),this.clearCompounds(),this.clearZeroDegreeMembers())},d.prototype.tilingPostLayout=function(){E.TILE&&(this.repopulateZeroDegreeMembers(),this.repopulateCompounds())},d.prototype.reduceTrees=function(){for(var t=[],e=!0,r;e;){var i=this.graphManager.getAllNodes(),o=[];e=!1;for(var a=0;a<i.length;a++)r=i[a],r.getEdges().length==1&&!r.getEdges()[0].isInterGraph&&r.getChild()==null&&(o.push([r,r.getEdges()[0],r.getOwner()]),e=!0);if(e==!0){for(var s=[],h=0;h<o.length;h++)o[h][0].getEdges().length==1&&(s.push(o[h]),o[h][0].getOwner().remove(o[h][0]));t.push(s),this.graphManager.resetAllNodes(),this.graphManager.resetAllEdges()}}this.prunedNodesAll=t},d.prototype.growTree=function(t){for(var e=t.length,r=t[e-1],i,o=0;o<r.length;o++)i=r[o],this.findPlaceforPrunedNode(i),i[2].add(i[0]),i[2].add(i[1],i[1].source,i[1].target);t.splice(t.length-1,1),this.graphManager.resetAllNodes(),this.graphManager.resetAllEdges()},d.prototype.findPlaceforPrunedNode=function(t){var e,r,i=t[0];i==t[1].source?r=t[1].target:r=t[1].source;var o=r.startX,a=r.finishX,s=r.startY,h=r.finishY,f=0,C=0,m=0,N=0,c=[f,m,C,N];if(s>0)for(var v=o;v<=a;v++)c[0]+=this.grid[v][s-1].length+this.grid[v][s].length-1;if(a<this.grid.length-1)for(var v=s;v<=h;v++)c[1]+=this.grid[a+1][v].length+this.grid[a][v].length-1;if(h<this.grid[0].length-1)for(var v=o;v<=a;v++)c[2]+=this.grid[v][h+1].length+this.grid[v][h].length-1;if(o>0)for(var v=s;v<=h;v++)c[3]+=this.grid[o-1][v].length+this.grid[o][v].length-1;for(var L=B.MAX_VALUE,D,O,F=0;F<c.length;F++)c[F]<L?(L=c[F],D=1,O=F):c[F]==L&&D++;if(D==3&&L==0)c[0]==0&&c[1]==0&&c[2]==0?e=1:c[0]==0&&c[1]==0&&c[3]==0?e=0:c[0]==0&&c[2]==0&&c[3]==0?e=3:c[1]==0&&c[2]==0&&c[3]==0&&(e=2);else if(D==2&&L==0){var M=Math.floor(Math.random()*2);c[0]==0&&c[1]==0?M==0?e=0:e=1:c[0]==0&&c[2]==0?M==0?e=0:e=2:c[0]==0&&c[3]==0?M==0?e=0:e=3:c[1]==0&&c[2]==0?M==0?e=1:e=2:c[1]==0&&c[3]==0?M==0?e=1:e=3:M==0?e=2:e=3}else if(D==4&&L==0){var M=Math.floor(Math.random()*4);e=M}else e=O;e==0?i.setCenter(r.getCenterX(),r.getCenterY()-r.getHeight()/2-T.DEFAULT_EDGE_LENGTH-i.getHeight()/2):e==1?i.setCenter(r.getCenterX()+r.getWidth()/2+T.DEFAULT_EDGE_LENGTH+i.getWidth()/2,r.getCenterY()):e==2?i.setCenter(r.getCenterX(),r.getCenterY()+r.getHeight()/2+T.DEFAULT_EDGE_LENGTH+i.getHeight()/2):i.setCenter(r.getCenterX()-r.getWidth()/2-T.DEFAULT_EDGE_LENGTH-i.getWidth()/2,r.getCenterY())},y.exports=d},function(y,w,l){var n={};n.layoutBase=l(0),n.CoSEConstants=l(1),n.CoSEEdge=l(2),n.CoSEGraph=l(3),n.CoSEGraphManager=l(4),n.CoSELayout=l(6),n.CoSENode=l(5),y.exports=n}])})}(S)),S.exports}export{z as r};
