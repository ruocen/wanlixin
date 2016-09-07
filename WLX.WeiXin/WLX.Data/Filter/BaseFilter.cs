using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Data.Filter
{
    /// <summary>
    /// 基础数据
    /// </summary>
    public class BaseFilter
    {
        public BaseFilter()
        {
        }
        /// <summary>
        /// 是否加载全部数据
        /// </summary>
        [Required]
        public bool All { get; set; }
    }
}
