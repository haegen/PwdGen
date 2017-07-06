using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwdGen
{
    public partial class MainForm : Form
    {
        PasswordManager pwdMngr;

        public MainForm()
        {
            InitializeComponent();

            initCheckBoxList();
        }

        // events
        private void MainForm_Load(object sender, EventArgs e)
        {
            pwdMngr = new PasswordManager(tbPassword, tbName, tbPassphrase, initCheckBoxList(), domainUpDownLength, btnSave, toolStripStatus);
        }

        private void tbPassword_Click(object sender, EventArgs e)
        {
            tbPassword.SelectAll();
        }

        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            pwdMngr.generatePassword();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            pwdMngr.savePassword();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            pwdMngr.decryptPassword();
        }

        // other methods
        private List<CheckBox> initCheckBoxList()
        {
            List<CheckBox> cbList = new List<CheckBox>();
            cbList.Add(checkBoxDigits);
            cbList.Add(checkBoxSpecialCase);
            cbList.Add(checkBoxLowerCase);
            cbList.Add(checkBoxUpperCase);
            cbList.Add(checkBoxUmlaute);

            return cbList;
        }
    }
}
