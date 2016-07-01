namespace SimGUI_WinForms
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
      Graph.Compatibility.AlwaysCompatible alwaysCompatible9 = new Graph.Compatibility.AlwaysCompatible();
      this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
      this.graphControl = new Graph.GraphControl();
      this.compToolstrip = new System.Windows.Forms.ToolStrip();
      this.compToolStripLabel = new System.Windows.Forms.ToolStripLabel();
      this.andButton = new System.Windows.Forms.ToolStripButton();
      this.orButton = new System.Windows.Forms.ToolStripButton();
      this.notButton = new System.Windows.Forms.ToolStripButton();
      this.xorButton = new System.Windows.Forms.ToolStripButton();
      this.compToolstrip.SuspendLayout();
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
      this.graphControl.CompatibilityStrategy = alwaysCompatible9;
      this.graphControl.FocusElement = null;
      this.graphControl.HighlightCompatible = true;
      this.graphControl.LargeGridStep = 128F;
      this.graphControl.LargeStepGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
      this.graphControl.Location = new System.Drawing.Point(108, 12);
      this.graphControl.Name = "graphControl";
      this.graphControl.ShowLabels = false;
      this.graphControl.Size = new System.Drawing.Size(1180, 526);
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
            this.xorButton});
      this.compToolstrip.Location = new System.Drawing.Point(0, 0);
      this.compToolstrip.Name = "compToolstrip";
      this.compToolstrip.Size = new System.Drawing.Size(1300, 59);
      this.compToolstrip.TabIndex = 6;
      this.compToolstrip.Text = "toolStrip1";
      // 
      // compToolStripLabel
      // 
      this.compToolStripLabel.Name = "compToolStripLabel";
      this.compToolStripLabel.Size = new System.Drawing.Size(122, 56);
      this.compToolStripLabel.Text = "Add new component:";
      // 
      // andButton
      // 
      this.andButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.andButton.Image = global::SimGUI_WinForms.Properties.Resources.AND;
      this.andButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.andButton.Name = "andButton";
      this.andButton.Size = new System.Drawing.Size(104, 56);
      this.andButton.Text = "toolStripButton1";
      this.andButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.createNewAndNode);
      // 
      // orButton
      // 
      this.orButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.orButton.Image = global::SimGUI_WinForms.Properties.Resources.OR;
      this.orButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.orButton.Name = "orButton";
      this.orButton.Size = new System.Drawing.Size(104, 56);
      this.orButton.Text = "toolStripButton2";
      this.orButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.createNewOrNode);
      // 
      // notButton
      // 
      this.notButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.notButton.Image = global::SimGUI_WinForms.Properties.Resources.NOT;
      this.notButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.notButton.Name = "notButton";
      this.notButton.Size = new System.Drawing.Size(104, 56);
      this.notButton.Text = "toolStripButton3";
      this.notButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.createNewNotNode);
      // 
      // xorButton
      // 
      this.xorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.xorButton.Image = global::SimGUI_WinForms.Properties.Resources.XOR;
      this.xorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.xorButton.Name = "xorButton";
      this.xorButton.Size = new System.Drawing.Size(104, 56);
      this.xorButton.Text = "toolStripButton4";
      this.xorButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.createNewXorNode);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new System.Drawing.Size(1300, 550);
      this.Controls.Add(this.compToolstrip);
      this.Controls.Add(this.showLabelsCheckBox);
      this.Controls.Add(this.graphControl);
      this.DoubleBuffered = true;
      this.Name = "MainForm";
      this.Text = "Form1";
      this.compToolstrip.ResumeLayout(false);
      this.compToolstrip.PerformLayout();
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
  }
}

