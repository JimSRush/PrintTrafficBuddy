// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.MainForm
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using PrintTrafficBuddy.Model;
using PrintTrafficBuddy.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PrintTrafficBuddy
{
  public class MainForm : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox infoDatabaseTextBox;
    private Button generateNameplateButton;
    private ProgressBar progressBar;
    private Label statusLabel;
    private TextBox outputFolderTextBox;
    private Label label2;
    private OpenFileDialog openFileDialog;
    private Button infoFileOpenButton;
    private Button outputFileOpenButton;
    private FolderBrowserDialog folderBrowserDialog;

    public MainForm()
    {
      this.InitializeComponent();
    }

    private void generateNameplateButton_Click(object sender, EventArgs e)
    {
      if (!this.EnsureFieldsFilled())
        return;
      this.progressBar.Value = 10;
      IList<FilmDetails> films = new ExcelExtractor()
      {
        SpreadsheetLocation = this.infoDatabaseTextBox.Text
      }.ExtractFilms();
      this.progressBar.Value = 60;
      new NamePlateGenerator()
      {
        OutputFilePath = (this.outputFolderTextBox.Text + (this.outputFolderTextBox.Text.EndsWith("\\") ? string.Empty : "\\"))
      }.Execute(films);
      this.progressBar.Value = 100;
      int num = (int) MessageBox.Show("I have generated the PDF nameplates", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void transitTimesButton_Click(object sender, EventArgs e)
    {
      if (!this.EnsureFieldsFilled())
        return;
      this.progressBar.Value = 10;
      IList<FilmDetails> films = new ExcelExtractor()
      {
        SpreadsheetLocation = this.infoDatabaseTextBox.Text
      }.ExtractFilms();
      this.progressBar.Value = 60;
      new ShippingCalculator()
      {
        OutputFilePath = (this.outputFolderTextBox.Text + (this.outputFolderTextBox.Text.EndsWith("\\") ? string.Empty : "\\"))
      }.Execute(films);
      this.progressBar.Value = 100;
      int num = (int) MessageBox.Show("I have calculated the transit times", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private bool EnsureFieldsFilled()
    {
      if (string.IsNullOrEmpty(this.infoDatabaseTextBox.Text))
      {
        int num = (int) MessageBox.Show("You must fill out the location for the Excel Info database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!string.IsNullOrEmpty(this.outputFolderTextBox.Text))
        return true;
      int num1 = (int) MessageBox.Show("You must fill out the location for files to be saved to", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void infoFileOpenButton_Click(object sender, EventArgs e)
    {
      this.openFileDialog.FileName = "infodatabase.xlsx";
	  this.openFileDialog.Filter = "Excel Files (*.xlsx, *.xls, *.xlsm)|*.xlsx;*.xls;*.xlsm";
      if (this.openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.infoDatabaseTextBox.Text = this.openFileDialog.FileName;
    }

    private void outputFileOpenButton_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.outputFolderTextBox.Text = this.folderBrowserDialog.SelectedPath;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (!this.EnsureFieldsFilled())
        return;
      int num = (int) new PrintTrafficReport(this.infoDatabaseTextBox.Text).ShowDialog((IWin32Window) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.infoDatabaseTextBox = new TextBox();
      this.generateNameplateButton = new Button();
      this.progressBar = new ProgressBar();
      this.statusLabel = new Label();
      this.outputFolderTextBox = new TextBox();
      this.label2 = new Label();
      this.openFileDialog = new OpenFileDialog();
      this.infoFileOpenButton = new Button();
      this.outputFileOpenButton = new Button();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(107, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Location of Info XLS:";
      this.infoDatabaseTextBox.Location = new Point(125, 22);
      this.infoDatabaseTextBox.Name = "infoDatabaseTextBox";
      this.infoDatabaseTextBox.Size = new Size(264, 20);
      this.infoDatabaseTextBox.TabIndex = 1;
      this.generateNameplateButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.generateNameplateButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.generateNameplateButton.Location = new Point(125, 103);
      this.generateNameplateButton.Name = "generateNameplateButton";
      this.generateNameplateButton.Size = new Size(214, 68);
      this.generateNameplateButton.TabIndex = 0;
      this.generateNameplateButton.Text = "Generate Nameplates";
      this.generateNameplateButton.UseVisualStyleBackColor = true;
      this.generateNameplateButton.Click += new EventHandler(this.generateNameplateButton_Click);
      this.progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar.Location = new Point(0, 191);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(433, 23);
      this.progressBar.TabIndex = 3;
      this.statusLabel.AutoSize = true;
      this.statusLabel.Location = new Point(206, 227);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new Size(0, 13);
      this.statusLabel.TabIndex = 4;
      this.statusLabel.TextAlign = ContentAlignment.TopCenter;
      this.outputFolderTextBox.Location = new Point(125, 52);
      this.outputFolderTextBox.Name = "outputFolderTextBox";
      this.outputFolderTextBox.Size = new Size(264, 20);
      this.outputFolderTextBox.TabIndex = 6;
      this.outputFolderTextBox.Text = "C:\\Temp";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Output Folder:";
      this.openFileDialog.FileName = "openFileDialog";
      this.infoFileOpenButton.Location = new Point(396, 22);
      this.infoFileOpenButton.Name = "infoFileOpenButton";
      this.infoFileOpenButton.Size = new Size(26, 20);
      this.infoFileOpenButton.TabIndex = 7;
      this.infoFileOpenButton.Text = "...";
      this.infoFileOpenButton.UseVisualStyleBackColor = true;
      this.infoFileOpenButton.Click += new EventHandler(this.infoFileOpenButton_Click);
      this.outputFileOpenButton.Location = new Point(395, 52);
      this.outputFileOpenButton.Name = "outputFileOpenButton";
      this.outputFileOpenButton.Size = new Size(26, 20);
      this.outputFileOpenButton.TabIndex = 8;
      this.outputFileOpenButton.Text = "...";
      this.outputFileOpenButton.UseVisualStyleBackColor = true;
      this.outputFileOpenButton.Click += new EventHandler(this.outputFileOpenButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(434, 217);
      this.Controls.Add((Control) this.generateNameplateButton);
      this.Controls.Add((Control) this.outputFileOpenButton);
      this.Controls.Add((Control) this.infoFileOpenButton);
      this.Controls.Add((Control) this.outputFolderTextBox);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.statusLabel);
      this.Controls.Add((Control) this.infoDatabaseTextBox);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.progressBar);
      this.Name = "MainForm";
      this.Text = "Print Traffic Buddy";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
