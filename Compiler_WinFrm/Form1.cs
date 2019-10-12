using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Compiler_WinFrm
{
    public partial class frmMain : Form
    {
        private Graphics g;
        private int timer = 0;
        
        private string fn;
        private bool saveflag;
        
        public frmMain()
        {
            InitializeComponent();
            
            g = txtErrors.CreateGraphics();
            LexicalAnalyst.setKeysSymbolTable();
            saveflag = true;
            saveToolStripButton.Enabled = !saveflag;
            txtErrors.ClientSize = new Size(txtErrors.Width, txtErrors.Height);
        }

        private void ErrorList(string error)
        {
            //new Font(Font.FontFamily, 9)
            // g.Clear(txtErrors.BackColor);
            g.DrawString(error, Font, new SolidBrush(Color.White), new Point(38, (timer * 20) + 2));
            g.FillEllipse(new SolidBrush(Color.Red), 8, (timer * 20) + 2, 18, 18);
            g.DrawString("Line " + (txtCompiler.GetLineFromCharIndex(txtCompiler.SelectionStart) + 1).ToString(), Font, new SolidBrush(Color.White), new Point(500, (timer++ * 20) + 2));
        }

        private void txtCompiler_TextChanged(object sender, EventArgs e)
        {
            if (txtCompiler.Text == string.Empty)
            {
                saveflag = true;
                saveToolStripButton.Enabled = !saveflag;
                runToolStripButton.Enabled = !saveflag;
            }
            else
            {
                saveflag = false;
                saveToolStripButton.Enabled = !saveflag;
                runToolStripButton.Enabled = !saveflag;
            }
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            if (txtCompiler.Text != string.Empty)
            {
                //string compile = LexicalAnalyst.Compile(txtCompiler.Text);
                //if (compile != string.Empty)
                //    txtErrors.Text = compile;
                //    //ErrorList(compile);
                //compile = SyntacticAnalyst.Compile();
                //if (compile != string.Empty)
                //    txtErrors.Text += compile;
                //    //ErrorList(compile);
                txtErrors.Text = LexicalAnalyst.Compile(txtCompiler.Text);
                //txtErrors.Text += SyntacticAnalyst.Compile();
                txtErrors.Text += SemanticAnalyst.Compile();
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (!saveflag)
                if (MessageBox.Show("Do you want to save?", "Save...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    saveToolStripButton_Click(null, null);
            txtCompiler.Text = string.Empty;
            Text = "Compiler";
            saveflag = true;
            saveToolStripButton.Enabled = !saveflag;
            fn = null;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripButton_Click(null, null);
            openFileDialog.Filter = "Code File|*.cpp";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            fn = openFileDialog.FileName;
            txtCompiler.Text = File.ReadAllText(fn);
            saveflag = true;
            this.Text = fn;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (fn == null)
            {
                saveFileDialog.DefaultExt = "cpp";
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                fn = saveFileDialog.FileName;
            }
            File.WriteAllText(fn, txtCompiler.Text);
            saveflag = true;
            saveToolStripButton.Enabled = !saveflag;
            this.Text = fn;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                runToolStripButton_Click(null, null);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saveflag)
                if (MessageBox.Show("Do you want to save?", "Save...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    saveToolStripButton_Click(null, null);
        }
    }
}