
using System.ComponentModel;

namespace DatabazeProjekt;

partial class MainWindow
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
        ServerField = new System.Windows.Forms.TextBox();
        DBField = new System.Windows.Forms.TextBox();
        LoadDB = new System.Windows.Forms.Button();
        ServerLabel = new System.Windows.Forms.Label();
        DBlabel = new System.Windows.Forms.Label();
        UsernameField = new System.Windows.Forms.TextBox();
        UsernameLabel = new System.Windows.Forms.Label();
        PasswordField = new System.Windows.Forms.TextBox();
        PasswordLabel = new System.Windows.Forms.Label();
        ExitDB = new System.Windows.Forms.Button();
        ErrorField = new System.Windows.Forms.Label();
        Doprava = new System.Windows.Forms.Button();
        obecná = new System.Windows.Forms.Label();
        dp = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        WinAuth = new System.Windows.Forms.RadioButton();
        ConnStr = new System.Windows.Forms.RadioButton();
        label2 = new System.Windows.Forms.Label();
        ConfigName = new System.Windows.Forms.TextBox();
        SuspendLayout();
        // 
        // ServerField
        // 
        ServerField.Location = new System.Drawing.Point(537, 41);
        ServerField.Name = "ServerField";
        ServerField.Size = new System.Drawing.Size(216, 23);
        ServerField.TabIndex = 0;
        // 
        // DBField
        // 
        DBField.Location = new System.Drawing.Point(537, 89);
        DBField.Name = "DBField";
        DBField.Size = new System.Drawing.Size(213, 23);
        DBField.TabIndex = 1;
        // 
        // LoadDB
        // 
        LoadDB.BackColor = System.Drawing.Color.Lime;
        LoadDB.Location = new System.Drawing.Point(19, 139);
        LoadDB.Name = "LoadDB";
        LoadDB.Size = new System.Drawing.Size(155, 66);
        LoadDB.TabIndex = 2;
        LoadDB.Text = "Load Database";
        LoadDB.UseVisualStyleBackColor = false;
        LoadDB.Click += LoadDB_Click;
        // 
        // ServerLabel
        // 
        ServerLabel.Location = new System.Drawing.Point(537, 18);
        ServerLabel.Name = "ServerLabel";
        ServerLabel.Size = new System.Drawing.Size(211, 20);
        ServerLabel.TabIndex = 3;
        ServerLabel.Text = "Server";
        // 
        // DBlabel
        // 
        DBlabel.Location = new System.Drawing.Point(537, 67);
        DBlabel.Name = "DBlabel";
        DBlabel.Size = new System.Drawing.Size(213, 20);
        DBlabel.TabIndex = 4;
        DBlabel.Text = "Database";
        // 
        // UsernameField
        // 
        UsernameField.Location = new System.Drawing.Point(537, 133);
        UsernameField.Name = "UsernameField";
        UsernameField.Size = new System.Drawing.Size(215, 23);
        UsernameField.TabIndex = 6;
        // 
        // UsernameLabel
        // 
        UsernameLabel.Location = new System.Drawing.Point(537, 115);
        UsernameLabel.Name = "UsernameLabel";
        UsernameLabel.Size = new System.Drawing.Size(211, 20);
        UsernameLabel.TabIndex = 7;
        UsernameLabel.Text = "Username";
        // 
        // PasswordField
        // 
        PasswordField.Location = new System.Drawing.Point(537, 182);
        PasswordField.Name = "PasswordField";
        PasswordField.Size = new System.Drawing.Size(215, 23);
        PasswordField.TabIndex = 8;
        // 
        // PasswordLabel
        // 
        PasswordLabel.Location = new System.Drawing.Point(537, 159);
        PasswordLabel.Name = "PasswordLabel";
        PasswordLabel.Size = new System.Drawing.Size(213, 20);
        PasswordLabel.TabIndex = 9;
        PasswordLabel.Text = "Password";
        // 
        // ExitDB
        // 
        ExitDB.BackColor = System.Drawing.Color.Crimson;
        ExitDB.Location = new System.Drawing.Point(180, 139);
        ExitDB.Name = "ExitDB";
        ExitDB.Size = new System.Drawing.Size(154, 66);
        ExitDB.TabIndex = 10;
        ExitDB.Text = "Exit";
        ExitDB.UseVisualStyleBackColor = false;
        ExitDB.Click += ExitDB_Click;
        // 
        // ErrorField
        // 
        ErrorField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        ErrorField.BackColor = System.Drawing.SystemColors.AppWorkspace;
        ErrorField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        ErrorField.ForeColor = System.Drawing.Color.DarkRed;
        ErrorField.Location = new System.Drawing.Point(23, 516);
        ErrorField.Name = "ErrorField";
        ErrorField.Size = new System.Drawing.Size(731, 250);
        ErrorField.TabIndex = 11;
        // 
        // Doprava
        // 
        Doprava.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Doprava.Location = new System.Drawing.Point(23, 362);
        Doprava.Name = "Doprava";
        Doprava.Size = new System.Drawing.Size(358, 75);
        Doprava.TabIndex = 13;
        Doprava.Text = "Databáze doprava";
        Doprava.UseVisualStyleBackColor = true;
        Doprava.Click += Doprava_Click;
        // 
        // obecná
        // 
        obecná.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        obecná.Location = new System.Drawing.Point(23, 47);
        obecná.Name = "obecná";
        obecná.Size = new System.Drawing.Size(407, 68);
        obecná.TabIndex = 14;
        obecná.Text = "Databáze obecná";
        // 
        // dp
        // 
        dp.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        dp.Location = new System.Drawing.Point(23, 291);
        dp.Name = "dp";
        dp.Size = new System.Drawing.Size(407, 68);
        dp.TabIndex = 15;
        dp.Text = "Doprava - moje databáze";
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(19, 470);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(407, 46);
        label1.TabIndex = 16;
        label1.Text = "Prostor pro chyby..";
        // 
        // WinAuth
        // 
        WinAuth.Location = new System.Drawing.Point(354, 118);
        WinAuth.Name = "WinAuth";
        WinAuth.Size = new System.Drawing.Size(150, 48);
        WinAuth.TabIndex = 18;
        WinAuth.TabStop = true;
        WinAuth.Text = "Using Windows Auth";
        WinAuth.UseVisualStyleBackColor = true;
        // 
        // ConnStr
        // 
        ConnStr.Location = new System.Drawing.Point(354, 159);
        ConnStr.Name = "ConnStr";
        ConnStr.Size = new System.Drawing.Size(150, 48);
        ConnStr.TabIndex = 19;
        ConnStr.TabStop = true;
        ConnStr.Text = "Use the same login as in App.config";
        ConnStr.UseVisualStyleBackColor = false;
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label2.Location = new System.Drawing.Point(445, 247);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(189, 27);
        label2.TabIndex = 20;
        label2.Text = "App.config string name:";
        // 
        // ConfigName
        // 
        ConfigName.Location = new System.Drawing.Point(445, 277);
        ConfigName.Name = "ConfigName";
        ConfigName.Size = new System.Drawing.Size(215, 23);
        ConfigName.TabIndex = 21;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 776);
        Controls.Add(ConfigName);
        Controls.Add(label2);
        Controls.Add(ConnStr);
        Controls.Add(WinAuth);
        Controls.Add(label1);
        Controls.Add(dp);
        Controls.Add(obecná);
        Controls.Add(Doprava);
        Controls.Add(ErrorField);
        Controls.Add(ExitDB);
        Controls.Add(PasswordLabel);
        Controls.Add(PasswordField);
        Controls.Add(UsernameLabel);
        Controls.Add(UsernameField);
        Controls.Add(DBlabel);
        Controls.Add(ServerLabel);
        Controls.Add(LoadDB);
        Controls.Add(DBField);
        Controls.Add(ServerField);
        Text = "MainWindow";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox ConfigName;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.RadioButton WinAuth;
    private System.Windows.Forms.RadioButton ConnStr;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Label dp;

    private System.Windows.Forms.Label obecná;

    private System.Windows.Forms.Button Doprava;

    private System.Windows.Forms.Label ErrorField;

    private System.Windows.Forms.TextBox UsernameField;

    private System.Windows.Forms.Button ExitDB;

    private System.Windows.Forms.Label UsernameLabel;
    private System.Windows.Forms.TextBox PasswordField;
    private System.Windows.Forms.Label PasswordLabel;

    private System.Windows.Forms.Label DBlabel;

    private System.Windows.Forms.Label ServerLabel;

    private System.Windows.Forms.TextBox ServerField;
    private System.Windows.Forms.TextBox DBField;
    private System.Windows.Forms.Button LoadDB;

    #endregion
}