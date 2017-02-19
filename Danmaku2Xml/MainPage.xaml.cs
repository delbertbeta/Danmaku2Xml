using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Danmaku2Xml
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Model.Danmaku> danmakuList = null;
        ObservableCollection<Model.Danmaku> targetDanmakuList = new ObservableCollection<Model.Danmaku>();
        ObservableCollection<Model.BreakPoint> breakPointList = new ObservableCollection<Model.BreakPoint>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                danmakuList = await Model.DanmakuReader.GetDanmakuList(file);
                if (danmakuList != null)
                {
                    AddBreakPointButton.IsEnabled = true;
                    ExportButton.IsEnabled = true;
                    GenerateButton.IsEnabled = true;

                    targetDanmakuList.Clear();
                    foreach (Model.Danmaku danmaku in danmakuList)
                    {
                        targetDanmakuList.Add(danmaku);
                    }
                }
                else
                {
                    AddBreakPointButton.IsEnabled = false;
                    ExportButton.IsEnabled = false;
                    GenerateButton.IsEnabled = false;
                }
            }
        }

        private async void ToolsButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;

            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(View.Tools), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void AddBreakPointButton_Click(object sender, RoutedEventArgs e)
        {
            Model.BreakPoint breakPoint = new Model.BreakPoint();

            DateTime lastStartTime;

            if (breakPointList.Count > 0)
            {
                lastStartTime = breakPointList[breakPointList.Count - 1].EndTime;
            }
            else
            {
                lastStartTime = DateTime.Parse("0:0:0");
            }

            List<object> dataContext = new List<object>();
            dataContext.Add(lastStartTime);
            dataContext.Add(breakPoint);
            var dialog = new ContentDialog()
            {
                Title = "Add a break point",
                Content = new Controller.AddNewBreakPointControll(),
                DataContext = dataContext
            };

            await dialog.ShowAsync();
            if (((Model.BreakPoint)dataContext[1]).LastTime.TimeOfDay != DateTime.Parse("0:0:0").TimeOfDay)
            {
                breakPointList.Add(breakPoint);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var buttonParent = button.Parent as StackPanel;
            var breakPoint = buttonParent.DataContext as Model.BreakPoint;

            breakPointList.Remove(breakPoint);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;

            string endTimeH = (EndTimeHTextBox.Text != "") ? EndTimeHTextBox.Text : "0";
            string endTimeM = (EndTimeMTextBox.Text != "") ? EndTimeMTextBox.Text : "0";
            string endTimeS = (EndTimeSTextBox.Text != "") ? EndTimeSTextBox.Text : "0";
            string endTimeString = $"{endTimeH}:{endTimeM}:{endTimeS}";
            DateTime endTime = DateTime.Parse(endTimeString);

            string startTimeH = (StartTimeHTextBox.Text != "") ? StartTimeHTextBox.Text : "0";
            string startTimeM = (StartTimeMTextBox.Text != "") ? StartTimeMTextBox.Text : "0";
            string startTimeS = (StartTimeSTextBox.Text != "") ? StartTimeSTextBox.Text : "0";
            string startTimeString = $"{startTimeH}:{startTimeM}:{startTimeS}";
            DateTime startTime = DateTime.Parse(startTimeString);

            var tempDanmakuList = new ObservableCollection<Model.Danmaku>();
            foreach (var danmaku in danmakuList)
            {
                if (danmaku.Time < startTime || danmaku.Time > endTime)
                {
                    continue;
                }

                var danmakuToAdd = danmaku;
                DateTime timeInTimeLine = danmaku.Time.Subtract(startTime.TimeOfDay);

                bool isInBreakPoint = false;

                foreach (Model.BreakPoint breakPoint in breakPointList)
                {
                    if (danmaku.Time > breakPoint.StartTime && danmaku.Time < breakPoint.EndTime)
                    {
                        isInBreakPoint = true;
                        break;
                    }
                    if (danmaku.Time >= breakPoint.EndTime)
                    {
                        timeInTimeLine = timeInTimeLine.Subtract(breakPoint.LastTime.TimeOfDay);
                    }

                }

                if (isInBreakPoint)
                {
                    continue;
                }

                danmakuToAdd.TimeInTimeLine = timeInTimeLine;

                tempDanmakuList.Add(danmakuToAdd);

            }

            targetDanmakuList.Clear();
            foreach (var danmaku in tempDanmakuList)
            {
                targetDanmakuList.Add(danmaku);
            }
            MyProgressRing.IsActive = false;
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Windows.Storage.Pickers.FileSavePicker();
            fileDialog.SuggestedFileName = "danmaku";
            fileDialog.DefaultFileExtension = ".xml";
            fileDialog.FileTypeChoices.Add("danmaku", new List<string>() { ".xml" });

            var file = await fileDialog.PickSaveFileAsync();
            
            if (file != null)
            {
                var fileStream = await file.OpenStreamForWriteAsync();
                StreamWriter sw = new StreamWriter(fileStream);
                await sw.WriteLineAsync("<?xml version=\"1.0\"?>");
                await sw.WriteLineAsync("<i xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

                Random random = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds));
                foreach (var danmaku in targetDanmakuList)
                {
                    string timeToAppear = Convert.ToString(Convert.ToInt32((danmaku.TimeInTimeLine.TimeOfDay.TotalSeconds))) + "." + Convert.ToString(random.Next(0, 999));

                    //  <d p="2669.27,1,25,16777215,1427278336,0,0,0">星星！！！</d>
                    await sw.WriteLineAsync($"<d p=\"{timeToAppear},1,25,16777215,{(danmaku.Time - new DateTime(1970, 1, 1)).TotalSeconds},0,0,0\">{danmaku.Content}</d>");
                }

                await sw.WriteLineAsync("</i>");
                sw.Dispose();

            }
            else
            {

            }

        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            EndTimeHTextBox.Text = App.clipBoard.Hour.ToString();
            EndTimeMTextBox.Text = App.clipBoard.Minute.ToString();
            EndTimeSTextBox.Text = App.clipBoard.Second.ToString();
        }
    }

}
