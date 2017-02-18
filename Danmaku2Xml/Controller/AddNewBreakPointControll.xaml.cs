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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Danmaku2Xml.Controller
{
    public sealed partial class AddNewBreakPointControll : UserControl
    {



        public string endTimeH { get; set; }
        public string endTimeM { get; set; }
        public string endTimeS { get; set; }

        public string nextStartTimeH { get; set; }
        public string nextStartTimeM { get; set; }
        public string nextStartTimeS { get; set; }


        public AddNewBreakPointControll()
        {
            this.InitializeComponent();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ((ContentDialog)Parent).Hide();
            string endTimeH = EndTimeHTextBox.Text;
            string endTimeM = EndTimeMTextBox.Text;
            string endTimeS = EndTimeSTextBox.Text;
            string endTimeString = $"{endTimeH}:{endTimeM}:{endTimeS}";
            DateTime endTime = DateTime.Parse(endTimeString);

            string nextStartTimeH = NextStartTimeHTextBox.Text;
            string nextStartTimeM = NextStartTimeMTextBox.Text;
            string nextStartTimeS = NextStartTimeSTextBox.Text;

            string nextStartTimeString = $"{nextStartTimeH}:{nextStartTimeM}:{nextStartTimeS}";
            DateTime nextStartTime = DateTime.Parse(nextStartTimeString);

            var breakPoint = this.DataContext as Model.BreakPoint;
            breakPoint.StartTime = endTime;
            breakPoint.EndTime = nextStartTime;

            var dialog = this.Parent as ContentDialog;
            dialog.Hide();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string startTimeH = StartTimeHTextBox.Text;
            string startTimeM = StartTimeMTextBox.Text;
            string startTimeS = StartTimeSTextBox.Text;

            string startTimeString = $"{startTimeH}:{startTimeM}:{startTimeS}";
            DateTime startTime = DateTime.Parse(startTimeString);

            string lastTimeH = lastTimeHTextBox.Text;
            string lastTimeM = lastTimeMTextBox.Text;
            string lastTimeS = lastTimeSTextBox.Text;

            string lastTimeString = $"{lastTimeH}:{lastTimeM}:{lastTimeS}";
            DateTime lastTime = DateTime.Parse(lastTimeString);

            DateTime endTime = startTime.Add(lastTime.TimeOfDay);

            EndTimeHTextBox.Text = Convert.ToString(endTime.Hour);
            EndTimeMTextBox.Text = Convert.ToString(endTime.Minute);
            EndTimeSTextBox.Text = Convert.ToString(endTime.Second);
        }
    }
}
