﻿using System.ComponentModel;

namespace DatabazeProjekt;

partial class AddTrainStation
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
        label1 = new System.Windows.Forms.Label();
        StaniceJmeno = new System.Windows.Forms.TextBox();
        Nastupiste = new System.Windows.Forms.TrackBar();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        Elektrifikovana = new System.Windows.Forms.CheckBox();
        Soustava = new System.Windows.Forms.TextBox();
        Rozchod = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        label5 = new System.Windows.Forms.Label();
        label6 = new System.Windows.Forms.Label();
        Odeslat = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)Nastupiste).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(12, 10);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(231, 53);
        label1.TabIndex = 0;
        label1.Text = "Název základní stanice (vytvořeno v jiné tabulce)";
        // 
        // StaniceJmeno
        // 
        StaniceJmeno.Location = new System.Drawing.Point(12, 66);
        StaniceJmeno.Name = "StaniceJmeno";
        StaniceJmeno.Size = new System.Drawing.Size(195, 23);
        StaniceJmeno.TabIndex = 1;
        // 
        // Nastupiste
        // 
        Nastupiste.Location = new System.Drawing.Point(12, 135);
        Nastupiste.Name = "Nastupiste";
        Nastupiste.Size = new System.Drawing.Size(222, 45);
        Nastupiste.TabIndex = 3;
        Nastupiste.Value = 1;
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label2.Location = new System.Drawing.Point(12, 99);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(202, 33);
        label2.TabIndex = 4;
        label2.Text = "Počet nástupišť";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(21, 183);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(222, 35);
        label3.TabIndex = 5;
        label3.Text = " 0    1    2    3    4     5    6    7    8    9    10";
        // 
        // Elektrifikovana
        // 
        Elektrifikovana.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Elektrifikovana.Location = new System.Drawing.Point(21, 221);
        Elektrifikovana.Name = "Elektrifikovana";
        Elektrifikovana.Size = new System.Drawing.Size(186, 29);
        Elektrifikovana.TabIndex = 6;
        Elektrifikovana.Text = "Je elektrifikovaná";
        Elektrifikovana.UseVisualStyleBackColor = true;
        // 
        // Soustava
        // 
        Soustava.Location = new System.Drawing.Point(12, 303);
        Soustava.Name = "Soustava";
        Soustava.Size = new System.Drawing.Size(195, 23);
        Soustava.TabIndex = 7;
        // 
        // Rozchod
        // 
        Rozchod.Location = new System.Drawing.Point(12, 373);
        Rozchod.Name = "Rozchod";
        Rozchod.Size = new System.Drawing.Size(195, 23);
        Rozchod.TabIndex = 8;
        // 
        // label4
        // 
        label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label4.Location = new System.Drawing.Point(12, 337);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(202, 33);
        label4.TabIndex = 9;
        label4.Text = "Rozchod kolejí";
        // 
        // label5
        // 
        label5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label5.Location = new System.Drawing.Point(12, 267);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(202, 33);
        label5.TabIndex = 10;
        label5.Text = "Soustava elektrifikace";
        // 
        // label6
        // 
        label6.Location = new System.Drawing.Point(207, 380);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(36, 16);
        label6.TabIndex = 11;
        label6.Text = "mm";
        // 
        // Odeslat
        // 
        Odeslat.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)128)), ((int)((byte)255)));
        Odeslat.Font = new System.Drawing.Font("Old English Text MT", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        Odeslat.Location = new System.Drawing.Point(268, 320);
        Odeslat.Name = "Odeslat";
        Odeslat.Size = new System.Drawing.Size(183, 76);
        Odeslat.TabIndex = 12;
        Odeslat.Text = "Odeslat";
        Odeslat.UseVisualStyleBackColor = false;
        Odeslat.Click += Odeslat_Click;
        // 
        // Přidej_vlakovou_stanici
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(475, 450);
        Controls.Add(Odeslat);
        Controls.Add(label6);
        Controls.Add(label5);
        Controls.Add(label4);
        Controls.Add(Rozchod);
        Controls.Add(Soustava);
        Controls.Add(Elektrifikovana);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(Nastupiste);
        Controls.Add(StaniceJmeno);
        Controls.Add(label1);
        Text = "Přidej_vlakovou_stanici";
        ((System.ComponentModel.ISupportInitialize)Nastupiste).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button Odeslat;

    private System.Windows.Forms.TextBox Soustava;
    private System.Windows.Forms.TextBox Rozchod;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.CheckBox Elektrifikovana;

    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.TrackBar Nastupiste;

    private System.Windows.Forms.TextBox StaniceJmeno;

    private System.Windows.Forms.Label label1;

    #endregion
}