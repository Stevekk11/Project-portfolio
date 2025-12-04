using System.ComponentModel;

namespace MassImageEditor;

partial class Settings
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        ResizeCheckBox = new System.Windows.Forms.CheckBox();
        WidthBox = new System.Windows.Forms.TextBox();
        HeigthBox = new System.Windows.Forms.TextBox();
        Rotate = new System.Windows.Forms.CheckBox();
        RotateBox = new System.Windows.Forms.ComboBox();
        ConvertBox = new System.Windows.Forms.ComboBox();
        Convert = new System.Windows.Forms.CheckBox();
        PerformanceGroupBox = new System.Windows.Forms.GroupBox();
        MaxThreadsChooser = new System.Windows.Forms.NumericUpDown();
        MaxThreadsLabel = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        ExitButton = new System.Windows.Forms.Button();
        BlackAndWhiteBox = new System.Windows.Forms.CheckBox();
        BrightnessBox = new System.Windows.Forms.CheckBox();
        trackBar1 = new System.Windows.Forms.TrackBar();
        label1 = new System.Windows.Forms.Label();
        ContrastCheckBox = new System.Windows.Forms.CheckBox();
        ContrastTrackBar = new System.Windows.Forms.TrackBar();
        label5 = new System.Windows.Forms.Label();
        Sharpness = new System.Windows.Forms.NumericUpDown();
        SharpnessChkBox = new System.Windows.Forms.CheckBox();
        PerformanceGroupBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)MaxThreadsChooser).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ContrastTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)Sharpness).BeginInit();
        SuspendLayout();
        // 
        // ResizeCheckBox
        // 
        ResizeCheckBox.Location = new System.Drawing.Point(23, 62);
        ResizeCheckBox.Name = "ResizeCheckBox";
        ResizeCheckBox.Size = new System.Drawing.Size(103, 29);
        ResizeCheckBox.TabIndex = 1;
        ResizeCheckBox.Text = "Resize images";
        ResizeCheckBox.UseVisualStyleBackColor = true;
        ResizeCheckBox.CheckedChanged += ResizeCheckBox_CheckedChanged;
        // 
        // WidthBox
        // 
        WidthBox.Enabled = false;
        WidthBox.Location = new System.Drawing.Point(23, 133);
        WidthBox.Name = "WidthBox";
        WidthBox.Size = new System.Drawing.Size(139, 23);
        WidthBox.TabIndex = 2;
        // 
        // HeigthBox
        // 
        HeigthBox.Enabled = false;
        HeigthBox.Location = new System.Drawing.Point(23, 178);
        HeigthBox.Name = "HeigthBox";
        HeigthBox.Size = new System.Drawing.Size(139, 23);
        HeigthBox.TabIndex = 3;
        // 
        // Rotate
        // 
        Rotate.Location = new System.Drawing.Point(267, 66);
        Rotate.Name = "Rotate";
        Rotate.Size = new System.Drawing.Size(145, 21);
        Rotate.TabIndex = 6;
        Rotate.Text = "Rotate images";
        Rotate.UseVisualStyleBackColor = true;
        Rotate.CheckedChanged += Rotate_CheckedChanged;
        // 
        // RotateBox
        // 
        RotateBox.Enabled = false;
        RotateBox.FormattingEnabled = true;
        RotateBox.Items.AddRange(new object[] { "90°", "180°", "270°", "-90°", "-180°", "-270°" });
        RotateBox.Location = new System.Drawing.Point(264, 103);
        RotateBox.Name = "RotateBox";
        RotateBox.Size = new System.Drawing.Size(182, 23);
        RotateBox.TabIndex = 7;
        RotateBox.Text = "Choose ..";
        // 
        // ConvertBox
        // 
        ConvertBox.Enabled = false;
        ConvertBox.FormattingEnabled = true;
        ConvertBox.Items.AddRange(new object[] { "JPG", "PNG", "BMP", "WEBP" });
        ConvertBox.Location = new System.Drawing.Point(264, 195);
        ConvertBox.Name = "ConvertBox";
        ConvertBox.Size = new System.Drawing.Size(182, 23);
        ConvertBox.TabIndex = 9;
        ConvertBox.Text = "Choose ..";
        // 
        // Convert
        // 
        Convert.Location = new System.Drawing.Point(267, 158);
        Convert.Name = "Convert";
        Convert.Size = new System.Drawing.Size(145, 21);
        Convert.TabIndex = 8;
        Convert.Text = "Convert to..";
        Convert.UseVisualStyleBackColor = true;
        Convert.CheckedChanged += Convert_CheckedChanged;
        // 
        // PerformanceGroupBox
        // 
        PerformanceGroupBox.Controls.Add(MaxThreadsChooser);
        PerformanceGroupBox.Controls.Add(MaxThreadsLabel);
        PerformanceGroupBox.Location = new System.Drawing.Point(29, 278);
        PerformanceGroupBox.Name = "PerformanceGroupBox";
        PerformanceGroupBox.Size = new System.Drawing.Size(416, 101);
        PerformanceGroupBox.TabIndex = 10;
        PerformanceGroupBox.TabStop = false;
        PerformanceGroupBox.Text = "Performance";
        // 
        // MaxThreadsChooser
        // 
        MaxThreadsChooser.Location = new System.Drawing.Point(116, 43);
        MaxThreadsChooser.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
        MaxThreadsChooser.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        MaxThreadsChooser.Name = "MaxThreadsChooser";
        MaxThreadsChooser.Size = new System.Drawing.Size(141, 23);
        MaxThreadsChooser.TabIndex = 1;
        MaxThreadsChooser.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // MaxThreadsLabel
        // 
        MaxThreadsLabel.Location = new System.Drawing.Point(144, 19);
        MaxThreadsLabel.Name = "MaxThreadsLabel";
        MaxThreadsLabel.Size = new System.Drawing.Size(103, 21);
        MaxThreadsLabel.TabIndex = 0;
        MaxThreadsLabel.Text = "Max threads";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(23, 110);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(66, 20);
        label2.TabIndex = 11;
        label2.Text = "Width";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(23, 160);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(66, 19);
        label3.TabIndex = 12;
        label3.Text = "Heigth";
        // 
        // label4
        // 
        label4.Font = new System.Drawing.Font("Simple Indust Outline", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)178));
        label4.Location = new System.Drawing.Point(12, 9);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(595, 38);
        label4.TabIndex = 13;
        label4.Text = "Choose what you want to do with your images here";
        // 
        // ExitButton
        // 
        ExitButton.BackColor = System.Drawing.Color.BurlyWood;
        ExitButton.Location = new System.Drawing.Point(614, 435);
        ExitButton.Name = "ExitButton";
        ExitButton.Size = new System.Drawing.Size(174, 68);
        ExitButton.TabIndex = 14;
        ExitButton.Text = "Exit";
        ExitButton.UseVisualStyleBackColor = false;
        ExitButton.Click += ExitButton_Click;
        // 
        // BlackAndWhiteBox
        // 
        BlackAndWhiteBox.Location = new System.Drawing.Point(23, 234);
        BlackAndWhiteBox.Name = "BlackAndWhiteBox";
        BlackAndWhiteBox.Size = new System.Drawing.Size(192, 27);
        BlackAndWhiteBox.TabIndex = 15;
        BlackAndWhiteBox.Text = "Black and white output.";
        BlackAndWhiteBox.UseVisualStyleBackColor = true;
        // 
        // BrightnessBox
        // 
        BrightnessBox.Location = new System.Drawing.Point(489, 66);
        BrightnessBox.Name = "BrightnessBox";
        BrightnessBox.Size = new System.Drawing.Size(145, 21);
        BrightnessBox.TabIndex = 16;
        BrightnessBox.Text = "Change brightness";
        BrightnessBox.UseVisualStyleBackColor = true;
        BrightnessBox.CheckedChanged += BrightnessBox_CheckedChanged;
        // 
        // trackBar1
        // 
        trackBar1.Enabled = false;
        trackBar1.Location = new System.Drawing.Point(483, 93);
        trackBar1.Maximum = 100;
        trackBar1.Minimum = -100;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new System.Drawing.Size(290, 45);
        trackBar1.TabIndex = 17;
        trackBar1.TickFrequency = 20;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(489, 129);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(299, 26);
        label1.TabIndex = 18;
        label1.Text = "0     10     20     30    40     def.     60    70     80    90     100";
        // 
        // ContrastCheckBox
        // 
        ContrastCheckBox.Location = new System.Drawing.Point(492, 167);
        ContrastCheckBox.Name = "ContrastCheckBox";
        ContrastCheckBox.Size = new System.Drawing.Size(173, 28);
        ContrastCheckBox.TabIndex = 19;
        ContrastCheckBox.Text = "Change contrast";
        ContrastCheckBox.UseVisualStyleBackColor = true;
        ContrastCheckBox.CheckedChanged += ContrastCheckBox_CheckedChanged;
        // 
        // ContrastTrackBar
        // 
        ContrastTrackBar.Enabled = false;
        ContrastTrackBar.Location = new System.Drawing.Point(489, 215);
        ContrastTrackBar.Maximum = 100;
        ContrastTrackBar.Minimum = -100;
        ContrastTrackBar.Name = "ContrastTrackBar";
        ContrastTrackBar.Size = new System.Drawing.Size(283, 45);
        ContrastTrackBar.TabIndex = 20;
        ContrastTrackBar.TickFrequency = 20;
        // 
        // label5
        // 
        label5.Location = new System.Drawing.Point(492, 248);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(299, 26);
        label5.TabIndex = 21;
        label5.Text = "0     10     20     30    40     def.     60    70     80    90     100";
        // 
        // Sharpness
        // 
        Sharpness.BackColor = System.Drawing.Color.LightGreen;
        Sharpness.Location = new System.Drawing.Point(492, 321);
        Sharpness.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        Sharpness.Name = "Sharpness";
        Sharpness.Size = new System.Drawing.Size(133, 23);
        Sharpness.TabIndex = 22;
        Sharpness.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // SharpnessChkBox
        // 
        SharpnessChkBox.Location = new System.Drawing.Point(494, 284);
        SharpnessChkBox.Name = "SharpnessChkBox";
        SharpnessChkBox.Size = new System.Drawing.Size(170, 33);
        SharpnessChkBox.TabIndex = 23;
        SharpnessChkBox.Text = "Sharpen image";
        SharpnessChkBox.UseVisualStyleBackColor = true;
        SharpnessChkBox.CheckedChanged += SharpnessChkBox_CheckedChanged;
        // 
        // Settings
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 515);
        Controls.Add(SharpnessChkBox);
        Controls.Add(Sharpness);
        Controls.Add(label5);
        Controls.Add(ContrastTrackBar);
        Controls.Add(ContrastCheckBox);
        Controls.Add(label1);
        Controls.Add(trackBar1);
        Controls.Add(BrightnessBox);
        Controls.Add(BlackAndWhiteBox);
        Controls.Add(ExitButton);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(PerformanceGroupBox);
        Controls.Add(ConvertBox);
        Controls.Add(Convert);
        Controls.Add(RotateBox);
        Controls.Add(Rotate);
        Controls.Add(HeigthBox);
        Controls.Add(WidthBox);
        Controls.Add(ResizeCheckBox);
        Text = "Settings";
        PerformanceGroupBox.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)MaxThreadsChooser).EndInit();
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        ((System.ComponentModel.ISupportInitialize)ContrastTrackBar).EndInit();
        ((System.ComponentModel.ISupportInitialize)Sharpness).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.NumericUpDown Sharpness;
    private System.Windows.Forms.CheckBox SharpnessChkBox;

    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.TrackBar ContrastTrackBar;

    private System.Windows.Forms.CheckBox ContrastCheckBox;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TrackBar trackBar1;

    private System.Windows.Forms.CheckBox BrightnessBox;

    private System.Windows.Forms.CheckBox BlackAndWhiteBox;

    private System.Windows.Forms.Button ExitButton;

    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.NumericUpDown MaxThreadsChooser;

    private System.Windows.Forms.Label MaxThreadsLabel;

    private System.Windows.Forms.GroupBox PerformanceGroupBox;

    private System.Windows.Forms.ComboBox ConvertBox;
    private System.Windows.Forms.CheckBox Convert;

    private System.Windows.Forms.ComboBox RotateBox;

    private System.Windows.Forms.CheckBox Rotate;

    private System.Windows.Forms.TextBox WidthBox;
    private System.Windows.Forms.TextBox HeigthBox;

    private System.Windows.Forms.CheckBox ResizeCheckBox;

    #endregion
}