using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GrapLab1
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public int[,] StructElem;

        public Form1()
        {
            StructElem = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Сохранить картинку как...";
                dialog.OverwritePrompt = true;
                dialog.CheckPathExists = true;
                dialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                dialog.ShowHelp = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеГауссToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void увеличитьЯркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new IncreaseBrightness();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void собеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void повышениеРезкостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new IncreaseSharpness();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayWorldFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new PerfectReflectorFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void коррекцияСОпорнымЦветомToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new CorrectionWithReferenceColor();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void идеальныйОтражательToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new LinearStretchingHistogram();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new CarryFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new TurnFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void волныToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new WavesFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эфектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void motionBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MotionBlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void резкостьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new SharpenFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new StampingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void выделеToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Filters filter = new BorderSelectionFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void светящиесяКраяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);

            while (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(200);
                Application.DoEvents();
            }

            Filters filter2 = new BorderSelectionFilter();
            backgroundWorker1.RunWorkerAsync(filter2);

            while (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(400);
                Application.DoEvents();
            }

            Filters filter3 = new MaxFilter();
            backgroundWorker1.RunWorkerAsync(filter3);
        }

        private void переводВБинарноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BinaryFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Dilation(StructElem);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Erosion(StructElem);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Opening();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Closing();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void topHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new TopHat();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void blackHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlackHat();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Grad();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void задатьСтруктурныйЭлементToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.ShowDialog();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    StructElem[i, j] = form2.StructElem[i, j];

        }
    }
}