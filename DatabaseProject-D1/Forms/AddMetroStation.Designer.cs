using System.ComponentModel;

namespace DatabazeProjekt;

partial class AddMetroStation
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
        nazevStanice = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        hloubka = new System.Windows.Forms.TextBox();
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        uklid = new System.Windows.Forms.TextBox();
        wc = new System.Windows.Forms.CheckBox();
        label5 = new System.Windows.Forms.Label();
        datPoslUkl = new System.Windows.Forms.DateTimePicker();
        odeslat = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(12, 14);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(304, 42);
        label1.TabIndex = 0;
        label1.Text = "Název stanice (z tabulky stanice)";
        // 
        // nazevStanice
        // 
        nazevStanice.Location = new System.Drawing.Point(18, 45);
        nazevStanice.Name = "nazevStanice";
        nazevStanice.Size = new System.Drawing.Size(181, 23);
        nazevStanice.TabIndex = 1;
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label2.Location = new System.Drawing.Point(12, 83);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(304, 42);
        label2.TabIndex = 2;
        label2.Text = "Hloubka pod zemí";
        // 
        // hloubka
        // 
        hloubka.Location = new System.Drawing.Point(18, 117);
        hloubka.Name = "hloubka";
        hloubka.Size = new System.Drawing.Size(181, 23);
        hloubka.TabIndex = 3;
        // 
        // label3
        // 
        label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label3.Location = new System.Drawing.Point(196, 117);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(35, 42);
        label3.TabIndex = 4;
        label3.Text = "m";
        // 
        // label4
        // 
        label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label4.Location = new System.Drawing.Point(12, 159);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(304, 42);
        label4.TabIndex = 5;
        label4.Text = "Četnost úklidu";
        // 
        // uklid
        // 
        uklid.Location = new System.Drawing.Point(18, 191);
        uklid.Name = "uklid";
        uklid.Size = new System.Drawing.Size(181, 23);
        uklid.TabIndex = 6;
        // 
        // wc
        // 
        wc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        wc.Location = new System.Drawing.Point(32, 239);
        wc.Name = "wc";
        wc.Size = new System.Drawing.Size(166, 47);
        wc.TabIndex = 7;
        wc.Text = "Má WC";
        wc.UseVisualStyleBackColor = true;
        // 
        // label5
        // 
        label5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label5.Location = new System.Drawing.Point(18, 289);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(304, 42);
        label5.TabIndex = 8;
        label5.Text = "Datum posledního úklidu";
        // 
        // datPoslUkl
        // 
        datPoslUkl.Location = new System.Drawing.Point(18, 334);
        datPoslUkl.Name = "datPoslUkl";
        datPoslUkl.Size = new System.Drawing.Size(200, 23);
        datPoslUkl.TabIndex = 9;
        // 
        // odeslat
        // 
        odeslat.BackColor = System.Drawing.Color.Peru;
        odeslat.Font = new System.Drawing.Font("Papyrus", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        odeslat.Location = new System.Drawing.Point(268, 289);
        odeslat.Name = "odeslat";
        odeslat.Size = new System.Drawing.Size(186, 78);
        odeslat.TabIndex = 10;
        odeslat.Text = "Odeslat";
        odeslat.UseVisualStyleBackColor = false;
        odeslat.Click += odeslat_Click;
        // 
        // Přidej_metro_stanici
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(473, 450);
        Controls.Add(odeslat);
        Controls.Add(datPoslUkl);
        Controls.Add(label5);
        Controls.Add(wc);
        Controls.Add(uklid);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(hloubka);
        Controls.Add(label2);
        Controls.Add(nazevStanice);
        Controls.Add(label1);
        Text = "Přidej_metro_stanici";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.DateTimePicker datPoslUkl;
    private System.Windows.Forms.Button odeslat;

    private System.Windows.Forms.CheckBox wc;

    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox uklid;

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox hloubka;
    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.TextBox nazevStanice;

    private System.Windows.Forms.Label label1;

    #endregion
}