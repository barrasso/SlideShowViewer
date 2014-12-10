using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab8
{
    public partial class Form1 : Form
    {

        public string openCollectionFileName;

        public int slideInterval;

        public Form1()
        {
            InitializeComponent();
            
            // clear list box text at startup
            this.listBox.Text = "";
        }

        private void openCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // init new open file dialog
            OpenFileDialog openCollection = new OpenFileDialog();
            openCollection.Filter = " | *.pix";
            openCollection.FilterIndex = 1;

            // if user pressed ok
            if (openCollection.ShowDialog() == DialogResult.OK)
            {
                // first, clear the current list items
                this.listBox.Items.Clear();

                // set the open collection file name
                this.openCollectionFileName = openCollection.FileName;

                // open stream reader
                StreamReader openStream = new StreamReader(openCollection.OpenFile());
                while (true)
                {
                    string currentLine = openStream.ReadLine();
                    string temp = currentLine;

                    // check if its null
                    if (currentLine == null) break;

                    this.listBox.Items.Add(temp);
                }

                // close the stream
                openStream.Close();
            }
        }

        private void saveCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // init save file dialog
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = " | *.pix";
            saveDialog.DefaultExt = "pix";

            // first check if there are any items in the list box
            if (this.listBox.Items.Count == 0)
            {
                MessageBox.Show("No file names to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            // then check if the file name is null
            if (this.openCollectionFileName == null)
                saveDialog.FileName = null;
            else
                saveDialog.FileName = this.openCollectionFileName;

            // if user pressed ok on save dialog
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // open write stream
                StreamWriter streamWriter = new StreamWriter(saveDialog.OpenFile());
                // save each item in list box
                foreach (string item in this.listBox.Items)
                {
                    streamWriter.WriteLine(item);
                }
                // close stream writer
                streamWriter.Close();
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // close program
            base.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // init new open dialog
            OpenFileDialog openDialog = new OpenFileDialog();

            // check for user click OK
            if (openDialog.ShowDialog() != DialogResult.OK) return;

            // add selected file names to list box items
            string[] files = openDialog.FileNames;
            for (int i = 0; i < (int)files.Length; i++)
            {
                string temp = files[i];
                this.listBox.Items.Add(temp);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // init obj array of selected items in the list box
            object[] listItemsArray = new object[this.listBox.SelectedItems.Count];
            this.listBox.SelectedItems.CopyTo(listItemsArray, 0);

            // create temp array to remove items
            object[] tempItemsArray = listItemsArray;
            for (int i = 0; i < (int)tempItemsArray.Length; i++)
            {
                object temp = tempItemsArray[i];
                this.listBox.Items.Remove(temp);
            }
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            // check for an empty list box
            if (this.listBox.Items.Count == 0)
            {
                MessageBox.Show("No images to show.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            // get the interval
            string interval = this.textBox.Text;

            try
            {
                // convert string to int
                this.slideInterval = Convert.ToInt32(interval);
                // check for valid interval
                if (this.slideInterval <= 0 || this.slideInterval > 99) throw new Exception();
            }

            catch
            {
                MessageBox.Show("Please enter a valid integer time interval greater than 0 or less than 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            // init slide viewer form
            (new SlideViewer() { Owner = this }).ShowDialog();
        }
    }
}
