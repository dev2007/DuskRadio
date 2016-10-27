using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace DuskRadio
{
    /// <summary>
    /// 播放器，封装了<see cref="MediaElement"/>的常用操作
    /// </summary>
    class Player
    {
        private double volumn;
        /// <summary>
        /// 获取、设置<see cref="MediaElement"/>的音量
        /// </summary>
        public double Volumn
        {
            get
            {
                return volumn;
            }
            set
            {
                if (value > 1)
                {
                    volumn = 1;
                }else if(volumn < 0)
                {
                    volumn = 0;
                }

                SetVolumn();
            }
        }
        private MediaElement mediaElement;
        private AdaptiveMediaSourceCreationResult amsResult;
        public Player()
        {
            mediaElement = new MediaElement();
            mediaElement.Volume = 1;
        }

        /// <summary>
        /// 设置MediaElement音量
        /// </summary>
        private void SetVolumn()
        {
            mediaElement.Volume = volumn;
        }

        /// <summary>
        /// 播放为<see cref="MediaElement"/>指定的链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> Play(string url)
        {
            if (amsResult == null)
                amsResult = await AdaptiveMediaSource.CreateFromUriAsync(new Uri(url, UriKind.Absolute));
            if (amsResult.Status == AdaptiveMediaSourceCreationStatus.Success)
            {

                AdaptiveMediaSource ams = amsResult.MediaSource;
                mediaElement.SetMediaStreamSource(ams);
                mediaElement.Play();
                return true;
            };
            return false;
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            if (mediaElement != null)
                mediaElement.Stop();
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            if (mediaElement != null)
                mediaElement.Pause();
        }
    }
}
