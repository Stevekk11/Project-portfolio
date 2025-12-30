using System.ComponentModel;

namespace DatabazeProjekt;

partial class Chooser
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
        TableSelectBox = new System.Windows.Forms.ComboBox();
        TableSelectLabel = new System.Windows.Forms.Label();
        Add = new System.Windows.Forms.Button();
        Delete = new System.Windows.Forms.Button();
        dataGridView1 = new System.Windows.Forms.DataGridView();
        Save = new System.Windows.Forms.Button();
        Import = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        Exit = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // TableSelectBox
        // 
        TableSelectBox.FormattingEnabled = true;
        TableSelectBox.Location = new System.Drawing.Point(9, 44);
        TableSelectBox.Name = "TableSelectBox";
        TableSelectBox.Size = new System.Drawing.Size(294, 23);
        TableSelectBox.TabIndex = 0;
        TableSelectBox.SelectedIndexChanged += TableSelectBox_SelectedIndexChanged;
        // 
        // TableSelectLabel
        // 
        TableSelectLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        TableSelectLabel.Location = new System.Drawing.Point(9, 3);
        TableSelectLabel.Name = "TableSelectLabel";
        TableSelectLabel.Size = new System.Drawing.Size(294, 38);
        TableSelectLabel.TabIndex = 1;
        TableSelectLabel.Text = "Choose a table to work with:";
        // 
        // Add
        // 
        Add.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Add.Location = new System.Drawing.Point(13, 86);
        Add.Name = "Add";
        Add.Size = new System.Drawing.Size(187, 54);
        Add.TabIndex = 2;
        Add.Text = "Add Info";
        Add.UseVisualStyleBackColor = true;
        Add.Click += Add_Click;
        // 
        // Delete
        // 
        Delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Delete.Location = new System.Drawing.Point(15, 146);
        Delete.Name = "Delete";
        Delete.Size = new System.Drawing.Size(185, 53);
        Delete.TabIndex = 3;
        Delete.Text = "Delete info";
        Delete.UseVisualStyleBackColor = true;
        Delete.Click += Delete_Click;
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new System.Drawing.Point(271, 115);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new System.Drawing.Size(830, 282);
        dataGridView1.TabIndex = 5;
        dataGridView1.Text = "dataGridView1";
        // 
        // Save
        // 
        Save.BackColor = System.Drawing.Color.FromArgb(((int)((byte)128)), ((int)((byte)255)), ((int)((byte)128)));
        Save.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Save.Location = new System.Drawing.Point(16, 218);
        Save.Name = "Save";
        Save.Size = new System.Drawing.Size(184, 44);
        Save.TabIndex = 6;
        Save.Text = "Save changes";
        Save.UseVisualStyleBackColor = false;
        Save.Click += Save_Click;
        // 
        // Import
        // 
        Import.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Import.ForeColor = System.Drawing.SystemColors.Highlight;
        Import.Location = new System.Drawing.Point(15, 355);
        Import.Name = "Import";
        Import.Size = new System.Drawing.Size(184, 42);
        Import.TabIndex = 7;
        Import.Text = "Import From CSV...";
        Import.UseVisualStyleBackColor = true;
        Import.Click += Import_Click;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(15, 335);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(151, 17);
        label1.TabIndex = 8;
        label1.Text = "use ; in csv";
        // 
        // Exit
        // 
        Exit.BackColor = System.Drawing.Color.Red;
        Exit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
        Exit.Location = new System.Drawing.Point(16, 268);
        Exit.Name = "Exit";
        Exit.Size = new System.Drawing.Size(184, 44);
        Exit.TabIndex = 9;
        Exit.Text = "Exit";
        Exit.UseVisualStyleBackColor = false;
        Exit.Click += Exit_Click;
        // 
        // Chooser
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1143, 450);
        Controls.Add(Exit);
        Controls.Add(label1);
        Controls.Add(Import);
        Controls.Add(Save);
        Controls.Add(dataGridView1);
        Controls.Add(Delete);
        Controls.Add(Add);
        Controls.Add(TableSelectLabel);
        Controls.Add(TableSelectBox);
        Text = "Chooser";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button Exit;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button Import;

    private System.Windows.Forms.Button Save;

    private System.Windows.Forms.DataGridView dataGridView1;

    private System.Windows.Forms.Button Add;

    private System.Windows.Forms.Button Delete;

    private System.Windows.Forms.ComboBox TableSelectBox;
    private System.Windows.Forms.Label TableSelectLabel;

    #endregion
}