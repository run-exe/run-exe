namespace SpiderNextBoot;
partial class Form1
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        this.listBox1 = new System.Windows.Forms.ListBox();
        this.SuspendLayout();
        //
        // backgroundWorker1
        //
        this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
        this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
        //
        // listBox1
        //
        this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listBox1.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.listBox1.FormattingEnabled = true;
        this.listBox1.ItemHeight = 32;
        this.listBox1.Location = new System.Drawing.Point(0, 0);
        this.listBox1.Name = "listBox1";
        this.listBox1.Size = new System.Drawing.Size(298, 195);
        this.listBox1.TabIndex = 0;
        this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
        //
        // Form1
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(298, 195);
        this.Controls.Add(this.listBox1);
        this.Name = "Form1";
        this.Text = "SpiderNextBoot";
        this.ResumeLayout(false);
    }
    #endregion
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ListBox listBox1;
}