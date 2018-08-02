using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLYD.WLYC.Mall.Domain.Models
{
    /// <summary>
    ///     商品价格
    /// </summary>
    public class GoodsPrice
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid GoodsPriceId { set; get; }

        /// <summary>
        ///     外键ID，存ProductId or GoodsId
        /// </summary>
        public Guid ObjectId { set; get; }

        /// <summary>
        ///     起订数量(开始)
        /// </summary>
        public int StartNumber { set; get; }

        /// <summary>
        ///     起订数量(结束)
        /// </summary>
        public int EndNumber { set; get; }

        /// <summary>
        ///     价格
        /// </summary>
        public decimal Price { set; get; }
    }
}
