using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string fn = string.Empty;
        private bool docChanged = false;
        //матрица начальной перестановки
        public int[] arrStart = new int[]{58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
                                           57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7};
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "текст|*.txt";
            openFileDialog1.Title = "Открыть сообщение";
            openFileDialog1.Multiselect = false;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "текст|*.txt";
            saveFileDialog1.Title = "Сохранить сообщение";
        }
        private void OpenDocument()
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fn = openFileDialog1.FileName;
                this.Text = fn;
                try
                {
                    System.IO.StreamReader sr =
                        new System.IO.StreamReader(fn, Encoding.GetEncoding(1251));
                    richTextBox1.Text = sr.ReadToEnd();
                    richTextBox1.SelectionStart =
                            richTextBox1.TextLength;
                    sr.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка доступа к файлу\n" +
                        exc.ToString(), "CryptoProgram",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        private int SaveDocument()
        {
            int result = 0;
            if (fn == string.Empty)
            {
                if (saveFileDialog1.ShowDialog() ==
                            DialogResult.OK)
                {
                    fn = saveFileDialog1.FileName;
                    this.Text = fn;
                }
                else result = -1;
            }
            if (fn != string.Empty)
            {
                try
                {
                    System.IO.FileInfo fi =
                        new System.IO.FileInfo(fn);
                    System.IO.StreamWriter sw =
                        fi.CreateText();
                    sw.Write(richTextBox3.Text);
                    sw.Close();
                    result = 0;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString(),
                            "CryptoProgram",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                }
            }
            return result;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            //string strKey - ключ
            string strKey = textBox1.Text;
            //string str исходный текст;
            string str = richTextBox1.Text;
            //переводим символы исходного текста в их ASCII код
            byte[] bytes = Encoding.Default.GetBytes(str);
            //выводим коды символов
            richTextBox2.Text += "ASCII коды исходного текста\n";
            for (int i = 0; i < bytes.Length; i++)
                richTextBox2.Text += bytes[i] + " ";
            string[] strArr = new string[bytes.Length];
            richTextBox2.Text += "\n";
            //конвертируем ASCII коды в двоичную систему, представление восьмиразрядное
            //при необходимости дополняем слева нулями
            for(int i = 0; i < strArr.Length; i++)
            {
                strArr[i] = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
            }
            //объявляем int-массив, равный по длине строковому массиву, умноженному на восемь
            int[] intArr = new int[strArr.Length*8];
            //переводим символы исходного ключа в их ASCII код
            byte[] bytesKey = Encoding.Default.GetBytes(strKey);
            //выводим коды символов ключа
            richTextBox2.Text += "ASCII коды ключа\n";
            for (int i = 0; i < bytesKey.Length; i++)
                richTextBox2.Text += bytesKey[i] + " ";
            richTextBox2.Text += "\n";
            string[] strArrKey = new string[bytesKey.Length];
            //конвертируем ASCII коды в двоичную систему, представление восьмиразрядное
            //при необходимости дополняем слева нулями
            for (int i = 0; i < strArrKey.Length; i++)
            {
                strArrKey[i] = Convert.ToString(bytesKey[i], 2).PadLeft(8, '0');
            }
            int[] intArrKey = new int[strArrKey.Length * 8];
            richTextBox2.Text += "Биты исходного текста";
            richTextBox2.Text += "\n";
            //записываем в числовой массив все биты исходного текста
            int j = 0;
            while(j != intArr.Length)
            {
                for (int i = 0; i < strArr.Length; i++)
                {
                    foreach (char ch in strArr[i])
                    {
                        intArr[j] = (int)Char.GetNumericValue(ch);
                        richTextBox2.Text += intArr[j];
                        j++;
                    }
                    richTextBox2.Text += " ";
                }
            }
            int len = intArr.Length;
            if(len%64 != 0)
            {
                while(len%64!=0)
                {
                    len++;
                }
            }
            richTextBox2.Text += len + "\n";
            //создаём расширяющий массив до длины, чтобы можно было поделить на равные блоки по 64 бита
            //пока счётчик индексов меньше длины исходного битового массива, в расширяющий массив записываем
            //его значения, а потом заполняем нулями
            int[] intArrExtended = new int[len];
            for (int i = 0; i < intArrExtended.Length; i++)
            {
                if (i < intArr.Length) { intArrExtended[i] = intArr[i]; }
                else intArrExtended[i] = 0;
            }
            for(int i = 0; i < intArrExtended.Length; i++)
            {
                richTextBox2.Text += intArrExtended[i];
            }
            int[] intArrExtended2 = new int[intArrExtended.Length];
            for(int i = 0; i < intArrExtended2.Length; i++)
            {
                intArrExtended2[i] = intArrExtended[arrStart[i]-1];
            }
            richTextBox2.Text += "\n";
            for (int i = 0; i < intArrExtended2.Length; i++)
            {
                richTextBox2.Text += intArrExtended2[i];
            }





            j = 0;
            /*richTextBox2.Text += "\n";
            richTextBox2.Text += "Биты исходного ключа";
            richTextBox2.Text += "\n";
            //записываем в числовой массив все биты исходного ключа
            while (j != intArrKey.Length)
            {
                for (int i = 0; i < strArrKey.Length; i++)
                {
                    foreach (char ch in strArrKey[i])
                    {
                        intArrKey[j] = (int)Char.GetNumericValue(ch);
                        richTextBox2.Text += intArrKey[j];
                        j++;
                    }
                    richTextBox2.Text += " ";
                }
            }







            //строковый массив strExit - каждый элемент содержит 8 бит числового массива исходного текста
            string[] strExit = new string[strArr.Length];
            //переписываем содержимое числового массива в строковый (нужно для вывода текста)
            //каждые восемь бит в отдельный элемент строкового массива
            int count = 0;
            for(int i = 0; i < strExit.Length; i++)
            {
                for(int k = count; k < intArr.Length; k++)
                {
                    if((k>0)&&(k%8==0))
                    {
                        strExit[i+1] += intArr[k];
                        count = k+1;
                        break;
                    }
                    strExit[i] += intArr[k];
                }
            }
            int[] intArr2 = new int[strExit.Length];
            //конвертируем обратно в int-массив
            for (int i = 0; i < intArr2.Length; i++)
            {
                intArr2[i] = Convert.ToInt32(strExit[i]);
            }
            int[] intArr3 = new int[intArr2.Length];
            //теперь переносим сконвертированные десятичные значения в другой целочисленный массив
            for (int i = 0; i < intArr2.Length; i++)
            {
                intArr3[i] = Convert.ToInt32(intArr2[i].ToString(), 2);
            }
            byte[] byteArr = new byte[intArr3.Length];
            //переносим те же значения в byte-массив
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = (byte)intArr3[i];
            }
            //переводим ASCII коды byte-массива в их символьное представление
            string str4 = Encoding.Default.GetString(byteArr);
            richTextBox3.Text += str4;  */
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() ==
                                DialogResult.OK)
            {
                fn = openFileDialog1.FileName;
                this.Text = fn;
                try
                {
                    System.IO.StreamReader sr =
                        new System.IO.StreamReader(fn, Encoding.UTF8);
                    richTextBox1.Text = sr.ReadToEnd();
                    richTextBox1.SelectionStart =
                        richTextBox1.TextLength;
                    sr.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка чтения файла.\n" +
                            exc.ToString(), "MEdit",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            docChanged = true;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (docChanged)
            {
                DialogResult dr;
                dr = MessageBox.Show("Сохранить изменения?",
                                "Crypto Program",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (SaveDocument() != 0)
                            e.Cancel = true;
                        break;
                    case DialogResult.No:
                        ;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}