﻿
function init(data, setting) {
	var goGraph = go.GraphObject.make;

	var defaultSettings = {
		layout:"ForceDirectedLayout"
	}

	var object = $.extend(defaultSettings, setting);
	//console.log(defaultSettings);

		var myDiagram = goGraph(go.Diagram, "erModel", {
			initialContentAlignment: go.Spot.Center,
			allowDelete: false,
			allowCopy: false,
			allowResize: false,
			layout: goGraph(go[defaultSettings.layout]),
			"undoManager.isEnabled": true
		});
		

		// define several shared Brushes
		var bluegrad = goGraph(go.Brush, "Linear", { 0: "rgb(150, 150, 250)", 0.5: "rgb(86, 86, 186)", 1: "rgb(86, 86, 186)" });
		var greengrad = goGraph(go.Brush, "Linear", { 0: "rgb(158, 209, 159)", 1: "rgb(67, 101, 56)" });
		var redgrad = goGraph(go.Brush, "Linear", { 0: "rgb(206, 106, 100)", 1: "rgb(180, 56, 50)" });
		var yellowgrad = goGraph(go.Brush, "Linear", { 0: "rgb(254, 221, 50)", 1: "rgb(254, 182, 50)" });
		var lightgrad = goGraph(go.Brush, "Linear", { 1: "#E6E6FA", 0: "#FFFAF0" });
		//// the template for each attribute in a node's array of item data
		var itemTempl =
		  goGraph(go.Panel, "Horizontal",
			//goGraph(go.Shape,
			//  { desiredSize: new go.Size(100, 100) },
			//  new go.Binding("figure", "figure"),
			//  new go.Binding("fill", "color")),
			goGraph(go.TextBlock,
			  {
			  	stroke: "#333333",
			  	font: "bold 14px sans-serif"
			  },
			  new go.Binding("text", "name"))
		  );
		//// define the Node template, representing an entity
		myDiagram.nodeTemplate =
		  goGraph(go.Node, "Auto",  // the whole node panel
			{
				selectionAdorned: true,
				resizable: true,
				layoutConditions: go.Part.LayoutStandard & ~go.Part.LayoutNodeSized,
				fromSpot: go.Spot.AllSides,
				toSpot: go.Spot.AllSides,
				isShadowed: true,
				shadowColor: "#C5C1AA"
			},
			new go.Binding("location", "location").makeTwoWay(),
			// define the node's outer shape, which will surround the Table
			goGraph(go.Shape, "Rectangle",
			  { fill: greengrad, stroke: "#756875", strokeWidth: 3 }),
			goGraph(go.Panel, "Table",
			  { margin: 8, stretch: go.GraphObject.Fill },
			  goGraph(go.RowColumnDefinition, { row: 0, sizing: go.RowColumnDefinition.None }),
			  // the table header
			  goGraph(go.TextBlock,
				{
					row: 0, alignment: go.Spot.Center,
					margin: new go.Margin(0, 14, 0, 2),  // leave room for Button
					font: "bold 16px sans-serif"
				},
				new go.Binding("text", "key")),
			  // the collapse/expand button
			  goGraph("PanelExpanderButton", "LIST",  // the name of the element whose visibility this button toggles
				{ row: 0, alignment: go.Spot.TopRight }),
			  // the list of Panels, each showing an attribute
			  goGraph(go.Panel, "Vertical",
				{
					name: "LIST",
					row: 1,
					padding: 3,
					alignment: go.Spot.TopLeft,
					defaultAlignment: go.Spot.Left,
					stretch: go.GraphObject.Horizontal,
					itemTemplate: itemTempl
				},
				new go.Binding("itemArray", "items"))
			)  // end Table Panel
		  );  // end Node
		/*//// define the Link template, representing a relationship
		//myDiagram.linkTemplate =
		//  goGraph(go.Link,  // the whole link panel
		//	{
		//		selectionAdorned: true,
		//		layerName: "Foreground",
		//		reshapable: true,
		//		routing: go.Link.AvoidsNodes,
		//		corner: 5,
		//		curve: go.Link.JumpOver
		//	},
		//	goGraph(go.Shape,  // the link shape
		//	  { stroke: "#303B45", strokeWidth: 2.5 }),
		//	goGraph(go.TextBlock,  // the "from" label
		//	  {
		//	  	textAlign: "center",
		//	  	font: "bold 14px sans-serif",
		//	  	stroke: "#1967B3",
		//	  	segmentIndex: 0,
		//	  	segmentOffset: new go.Point(NaN, NaN),
		//	  	segmentOrientation: go.Link.OrientUpright
		//	  },
		//	  new go.Binding("text", "text")),
		//	goGraph(go.TextBlock,  // the "to" label
		//	  {
		//	  	textAlign: "center",
		//	  	font: "bold 14px sans-serif",
		//	  	stroke: "#1967B3",
		//	  	segmentIndex: -1,
		//	  	segmentOffset: new go.Point(NaN, NaN),
		//	  	segmentOrientation: go.Link.OrientUpright
		//	  },
		//	  new go.Binding("text", "toText"))
		//  );
		//// create the model for the E-R diagram*/
		var nodeDataArray = data[0];
		var linkDataArray = data[1];

		myDiagram.model = new go.GraphLinksModel(nodeDataArray, linkDataArray);
	}

	
