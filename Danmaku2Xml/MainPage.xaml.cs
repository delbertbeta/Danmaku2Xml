using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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
            var dialog = new ContentDialog()
            {
                Title = "Add a break point",
                Content = new Controller.AddNewBreakPointControll(),
                DataContext = breakPoint
            };

            await dialog.ShowAsync();

            breakPointList.Add(breakPoint);
        }
    }
    
}
