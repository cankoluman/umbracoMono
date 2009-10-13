﻿/// <reference path="/umbraco_client/Application/NamespaceManager.js" />
/// <reference path="/umbraco_client/Application/UmbracoUtils.js" />
/// <reference path="/umbraco_client/ui/jquery.js" />
/// <reference path="/umbraco_client/ui/jqueryui.js" />
/// <reference path="tree_component.js" />
/// <reference path="/umbraco_client/Application/UmbracoApplicationActions.js" />
/// <reference path="NodeDefinition.js" />
/// <reference name="MicrosoftAjax.js"/>

Umbraco.Sys.registerNamespace("Umbraco.Controls");

(function($) {
    $.fn.UmbracoTree = function(opts) {
        return this.each(function() {

            var conf = $.extend({
                jsonFullMenu: null,
                jsonInitNode: null,
                appActions: null,
                uiKeys: null,
                app: "",
                showContext: true,
                isDialog: false,
                treeType: "standard",
                umb_clientFolderRoot: "/umbraco_client", //default setting... this gets overriden.
                recycleBinId: -20 //default setting for content tree
            }, opts);
            new Umbraco.Controls.UmbracoTree().init($(this), conf);
        });
    };
    $.fn.UmbracoTreeAPI = function() {
        /// <summary>exposes the Umbraco Tree api for the selected object</summary>
        return $(this).data("UmbracoTree") == null ? null : $(this).data("UmbracoTree");
    };

    Umbraco.Controls.UmbracoTree = function() {
        /// <summary>
        /// The object that manages the Umbraco tree.
        /// Has these events: syncNotFound, syncFound, rebuiltTree, newchildNodeFound, nodeMoved, nodeCopied, ajaxError, nodeClicked
        /// </summary>
        return {
            _actionNode: new Umbraco.Controls.NodeDefinition(), //the most recent node right clicked for context menu
            _activeTreeType: "content", //tracks which is the active tree type, this is used in searching and syncing.
            _recycleBinId: -20,
            _umb_clientFolderRoot: "/umbraco_client", //this should be set externally!!!
            _fullMenu: null,
            _initNode: null,
            _appActions: null,
            _tree: null, //reference to the jsTree object
            _uiKeys: null,
            _container: null,
            _app: null, //the reference to the current app
            _showContext: true,
            _isEditMode: false,
            _isDialog: false,
            _isDebug: false, //set to true to enable alert debugging
            _loadedApps: [], //stores the application names that have been loaded to track which JavaScript code has been inserted into the DOM
            _serviceUrl: "", //a path to the tree client service url
            _dataUrl: "", //a path to the tree data service url
            _treeType: "standard", //determines the type of tree: 'standard', 'checkbox' = checkboxes enabled
            _treeClass: "umbTree", //used for other libraries to detect which elements are an umbraco tree
            _currenAJAXRequest: false, //used to determine if there is currently an ajax request being executed.

            addEventHandler: function(fnName, fn) {
                /// <summary>Adds an event listener to the event name event</summary>
                $(this).bind(fnName, fn);
            },

            removeEventHandler: function(fnName, fn) {
                /// <summary>Removes an event listener to the event name event</summary>
                $(this).unbind(fnName, fn);
            },

            init: function(jItem, opts) {
                /// <summary>Initializes the tree with the options and stores the tree API in the jQuery data object for the current element</summary>
                this._init(opts.jsonFullMenu, opts.jsonInitNode, jItem, opts.appActions, opts.uiKeys, opts.app, opts.showContext, opts.isDialog, opts.treeType, opts.serviceUrl, opts.dataUrl, opts.umb_clientFolderRoot, opts.recycleBinId);
                //store the api
                jItem.addClass(this._treeClass);
                jItem.data("UmbracoTree", this);
            },

            setRecycleBinNodeId: function(id) {
                this._recycleBinId = id;
            },

            clearTreeCache: function() {
                // <summary>This will remove all stored trees in client side cache so that the next time a tree needs loading it will be refreshed</summary>
                this._debug("clearTreeCache...");

                for (var a in this._loadedApps) {
                    this._debug("clearTreeCache: " + this._loadedApps[a]);
                    this._container.data("tree_" + this._loadedApps[a], null);
                }
            },

            toggleEditMode: function(enable) {
                this._debug("Edit mode. Currently: " + this._tree.settings.rules.draggable);
                this._isEditMode = enable;
                
                this.saveTreeState(this._app);
                //need to trick the system so it thinks it's a different app, then rebuild with new rules
                var app = this._app;
                this._app = "temp";
                this.rebuildTree(app);
                
                this._debug("Edit mode. New Mode: " + this._tree.settings.rules.draggable);
                this._appActions.showSpeachBubble("info", "Tree Edit Mode", "The tree is now operating in edit mode");
            },

            rebuildTree: function(app) {
                /// <summary>This will rebuild the tree structure for the application specified</summary>

                this._debug("rebuildTree");

                //are we already on the app being requested to load?
                if (this._app == null || (this._app.toLowerCase() == app.toLowerCase())) {
                    this._debug("not rebuilding");
                    return;
                }
                else {
                    this._app = app;
                }
                //kill the tree
                this._tree.destroy();
                this._container.hide();
                var _this = this;

                //check if we should rebuild from a saved tree
                var saveData = this._container.data("tree_" + app);
                if (saveData != null) {
                    this._debug("rebuildTree: rebuilding from cache: app = " + app);

                    //create the tree from the saved data.
                    //this._initNode = saveData.d;
                    this._tree = $.tree.create();
                    this._tree.init(this._container, this._getInitOptions(saveData.d));
                    
                    //this._tree.rename = this._umbracoRename; //replaces the jsTree rename method

                    this._configureNodes(this._container.find("li"), true);
                    //select the last node
                    var lastSelected = saveData.selected != null ? $(saveData.selected[0]).attr("id") : null;
                    if (lastSelected != null) {
                        //create an event handler for the tree sync
                        var _this = this;
                        var foundHandler = function(EV, node) {
                            //remove the event handler from firing again
                            _this.removeEventHandler("syncFound", foundHandler);
                            this._debug("rebuildTree: node synced, selecting node...");
                            //ensure the node is selected, ensure the event is fired and reselect is true since jsTree thinks this node is already selected by id
                            _this.selectNode(node, false, true);
                            this._container.show();
                        };
                        this._debug("rebuildTree: syncing to last selected: " + lastSelected);
                        //add the event handler for the tree sync and sync the tree
                        this.addEventHandler("syncFound", foundHandler);
                        this.setActiveTreeType($(saveData.selected[0]).attr("umb:type"));
                        this.syncTree(lastSelected);
                    }
                    else {
                        this._container.show();
                    }

                    return;
                }

                //need to get the init node for the new app
                var parameters = "{'app':'" + app + "','showContextMenu':'" + this._showContext + "', 'isDialog':'" + this._isDialog + "'}"
                this._currentAJAXRequest = true;
                
                _this._tree = $.tree.create();
                _this._tree.init(_this._container, _this._getInitOptions());

            },

            saveTreeState: function(appAlias) {
                /// <summary>
                /// Saves the state of the current application trees so we can restore it next time the user visits the app
                /// </summary>

                this._debug("saveTreeState: " + appAlias + " : ajax request? " + this._currentAJAXRequest);

                //clear the saved data for the current app before saving
                this._container.data("tree_" + appAlias, null);
                
                //if an ajax request is currently in progress, abort saving the tree state and set the 
                //data object for the application to null.
                if (!this._currentAJAXRequest) {                                                                    
                    //only save the data if there are nodes
                    var nodeCount = this._container.find("li[rel='dataNode']").length;
                    if (nodeCount > 0) {
                        this._debug("saveTreeState: node count = " + nodeCount);
                        var treeData = this._tree.get();                    
                        //need to update the 'state' of the data. jsTree get doesn't return the state of nodes properly!
                        this._updateJSONNodeState(treeData);                    
                        this._debug("saveTreeState: treeData = " + treeData);
                        this._container.data("tree_" + appAlias, { selected: this._tree.selected, d: treeData });
                    }
                }
            },

            _updateJSONNodeState: function(obj) {
                /// <summary>
                /// A recursive function to store the state of the node for the JSON object when using saveTreeState.
                /// This is required since jsTree doesn't output the state of the tree nodes with the request to getJSON method.
                /// This is also required to save the correct title for each node since we store our title in a div tag, not just the a tag
                /// </summary>              

                var node = $("li[id='" + obj.attributes.id + "']").filter(function() {
                    return ($(this).attr("umb:type") == obj.attributes["umb:type"]); //filter based on custom namespace requires custom function
                });
                
                //saves the correct title
                obj.data.title = $.trim(node.children("a").children("div").text());
                obj.state = obj.data.state;
                
                //recurse through children
                if (obj.children != null) {
                    for (var x in obj.children) {
                        this._updateJSONNodeState(obj.children[x]);
                    }
                }
            },

            syncTree: function(path, forceReload) {
                /// <summary>
                /// Syncronizes the tree with the path supplied and makes that node visible/selected.
                /// </summary>
                /// <param name="path">The path of the node</param>
                /// <param name="forceReload">If true, will ensure that the node to be synced is synced with data from the server</param>

                this._debug("syncTree: " + path + ", " + forceReload);

                this._syncTree.call(this, path, forceReload, null, null);

            },

            childNodeCreated: function() {
                /// <summary>
                /// Reloads the children of the current action node and selects the node that didn't exist there before.
                /// If it cannot determine which node is new, then no node is selected.	If the children are not already
                /// loaded, then it is impossible for this method to determine which child is new.	
                /// </summary>

                this._debug("childNodeCreated");

                //store the current child ids so we can determine which one is the new one
                var childrenIds = new Array();
                this._actionNode.jsNode.find("ul > li").each(function() {
                    childrenIds.push($(this).attr("id"));
                });
                var _this = this;
                var currId = this._actionNode.nodeId;
                this.reloadActionNode(true, false, function(success) {
                    if (success && childrenIds.length > 0) {
                        var found = false;
                        var actionNode = _this.findNode(currId);
                        if (actionNode) {
                            actionNode.find("ul > li").each(function() {
                                //if the id of the current child is not found in the original list, then this is the new one, store it
                                if ($.inArray($(this).attr("id"), childrenIds) == -1) {
                                    found = $(this);
                                }
                            });
                        }
                        if (found) {
                            _this._debug("childNodeCreated: selecting new child node: " + found.attr("id"));
                            _this.selectNode(found, true, true);
                            $(_this).trigger("newChildNodeFound", [found]);
                            return;
                        }
                    }
                    _this._debug("childNodeCreated: could not select new child!");
                });
            },

            moveNode: function(nodeId, parentPath) {
                /// <summary>Moves a node in the tree. This will remove the existing node by id and sync the tree to the new path</summary>

                this._debug("moveNode");

                //remove the old node
                var old = this.findNode(nodeId);
                if (old) old.remove();

                //build the path to the new node
                var newPath = parentPath + "," + nodeId;
                //create an event handler for the tree sync
                var _this = this;
                var foundHandler = function(EV, node) {
                    //remove the event handler from firing again
                    _this.removeEventHandler("syncFound", foundHandler);
                    //ensure the node is selected, ensure the event is fired and reselect is true since jsTree thinks this node is already selected by id
                    _this.selectNode(node, false, true);
                    $(_this).trigger("nodeMoved", [node]);
                };
                //add the event handler for the tree sync and sync the tree
                this.addEventHandler("syncFound", foundHandler);
                this.syncTree(newPath);
            },

            copyNode: function(nodeId, parentPath) {
                /// <summary>Copies a node in the tree. This will keep the current node selected but will sync the tree to show the copied node too</summary>

                this._debug("copyNode");

                var originalNode = this.findNode(nodeId);

                //create an event handler for the tree sync
                var _this = this;
                var foundHandler = function(EV, node) {
                    //remove the event handler from firing again
                    _this.removeEventHandler("syncFound", foundHandler);
                    //now that the new parent node is found, expand it
                    _this._loadChildNodes(node, null);
                    //reselect the original node since sync will select the one that was copied
                    if (originalNode) _this.selectNode(originalNode, true);
                    $(_this).trigger("nodeCopied", [node]);
                };
                //add the event handler for the tree sync and sync the to the parent path
                this.addEventHandler("syncFound", foundHandler);
                this.syncTree(parentPath);
            },

            findNode: function(nodeId, findGlobal) {
                /// <summary>Returns either the found branch or false if not found in the tree</summary>
                /// <param name="findGlobal">Optional. If true, disregards the tree type and searches the entire tree for the id</param>
                var _this = this;
                var branch = this._container.find("li[id='" + nodeId + "']");
                if (!findGlobal) branch = branch.filter(function() {
                    return ($(this).attr("umb:type") == _this._activeTreeType); //filter based on custom namespace requires custom function
                });
                var found = branch.length > 0 ? branch : false;
                this._debug("findNode: " + nodeId + " in '" + this._activeTreeType + "' tree. Found? " + found.length);
                return found;
            },

            selectNode: function(node, supressEvent, reselect) {
                /// <summary>
                /// Makes the selected node the active node, but only if it is not already selected or if reselect is true.            
                /// </summary>
                /// <param name="supressEvent">If set to true, will select the node but will supress the onSelected event</param>
                /// <param name="reselect">If set to true, will call the select_branch method even if the node is already selected</param>
            
                this._debug("selectNode, edit mode? " + this._isEditMode);

                var selectedId = this._tree.selected != null ? $(this._tree.selected[0]).attr("id") : null;
                if (reselect || (selectedId == null || selectedId != node.attr("id"))) {
                    //if we don't wan the event to fire, we'll set the callback to a null method and set it back after we call the select_branch method
                    if (supressEvent || this._isEditMode) {
                        this._tree.settings.callback.onselect = function() { };
                    }
                    this._tree.select_branch(node);
                    //reset the method / maintain scope in callback
                    var _this = this;
                    this._tree.settings.callback.onselect = function(N, T) { _this.onSelect(N, T) };
                }
                
            },

            reloadActionNode: function(supressSelect, supressChildReload, callback) {
                /// <summary>
                /// Gets the current action node's parent's data source url, then passes this url and the current action node's id
                /// to a web service. The webservice will find the JSON data for the current action node and return it. This
                /// will parse the returned JSON into html and replace the current action nodes' markup with the refreshed server data.
                /// If by chance, the ajax call fails because of inconsistent data (a developer has implemented poor tree design), then
                /// this use the build in jsTree reload which works ok.
                /// </summary>
                /// <param name="callback">
                /// A callback function which will have a boolean parameter passed. True = the reload was succesful,
                /// False = the reload failed and the generic _tree.refresh() method was used.
                /// </param>
                this._debug("reloadActionNode: supressSelect = " + supressSelect + ", supressChildReload = " + supressChildReload);

                if (this._actionNode != null && this._actionNode.jsNode != null) {
                    var nodeParent = this._actionNode.jsNode.parents("li:first");
                    this._debug("reloadActionNode: found " + nodeParent.length + " parent nodes");
                    if (nodeParent.length == 1) {
                        var nodeDef = this.getNodeDef(nodeParent);
                        this._debug("reloadActionNode: loading ajax for node: " + nodeDef.nodeId);
                        var _this = this;
                        //replace the node to refresh with loading and return the new loading element
                        var toReplace = $("<li class='last'><a class='loading' href='#'><ins></ins><div>" + (this._tree.settings.lang.loading || "Loading ...") + "</div></a></li>").replaceAll(this._actionNode.jsNode);
                        $.get(this._getUrl(nodeDef.sourceUrl), null,
                            function(msg) {
                                if (!msg || msg.length == 0) {
                                    _this._debug("reloadActionNode: error loading ajax data, performing jsTree refresh");
                                    _this._tree.refresh(); /*try jsTree refresh as last resort */
                                    if (callback != null) callback.call(_this, false);
                                    return;
                                }
                                //filter the results to find the object corresponding to the one we want refreshed
                                var oFound = null;
                                for (var o in msg) {
                                    if (msg[o].attributes != null && msg[o].attributes.id == _this._actionNode.nodeId) {
                                        oFound = $.tree.datastores.json().parse(msg[o], _this._tree);
                                        //ensure the tree type is the same too
                                        if ($(oFound).attr("umb:type") == _this._actionNode.treeType) { break; }
                                        else { oFound = null; }
                                    }
                                }
                                if (oFound != null) {
                                    _this._debug("reloadActionNode: node is refreshed! : " + supressSelect);
                                    var reloaded = $(oFound).replaceAll(toReplace);
                                    _this._configureNodes(reloaded, true);
                                    if (!supressSelect) _this.selectNode(reloaded, true, true);
                                    if (!supressChildReload) {
                                        _this._loadChildNodes(reloaded, function() {
                                            if (callback != null) callback.call(_this, true);
                                        });
                                    }
                                    else { if (callback != null) callback.call(_this, true); }
                                }
                                else {
                                    _this._debug("reloadActionNode: error finding child node in ajax data, performing jsTree refresh");
                                    _this._tree.refresh(); /*try jsTree refresh as last resort */
                                    if (callback != null) callback.call(_this, false);
                                }
                            }, "json");
                        return;
                    }

                    this._debug("reloadActionNode: error finding parent node, performing jsTree refresh");
                    this._tree.refresh(); /*try jsTree refresh as last resort */
                    if (callback != null) callback.call(this, false);
                }
            },

            getActionNode: function() {
                /// <summary>Returns the latest node interacted with</summary>
                this._debug("getActionNode: " + this._actionNode.nodeId);
                return this._actionNode;
            },

            setActiveTreeType: function(treeType) {
                /// <summary>
                /// All interactions with the tree are done so based on the current tree type (i.e. content, media).
                /// When sycning, or searching, the operations will be done on the current tree type so developers
                /// can explicitly specify on with this method before performing the operations.
                /// The active tree type is always updated any time a node interaction takes place.
                /// </summary>

                this._activeTreeType = treeType;
            },

            onNodeDeleting: function(EV) {
                /// <summary>Event handler for when a tree node is about to be deleted</summary>

                this._debug("onNodeDeleting")

                //first, close the branch
                this._tree.close_branch(this._actionNode.jsNode);
                //show the ajax loader with deleting text
                this._actionNode.jsNode.find("a").attr("class", "loading");
                this._actionNode.jsNode.find("a").css("background-image", "");
                this._actionNode.jsNode.find("a").html(this._uiKeys['deleting']);
            },

            onNodeDeleted: function(EV) {
                /// <summary>Event handler for when a tree node is deleted after ajax call</summary>

                this._debug("onNodeDeleted");

                //remove the ajax loader
                this._actionNode.jsNode.find("a").removeClass("loading");
                //ensure the branch is closed
                this._tree.close_branch(this._actionNode.jsNode);
                //make the node disapear
                this._actionNode.jsNode.hide("drop", { direction: "down" }, 400);

                this._updateRecycleBin();
            },

            onNodeRefresh: function(EV) {
                /// <summary>Handles the nodeRefresh event of the context menu and does the refreshing</summary>

                this._debug("onNodeRefresh");

                this._loadChildNodes(this._actionNode.jsNode, null);
            },

            onSelect: function(NODE, TREE_OBJ) {
                /// <summary>Fires the JS associated with the node, if the tree is in edit mode, allows for rename instead</summary>
                this._debug("onSelect, edit mode? " + this._isEditMode);
                 if (this._isEditMode) {
                    this._tree.rename(NODE);
                    return false;
                }
                else {
                    this.setActiveTreeType($(NODE).attr("umb:type"));
                    var js = $(NODE).children("a").attr("href").replace("javascript:", "");

                    this._debug("onSelect: js: " + js);

                    try {
                        var func = eval(js);
                        if (func != null) {
                            func.call();
                        }
                    } catch (e) { }

                    return true;
                }
                
                
            },

            onBeforeOpen: function(NODE, TREE_OBJ) {
                /// <summary>Before opening child nodes, ensure that the data method and url are set properly</summary>
                this._currentAJAXRequest = true;                
                TREE_OBJ.settings.data.opts.url = this._dataUrl;
                TREE_OBJ.settings.data.opts.method = "GET";
            },

            onJSONData: function(DATA, TREE_OBJ) {
                this._debug("onJSONData");
                
                this._currentAJAXRequest = false;
                
                if (typeof DATA.d != "undefined") {
                    
                    var msg = DATA.d;                
                    //recreates the tree
                    if ($.inArray(msg.app, this._loadedApps) == -1) {
                        this._debug("loading js for app: " + msg.app);
                        this._loadedApps.push(msg.app);
                        //inject the scripts
                        this._container.after("<script>" + msg.js + "</script>");
                    }
                    return eval(msg.json);
                }
                
                return DATA;
            },
            
            onBeforeRequest: function(NODE, TREE_OBJ) {
                if (TREE_OBJ.settings.data.opts.method == "POST") {
                    var parameters = "{'app':'" + this._app + "','showContextMenu':'" + this._showContext + "', 'isDialog':'" + this._isDialog + "'}"
                    return parameters; 
                }
                else {
                    var nodeDef = this.getNodeDef($(NODE));
                    return this._getUrlParams(nodeDef.sourceUrl);
                }
            },
            
            onChange: function(NODE, TREE_OBJ) {
                //bubble an event!
                $(this).trigger("nodeClicked", [NODE]);
            },

            onBeforeContext: function(NODE, TREE_OBJ, EV) {
                
                //update the action node's NodeDefinition and set the active tree type
                this._actionNode = this.getNodeDef($(NODE));
                this.setActiveTreeType($(NODE).attr("umb:type"));

                this._debug("onBeforeContext: " + this._actionNode.menu);
                
                return this._actionNode.menu;              
            },
                
            onLoad: function(TREE_OBJ) {
                /// <summary>When the application first loads, load the child nodes</summary>
                
                this._debug("onLoad");
                
                this._container.show();
                //ensure the static data is gone
                this._tree.settings.data.opts.static = null;
                var _this = this;
                _this._loadChildNodes($(_this._container).find("li"), null);
            },
            
            onBeforeMove: function(NODE,REF_NODE,TYPE,TREE_OBJ) {
                /// <summary>
                /// First, check if it's a move or a sort
                /// Second, check for move or sort permissions, depending on the request 
                /// Third, 
                /// </summary>
    
                var nodeDef = this.getNodeDef($(NODE));
                var nodeParent = nodeDef.jsNode.parents("li:first");
                var nodeParentDef = this.getNodeDef(nodeParent);
                
                var refNodeDef = this.getNodeDef($(REF_NODE));
                
                this._debug("onBeforeMove, TYPE: " + TYPE);                
                this._debug("onBeforeMove, NODE ID: " + nodeDef.nodeId);
                this._debug("onBeforeMove, PARENT NODE ID: " + nodeParentDef.nodeId);
                this._debug("onBeforeMove, REF NODE ID: " + refNodeDef.nodeId);
                
                switch(TYPE){
                    case "inside":
                        if (nodeParentDef.nodeId == refNodeDef.nodeId) {
                            //moving to the same node!
                            this._appActions.showSpeachBubble("warning", "Tree Edit Mode", "Cannot move a node to it's same parent node");
                            return false;
                        }
                        //check move permissions, then attempt move
                        
                        break;
                    case "before":
                        //if (nodeParentDef.nodeId == nodeDef.
                        break;
                    case "after":
                        break;
                }
                
                
                //this._currentAJAXRequest = true; 
                
                return false;
            },
            
            onParse: function(STR, TREE_OBJ) {
                this._debug("onParse");
                
                var obj = $(STR);
                this._configureNodes(obj);
                //this will return the full html of the configured node
                return $('<div>').append($(obj).clone()).remove().html();
            },
            
            _debug: function(strMsg) {
                if (this._isDebug) {
                    Sys.Debug.trace("UmbracoTree: " + strMsg);
                }
            },

            _configureNodes: function(nodes, reconfigure) {
                /// <summary>
                /// Ensures the node is configured properly after it's loaded via ajax.
                /// This includes setting overlays and ensuring the correct icon paths are used.
                /// This also ensures that the correct markup is rendered for the tree (i.e. inserts html nodes for text, etc...)
                /// </summary>

                var _this = this;

                //don't process the nodes that have already been loaded, unless reconfigure is true
                if (!reconfigure) {
                    nodes = nodes.not("li[class*='loaded']");
                }

                this._debug("_configureNodes: " + nodes.length);

                var rxInput = new RegExp("\\boverlay-\\w+\\b", "gi");
                nodes.each(function() {
                    //if it is checkbox tree (not standard), don't worry about overlays and remove the default icon.
                    if (_this._treeType != "standard") {
                        $(this).children("a:first").css("background", "");
                        return;
                    }
                    //remove all overlays if reconfiguring
                    $(this).children("div").remove();
                    var m = $(this).attr("class").match(rxInput);
                    if (m != null) {
                        for (i = 0; i < m.length; i++) {
                            _this._debug("_configureNodes: adding overlay: " + m[i] + " for node: " + $(this).attr("id"));
                            $(this).children("a:first").before("<div class='overlay " + m[i] + "'></div>");
                        }
                    }
                    //create a div for the text
                    var a = $(this).children("a");
                    var ins = a.children("ins");
                    var txt = $("<div>" + a.text() + "</div>");
                    //check if it's not a sprite, if not then move the ins node just after the anchor, otherwise remove                    
                    if (a.hasClass("noSpr")) {
                        a.attr("style", ins.attr("style"));
                    }
                    else {                        
                        
                    }
                    a.html(txt);
                    ins.remove();
                    //add the loaded class to each element so we know not to process it again
                    $(this).addClass("loaded");
                });
            },

            getNodeDef: function(NODE) {
                /// <summary>Converts a jquery node with metadata to a NodeDefinition</summary>

                //get our meta data stored with our node
                var nodedata = $(NODE).children("a").metadata({ type: 'attr', name: 'umb:nodedata' });
                this._debug("getNodeDef: " + $(NODE).attr("id") + ", " + nodedata.nodeType + ", " + nodedata.source);
                var def = new Umbraco.Controls.NodeDefinition();
                def.updateDefinition(this._tree, $(NODE), $(NODE).attr("id"), $(NODE).find("a > div").html(), nodedata.nodeType, nodedata.source, nodedata.menu, $(NODE).attr("umb:type"));
                return def;
            },

            _updateRecycleBin: function() {
                this._debug("_updateRecycleBin BinId: " + this._recycleBinId);

                var rNode = this.findNode(this._recycleBinId, true);
                if (rNode) {
                    this._actionNode = this.getNodeDef(rNode);
                    var _this = this;
                    this.reloadActionNode(true, true, function(success) {
                        if (success) {
                            _this.findNode(_this._recycleBinId, true).effect("highlight", {}, 1000);
                        }
                    });
                }
            },

            _loadChildNodes: function(liNode, callback) {
                /// <summary>jsTree won't allow you to open a node that doesn't explitly have childen, this will force it to try</summary>
                /// <param name="node">a jquery object for the current li node</param>

                this._debug("_loadChildNodes: " + liNode);

                liNode.removeClass("leaf");
                this._tree.close_branch(liNode, true);
                liNode.children("ul:eq(0)").html("");
                this._tree.open_branch(liNode, false, callback);
            },

            _syncTree: function(path, forceReload, numPaths, numAsync) {
                /// <summary>
                /// This is the internal method that will recursively search for the nodes to sync. If an invalid path is 
                /// passed to this method, it will raise an event which can be handled.
                /// </summary>
                /// <param name="path">The path of the node to find</param>
                /// <param name="forceReload">If true, will ensure that the node to be synced is synced with data from the server</param>
                /// <param name="numPaths">the number of id's deep to search starting from the end of the path. Used in recursion.</param>
                /// <param name="numAsync">the number of async calls made so far to sync. Used in recursion and used to determine if the found node has been loaded by ajax.</param>

                this._debug("_syncTree");

                var paths = path.split(",");
                var found = null;
                var foundIndex = null;
                if (numPaths == null) numPaths = (paths.length - 0);
                for (var i = 0; i < numPaths; i++) {
                    foundIndex = paths.length - (1 + i);
                    found = this.findNode(paths[foundIndex]);
                    this._debug("_syncTree: finding... " + paths[foundIndex] + " found? " + found);
                    if (found) break;
                }

                //if no node has been found at all in the entire path, then bubble an error event
                if (!found) {
                    this._debug("no node found in path: " + path + " : " + numPaths);
                    $(this).trigger("syncNotFound", [path]);
                    return;
                }

                //if the found node was not the end of the path, we need to load them in recursively.
                if (found.attr("id") != paths[paths.length - 1]) {
                    var _this = this;
                    this._loadChildNodes(found, function(NODE, TREE_OBJ) {
                        //check if the next node to be found is in the children, if it is not, there's a problem bubble an event!
                        var pathsToSearch = paths.length - (Number(foundIndex) + 1);
                        if (_this.findNode(paths[foundIndex + 1])) {
                            _this._syncTree(path, forceReload, pathsToSearch, (numAsync == null ? numAsync == 1 : ++numAsync));
                        }
                        else {
                            _this._debug("node not found in children: " + path + " : " + numPaths);
                            $(this).trigger("syncNotFound", [path]);
                        }
                    });
                }
                else {
                    //only force the reload of this nodes data if forceReload is specified and the node has not already come from the server
                    var doReload = (forceReload && (numAsync == null || numAsync < 1));
                    this._debug("_syncTree: found! numAsync: " + numAsync + ", forceReload: " + forceReload);
                    if (doReload) {
                        this._actionNode = this.getNodeDef(found);
                        this.reloadActionNode(false, true, null);
                    }
                    else {
                        //we have found our node, select it but supress the selecting event
                        if (found.attr("id") != "-1") this.selectNode(found, true);
                        this._configureNodes(found, doReload);
                    }
                    //bubble event
                    $(this).trigger("syncFound", [found]);
                }
            },

            _init: function(jFullMenu, jInitNode, treeContainer, appActions, uiKeys, app, showContext, isDialog, treeType, serviceUrl, dataUrl, umbClientFolder, recycleBinId) {
                /// <summary>initialization method, must be called on page ready.</summary>
                /// <param name="jFullMenu">JSON markup for the full context menu in accordance with the jsTree context menu object standard</param>
                /// <param name="jInitNode">JSON markup for the initial node to show</param>
                /// <param name="treeContainer">the jQuery element to be tree enabled</param>
                /// <param name="appActions">A reference to a MenuActions object</param>
                /// <param name="uiKeys">A reference to a uiKeys object</param>
                /// <param name="showContext">boolean indicating whether or not to show a context menu</param>
                /// <param name="isDialog">boolean indicating whether or not the tree is in dialog mode</param>
                /// <param name="treeType">determines the type of tree: false/null = normal, 'checkbox' = checkboxes enabled, 'inheritedcheckbox' = parent nodes have checks inherited from children</param>
                /// <param name="serviceUrl">Url path for the tree client service</param>
                /// <param name="dataUrl">Url path for the tree data service</param>
                /// <param name="umbClientFolder">Should be set externally!... the root to the umbraco_client folder</param>
                /// <param name="recycleBinId">the id of the recycle bin for the current tree</param>

                this._debug("_init: creating new tree with class/id: " + treeContainer.attr("class") + " / " + treeContainer.attr("id"));

                this._fullMenu = jFullMenu;
                this._initNode = jInitNode;
                this._appActions = appActions;
                this._uiKeys = uiKeys;
                this._app = app;
                this._showContext = showContext;
                this._isDialog = isDialog;
                this._treeType = treeType;
                this._serviceUrl = serviceUrl;
                this._dataUrl = dataUrl;
                this._umb_clientFolderRoot = umbClientFolder;
                this._recycleBinId = recycleBinId;

                //wire up event handlers
                if (this._appActions != null) {
                    var _this = this;
                    //wrapped functions maintain scope
                    this._appActions.addEventHandler("nodeDeleting", function(E) { _this.onNodeDeleting(E) });
                    this._appActions.addEventHandler("nodeDeleted", function(E) { _this.onNodeDeleted(E) });
                    this._appActions.addEventHandler("nodeRefresh", function(E) { _this.onNodeRefresh(E) });
                }

                //initializes the jsTree
                this._container = treeContainer;
                this._tree = $.tree.create();
                this._tree.init(this._container, this._getInitOptions());
                //this._tree.rename = this._umbracoRename; //replaces the jsTree rename method
                
                //add this app to the loaded apps array
                //if ($.inArray(app, this._loadedApps) == -1) {
                //    this._loadedApps.push(app);
                //}

                //load child nodes of the init node
                //this._loadChildNodes(this._container.find("li:first"), null);
            },

//            _umbracoRename : function (obj) {
//                /// <summary>A modified version of the original jsTree rename method. We need to use our own since
//                /// we've modified the rendering so much. This method replaces the tree rename method.
//                /// 'this' in this method context is jsTree.
//                /// </summary>
//				if(this.locked) return this.error("LOCKED");
//				obj = obj ? this.get_node(obj) : this.selected;
//				var _this = this;
//				if(!obj || !obj.size()) return this.error("RENAME: NO NODE SELECTED");
//				if(!this.check("renameable", obj)) return this.error("RENAME: NODE NOT RENAMABLE");
//				if(!this.settings.callback.beforerename.call(null,obj.get(0), _this.current_lang, _this)) return this.error("RENAME: STOPPED BY USER");

//				obj.parents("li.closed").each(function () { _this.open_branch(this) });
//				//if(this.current_lang)	obj = obj.find("a." + this.current_lang).get(0);
//				//else					obj = obj.find("a:first").get(0);
//				obj = obj.find("a:first div");				
//				last_value = obj.html();
//				_this.inp = $("<input type='text' autocomplete='off' />");
//				_this.inp
//					.val(last_value.replace(/&amp;/g,"&").replace(/&gt;/g,">").replace(/&lt;/g,"<"))
//					.bind("mousedown",		function (event) { event.stopPropagation(); })
//					.bind("mouseup",		function (event) { event.stopPropagation(); })
//					.bind("click",			function (event) { event.stopPropagation(); })
//					.bind("keyup",			function (event) { 
//							var key = event.keyCode || event.which;
//							if(key == 27) { this.value = last_value; this.blur(); return }
//							if(key == 13) { this.blur(); return }
//						});
//				// Rollback
//				var rb = {}; 
//				rb[this.container.attr("id")] = this.get_rollback();
//					
//				
//				var spn = $("<div />").addClass($(obj).parent().attr("class")).addClass("renaming").append(_this.inp);
//				spn.attr("style", $(obj).attr("style"));
//				obj.parent().hide();
//				
//				obj.parents("li:first").prepend(spn);
//				//_this.inp.get(0).focus();
//				//_this.inp.get(0).select();
//				
////				_this.inp.blur(function(event) {
////						if(this.value == "") this.value = last_value; 
////						var li = obj.parents("li:first")
////						obj.html(li.find("input").val());
////						obj.parent().show(); 
////						li.find("div.renaming").remove(); 
////						_this.settings.callback.onrename.call(null, _this.get_node(li).get(0), _this.current_lang, _this, rb);
////						_this.inp = false;
////					});
//			},

            _getUrlParams: function(nodeSource) {
                /// <summary>This converts Url query string params to json</summary>
                var p = {};
                var sp = nodeSource.split("?")[1].split("&");
                for(var i=0;i<sp.length;i++) {
                    var e = sp[i].split("=");
                    p[e[0]] = e[1];
                }
                p["rnd2"] = Umbraco.Utils.generateRandom();
                return p;
            },
            
            _getUrl: function(nodeSource) {
                /// <summary>Returns the json service url</summary>

                if (nodeSource == null || nodeSource == "") {
                    return this._dataUrl;
                }
                var params = nodeSource.split("?")[1];
                return this._dataUrl + "?" + params + "&rnd2=" + Umbraco.Utils.generateRandom();
            },

            _getInitOptions: function(initData) {
                /// <summary>return the initialization objects for the tree</summary>

                this._debug("_getInitOptions");

                var _this = this;

                var options = {
                    data: {
                        type: "json",
                        async: true,
                        opts : {
                            static: initData == null ? null: initData,
                            method: "POST",
                            url: _this._serviceUrl,
                            outer_attrib: ["id", "umb:type", "class", "rel"],
                            inner_attrib: ["umb:nodedata", "href", "class", "style"]
                        }
                    },
                    ui: {
                        dots: false,
                        rtl: false,
                        animation: false,
                        hover_mode: true,
                        theme_path: this._umb_clientFolderRoot + "/Tree/Themes/",
                        theme_name: "umbraco"
                    },
                    langs: {
                        new_node: "New folder",
                        loading: "<div>" + (this._tree.settings.lang.loading || "Loading ...") + "</div>"
                    },
                    callback: {
                        //ensures that the node id isn't appended to the async url
                        beforedata: function(N, T) { return _this.onBeforeRequest(N,T); },
                        //wrapped functions maintain scope in callback
                        beforemove  : function(N,RN,TYPE,T) { _this.onBeforeMove(N,RN,TYPE,T); },
                        beforeopen: function(N, T) { _this.onBeforeOpen(N, T); },
                        onselect: function(N, T) { _this.onSelect(N, T); },
                        onchange: function(N, T) { _this.onChange(N, T); },
                        ondata: function(D, T) { return _this.onJSONData(D, T); },
                        onload: function(T) { if (initData == null) _this.onLoad(T); },
                        onparse: function(S,T) { return _this.onParse(S,T); }
                    },
                    plugins: {
                        //UmbracoContext comes before context menu so that the events fire first
                        UmbracoContext: {
                            fullMenu: _this._fullMenu,
                            onBeforeContext: function(N,T,E) { return _this.onBeforeContext(N,T,E); }
                        },
                        contextmenu: {}
                    }
                };
                if (this._treeType != "standard") {
                    options.plugins.checkbox = {three_state:false}
                }
                
                //set global ajax settings:
                $.ajaxSetup({
                  contentType: "application/json; charset=utf-8"
                });

                
                return options;
            }

        };
    }
})(jQuery);
