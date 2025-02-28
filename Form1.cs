using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using QRCoder;

namespace QRCodeGeneratorApp
{
    public partial class Form1 : Form
    {
        private TextBox? txtUrl;
        private Button? btnGenerate;
        private PictureBox? pictureBoxQR;
        private Button? btnSave;
        private Label? lblUrl;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtUrl = new TextBox();
            btnGenerate = new Button();
            pictureBoxQR = new PictureBox();
            btnSave = new Button();
            lblUrl = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQR).BeginInit();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(50, 12);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(400, 23);
            txtUrl.TabIndex = 1;
            txtUrl.Text = "https://example.com";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(460, 11);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(100, 25);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // pictureBoxQR
            // 
            pictureBoxQR.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxQR.Location = new Point(50, 50);
            pictureBoxQR.Name = "pictureBoxQR";
            pictureBoxQR.Size = new Size(300, 300);
            pictureBoxQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxQR.TabIndex = 3;
            pictureBoxQR.TabStop = false;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(370, 170);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save QR Code";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(12, 15);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(31, 15);
            lblUrl.TabIndex = 0;
            lblUrl.Text = "URL:";
            // 
            // Form1
            // 
            ClientSize = new Size(580, 370);
            Controls.Add(lblUrl);
            Controls.Add(txtUrl);
            Controls.Add(btnGenerate);
            Controls.Add(pictureBoxQR);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            ShowIcon = false;
            Text = "QR Code Generator by HexCode";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxQR).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnGenerate_Click(object? sender, EventArgs e)
        {
            if (txtUrl == null || pictureBoxQR == null || btnSave == null)
                return;

            string url = txtUrl.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a valid URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCoder.QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);
                pictureBoxQR.Image = qrCodeImage;
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (pictureBoxQR?.Image == null)
            {
                MessageBox.Show("No QR code to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            saveDialog.Title = "Save QR Code";
            saveDialog.DefaultExt = "png";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImageFormat format = ImageFormat.Png;
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();

                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }

                    pictureBoxQR.Image.Save(saveDialog.FileName, format);
                    MessageBox.Show("QR code saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving QR code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}