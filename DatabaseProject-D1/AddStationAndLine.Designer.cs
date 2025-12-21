using System.ComponentModel;

namespace DatabazeProjekt;

partial class AddStationAndLine
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
        nazevStanice = new System.Windows.Forms.TextBox();
        nazev = new System.Windows.Forms.Label();
        typ = new System.Windows.Forms.Label();
        typStanice = new System.Windows.Forms.TextBox();
        prist = new System.Windows.Forms.CheckBox();
        lavice = new System.Windows.Forms.CheckBox();
        kos = new System.Windows.Forms.CheckBox();
        infop = new System.Windows.Forms.CheckBox();
        naznam = new System.Windows.Forms.CheckBox();
        bezba = new System.Windows.Forms.CheckBox();
        cislo = new System.Windows.Forms.Label();
        cisloLinky = new System.Windows.Forms.TextBox();
        odeslat = new System.Windows.Forms.Button();
        nazevLinky = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // nazevStanice
        // 
        nazevStanice.Location = new System.Drawing.Point(11, 39);
        nazevStanice.Name = "nazevStanice";
        nazevStanice.Size = new System.Drawing.Size(195, 23);
        nazevStanice.TabIndex = 0;
        // 
        // nazev
        // 
        nazev.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        nazev.Location = new System.Drawing.Point(11, 10);
        nazev.Name = "nazev";
        nazev.Size = new System.Drawing.Size(130, 26);
        nazev.TabIndex = 1;
        nazev.Text = "Název stanice";
        // 
        // typ
        // 
        typ.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        typ.Location = new System.Drawing.Point(11, 74);
        typ.Name = "typ";
        typ.Size = new System.Drawing.Size(305, 26);
        typ.TabIndex = 3;
        typ.Text = "Typ stanice (bus, metro, vlak, tram)";
        // 
        // typStanice
        // 
        typStanice.Location = new System.Drawing.Point(11, 103);
        typStanice.Name = "typStanice";
        typStanice.Size = new System.Drawing.Size(370, 23);
        typStanice.TabIndex = 2;
        // 
        // prist
        // 
        prist.Location = new System.Drawing.Point(11, 132);
        prist.Name = "prist";
        prist.Size = new System.Drawing.Size(173, 26);
        prist.TabIndex = 4;
        prist.Text = "Má přístřešek?";
        prist.UseVisualStyleBackColor = true;
        // 
        // lavice
        // 
        lavice.Location = new System.Drawing.Point(11, 164);
        lavice.Name = "lavice";
        lavice.Size = new System.Drawing.Size(173, 26);
        lavice.TabIndex = 5;
        lavice.Text = "Má lavici?";
        lavice.UseVisualStyleBackColor = true;
        // 
        // kos
        // 
        kos.Location = new System.Drawing.Point(11, 196);
        kos.Name = "kos";
        kos.Size = new System.Drawing.Size(173, 26);
        kos.TabIndex = 6;
        kos.Text = "Má koš?";
        kos.UseVisualStyleBackColor = true;
        // 
        // infop
        // 
        infop.Location = new System.Drawing.Point(11, 228);
        infop.Name = "infop";
        infop.Size = new System.Drawing.Size(173, 26);
        infop.TabIndex = 7;
        infop.Text = "Má infopanel?";
        infop.UseVisualStyleBackColor = true;
        // 
        // naznam
        // 
        naznam.Location = new System.Drawing.Point(11, 260);
        naznam.Name = "naznam";
        naznam.Size = new System.Drawing.Size(173, 26);
        naznam.TabIndex = 8;
        naznam.Text = "Na znamení?";
        naznam.UseVisualStyleBackColor = true;
        // 
        // bezba
        // 
        bezba.Location = new System.Drawing.Point(11, 292);
        bezba.Name = "bezba";
        bezba.Size = new System.Drawing.Size(173, 26);
        bezba.TabIndex = 9;
        bezba.Text = "Je bezbariérová?";
        bezba.UseVisualStyleBackColor = true;
        // 
        // cislo
        // 
        cislo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)238));
        cislo.Location = new System.Drawing.Point(11, 321);
        cislo.Name = "cislo";
        cislo.Size = new System.Drawing.Size(195, 28);
        cislo.TabIndex = 10;
        cislo.Text = "Číslo linky, např.: 240";
        // 
        // cisloLinky
        // 
        cisloLinky.Location = new System.Drawing.Point(11, 352);
        cisloLinky.Name = "cisloLinky";
        cisloLinky.Size = new System.Drawing.Size(195, 23);
        cisloLinky.TabIndex = 11;
        // 
        // odeslat
        // 
        odeslat.BackColor = System.Drawing.SystemColors.Highlight;
        odeslat.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        odeslat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
        odeslat.Location = new System.Drawing.Point(16, 384);
        odeslat.Name = "odeslat";
        odeslat.Size = new System.Drawing.Size(251, 54);
        odeslat.TabIndex = 12;
        odeslat.Text = "Odeslat";
        odeslat.UseVisualStyleBackColor = false;
        odeslat.Click += odeslat_Click;
        // 
        // nazevLinky
        // 
        nazevLinky.Location = new System.Drawing.Point(237, 352);
        nazevLinky.Name = "nazevLinky";
        nazevLinky.Size = new System.Drawing.Size(195, 23);
        nazevLinky.TabIndex = 13;
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label1.Location = new System.Drawing.Point(237, 321);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(215, 28);
        label1.TabIndex = 14;
        label1.Text = "Alternativní název linky";
        // 
        // Přidej_stanici_a_linku
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(472, 450);
        Controls.Add(label1);
        Controls.Add(nazevLinky);
        Controls.Add(odeslat);
        Controls.Add(cisloLinky);
        Controls.Add(cislo);
        Controls.Add(bezba);
        Controls.Add(naznam);
        Controls.Add(infop);
        Controls.Add(kos);
        Controls.Add(lavice);
        Controls.Add(prist);
        Controls.Add(typ);
        Controls.Add(typStanice);
        Controls.Add(nazev);
        Controls.Add(nazevStanice);
        Text = "Přidej_stanici_a_linku";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox nazevLinky;
    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button odeslat;

    private System.Windows.Forms.CheckBox prist;

    private System.Windows.Forms.TextBox cisloLinky;

    private System.Windows.Forms.Label cislo;

    private System.Windows.Forms.CheckBox naznam;
    private System.Windows.Forms.CheckBox bezba;

    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.CheckBox lavice;
    private System.Windows.Forms.CheckBox kos;
    private System.Windows.Forms.CheckBox infop;

    private System.Windows.Forms.Label typ;
    private System.Windows.Forms.TextBox typStanice;

    private System.Windows.Forms.TextBox nazevStanice;
    private System.Windows.Forms.Label nazev;

    #endregion
}