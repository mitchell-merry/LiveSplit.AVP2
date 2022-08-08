using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Livesplit.AVP2.Components
{
    public partial class AddSplitsDialog : Form
    {
        public AddSplitsOption Action = AddSplitsOption.Pending;

        public AddSplitsDialog()
        {
            InitializeComponent();
        }

        private void AddSplitsDialog_Load(object sender, EventArgs e)
        {

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Action = AddSplitsOption.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void addSubsplits_Click(object sender, EventArgs e)
        {
            Action = AddSplitsOption.AddSubsplits;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void addNormal_Click(object sender, EventArgs e)
        {
            Action = AddSplitsOption.AddNormal;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void areyousure_Click(object sender, EventArgs e)
        {

        }
    }

    public enum AddSplitsOption { AddSubsplits, AddNormal, Cancel, Pending }
}
