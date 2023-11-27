using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ImageProcessingPractice;
using WebCamLib;
using HNUDIP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Secoya___Activity_on_Image_Processing
{
    public partial class Form1 : Form
    {
        private Size changesize;
        private string lastButtonClicked;
        Bitmap loaded, processed, background;
        Device selectDevice;
        Device[] Devices;
        private void imagezoom()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }
        public Form1()
        {
            InitializeComponent();
            this.Text = "Secoya - Activity on Image Processing";
            changesize = this.Size;
            this.Size = new Size(816, 489);
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
           imagezoom();
           comboBox1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lastButtonClicked = "file";
            imagezoom();
            openFileDialog1.ShowDialog();
        }
        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lastButtonClicked == "button2"&&lastButtonClicked=="file")
            {
                loaded = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = loaded;
            }
            else if (lastButtonClicked == "button3")
            {
                background = new Bitmap(openFileDialog1.FileName);
                pictureBox2.Image = background;
            }
            else
            {
                loaded = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = loaded;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offbutton();
            imagezoom();
            this.Size = new Size(816, 489);
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);
                    processed.SetPixel(i, j, pixel);
                }
            }
            pictureBox2.Image = processed;
            button1.Visible = pictureBox2.Image != null;

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offbutton();
            imagezoom();
            this.Size = new Size(816, 489);
            Color Pixel;
            byte gray;

            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Pixel = loaded.GetPixel(x, y);
                    gray = (byte)((Pixel.R + Pixel.G + Pixel.B) / 3);
                    processed.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            pictureBox2.Image = processed;
            button1.Visible = pictureBox2.Image != null;

        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offbutton();
            imagezoom();

            this.Size = new Size(816, 489);
            Color Pixel;

            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, Color.FromArgb(255 - Pixel.R, 255 - Pixel.G, 255 - Pixel.B));
                }
            }

            pictureBox2.Image = processed;
            button1.Visible = pictureBox2.Image != null;

        }

        //MODIFIED VERSION OF SIR ALIAC CODE 
        public static void Histogram(ref Bitmap a, ref Bitmap b)
        {
            Color sample;
            Color gray;
            Byte graydata;
            //Grayscale Convertion;
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(x, y, gray);
                }
            }

            //histogram 1d data;
            int[] histdata = new int[256]; // array from 0 to 255
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    histdata[sample.R]++; // can be any color property r,g or b
                }
            }

            int maxHistValue = histdata.Max();

            // Bitmap Graph Generation
            // Setting empty Bitmap with background color
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.White); // Clear the histogram image

                // Calculate the width of each bar in the histogram
                float barWidth = (float)b.Width / histdata.Length;

                // Draw each bar in the histogram
                for (int i = 0; i < histdata.Length; i++)
                {
                    float barHeight = (histdata[i] / (float)maxHistValue) * b.Height;
                    g.FillRectangle(Brushes.Black, i * barWidth, b.Height - barHeight, barWidth, barHeight);
                }
            }
        }


        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offbutton();
            imagezoom();

            this.Size = new Size(816, 489);
            if (loaded != null)
            {
                Bitmap histogramImage = new Bitmap(pictureBox2.Width, pictureBox2.Width);
                Bitmap loadedCopy = new Bitmap(loaded);
                //MATIC CALL MODIFIED VERSION OF SIR ALIAC
                Histogram(ref loadedCopy, ref histogramImage);
                pictureBox2.Image = histogramImage;
                button1.Visible = pictureBox2.Image != null;
                loadedCopy.Dispose();
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offbutton();
            imagezoom();

            this.Size = new Size(816, 489);
            Color Pixel;
            int r, g, b;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Pixel = loaded.GetPixel(x, y);
                    r = (int)((Pixel.R * .393) + (Pixel.G * .769) + (Pixel.B * .189));
                    g = (int)((Pixel.R * .349) + (Pixel.G * .686) + (Pixel.B * .168));
                    b = (int)((Pixel.R * .272) + (Pixel.G * .534) + (Pixel.B * .131));
                    processed.SetPixel(x, y, Color.FromArgb(Math.Min(255, r), Math.Min(255, g), Math.Max(0, Math.Min(255, b))));

                }
            }
            pictureBox2.Image = processed;
            button1.Visible = pictureBox2.Image != null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                saveFileDialog1.DefaultExt = "png"; // Default file extension
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(saveFileDialog1.FileName);
                    MessageBox.Show("Image successfully saved.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = changesize;
            button1.Visible = false;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lastButtonClicked = "button2";
            openFileDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lastButtonClicked = "button3";
            openFileDialog1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null && pictureBox1.Image != null)
            {
                processed = new Bitmap(loaded.Width, loaded.Height);
                Color colGreen = Color.FromArgb(0, 0, 255);
                int green = (int)(colGreen.R + colGreen.G + colGreen.B) / 3;
                int gThreshold = 5;

                for (int i = 0; i < loaded.Width; i++)
                {
                    for (int j = 0; j < loaded.Height; j++)
                    {
                        Color imageC = loaded.GetPixel(i, j);
                        Color backgroundC = background.GetPixel(i, j);
                        int grey = (int)(imageC.R + imageC.G + imageC.B) / 3;
                        int subtract = Math.Abs(grey - green);

                        if (subtract < gThreshold)
                        {
                            processed.SetPixel(i, j, backgroundC);
                        }
                        else
                        {
                            processed.SetPixel(i, j, imageC);
                        }
                    }
                }
                pictureBox3.Image = processed;
            }
        }

        private void offbutton()
        {
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDevice = DeviceManager.GetDevice(comboBox1.SelectedIndex);
            selectDevice.Init(pictureBox2.Height, pictureBox2.Width, pictureBox2.Handle.ToInt32());
            timer1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            Devices = DeviceManager.GetAllDevices();
            foreach (Device device in Devices)
            {
                comboBox1.Items.Add(device.Name);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Device devic = DeviceManager.GetDevice(0);
            devic.Stop();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDevice = DeviceManager.GetDevice(comboBox1.SelectedIndex);
            selectDevice.Init(pictureBox2.Height, pictureBox2.Width, pictureBox2.Handle.ToInt32());
            timer1.Start();
        }

        private Bitmap CaptureDisplay()
        {
            if (selectDevice == null)
            {
                MessageBox.Show("Please Select A Device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            selectDevice.Sendmessage();
            IDataObject data = Clipboard.GetDataObject();
            if (data != null && data.GetData("System.Drawing.Bitmap", true) != null)
            {
                Image bmap = (Image)data.GetData("System.Drawing.Bitmap", true);
                if (bmap != null)
                {
                    return new Bitmap(bmap);
                }
            }
            return null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (selectDevice != null)
            {
                background = CaptureDisplay();
                if (background != null)
                {
                    if (pictureBox2 != null && pictureBox2.Image != null)
                    {
                        pictureBox2.Image.Dispose();
                    }
                    pictureBox2.Image = background;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Bitmap nframe = CaptureDisplay();
            if (nframe != null)
            {
                processed = new Bitmap(loaded.Width, loaded.Height);
                Color mygreen = Color.FromArgb(0, 255, 0);
                int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                int threshold = 10;
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        Color fg = loaded.GetPixel(x, y);
                        Color bg = background.GetPixel(x, y);
                        int grey = (fg.R + fg.G + fg.B) / 3;
                        bool s = Math.Abs(grey - greygreen) < threshold;
                        if (s)
                            processed.SetPixel(x, y, bg);
                        else
                            processed.SetPixel(x, y, fg);
                    }
                }
                pictureBox3.Image = processed;
            }
        }
    }
}