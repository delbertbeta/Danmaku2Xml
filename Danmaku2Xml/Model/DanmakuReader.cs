using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku2Xml.Model
{
    public static class DanmakuReader
    {
        public static async Task<ObservableCollection<Danmaku>> GetDanmakuList(Windows.Storage.StorageFile danmakuFile)
        {
            ObservableCollection<Danmaku> danmakuList = new ObservableCollection<Danmaku>();
            var lines = await Windows.Storage.FileIO.ReadLinesAsync(danmakuFile);
            foreach (string line in lines)
            {
                if (line.Contains("收到彈幕"))
                {
                    Danmaku danmaku = new Danmaku();
                    var timeString = line.Substring(0, line.IndexOf(" "));
                    danmaku.Time = DateTime.Parse(timeString);
                    int splitIndex = line.IndexOf(" ", 16);
                    danmaku.User = line.Substring(16, splitIndex - 16);
                    int ContentIndex = line.IndexOf(" ", splitIndex + 1) + 1;
                    danmaku.Content = line.Substring(ContentIndex);
                    danmakuList.Add(danmaku);
                }
            }
            return danmakuList;
        }
    }
}
