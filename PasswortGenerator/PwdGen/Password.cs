using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwdGen
{
    public class Password
    {
        Random rand = new Random();

        private List<char> digits = new List<char>();
        private List<char> specialCases = new List<char>();
        private List<char> lowerCases = new List<char>();
        private List<char> upperCases = new List<char>();
        private List<char> umlauteLowerCase = new List<char>();
        private List<char> umlauteUpperCase = new List<char>();

        private List<int> cases;

        private int id;
        private string name;
        private string pwd;
        private int length;
        private bool isDigit;
        private bool isSpecialCase;
        private bool isLowerCase;
        private bool isUpperCase;
        private bool isUmlaute;

        public string Pwd
        {
            get
            {
                return pwd;
            }

            private set
            {
                pwd = value;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }

            private set
            {
                length = value;
            }
        }

        public bool IsDigit
        {
            get
            {
                return isDigit;
            }

            private set
            {
                isDigit = value;
            }
        }

        public bool IsSpecialCase
        {
            get
            {
                return isSpecialCase;
            }

            private set
            {
                isSpecialCase = value;
            }
        }

        public bool IsLowerCase
        {
            get
            {
                return isLowerCase;
            }

            private set
            {
                isLowerCase = value;
            }
        }

        public bool IsUmlaute
        {
            get
            {
                return isUmlaute;
            }

            private set
            {
                isUmlaute = value;
            }
        }

        public bool IsUpperCase
        {
            get
            {
                return isUpperCase;
            }

            set
            {
                isUpperCase = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public Password(int id, string name, string encryptedPassword)
        {
            this.Id = id;
            this.Name = name;
            this.Pwd = encryptedPassword;
        }

        public Password(string name, int length, bool digit, bool specialCase, bool lowerCase, bool upperCase, bool umlaute)
        {
            initLists();

            this.Length = length;
            this.Name = name;
            this.IsDigit = digit;
            this.IsSpecialCase = specialCase;
            this.IsLowerCase = lowerCase;
            this.IsUpperCase = upperCase;
            this.IsUmlaute = umlaute;

            generatePassword();
        }

        public void encrypt(string passphrase)
        {
            try
            {
                Pwd = StringCipher.Encrypt(Pwd, passphrase);
            }
            catch
            {

            }
        }

        public void decrypt(string passphrase)
        {
            try
            {
                Pwd = StringCipher.Decrypt(Pwd, passphrase);
            }
            catch
            {

            }
        }

        private void generatePassword()
        {
            int digitCount;
            int specialCaseCount;
            int lowerCaseCount;
            int upperCaseCount;
            int umlautCount;

            do
            {
                Pwd = "";

                digitCount = 0;
                specialCaseCount = 0;
                lowerCaseCount = 0;
                upperCaseCount = 0;
                umlautCount = 0;

                switchCaseValues();

                while (Pwd.Length < Length)
                {
                    int index = rand.Next(0, cases.Count);
                    switch (cases.ElementAt(index))
                    {
                        case 0:
                            if (IsDigit)
                            {
                                Pwd += getDigit();
                                digitCount++;
                            }
                            break;
                        case 1:
                            if (IsSpecialCase)
                            {
                                Pwd += getSpecialCase();
                                specialCaseCount++;
                            }
                            break;
                        case 2:
                            if (IsLowerCase)
                            {
                                Pwd += getLowerCase();
                                lowerCaseCount++;
                            }
                            break;
                        case 3:
                            if (IsUpperCase)
                            {
                                Pwd += getUpperCase();
                                upperCaseCount++;
                            }
                            break;
                        case 4:
                            if (IsUmlaute)
                            {
                                int j = rand.Next(0, 2);
                                switch (j)
                                {
                                    case 0:
                                        Pwd += getUmlautLowerCase();
                                        break;
                                    case 1:
                                        Pwd += getUmlautUpperCase();
                                        break;
                                }
                                umlautCount++;
                            }
                            break;
                    }
                }
            } while (!isCheckedCasesGreaterZero(digitCount, specialCaseCount, lowerCaseCount, upperCaseCount, umlautCount));
        }

        private bool isCheckedCasesGreaterZero(int digitCount, int specialCaseCount, int lowerCaseCount, int upperCaseCount, int umlauteCount)
        {
            bool ret = true;

            if (IsDigit && digitCount == 0)
                ret = false;
            if (IsSpecialCase && specialCaseCount == 0)
                ret = false;
            if (IsLowerCase && lowerCaseCount == 0)
                ret = false;
            if (IsUpperCase && upperCaseCount == 0)
                ret = false;
            if (IsUmlaute && umlauteCount == 0)
                ret = false;

            return ret;
        }

        private void switchCaseValues()
        {
            cases = new List<int>();

            if (IsDigit)
                cases.Add(0);
            if (IsSpecialCase)
                cases.Add(1);
            if (IsLowerCase)
                cases.Add(2);
            if (IsUpperCase)
                cases.Add(3);
            if (IsUmlaute)
                cases.Add(4);
        }

        private int getDigit()
        {
            return digits.ElementAt(rand.Next(0, digits.Count));
        }

        private char getSpecialCase()
        {
            return specialCases.ElementAt(rand.Next(0, specialCases.Count));
        }

        private char getLowerCase()
        {
            return lowerCases.ElementAt(rand.Next(0, lowerCases.Count));
        }

        private char getUpperCase()
        {
            return upperCases.ElementAt(rand.Next(0, upperCases.Count));
        }

        private char getUmlautLowerCase()
        {
            return umlauteLowerCase.ElementAt(rand.Next(0, umlauteLowerCase.Count));
        }

        private char getUmlautUpperCase()
        {
            return umlauteUpperCase.ElementAt(rand.Next(0, umlauteUpperCase.Count));
        }

        private void initLists()
        {
            // digits
            for (int i = 48; i <= 57; i++)
            {
                digits.Add(Convert.ToChar(i));
            }

            // specialCases
            for (int i = 33; i <= 47; i++)
            {
                specialCases.Add(Convert.ToChar(i));
            }

            specialCases.Add(Convert.ToChar(58));

            for (int i = 60; i <= 64; i++)
            {
                specialCases.Add(Convert.ToChar(i));
            }
            for (int i = 91; i <= 96; i++)
            {
                specialCases.Add(Convert.ToChar(i));
            }
            for (int i = 123; i <= 126; i++)
            {
                specialCases.Add(Convert.ToChar(i));
            }

            // lowerCases
            for (int i = 97; i <= 122; i++)
            {
                lowerCases.Add(Convert.ToChar(i));
            }

            // upperCases
            for (int i = 65; i <= 90; i++)
            {
                upperCases.Add(Convert.ToChar(i));
            }

            // umlaute
            umlauteLowerCase.Add('ü');  //ü
            umlauteLowerCase.Add('ä');  //ä
            umlauteLowerCase.Add('ö');  //ö

            umlauteUpperCase.Add('Ä');  //Ä
            umlauteUpperCase.Add('Ö');  //Ö
            umlauteUpperCase.Add('Ü');  //Ü
        }

        public override string ToString()
        {
            return Id + ";" + Name + ";" + Pwd +"\n";
        }
    }
}
