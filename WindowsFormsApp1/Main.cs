using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WindowsFormsApp1.data;

namespace WindowsFormsApp1
{
    public partial class Main : Krypton.Toolkit.KryptonForm
    {
        private int running = 0;
        public Main()
        {
            InitializeComponent();
        }
        private void kryptonTextBox1_Click(object sender, EventArgs e)
        {
            apikey.SelectAll();
        }
        private async void Enter_Click(object sender, EventArgs e)
        {
            try
            {
               
                running++;
                Enter.Enabled = false;
                OpenAIClient client = new OpenAIClient(apikey.Text);
                ChatClient chatClient = client.GetChatClient("gpt-4");
                try {
                    ChatCompletion completion = await chatClient.CompleteChatAsync("Say 'hello'.");
                    this.Hide();
                    MainPage access = new MainPage(apikey.Text);
                    access.EnterPage = this;
                    access.ShowDialog();
                }
                catch {
                     MessageBox.Show($"Your API key can't be use!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
               
            }
            finally {
                running--;
                Enter.Enabled = true;
            }
            
        }

        private void apikey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (running == 0)
            {
                if (e.KeyChar == (char)Keys.Return)
                {
                    Enter_Click(sender, e);
                }
            }

        }
    }
}
