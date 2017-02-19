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

        public List<object> dataContext;

        public AddNewBreakPointControll()
        {
            this.InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                dataContext = this.DataContext as List<object>;

                var lastStartTime = (DateTime)dataContext[0];
                StartTimeHTextBox.Text = lastStartTime.Hour.ToString();
                StartTimeMTextBox.Text = lastStartTime.Minute.ToString();
                StartTimeSTextBox.Text = lastStartTime.Second.ToString();
            };
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ((ContentDialog)Parent).Hide();
            string endTimeH = (EndTimeHTextBox.Text != "") ? EndTimeHTextBox.Text : "0";
            string endTimeM = (EndTimeMTextBox.Text != "") ? EndTimeMTextBox.Text : "0";
            string endTimeS = (EndTimeSTextBox.Text != "") ? EndTimeSTextBox.Text : "0";
            string endTimeString = $"{endTimeH}:{endTimeM}:{endTimeS}";
            DateTime endTime = DateTime.Parse(endTimeString);

            string nextStartTimeH = (NextStartTimeHTextBox.Text != "") ? NextStartTimeHTextBox.Text : "0";
            string nextStartTimeM = (NextStartTimeMTextBox.Text != "") ? NextStartTimeMTextBox.Text : "0";
            string nextStartTimeS = (NextStartTimeSTextBox.Text != "") ? NextStartTimeSTextBox.Text : "0";

            string nextStartTimeString = $"{nextStartTimeH}:{nextStartTimeM}:{nextStartTimeS}";
            DateTime nextStartTime = DateTime.Parse(nextStartTimeString);

            var breakPoint = dataContext[1] as Model.BreakPoint;
            breakPoint.StartTime = endTime;
            breakPoint.EndTime = nextStartTime;

            var dialog = this.Parent as ContentDialog;
            dialog.Hide();
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

            EndTimeHTextBox.Text = Convert.ToString(endTime.Hour);
            EndTimeMTextBox.Text = Convert.ToString(endTime.Minute);
            EndTimeSTextBox.Text = Convert.ToString(endTime.Second);

            NextStartTimeHTextBox.Focus(FocusState.Programmatic);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as ContentDialog).Hide();
        }

        private void lastTimeSTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CalculateButton_Click(CalculateButton, new RoutedEventArgs());
            }
        }

        private void NextStartTimeSTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddButton_Click(AddButton, new RoutedEventArgs());
            }
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
