using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Data.Filter
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    public class KeywordPageFilter : BasePageFilter
    {
        public KeywordPageFilter()
        {
            base.Start = 0;
            base.Limit = 10;
            base.PageIndex = 1;
        }
        public KeywordPageFilter(string keyword,int pageIndex, int pageSize)
        {
            base.Start = (pageIndex - 1) * pageSize;
            base.Limit = pageSize;
            base.PageIndex = pageIndex;
            Keyword = keyword;
        }
        /// <summary>
        /// 公共参数
        /// </summary>
        [Display(Name = "参数")]
        public string Keyword { get; set; }
    }
}
