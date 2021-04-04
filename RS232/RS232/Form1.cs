using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RS232
{    
    public partial class Form1 : Form
    {
        private string text;
        string[] message;
        ArrayList listBinaryChars;
        public Form1()
        {
            InitializeComponent();
        }
        private void textToTab(Button b)
        {   
            if(b.Name.ToString()=="button1")
            {
                this.text = this.textBox1.Text;
                this.message = text.Split(' ');
            }
            else if (b.Name.ToString() == "button2")
            {
                this.text = this.textBox2.Text;
                this.message = this.text.Split(' ');
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox3.Clear();
            this.textBox4.Clear();            
            this.textBox2.Clear();

            Button button = (Button)sender;
            textToTab(button);
            curseFinder();            
            encodeMessage();
           this.textBox2.Text = decodeMessage();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox3.Clear();
            this.textBox4.Clear();            
            this.textBox1.Clear();

            Button button = (Button)sender;
            textToTab(button);
            curseFinder();
            encodeMessage();
            this.textBox1.Text = decodeMessage();
        }

        private void curseFinder()
        {
            for(int i = 0; i<message.Length;i++)
            {
                string line = "";

                using (StreamReader sr = new StreamReader("curses.txt"))
                {
                    while((line = sr.ReadLine())!=null)
                    {
                        if(message[i].ToLower() == line)
                        {
                            foreach (char c in message[i])
                            {
                                message[i] = message[i].Replace(c, '*');
                            }
                        }
                    }

                }
            }
        }
        private void encodeMessage()
        {
            string value = "";
            foreach(string s in message)
            {
                value += s;
                value += " ";
            }

            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
            listBinaryChars = new ArrayList();
            foreach(byte b in asciiBytes)
            {
                this.textBox3.Text += b.ToString() + " ";
                listBinaryChars.Add(new ArrayList { "0",Convert.ToString(b, 2) , "11"});
            }
            foreach (ArrayList binary in listBinaryChars)
            {
                foreach (string s in binary)
                {
                    this.textBox4.Text += s.ToString();
                }
                this.textBox4.Text += " ";
            }

        }
                
        private string decodeMessage()
        {
            string decodedMessage = "";
            foreach (ArrayList binary in listBinaryChars)
            {
                binary.RemoveAt(0);
                binary.RemoveAt(binary.Count-1);
                foreach(string s in binary)
                {
                    decodedMessage += Convert.ToChar(Convert.ToInt32(s, 2)).ToString();
                }
            }
            
            return decodedMessage;
        }

    }
}
