using EAGetMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mailclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MailClient client = null;
        MailServer server = null;
        public MainWindow()
        {
            InitializeComponent();
            client = new MailClient("TryIt");
        }
        public void Init()
        {
            try
            {
                client.Connect(server);
                lConnect.Content = "Connected";

                var messages = client.GetMailInfos();

                foreach (var m in messages)
                {
                    lbMail.Items.Add(m);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbLogin.Text) || String.IsNullOrWhiteSpace(tbPassword.Password))
                MessageBox.Show("Enter Login/Password");
            else
            {
                server = new MailServer(
                    "imap.gmail.com",
                    tbLogin.Text,
                    tbPassword.Password,
                    ServerProtocol.Imap4)
                {
                    SSLConnection = true,
                    Port = 993
                };
                Init();
            }
        }

        private void LbMail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MailInfo m = (MailInfo)(sender as ListBox).SelectedItem;
            Mail message = client.GetMail(m);
            tbFrom.Text = message.From.ToString();
            tbTo.Text = tbLogin.Text;
            tbSubject.Text = message.Subject;
            tbBody.Text = message.TextBody;
            foreach (var item in message.Attachments)
            {
                lbAttach.Items.Add(item);
            }
        }
    }
}
