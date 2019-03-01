namespace SimGUI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      Graph.Compatibility.AlwaysCompatible alwaysCompatible1 = new Graph.Compatibility.AlwaysCompatible();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
      this.graphControl = new Graph.GraphControl();
      this.compToolstrip = new System.Windows.Forms.ToolStrip();
      this.compToolStripLabel = new System.Windows.Forms.ToolStripLabel();
      this.andButton = new System.Windows.Forms.ToolStripButton();
      this.orButton = new System.Windows.Forms.ToolStripButton();
      this.notButton = new System.Windows.Forms.ToolStripButton();
      this.xorButton = new System.Windows.Forms.ToolStripButton();
      this.clkButton = new System.Windows.Forms.ToolStripButton();
      this.toggleButton = new System.Windows.Forms.ToolStripButton();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.simulationControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.startSimulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.stopSimulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.compToolstrip.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // showLabelsCheckBox
      // 
      this.showLabelsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.showLabelsCheckBox.AutoSize = true;
      this.showLabelsCheckBox.Location = new System.Drawing.Point(12, 521);
      this.showLabelsCheckBox.Name = "showLabelsCheckBox";
      this.showLabelsCheckBox.Size = new System.Drawing.Size(87, 17);
      this.showLabelsCheckBox.TabIndex = 2;
      this.showLabelsCheckBox.Text = "Show Labels";
      this.showLabelsCheckBox.UseVisualStyleBackColor = true;
      this.showLabelsCheckBox.CheckedChanged += new System.EventHandler(this.OnShowLabelsChanged);
      // 
      // graphControl
      // 
      this.graphControl.AllowDrop = true;
      this.graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.graphControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
      this.graphControl.CompatibilityStrategy = alwaysCompatible1;
      this.graphControl.FocusElement = null;
      this.graphControl.HighlightCompatible = true;
      this.graphControl.LargeGridStep = 128F;
      this.graphControl.LargeStepGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
      this.graphControl.Location = new System.Drawing.Point(105, 62);
      this.graphControl.Name = "graphControl";
      this.graphControl.ShowLabels = false;
      this.graphControl.Size = new System.Drawing.Size(1183, 476);
      this.graphControl.SmallGridStep = 16F;
      this.graphControl.SmallStepGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
      this.graphControl.TabIndex = 0;
      this.graphControl.Text = "graphControl";
      // 
      // compToolstrip
      // 
      this.compToolstrip.AutoSize = false;
      this.compToolstrip.ImageScalingSize = new System.Drawing.Size(100, 50);
      this.compToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compToolStripLabel,
            this.andButton,
            this.orButton,
            this.notButton,
            this.xorButton,
            this.clkButton,
            this.toggleButton});
      this.compToolstrip.Location = new System.Drawing.Point(0, 24);
      this.compToolstrip.Name = "compToolstrip";
      this.compToolstrip.Size = new System.Drawing.Size(1300, 59);
      this.compToolstrip.TabIndex = 6;
      this.compToolstrip.Text = "Component toolstrip";
      this.compToolstrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.compToolstrip_ItemClicked);
      // 
      // compToolStripLabel
      // 
      this.compToolStripLabel.Name = "compToolStripLabel";
      this.compToolStripLabel.Size = new System.Drawing.Size(122, 56);
      this.compToolStripLabel.Text = "Add new component:";
      this.compToolStripLabel.Click += new System.EventHandler(this.compToolStripLabel_Click);
      // 
      // andButton
      // 
      this.andButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.andButton.Image = global::SimGUI.Properties.Resources.AND;
      this.andButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.andButton.Name = "andButton";
      this.andButton.Size = new System.Drawing.Size(104, 56);
      this.andButton.Text = "AND component";
      this.andButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewAndNode);
      // 
      // orButton
      // 
      this.orButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.orButton.Image = global::SimGUI.Properties.Resources.OR;
      this.orButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.orButton.Name = "orButton";
      this.orButton.Size = new System.Drawing.Size(104, 56);
      this.orButton.Text = "OR component";
      this.orButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewOrNode);
      // 
      // notButton
      // 
      this.notButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.notButton.Image = global::SimGUI.Properties.Resources.NOT;
      this.notButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.notButton.Name = "notButton";
      this.notButton.Size = new System.Drawing.Size(104, 56);
      this.notButton.Text = "NOT component";
      this.notButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewNotNode);
      // 
      // xorButton
      // 
      this.xorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.xorButton.Image = global::SimGUI.Properties.Resources.XOR;
      this.xorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.xorButton.Name = "xorButton";
      this.xorButton.Size = new System.Drawing.Size(104, 56);
      this.xorButton.Text = "XOR component";
      this.xorButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewXorNode);
      // 
      // clkButton
      // 
      this.clkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.clkButton.Image = global::SimGUI.Properties.Resources.CLK;
      this.clkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.clkButton.Name = "clkButton";
      this.clkButton.Size = new System.Drawing.Size(104, 56);
      this.clkButton.Text = "Clock component";
      this.clkButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewClkNode);
      // 
      // toggleButton
      // 
      this.toggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toggleButton.Image = global::SimGUI.Properties.Resources.TGL1;
      this.toggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toggleButton.Name = "toggleButton";
      this.toggleButton.Size = new System.Drawing.Size(104, 56);
      this.toggleButton.Text = "Toggle component";
      this.toggleButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewTglNode);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simulationControlsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1300, 24);
      this.menuStrip1.TabIndex = 7;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // simulationControlsToolStripMenuItem
      // 
      this.simulationControlsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startSimulationToolStripMenuItem,
            this.stopSimulationToolStripMenuItem});
      this.simulationControlsToolStripMenuItem.Name = "simulationControlsToolStripMenuItem";
      this.simulationControlsToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
      this.simulationControlsToolStripMenuItem.Text = "Simulation controls";
      // 
      // startSimulationToolStripMenuItem
      // 
      this.startSimulationToolStripMenuItem.Name = "startSimulationToolStripMenuItem";
      this.startSimulationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
      this.startSimulationToolStripMenuItem.Text = "Start Simulation";
      this.startSimulationToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartSimulation);
      // 
      // stopSimulationToolStripMenuItem
      // 
      this.stopSimulationToolStripMenuItem.Name = "stopSimulationToolStripMenuItem";
      this.stopSimulationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
      this.stopSimulationToolStripMenuItem.Text = "Stop Simulation";
      this.stopSimulationToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StopSimulation);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new System.Drawing.Size(1300, 550);
      this.Controls.Add(this.compToolstrip);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.showLabelsCheckBox);
      this.Controls.Add(this.graphControl);
      this.DoubleBuffered = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "PIDAC multitouch simulator";
      this.compToolstrip.ResumeLayout(false);
      this.compToolstrip.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Graph.GraphControl graphControl;
		private System.Windows.Forms.CheckBox showLabelsCheckBox;
    private System.Windows.Forms.ToolStrip compToolstrip;
    private System.Windows.Forms.ToolStripButton andButton;
    private System.Windows.Forms.ToolStripButton orButton;
    private System.Windows.Forms.ToolStripLabel compToolStripLabel;
    private System.Windows.Forms.ToolStripButton notButton;
    private System.Windows.Forms.ToolStripButton xorButton;
    private System.Windows.Forms.ToolStripButton clkButton;
    private System.Windows.Forms.ToolStripButton toggleButton;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem simulationControlsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem startSimulationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem stopSimulationToolStripMenuItem;
  }
}

