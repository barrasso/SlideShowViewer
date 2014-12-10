using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace Lab8
{
    public partial class SlideViewer : Form
    {
        public int slideIndex;

        public SlideViewer()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // instantiate graphics
            Graphics g = e.Graphics;
            // set form owner
            Form1 owner = (Form1)base.Owner;

            // check the slide index
            if (this.slideIndex + 1 > owner.listBox.Items.Count)
            {
                base.DialogResult = DialogResult.OK;
                return;
            }

            // get the current index in the list box
            string currentItem = (string)owner.listBox.Items[this.slideIndex];

            try
            {
                // create image from the file
                Image img = Image.FromFile(currentItem);

                // parameters
                int width = img.Width;
                int height = img.Height;
                SizeF clientSize = base.ClientSize;
                float space = Math.Min(clientSize.Height/(float)height, clientSize.Width/(float)width);

                // draw image to center of form
                g.DrawImage(img, (clientSize.Width - (float)width * space) / 2f, (clientSize.Height - (float)height*space) / 2f, (float)width*space, (float)height * space);
            }

            catch
            {
                // if not an image file
                g.DrawString("Not an image file!", new Font("Arial", 30), Brushes.Red, 25, 25);
            }
        }

        private void SlideViewer_KeyDown(object sender, KeyEventArgs e)
        {
            // check for escape key
            if (e.KeyData == Keys.Escape) base.DialogResult = DialogResult.OK;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SlideViewer slideView = this;

            // increment index
            slideView.slideIndex = slideView.slideIndex + 1;
            base.Invalidate();
        }

        private void SlideViewer_Activated(object sender, EventArgs e)
        {
            // set owner
            Form1 owner = (Form1)base.Owner;

            // set timer
            this.timer1.Interval = owner.slideInterval * 1000;
            this.timer1.Enabled = true;
        }
    }
}
