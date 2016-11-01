using DuskRadio.Models.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskRadio.ViewModels
{
    class RadioViewModel:RadioNode
    {
        /// <summary>
        /// 索引值
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public string Color { get; set; }
    }
}
