using Krypton.Toolkit;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.data;
using WindowsFormsApp1.model;
using WindowsFormsApp1.services;
using static System.Net.Mime.MediaTypeNames;
namespace WindowsFormsApp1
{
    public partial class ChatPage : UserControl
    {
        int running = 0;
        public History History;
        public ChatClient openAiApi;
        public HistoryPage historypage;
        public ChatPage()
        {
            InitializeComponent();
        }
        public void Createchat()
        {
            try
            {
                //chat = openAiApi.Chat.CreateConversation();
                //chat.Model = Model.ChatGPTTurbo;
                //chat.RequestParameters.Temperature = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ChatboxAppend(string text, Color color)
        {
            chatbox.SelectionStart = chatbox.TextLength;
            chatbox.SelectionLength = 0;
            chatbox.SelectionColor = color;
            chatbox.AppendText(text);
            chatbox.SelectionColor = chatbox.ForeColor;
        }
        private async void B_enterinput_Click(object sender, EventArgs e)
        {
            try
            {
                ConversationModel Temp = new ConversationModel();
                running = 1;
                B_enterinput.Enabled = false;

                progressBar1.Value = 20;
                string input = chatinput.Text;
                ChatboxAppend($"\n\n({Temp.TimeOfChat}) User: {input}", Color.White);

                progressBar1.Value = 40;
                ChatCompletion completion =await openAiApi.CompleteChatAsync(input);

                chatbox.SelectionStart = chatbox.Text.Length;
                chatbox.ScrollToCaret();
                chatinput.Clear();

                progressBar1.Value = 80;
                
                Temp.TimeOfMessage = DateTime.Now;
                ChatboxAppend($"\n\n({ Temp.TimeOfMessage}) Bot: {completion.Content[0].Text}", Color.Green);
                B_enterinput.Enabled = true;
                running = 0;
                chatbox.SelectionStart = chatbox.Text.Length;
                chatbox.ScrollToCaret();
              

                progressBar1.Value = 100;
                Temp.Prompt = input;
                Temp.Message = completion.Content[0].Text;
                History.Add(Temp);
                DataAccess.SaveFile("data.txt", History.getHistory());
                historypage.updatecombobox(History.getHistory().Keys.ToList());
                historypage.update();
                await Task.Delay(500);
                progressBar1.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"({DateTime.Now}) Error: {ex.Message}","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChatboxAppend($"\n\n({DateTime.Now})Error: {ex.Message}", Color.Red);
            }
            finally
            {
                B_enterinput.Enabled = true;
                running = 0;
                progressBar1.Value = 0;
            }
        }

        private void chatinput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (running == 0)
            {
                if (e.KeyChar == (char)Keys.Return)
                {
                    B_enterinput_Click(sender, e);
                }
            }

        }
    }
}
