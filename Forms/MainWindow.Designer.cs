﻿
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
        WinAuth = new System.Windows.Forms.CheckBox();
        ConnStr = new System.Windows.Forms.CheckBox();
        label2 = new System.Windows.Forms.Label();
        ConfigName = new System.Windows.Forms.TextBox();
        connAuthGroup = new System.Windows.Forms.GroupBox();
        customConnGroup = new System.Windows.Forms.GroupBox();
        configConnGroup = new System.Windows.Forms.GroupBox();
        presetGroup = new System.Windows.Forms.GroupBox();
        titleLabel = new System.Windows.Forms.Label();
        errorTitleLabel = new System.Windows.Forms.Label();
        separatorLine = new System.Windows.Forms.Panel();
        connAuthGroup.SuspendLayout();
        customConnGroup.SuspendLayout();
        configConnGroup.SuspendLayout();
        presetGroup.SuspendLayout();
        SuspendLayout();
        // 
        // ServerField
        // 
        ServerField.Location = new System.Drawing.Point(15, 50);
        ServerField.Name = "ServerField";
        ServerField.Size = new System.Drawing.Size(700, 25);
        ServerField.TabIndex = 0;
        ServerField.Text = "localhost";
        // 
        // DBField
        // 
        DBField.Location = new System.Drawing.Point(15, 105);
        DBField.Name = "DBField";
        DBField.Size = new System.Drawing.Size(700, 25);
        DBField.TabIndex = 1;
        // 
        // LoadDB
        // 
        LoadDB.BackColor = System.Drawing.Color.CornflowerBlue;
        LoadDB.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)238));
        LoadDB.ForeColor = System.Drawing.Color.White;
        LoadDB.Location = new System.Drawing.Point(20, 625);
        LoadDB.Name = "LoadDB";
        LoadDB.Size = new System.Drawing.Size(370, 50);
        LoadDB.TabIndex = 2;
        LoadDB.Text = "Připojit k databázi";
        LoadDB.UseVisualStyleBackColor = false;
        LoadDB.Click += LoadDB_Click;
        // 
        // ServerLabel
        // 
        ServerLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
        ServerLabel.Location = new System.Drawing.Point(15, 30);
        ServerLabel.Name = "ServerLabel";
        ServerLabel.Size = new System.Drawing.Size(300, 20);
        ServerLabel.TabIndex = 3;
        ServerLabel.Text = "Server:";
        // 
        // DBlabel
        // 
        DBlabel.Font = new System.Drawing.Font("Segoe UI", 10F);
        DBlabel.Location = new System.Drawing.Point(15, 85);
        DBlabel.Name = "DBlabel";
        DBlabel.Size = new System.Drawing.Size(300, 20);
        DBlabel.TabIndex = 4;
        DBlabel.Text = "Databáze:";
        // 
        // UsernameField
        // 
        UsernameField.Location = new System.Drawing.Point(200, 170);
        UsernameField.Name = "UsernameField";
        UsernameField.Size = new System.Drawing.Size(515, 25);
        UsernameField.TabIndex = 6;
        // 
        // UsernameLabel
        // 
        UsernameLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
        UsernameLabel.Location = new System.Drawing.Point(40, 175);
        UsernameLabel.Name = "UsernameLabel";
        UsernameLabel.Size = new System.Drawing.Size(150, 20);
        UsernameLabel.TabIndex = 7;
        UsernameLabel.Text = "Uživatelské jméno:";
        // 
        // PasswordField
        // 
        PasswordField.Location = new System.Drawing.Point(200, 205);
        PasswordField.Name = "PasswordField";
        PasswordField.Size = new System.Drawing.Size(515, 25);
        PasswordField.TabIndex = 8;
        PasswordField.UseSystemPasswordChar = true;
        // 
        // PasswordLabel
        // 
        PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
        PasswordLabel.Location = new System.Drawing.Point(40, 210);
        PasswordLabel.Name = "PasswordLabel";
        PasswordLabel.Size = new System.Drawing.Size(150, 20);
        PasswordLabel.TabIndex = 9;
        PasswordLabel.Text = "Heslo:";
        // 
        // ExitDB
        // 
        ExitDB.BackColor = System.Drawing.Color.LightGray;
        ExitDB.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        ExitDB.Location = new System.Drawing.Point(410, 625);
        ExitDB.Name = "ExitDB";
        ExitDB.Size = new System.Drawing.Size(370, 50);
        ExitDB.TabIndex = 10;
        ExitDB.Text = "Zavřít aplikaci";
        ExitDB.UseVisualStyleBackColor = false;
        ExitDB.Click += ExitDB_Click;
        // 
        // ErrorField
        // 
        ErrorField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        ErrorField.BackColor = System.Drawing.Color.White;
        ErrorField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        ErrorField.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        ErrorField.ForeColor = System.Drawing.Color.DarkRed;
        ErrorField.Location = new System.Drawing.Point(20, 720);
        ErrorField.Name = "ErrorField";
        ErrorField.Size = new System.Drawing.Size(760, 100);
        ErrorField.TabIndex = 11;
        // 
        // Doprava
        // 
        Doprava.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Doprava.Location = new System.Drawing.Point(20, 30);
        Doprava.Name = "Doprava";
        Doprava.Size = new System.Drawing.Size(720, 60);
        Doprava.TabIndex = 13;
        Doprava.Text = "Doprava - použít připojení z App.config";
        Doprava.UseVisualStyleBackColor = true;
        Doprava.Click += Doprava_Click;
        // 
        // WinAuth
        // 
        WinAuth.Location = new System.Drawing.Point(15, 145);
        WinAuth.Name = "WinAuth";
        WinAuth.Size = new System.Drawing.Size(700, 30);
        WinAuth.TabIndex = 18;
        WinAuth.Text = "Ověření Windows (Integrated Security)";
        WinAuth.UseVisualStyleBackColor = true;
        // 
        // ConnStr
        // 
        ConnStr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        ConnStr.Location = new System.Drawing.Point(20, 30);
        ConnStr.Name = "ConnStr";
        ConnStr.Size = new System.Drawing.Size(720, 30);
        ConnStr.TabIndex = 19;
        ConnStr.Text = "Použít název připojovacího řetězce z App.config";
        ConnStr.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label2.Location = new System.Drawing.Point(40, 68);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(200, 25);
        label2.TabIndex = 20;
        label2.Text = "Název z App.config:";
        // 
        // ConfigName
        // 
        ConfigName.Enabled = false;
        ConfigName.Location = new System.Drawing.Point(250, 65);
        ConfigName.Name = "ConfigName";
        ConfigName.Size = new System.Drawing.Size(490, 27);
        ConfigName.TabIndex = 21;
        ConfigName.Text = "Doprava";
        // 
        // connAuthGroup
        // 
        connAuthGroup.Controls.Add(ServerLabel);
        connAuthGroup.Controls.Add(ServerField);
        connAuthGroup.Controls.Add(DBlabel);
        connAuthGroup.Controls.Add(DBField);
        connAuthGroup.Controls.Add(WinAuth);
        connAuthGroup.Controls.Add(UsernameLabel);
        connAuthGroup.Controls.Add(UsernameField);
        connAuthGroup.Controls.Add(PasswordLabel);
        connAuthGroup.Controls.Add(PasswordField);
        connAuthGroup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        connAuthGroup.Location = new System.Drawing.Point(20, 20);
        connAuthGroup.Name = "connAuthGroup";
        connAuthGroup.Size = new System.Drawing.Size(730, 234);
        connAuthGroup.TabIndex = 104;
        connAuthGroup.TabStop = false;
        connAuthGroup.Text = "Přihlašovací údaje";
        // 
        // customConnGroup
        // 
        customConnGroup.Controls.Add(connAuthGroup);
        customConnGroup.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        customConnGroup.Location = new System.Drawing.Point(20, 194);
        customConnGroup.Name = "customConnGroup";
        customConnGroup.Size = new System.Drawing.Size(760, 266);
        customConnGroup.TabIndex = 103;
        customConnGroup.TabStop = false;
        customConnGroup.Text = "Vlastní připojení k databázi";
        // 
        // configConnGroup
        // 
        configConnGroup.Controls.Add(ConnStr);
        configConnGroup.Controls.Add(label2);
        configConnGroup.Controls.Add(ConfigName);
        configConnGroup.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        configConnGroup.Location = new System.Drawing.Point(20, 475);
        configConnGroup.Name = "configConnGroup";
        configConnGroup.Size = new System.Drawing.Size(760, 130);
        configConnGroup.TabIndex = 105;
        configConnGroup.TabStop = false;
        configConnGroup.Text = "Připojení z konfigurace";
        // 
        // presetGroup
        // 
        presetGroup.Controls.Add(Doprava);
        presetGroup.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        presetGroup.Location = new System.Drawing.Point(20, 85);
        presetGroup.Name = "presetGroup";
        presetGroup.Size = new System.Drawing.Size(760, 110);
        presetGroup.TabIndex = 102;
        presetGroup.TabStop = false;
        presetGroup.Text = "Předdefinované databáze";
        // 
        // titleLabel
        // 
        titleLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)238));
        titleLabel.Location = new System.Drawing.Point(20, 15);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new System.Drawing.Size(600, 50);
        titleLabel.TabIndex = 100;
        titleLabel.Text = "Správa databází";
        // 
        // errorTitleLabel
        // 
        errorTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)238));
        errorTitleLabel.Location = new System.Drawing.Point(20, 690);
        errorTitleLabel.Name = "errorTitleLabel";
        errorTitleLabel.Size = new System.Drawing.Size(400, 25);
        errorTitleLabel.TabIndex = 106;
        errorTitleLabel.Text = "Chybové zprávy:";
        // 
        // separatorLine
        // 
        separatorLine.BackColor = System.Drawing.Color.LightGray;
        separatorLine.Location = new System.Drawing.Point(20, 70);
        separatorLine.Name = "separatorLine";
        separatorLine.Size = new System.Drawing.Size(760, 2);
        separatorLine.TabIndex = 101;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 835);
        Controls.Add(errorTitleLabel);
        Controls.Add(ErrorField);
        Controls.Add(configConnGroup);
        Controls.Add(customConnGroup);
        Controls.Add(presetGroup);
        Controls.Add(separatorLine);
        Controls.Add(titleLabel);
        Controls.Add(ExitDB);
        Controls.Add(LoadDB);
        Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        Text = "Správa Databází - DatabazeProjekt";
        connAuthGroup.ResumeLayout(false);
        connAuthGroup.PerformLayout();
        customConnGroup.ResumeLayout(false);
        configConnGroup.ResumeLayout(false);
        configConnGroup.PerformLayout();
        presetGroup.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label titleLabel;

    private System.Windows.Forms.TextBox ConfigName;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.CheckBox WinAuth;
    private System.Windows.Forms.CheckBox ConnStr;


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

    private System.Windows.Forms.GroupBox connAuthGroup;
    private System.Windows.Forms.GroupBox customConnGroup;
    private System.Windows.Forms.GroupBox configConnGroup;
    private System.Windows.Forms.GroupBox presetGroup;
    private System.Windows.Forms.Label errorTitleLabel;
    private System.Windows.Forms.Panel separatorLine;

    #endregion
}