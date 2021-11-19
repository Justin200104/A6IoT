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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace A6Two
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient mqttClient;
        public string message;
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish("Application1/Message", Encoding.UTF8.GetBytes(message));
                }
            });
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient("127.0.0.1");
                mqttClient.MqttMsgPublishReceived += publisher;
                mqttClient.Subscribe(new string[] { "Application2/Message" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                mqttClient.Connect("Application2");
            });
        }

        private void publisher(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            message = Encoding.UTF8.GetString(e.Message);
            recieveBox.Text = recieveBox.Text + message.ToString();
        }

        private void recieveBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
