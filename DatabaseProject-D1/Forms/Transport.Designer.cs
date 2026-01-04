using System.ComponentModel;

namespace DatabazeProjekt.Forms;

partial class Transport
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
        components = new System.ComponentModel.Container();
        pristresek = new System.Windows.Forms.Button();
        metro = new System.Windows.Forms.Button();
        vlak = new System.Windows.Forms.Button();
        exit = new System.Windows.Forms.Button();
        csv = new System.Windows.Forms.Button();
        staniceLinka = new System.Windows.Forms.Button();
        Smazat = new System.Windows.Forms.Button();
        Report = new System.Windows.Forms.Button();
        Delimiter = new System.Windows.Forms.CheckBox();
        SmazStanici = new System.Windows.Forms.CheckBox();
        SmazLinku = new System.Windows.Forms.CheckBox();
        SmazPrist = new System.Windows.Forms.CheckBox();
        SmazMetro = new System.Windows.Forms.CheckBox();
        SmazVlak = new System.Windows.Forms.CheckBox();
        SmazStaniciJmeno = new System.Windows.Forms.TextBox();
        SmazLinkuCislo = new System.Windows.Forms.TextBox();
        SmazPristJmeno = new System.Windows.Forms.TextBox();
        SmazMetroJmeno = new System.Windows.Forms.TextBox();
        SmazVlakJmeno = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        EditorBtn = new System.Windows.Forms.Button();
        toolTip1 = new System.Windows.Forms.ToolTip(components);
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        label5 = new System.Windows.Forms.Label();
        label6 = new System.Windows.Forms.Label();
        label7 = new System.Windows.Forms.Label();
        SuspendLayout();
        //
        // pristresek
        //
        pristresek.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        pristresek.Location = new System.Drawing.Point(206, 22);
        pristresek.Name = "pristresek";
        pristresek.Size = new System.Drawing.Size(163, 74);
        pristresek.TabIndex = 1;
        pristresek.Text = "Přidat přístřešek";
        pristresek.UseVisualStyleBackColor = true;
        pristresek.Click += pristresek_Click;
        //
        // metro
        //
        metro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        metro.Location = new System.Drawing.Point(394, 24);
        metro.Name = "metro";
        metro.Size = new System.Drawing.Size(169, 71);
        metro.TabIndex = 2;
        metro.Text = "Přidat metro stanici";
        metro.UseVisualStyleBackColor = true;
        metro.Click += metro_Click;
        //
        // vlak
        //
        vlak.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        vlak.Location = new System.Drawing.Point(583, 24);
        vlak.Name = "vlak";
        vlak.Size = new System.Drawing.Size(167, 73);
        vlak.TabIndex = 4;
        vlak.Text = "Přidat vlakovou stanici";
        vlak.UseVisualStyleBackColor = true;
        vlak.Click += vlak_Click;
        //
        // exit
        //
        exit.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)128)), ((int)((byte)128)));
        exit.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        exit.Location = new System.Drawing.Point(22, 323);
        exit.Name = "exit";
        exit.Size = new System.Drawing.Size(183, 87);
        exit.TabIndex = 6;
        exit.Text = "Konec";
        exit.UseVisualStyleBackColor = false;
        exit.Click += exit_Click;
        //
        // csv
        //
        csv.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)192)), ((int)((byte)192)));
        csv.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        csv.Location = new System.Drawing.Point(22, 213);
        csv.Name = "csv";
        csv.Size = new System.Drawing.Size(166, 60);
        csv.TabIndex = 7;
        csv.Text = "Import CSV...";
        csv.UseVisualStyleBackColor = false;
        csv.Click += csv_Click;
        //
        // staniceLinka
        //
        staniceLinka.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        staniceLinka.Location = new System.Drawing.Point(22, 22);
        staniceLinka.Name = "staniceLinka";
        staniceLinka.Size = new System.Drawing.Size(164, 73);
        staniceLinka.TabIndex = 9;
        staniceLinka.Text = "Přidat stanici a linku";
        staniceLinka.UseVisualStyleBackColor = true;
        staniceLinka.Click += staniceLinka_Click;
        //
        // Smazat
        //
        Smazat.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)128)), ((int)((byte)0)));
        Smazat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Smazat.Location = new System.Drawing.Point(21, 120);
        Smazat.Name = "Smazat";
        Smazat.Size = new System.Drawing.Size(167, 72);
        Smazat.TabIndex = 10;
        Smazat.Text = "Smazat";
        Smazat.UseVisualStyleBackColor = false;
        Smazat.Click += Smazat_Click;
        //
        // Report
        //
        Report.BackColor = System.Drawing.Color.FromArgb(((int)((byte)192)), ((int)((byte)255)), ((int)((byte)192)));
        Report.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Report.Location = new System.Drawing.Point(732, 370);
        Report.Name = "Report";
        Report.Size = new System.Drawing.Size(207, 73);
        Report.TabIndex = 8;
        Report.Text = "Vygenerovat report (.md)...";
        Report.UseVisualStyleBackColor = false;
        Report.Click += Report_Click;
        //
        // Delimiter
        //
        Delimiter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Delimiter.Location = new System.Drawing.Point(22, 279);
        Delimiter.Name = "Delimiter";
        Delimiter.Size = new System.Drawing.Size(166, 38);
        Delimiter.TabIndex = 11;
        Delimiter.Text = "Delimiter = ;";
        Delimiter.UseVisualStyleBackColor = true;
        Delimiter.CheckedChanged += Delimiter_CheckedChanged;
        //
        // SmazStanici
        //
        SmazStanici.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SmazStanici.ForeColor = System.Drawing.SystemColors.Highlight;
        SmazStanici.Location = new System.Drawing.Point(269, 133);
        SmazStanici.Name = "SmazStanici";
        SmazStanici.Size = new System.Drawing.Size(163, 37);
        SmazStanici.TabIndex = 12;
        SmazStanici.Text = "Stanici";
        SmazStanici.UseVisualStyleBackColor = true;
        //
        // SmazLinku
        //
        SmazLinku.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SmazLinku.ForeColor = System.Drawing.SystemColors.Highlight;
        SmazLinku.Location = new System.Drawing.Point(269, 176);
        SmazLinku.Name = "SmazLinku";
        SmazLinku.Size = new System.Drawing.Size(163, 37);
        SmazLinku.TabIndex = 13;
        SmazLinku.Text = "Linku";
        SmazLinku.UseVisualStyleBackColor = true;
        //
        // SmazPrist
        //
        SmazPrist.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SmazPrist.ForeColor = System.Drawing.SystemColors.Highlight;
        SmazPrist.Location = new System.Drawing.Point(269, 219);
        SmazPrist.Name = "SmazPrist";
        SmazPrist.Size = new System.Drawing.Size(163, 37);
        SmazPrist.TabIndex = 14;
        SmazPrist.Text = "Přístřešek";
        SmazPrist.UseVisualStyleBackColor = true;
        //
        // SmazMetro
        //
        SmazMetro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SmazMetro.ForeColor = System.Drawing.SystemColors.Highlight;
        SmazMetro.Location = new System.Drawing.Point(269, 262);
        SmazMetro.Name = "SmazMetro";
        SmazMetro.Size = new System.Drawing.Size(163, 37);
        SmazMetro.TabIndex = 15;
        SmazMetro.Text = "Metro stanici";
        SmazMetro.UseVisualStyleBackColor = true;
        //
        // SmazVlak
        //
        SmazVlak.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        SmazVlak.ForeColor = System.Drawing.SystemColors.Highlight;
        SmazVlak.Location = new System.Drawing.Point(269, 305);
        SmazVlak.Name = "SmazVlak";
        SmazVlak.Size = new System.Drawing.Size(163, 37);
        SmazVlak.TabIndex = 16;
        SmazVlak.Text = "Vlakovou stanici";
        SmazVlak.UseVisualStyleBackColor = true;
        //
        // SmazStaniciJmeno
        //
        SmazStaniciJmeno.Location = new System.Drawing.Point(417, 133);
        SmazStaniciJmeno.Name = "SmazStaniciJmeno";
        SmazStaniciJmeno.Size = new System.Drawing.Size(166, 23);
        SmazStaniciJmeno.TabIndex = 18;
        //
        // SmazLinkuCislo
        //
        SmazLinkuCislo.Location = new System.Drawing.Point(417, 176);
        SmazLinkuCislo.Name = "SmazLinkuCislo";
        SmazLinkuCislo.Size = new System.Drawing.Size(166, 23);
        SmazLinkuCislo.TabIndex = 19;
        //
        // SmazPristJmeno
        //
        SmazPristJmeno.Location = new System.Drawing.Point(417, 219);
        SmazPristJmeno.Name = "SmazPristJmeno";
        SmazPristJmeno.Size = new System.Drawing.Size(166, 23);
        SmazPristJmeno.TabIndex = 20;
        //
        // SmazMetroJmeno
        //
        SmazMetroJmeno.Location = new System.Drawing.Point(417, 262);
        SmazMetroJmeno.Name = "SmazMetroJmeno";
        SmazMetroJmeno.Size = new System.Drawing.Size(166, 23);
        SmazMetroJmeno.TabIndex = 21;
        //
        // SmazVlakJmeno
        //
        SmazVlakJmeno.Location = new System.Drawing.Point(417, 309);
        SmazVlakJmeno.Name = "SmazVlakJmeno";
        SmazVlakJmeno.Size = new System.Drawing.Size(166, 23);
        SmazVlakJmeno.TabIndex = 22;
        //
        // label2
        //
        label2.Location = new System.Drawing.Point(595, 181);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 17);
        label2.TabIndex = 24;
        label2.Text = "číslo";
        //
        // EditorBtn
        //
        EditorBtn.Location = new System.Drawing.Point(732, 273);
        EditorBtn.Name = "EditorBtn";
        EditorBtn.Size = new System.Drawing.Size(206, 68);
        EditorBtn.TabIndex = 25;
        EditorBtn.Text = "Editor dat";
        EditorBtn.UseVisualStyleBackColor = true;
        //
        // label3
        //
        label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        label3.Location = new System.Drawing.Point(264, 103);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(105, 27);
        label3.TabIndex = 26;
        label3.Text = "Smazání:";
        //
        // label4
        //
        label4.Location = new System.Drawing.Point(595, 222);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(105, 20);
        label4.TabIndex = 27;
        label4.Text = "jméno stanice";
        //
        // label5
        //
        label5.Location = new System.Drawing.Point(595, 265);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(105, 20);
        label5.TabIndex = 28;
        label5.Text = "jméno stanice";
        //
        // label6
        //
        label6.Location = new System.Drawing.Point(595, 309);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(105, 20);
        label6.TabIndex = 29;
        label6.Text = "jméno stanice";
        //
        // label7
        //
        label7.Location = new System.Drawing.Point(595, 133);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(105, 20);
        label7.TabIndex = 30;
        label7.Text = "jméno stanice";
        //
        // Transport
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(950, 450);
        Controls.Add(label7);
        Controls.Add(label6);
        Controls.Add(label5);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(EditorBtn);
        Controls.Add(Report);
        Controls.Add(label2);
        Controls.Add(SmazVlakJmeno);
        Controls.Add(SmazMetroJmeno);
        Controls.Add(SmazPristJmeno);
        Controls.Add(SmazLinkuCislo);
        Controls.Add(SmazStaniciJmeno);
        Controls.Add(SmazVlak);
        Controls.Add(SmazMetro);
        Controls.Add(SmazPrist);
        Controls.Add(SmazLinku);
        Controls.Add(SmazStanici);
        Controls.Add(Delimiter);
        Controls.Add(Smazat);
        Controls.Add(staniceLinka);
        Controls.Add(csv);
        Controls.Add(exit);
        Controls.Add(vlak);
        Controls.Add(metro);
        Controls.Add(pristresek);
        Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        Text = "Doprava";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;

    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.Button EditorBtn;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.TextBox SmazMetroJmeno;
    private System.Windows.Forms.TextBox SmazVlakJmeno;

    private System.Windows.Forms.CheckBox SmazLinku;
    private System.Windows.Forms.CheckBox SmazPrist;
    private System.Windows.Forms.CheckBox SmazMetro;
    private System.Windows.Forms.CheckBox SmazVlak;
    private System.Windows.Forms.TextBox SmazStaniciJmeno;
    private System.Windows.Forms.TextBox SmazLinkuCislo;
    private System.Windows.Forms.TextBox SmazPristJmeno;

    private System.Windows.Forms.CheckBox SmazStanici;

    private System.Windows.Forms.CheckBox Delimiter;

    private System.Windows.Forms.Button Smazat;

    private System.Windows.Forms.Button staniceLinka;

    private System.Windows.Forms.Button csv;

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.Button exit;

    private System.Windows.Forms.Button vlak;
    private System.Windows.Forms.Button pristresek;
    private System.Windows.Forms.Button metro;
    private System.Windows.Forms.Button Report;

    private System.Windows.Forms.ToolTip toolTip1;

    #endregion
}


