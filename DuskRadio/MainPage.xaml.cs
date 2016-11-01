using DuskRadio.Models;
using DuskRadio.Models.Play;
using DuskRadio.Utility;
using DuskRadio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace DuskRadio
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string playUrl = string.Empty;
        private Player player;
        private IList<RadioNode> nodeList;
        public MainPage()
        {
            this.InitializeComponent();
            player = new Player();
            BindList();
        }

        private async void BindList()
        {
            nodeList = await XmlNodeHelper.GetNodeList(@".\Resources\live.xml");
            var nodeViewList = new List<RadioViewModel>();
            int i = 1;
            foreach (var item in nodeList)
            {
                nodeViewList.Add(new RadioViewModel()
                {
                    Index = i,
                    Label = item.Label,
                    PlayURL = item.PlayURL,
                    Color = i % 2 != 0 ? "White" : "#DCDCDC"
                });
                i++;
            }
            radioList.ItemsSource = nodeViewList;
        }

        private async void play_Click(object sender, RoutedEventArgs e)
        {
            if (!await CheckUrl())
                return;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await player.Play(playUrl);
                });
        }

        /// <summary>
        /// 校验播放链接是否为空
        /// </summary>
        /// <returns>True.不为空 False.为空</returns>
        private async Task<bool> CheckUrl()
        {
            if (string.IsNullOrEmpty(playUrl))
            {
                var dialog = new ContentDialog()
                {
                    Title = " 提示",
                    Content = "播放链接为空，请重新选择电台",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();

                return false;
            }
            return true;
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (player != null)
                player.Volumn = volumeSlider.Value / 100.0;
        }

        private void radioList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = (ItemsWrapGrid)radioList.ItemsPanelRoot;
            panel.ItemWidth = e.NewSize.Width - 20;
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (radioList.SelectedItem != null)
            {
                var item = radioList.SelectedItem as RadioNode;
                playUrl = item.PlayURL;
            }
        }

        /// <summary>
        /// 双击时播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void radioList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!await CheckUrl())
                return;

            var result = await player.Play(playUrl);
            if (!result)
            {
                var dialog = new ContentDialog()
                {
                    Title = " 提示",
                    Content = "播放失败，请重试",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var grid = sender as Grid;
            grid.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)220, (byte)220, (byte)220));
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var grid = sender as Grid;
            var block = grid.Children[1] as TextBlock;
            if (int.Parse(block.Text) % 2 != 0)
                grid.Background = new SolidColorBrush(Colors.White);
        }
    }
}
