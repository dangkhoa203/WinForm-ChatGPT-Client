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
using WindowsFormsApp1.data;
using WindowsFormsApp1.model;
using WindowsFormsApp1.services;

namespace WindowsFormsApp1
{
    public partial class MainPage : Form
    {
        private ChatClient openAiApi;
        private readonly History History=new History();
        public Main EnterPage;
        public MainPage(string apikey)
        {
            InitializeComponent();
            label1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ChatClient client = 
            openAiApi = new ChatClient(model: "gpt-4o", apiKey: apikey);
            chatPage1.History = this.History;
            chatPage1.openAiApi = this.openAiApi;
            chatPage1.Createchat();
            if (History.Find(DateTime.Now.Date.ToString("dd/MM/yyyy")) != null)
            {
                foreach (ConversationModel tem in History.Find(DateTime.Now.Date.ToString("dd/MM/yyyy")))
                {
                    chatPage1.ChatboxAppend("\n\n(" + tem.TimeOfChat + ") User: " + tem.Prompt, Color.White);
                    chatPage1.ChatboxAppend("\n\n(" + tem.TimeOfMessage + ") Bot: " + tem.Message, Color.Green);
                }
            }
            historyPage1.History = this.History;
            historyPage1.updatecombobox(History.getHistory().Keys.ToList());
            chatPage1.historypage = historyPage1;
        }

        private void fclose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void fminimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void b_history_Click(object sender, EventArgs e)
        {
            chatPage1.Visible = false;
            historyPage1.Visible = true;
            aboutPage1.Visible= false;
        }

        private void b_chat_Click(object sender, EventArgs e)
        {
            chatPage1.Visible = true;
            historyPage1.Visible = false;
            aboutPage1.Visible = false;
        }

        private void b_about_Click(object sender, EventArgs e)
        {
            chatPage1.Visible = false;
            historyPage1.Visible = false;
            aboutPage1.Visible = true;
        }

        private void b_back_Click(object sender, EventArgs e)
        {
            EnterPage.Show();
            this.Close();
        }
    }
}
