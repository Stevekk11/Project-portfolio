namespace MassImageEditor;

partial class MainWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        SettingsButton = new System.Windows.Forms.Button();
        WelcomeSign = new System.Windows.Forms.TextBox();
        InputButton = new System.Windows.Forms.Button();
        OutputButton = new System.Windows.Forms.Button();
        Images = new System.Windows.Forms.ListView();
        StartButton = new System.Windows.Forms.Button();
        ClearButton = new System.Windows.Forms.Button();
        CancelButton1 = new System.Windows.Forms.Button();
        progressBar1 = new System.Windows.Forms.ProgressBar();
        label1 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // SettingsButton
        // 
        SettingsButton.BackColor = System.Drawing.Color.Aqua;
        SettingsButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SettingsButton.Location = new System.Drawing.Point(22, 185);
        SettingsButton.Name = "SettingsButton";
        SettingsButton.Size = new System.Drawing.Size(189, 58);
        SettingsButton.TabIndex = 0;
        SettingsButton.Text = "Settings";
        SettingsButton.UseVisualStyleBackColor = false;
        SettingsButton.Click += SettingsButton_Click;
        // 
        // WelcomeSign
        // 
        WelcomeSign.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        WelcomeSign.Location = new System.Drawing.Point(219, 12);
        WelcomeSign.Name = "WelcomeSign";
        WelcomeSign.ReadOnly = true;
        WelcomeSign.Size = new System.Drawing.Size(378, 43);
        WelcomeSign.TabIndex = 1;
        WelcomeSign.Text = "Welcome to mass image editor";
        WelcomeSign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // InputButton
        // 
        InputButton.BackColor = System.Drawing.SystemColors.InactiveCaption;
        InputButton.Location = new System.Drawing.Point(22, 97);
        InputButton.Name = "InputButton";
        InputButton.Size = new System.Drawing.Size(189, 58);
        InputButton.TabIndex = 2;
        InputButton.Text = "Choose folder with images ...";
        InputButton.UseVisualStyleBackColor = false;
        // 
        // OutputButton
        // 
        OutputButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
        OutputButton.Location = new System.Drawing.Point(237, 97);
        OutputButton.Name = "OutputButton";
        OutputButton.Size = new System.Drawing.Size(189, 58);
        OutputButton.TabIndex = 3;
        OutputButton.Text = "Choose folder to output the processed images.";
        OutputButton.UseVisualStyleBackColor = false;
        // 
        // Images
        // 
        Images.GridLines = true;
        Images.Location = new System.Drawing.Point(457, 104);
        Images.Name = "Images";
        Images.Size = new System.Drawing.Size(597, 504);
        Images.TabIndex = 4;
        Images.UseCompatibleStateImageBehavior = false;
        // 
        // StartButton
        // 
        StartButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)192)), ((int)((byte)0)));
        StartButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        StartButton.Location = new System.Drawing.Point(237, 185);
        StartButton.Name = "StartButton";
        StartButton.Size = new System.Drawing.Size(189, 58);
        StartButton.TabIndex = 5;
        StartButton.Text = "Start process";
        StartButton.UseVisualStyleBackColor = false;
        // 
        // ClearButton
        // 
        ClearButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)192)), ((int)((byte)0)));
        ClearButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        ClearButton.Location = new System.Drawing.Point(22, 267);
        ClearButton.Name = "ClearButton";
        ClearButton.Size = new System.Drawing.Size(189, 58);
        ClearButton.TabIndex = 7;
        ClearButton.Text = "Clear";
        ClearButton.UseVisualStyleBackColor = false;
        // 
        // CancelButton1
        // 
        CancelButton1.BackColor = System.Drawing.Color.Red;
        CancelButton1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        CancelButton1.Location = new System.Drawing.Point(237, 267);
        CancelButton1.Name = "CancelButton1";
        CancelButton1.Size = new System.Drawing.Size(189, 58);
        CancelButton1.TabIndex = 8;
        CancelButton1.Text = "Cancel";
        CancelButton1.UseVisualStyleBackColor = false;
        // 
        // progressBar1
        // 
        progressBar1.Location = new System.Drawing.Point(22, 367);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new System.Drawing.Size(403, 33);
        progressBar1.TabIndex = 9;
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(22, 328);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(151, 36);
        label1.TabIndex = 10;
        label1.Text = "Progress";
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1068, 622);
        Controls.Add(label1);
        Controls.Add(progressBar1);
        Controls.Add(CancelButton1);
        Controls.Add(ClearButton);
        Controls.Add(StartButton);
        Controls.Add(Images);
        Controls.Add(OutputButton);
        Controls.Add(InputButton);
        Controls.Add(WelcomeSign);
        Controls.Add(SettingsButton);
        Text = "Mass image editor";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.ProgressBar progressBar1;

    private System.Windows.Forms.Button CancelButton1;

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.Button ClearButton;

    private System.Windows.Forms.Button StartButton;

    private System.Windows.Forms.ListView Images;

    private System.Windows.Forms.Button OutputButton;

    private System.Windows.Forms.Button InputButton;

    private System.Windows.Forms.TextBox WelcomeSign;

    private System.Windows.Forms.Button SettingsButton;

    #endregion
}