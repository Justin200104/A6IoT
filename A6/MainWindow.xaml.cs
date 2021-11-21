/*
File: MainWindow.xaml.cs
Project: A6
Programmers: Justin Langevin, Josiah Rehkopf, Nikola Ristic, Jesse Rutledge
First Version: 2021 - 11- 15
Description: This is a test mqtt client for the assignment 6 project for IoT
*/
using System.Text;
using System.Windows;
using System.Windows.Input;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace A6
{
    /*
    Class: MainWindow
    Purpose: runs all the mqtt client code for the project
    */
    public partial class MainWindow : Window
    {
        MqttClient mqttClient;
        bool connected = false;

        public MainWindow()
        {
            InitializeComponent();
            connectButton.Focus();

        }

        /* -------------------------------------------------------------------------------------------
        * Method	        :	Button_Click()
        * Description	    :	This Method is used to connect to the mqtt broker and subscribe to the topic				
        * Parameters	    :	object sender, RoutedEventArgs e
        * Returns		    :	void
        * ------------------------------------------------------------------------------------------*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mqttClient = new MqttClient("test.mosquitto.org");
            mqttClient.MqttMsgPublishReceived += Publisher;
            mqttClient.Subscribe(new string[] { "test/topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            mqttClient.Connect("Application1");
            connected = true;
        }

        /* -------------------------------------------------------------------------------------------
        * Method	        :	Publisher()
        * Description	    :	This Method is used to display the messages being published to the mqtt topic.			
        * Parameters	    :	object sender, MqttMsgPublisheventArgs e
        * Returns		    :	void
        * ------------------------------------------------------------------------------------------*/
        private void Publisher(object sender, MqttMsgPublishEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var message = Encoding.UTF8.GetString(e.Message);
                recieveBox.Items.Add(message);
            });
        }

        /* -------------------------------------------------------------------------------------------
        * Method	        :	UserI_KeyDown()
        * Description	    :	This Method is called when the user pushes the enter key to publish the message to the mqtt topic			
        * Parameters	    :	object sender, KeyEventArgs e
        * Returns		    :	void
        * ------------------------------------------------------------------------------------------*/
        private void UserI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (connected)
                {
                    mqttClient.Publish("test/topic", Encoding.UTF8.GetBytes(userI.Text));
                    userI.Text = string.Empty;
                }
            }
        }
    }
}
