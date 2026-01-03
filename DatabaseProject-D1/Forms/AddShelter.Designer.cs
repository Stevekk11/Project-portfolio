using System.ComponentModel;

namespace DatabazeProjekt;

partial class AddShelter
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
        label3 = new System.Windows.Forms.Label();
        typPrist = new System.Windows.Forms.TextBox();
        barva = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        vlastnik = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        spravce = new System.Windows.Forms.TextBox();
        typ = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        datum = new System.Windows.Forms.DateTimePicker();
        odeslat = new System.Windows.Forms.Button();
        stanice = new System.Windows.Forms.TextBox();
        label5 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // label3
        // 
        label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label3.Location = new System.Drawing.Point(15, 13);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(169, 37);
        label3.TabIndex = 0;
        label3.Text = "Typ přístřešku";
        // 
        // typPrist
        // 
        typPrist.Location = new System.Drawing.Point(15, 53);
        typPrist.Name = "typPrist";
        typPrist.Size = new System.Drawing.Size(178, 23);
        typPrist.TabIndex = 1;
        // 
        // barva
        // 
        barva.Location = new System.Drawing.Point(15, 132);
        barva.Name = "barva";
        barva.Size = new System.Drawing.Size(178, 23);
        barva.TabIndex = 3;
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(15, 92);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(169, 37);
        label1.TabIndex = 2;
        label1.Text = "Barva";
        // 
        // vlastnik
        // 
        vlastnik.Location = new System.Drawing.Point(15, 207);
        vlastnik.Name = "vlastnik";
        vlastnik.Size = new System.Drawing.Size(178, 23);
        vlastnik.TabIndex = 5;
        // 
        // label2
        // 
        label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label2.Location = new System.Drawing.Point(15, 167);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(169, 37);
        label2.TabIndex = 4;
        label2.Text = "Vlastník";
        // 
        // spravce
        // 
        spravce.Location = new System.Drawing.Point(15, 273);
        spravce.Name = "spravce";
        spravce.Size = new System.Drawing.Size(178, 23);
        spravce.TabIndex = 7;
        // 
        // typ
        // 
        typ.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        typ.Location = new System.Drawing.Point(15, 233);
        typ.Name = "typ";
        typ.Size = new System.Drawing.Size(169, 37);
        typ.TabIndex = 6;
        typ.Text = "Správce";
        // 
        // label4
        // 
        label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label4.Location = new System.Drawing.Point(308, 13);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(169, 37);
        label4.TabIndex = 8;
        label4.Text = "Datum výroby";
        // 
        // datum
        // 
        datum.Location = new System.Drawing.Point(285, 53);
        datum.Name = "datum";
        datum.Size = new System.Drawing.Size(175, 23);
        datum.TabIndex = 9;
        // 
        // odeslat
        // 
        odeslat.BackColor = System.Drawing.Color.FromArgb(((int)((byte)128)), ((int)((byte)128)), ((int)((byte)255)));
        odeslat.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        odeslat.Location = new System.Drawing.Point(287, 123);
        odeslat.Name = "odeslat";
        odeslat.Size = new System.Drawing.Size(172, 84);
        odeslat.TabIndex = 10;
        odeslat.Text = "Odeslat";
        odeslat.UseVisualStyleBackColor = false;
        odeslat.Click += odeslat_Click;
        // 
        // stanice
        // 
        stanice.Location = new System.Drawing.Point(15, 344);
        stanice.Name = "stanice";
        stanice.Size = new System.Drawing.Size(178, 23);
        stanice.TabIndex = 11;
        // 
        // label5
        // 
        label5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label5.Location = new System.Drawing.Point(15, 299);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(169, 37);
        label5.TabIndex = 12;
        label5.Text = "Stanice, kam patří";
        // 
        // Přidej_Přístřešek
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(505, 450);
        Controls.Add(label5);
        Controls.Add(stanice);
        Controls.Add(odeslat);
        Controls.Add(datum);
        Controls.Add(label4);
        Controls.Add(spravce);
        Controls.Add(typ);
        Controls.Add(vlastnik);
        Controls.Add(label2);
        Controls.Add(barva);
        Controls.Add(label1);
        Controls.Add(typPrist);
        Controls.Add(label3);
        Text = "Přidej_Přístřešek";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox stanice;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.Button odeslat;

    private System.Windows.Forms.DateTimePicker datum;

    private System.Windows.Forms.Label label4;

    private System.Windows.Forms.TextBox spravce;
    private System.Windows.Forms.Label typ;

    private System.Windows.Forms.TextBox barva;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox vlastnik;
    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.TextBox typPrist;

    private System.Windows.Forms.Label label3;

    #endregion
}