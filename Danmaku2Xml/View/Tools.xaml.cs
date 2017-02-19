using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Danmaku2Xml.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Tools : Page
    {
        public Tools()
        {
            this.InitializeComponent();
        }


        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {

            string startTimeH = (StartTimeHTextBox.Text != "") ? StartTimeHTextBox.Text : "0";
            string startTimeM = (StartTimeMTextBox.Text != "") ? StartTimeMTextBox.Text : "0";
            string startTimeS = (StartTimeSTextBox.Text != "") ? StartTimeSTextBox.Text : "0";

            string startTimeString = $"{startTimeH}:{startTimeM}:{startTimeS}";
            DateTime startTime = DateTime.Parse(startTimeString);

            string lastTimeH = (lastTimeHTextBox.Text != "") ? lastTimeHTextBox.Text : "0";
            string lastTimeM = (lastTimeMTextBox.Text != "") ? lastTimeMTextBox.Text : "0";
            string lastTimeS = (lastTimeSTextBox.Text != "") ? lastTimeSTextBox.Text : "0";

            string lastTimeString = $"{lastTimeH}:{lastTimeM}:{lastTimeS}";
            DateTime lastTime = DateTime.Parse(lastTimeString);

            DateTime endTime = startTime.Add(lastTime.TimeOfDay);

            App.clipBoard = endTime;

        }

        private void lastTimeSTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CalculateButton_Click(CalculateButton, new RoutedEventArgs());
            }
        }
    }
}
