using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwdGen
{
    public class PasswordManager
    {
        private TextBox tbPassword;
        private TextBox tbName;
        private TextBox tbPassphrase;
        private List<CheckBox> checkBoxList;
        private DomainUpDown domainUpDownLength;
        private Button btnSave;
        private ToolStripStatusLabel toolStripStatus;

        private Password pwd;
        private List<Password> pwdList;

        //
        private string fileName = "shadow.txt";
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string filePath;

        private const int maxLength = 256;
        private const int minLength = 8;

        public PasswordManager(TextBox tbPassword, TextBox tbName, TextBox tbPassphrase, List<CheckBox> checkBoxList, DomainUpDown domainUpDownLength, Button btnSave, ToolStripStatusLabel toolStripStatus)
        {
            this.tbPassword = tbPassword;
            this.tbName = tbName;
            this.tbPassphrase = tbPassphrase;
            this.checkBoxList = checkBoxList;
            this.domainUpDownLength = domainUpDownLength;
            this.btnSave = btnSave;
            this.toolStripStatus = toolStripStatus;

            filePath = path + "\\" + fileName;

            initDomainUpDownLength();
            getAllPasswords();

            toolStripStatus.Text = String.Empty;

            btnSave.Enabled = false;
        }

        public void decryptPassword()
        {
            if (!tbName.Text.Equals(String.Empty))
            {
                if (!tbPassphrase.Text.Equals(String.Empty))
                {
                    if (!isNameUnique())
                    {
                        pwd = getPassword();
                        pwd.decrypt(tbPassphrase.Text);

                        tbPassword.Text = pwd.Pwd;
                        getAllPasswords();

                        toolStripStatus.Text = "Password successfully decrypted.";
                    }
                    else
                    {
                        toolStripStatus.Text = "Please choose an unique name, this one already exists.";
                    }
                }
                else
                {
                    toolStripStatus.Text = "Please enter a passphrase first.";
                }
            }
            else
            {
                toolStripStatus.Text = "Please enter a name first.";
            }
        }

        public void savePassword()
        {
            if (!tbName.Text.Equals(String.Empty))
            {
                if (!tbPassphrase.Text.Equals(String.Empty))
                {
                    if (isNameUnique())
                    {
                        pwd.Id = getLastPasswordID();
                        pwd.Name = tbName.Text;
                        pwd.encrypt(tbPassphrase.Text);

                        //
                        File.AppendAllText(filePath, pwd.ToString());
                        getAllPasswords();

                        tbPassword.Clear();
                        tbName.Clear();
                        tbPassphrase.Clear();

                        btnSave.Enabled = false;

                        toolStripStatus.Text = "Password successfully saved.";
                    }
                    else
                    {
                        toolStripStatus.Text = "Please choose an unique name, this one already exists.";
                    }
                }
                else
                {
                    toolStripStatus.Text = "Please enter a passphrase first.";
                }
            }
            else
            {
                toolStripStatus.Text = "Please enter a name first.";
            }
        }

        public void generatePassword()
        {
            tbName.Clear();
            tbPassphrase.Clear();

            if (isAnyConditionChecked())
            {
                string name = tbName.Text;
                int length = Convert.ToInt32(domainUpDownLength.SelectedItem);
                bool isDigit = checkBoxList.Find(x => x.Name == "checkBoxDigits").Checked;
                bool isSpecialCase = checkBoxList.Find(x => x.Name == "checkBoxSpecialCase").Checked;
                bool isLowerCase = checkBoxList.Find(x => x.Name == "checkBoxLowerCase").Checked;
                bool isUpperCase = checkBoxList.Find(x => x.Name == "checkBoxUpperCase").Checked;
                bool isUmlaut = checkBoxList.Find(x => x.Name == "checkBoxUmlaute").Checked;

                pwd = new Password(name, length, isDigit, isSpecialCase, isLowerCase, isUpperCase, isUmlaut);
                tbPassword.Text = pwd.Pwd;
                btnSave.Enabled = true;

                toolStripStatus.Text = "Password successfully generated.";
            }
            else
            {
                toolStripStatus.Text = "Please select at least one password condition.";
            }
        }

        private Password getPassword()
        {
            Password obj = null;

            foreach (Password element in pwdList)
            {
                if (tbName.Text.Equals(element.Name))
                {
                    obj = element;
                }
            }

            return obj;
        }

        private int getLastPasswordID()
        {
            int id = 1;
            try
            {
                string[] lines = File.ReadAllLines(path + "\\" + fileName);

                if (lines.Length != 0)
                {
                    string last = lines.ElementAt(lines.Length - 1);
                    string[] fields = last.Split(';');
                    id = Convert.ToInt32(fields[0]) + 1;
                }
            }
            catch
            {

            }

            return id;
        }

        private bool isNameUnique()
        {
            bool unique = true;

            foreach (Password element in pwdList)
            {
                if (tbName.Text.Equals(element.Name))
                {
                    unique = false;
                    break;
                }
            }

            return unique;
        }

        private void getAllPasswords()
        {
            pwdList = new List<Password>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i <= lines.Length; i++)
                {
                    string element = lines.ElementAt(i);
                    string[] fields = element.Split(';');

                    Password obj = new Password(Convert.ToInt32(fields[0]), fields[1], fields[2]);
                    pwdList.Add(obj);
                }
            }
            catch
            {

            }
        }

        private void initDomainUpDownLength()
        {
            for (int i = maxLength; i >= minLength; i--)
            {
                domainUpDownLength.Items.Add(i);
            }

            domainUpDownLength.SelectedIndex = maxLength - minLength;
        }

        private bool isAnyConditionChecked()
        {
            bool check = false;
            foreach (CheckBox element in checkBoxList)
            {
                if (element.Checked)
                {
                    check = true;
                    break;
                }
            }
            return check;
        }
    }
}
