﻿using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Graph;
using System.Drawing.Drawing2D;
using Graph.Compatibility;
using Graph.Items;
using GateSim;

namespace SimGUI_WinForms
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			graphControl.CompatibilityStrategy = new AlwaysCompatible();


      /*
      var test2Node = new Node("test2");

      test2Node.AddItem(new NodeLabelItem("", true, false));
      test2Node.AddItem(new NodeTextBoxItem("test", false, true));

      test2Node.Location = new Point(500, 400);

      graphControl.AddNode(test2Node);


      var testNode = new Node("test");
      testNode.Location = new Point(500, 200);
      Console.WriteLine("hello");

      var check2Item = new NodeCheckboxItem("Check 1", true, false) { Tag = 31337 };
      testNode.AddItem(check2Item);
      testNode.AddItem(new NodeCheckboxItem("Check 2", true, false) { Tag = 42f });

      var imageItem2 = new NodeImageItem(Properties.Resources.example, 64, 64, false, true) { Tag = 1000f };
      testNode.AddItem(imageItem2);

      graphControl.AddNode(testNode);



      var someNode = new Node("My Title");
			someNode.Location = new Point(500, 100);
			var check1Item = new NodeCheckboxItem("Check 1", true, false) { Tag = 31337 };
			someNode.AddItem(check1Item);
			someNode.AddItem(new NodeCheckboxItem("Check 2", true, false) { Tag = 42f });
			
			graphControl.AddNode(someNode);

			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel		= new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var greenChannel	= new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var blueChannel		= new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var colorItem		= new NodeColorItem("Color", Color.Black, false, true) { Tag = 1337 };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object sender, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged		+= channelChangedDelegate;
			greenChannel.ValueChanged	+= channelChangedDelegate;
			blueChannel.ValueChanged	+= channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);
			graphControl.AddNode(colorNode);

			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
			var imageItem = new NodeImageItem(Properties.Resources.example, 64, 64, false, true) { Tag = 1000f };
			textureNode.AddItem(imageItem);
			graphControl.AddNode(textureNode);

			graphControl.ConnectionAdded	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdded);
			graphControl.ConnectionAdding	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdding);
			graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionRemoved);
			graphControl.ShowElementMenu	+= new EventHandler<AcceptElementLocationEventArgs>(OnShowElementMenu);

			graphControl.Connect(colorItem, check1Item);
      */
		}

    void createNewAndNode(object sender, MouseEventArgs e)
    {
      createNewNode(new PrimitiveComp(PrimCompType.AND, 0));
    }

    void createNewOrNode(object sender, MouseEventArgs e)
    {
      createNewNode(new PrimitiveComp(PrimCompType.OR, 0));
    }

    void createNewNotNode(object sender, MouseEventArgs e)
    {
      createNewNode(new PrimitiveComp(PrimCompType.NOT, 0));
    }

    void createNewXorNode(object sender, MouseEventArgs e)
    {
      createNewNode(new PrimitiveComp(PrimCompType.XOR, 0));
    }

    void createNewNode(Component comp)
    {
      var node = new Node("");

      System.Drawing.Bitmap imgBitmap = null;

      if (comp is PrimitiveComp)
      {
        PrimitiveComp primComp = (PrimitiveComp)comp;
        switch (primComp.type)
        {
          case PrimCompType.AND:
            imgBitmap = Properties.Resources.AND; break;
          case PrimCompType.OR:
            imgBitmap = Properties.Resources.OR; break;
          case PrimCompType.NOT:
            imgBitmap = Properties.Resources.NOT; break;
          case PrimCompType.XOR:
            imgBitmap = Properties.Resources.XOR; break;
        }

        node.AddItem(new NodeImageItem(imgBitmap, 50, 25, false, false));
      }

      for (int i = 0; i < comp.inputsLen; i++)
        node.AddItem(new NodeLabelItem("Input " + i, true, false));

      for (int i = 0; i < comp.outputsLen; i++)
        node.AddItem(new NodeLabelItem("Output " + i, false, true));

      //node.AddItem(new NodeLabelItem("Entry 1", true, false));
      //node.AddItem(new NodeLabelItem("Entry 2", true, false));
      //node.AddItem(new NodeLabelItem("Entry 3", false, true));
      //node.AddItem(new NodeTextBoxItem("TEXTTEXT", false, true));
      //node.AddItem(new NodeDropDownItem(new string[] { "1", "2", "3", "4" }, 0, false, false));
      this.DoDragDrop(node, DragDropEffects.Copy);

    }

		void OnColClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("Color");
		}

		void OnConnectionRemoved(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		void OnShowElementMenu(object sender, AcceptElementLocationEventArgs e)
		{
      /*
			if (e.Element == null)
			{
				// Show a test menu for when you click on nothing
				testMenuItem.Text = "(clicked on nothing)";
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is Node)
			{
				// Show a test menu for a node
				testMenuItem.Text = ((Node)e.Element).Title;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is NodeItem)
			{
				// Show a test menu for a nodeItem
				testMenuItem.Text = e.Element.GetType().Name;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			{
				// if you don't want to show a menu for this item (but perhaps show a menu for something more higher up) 
				// then you can cancel the event
				e.Cancel = true;
			}
      */
		}

		void OnConnectionAdding(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		static int counter = 1;
		void OnConnectionAdded(object sender, AcceptNodeConnectionEventArgs e)
		{
      Console.WriteLine("Connection added");
			//e.Cancel = true;
			e.Connection.Name = "Connection " + counter ++;
			e.Connection.DoubleClick += new EventHandler<NodeConnectionEventArgs>(OnConnectionDoubleClick);
		}

		void OnConnectionDoubleClick(object sender, NodeConnectionEventArgs e)
		{
			e.Connection.Name = "Connection " + counter++;
		}

		private void SomeNode_MouseDown(object sender, MouseEventArgs e)
		{
			var node = new Node("Some node");
			node.AddItem(new NodeLabelItem("Entry 1", true, false));
			node.AddItem(new NodeLabelItem("Entry 2", true, false));
			node.AddItem(new NodeLabelItem("Entry 3", false, true));
			node.AddItem(new NodeTextBoxItem("TEXTTEXT", false, true));
			node.AddItem(new NodeDropDownItem(new string[] { "1", "2", "3", "4" }, 0, false, false));
			this.DoDragDrop(node, DragDropEffects.Copy);
		}

		private void TextureNode_MouseDown(object sender, MouseEventArgs e)
		{
			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
			var imageItem = new NodeImageItem(Properties.Resources.example, 64, 64, false, true);
			textureNode.AddItem(imageItem);
			this.DoDragDrop(textureNode, DragDropEffects.Copy);
		}

		private void ColorNode_MouseDown(object sender, MouseEventArgs e)
		{
			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel = new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var greenChannel = new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var blueChannel = new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var colorItem = new NodeColorItem("Color", Color.Black, false, true);

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object s, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged += channelChangedDelegate;
			greenChannel.ValueChanged += channelChangedDelegate;
			blueChannel.ValueChanged += channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);

			this.DoDragDrop(colorNode, DragDropEffects.Copy);
		}

		private void OnShowLabelsChanged(object sender, EventArgs e)
		{
			graphControl.ShowLabels = showLabelsCheckBox.Checked;
		}
  }
}