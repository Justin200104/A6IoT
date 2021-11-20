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


namespace A6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient mqttClient;
        //public string message;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            if (mqttClient.IsConnected)
            {
                mqttClient.Publish("test/client", Encoding.UTF8.GetBytes(userI.Text));
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mqttClient = new MqttClient("127.0.0.1");
            mqttClient.MqttMsgPublishReceived += Publisher;
            mqttClient.Subscribe(new string[] { "test/client" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            mqttClient.Connect("Application1");
        }

        private void Publisher(object sender, MqttMsgPublishEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                var message = Encoding.UTF8.GetString(e.Message);
                recieveBox.Items.Add(message);
            });
        }


    }
}
