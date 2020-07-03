using System;
namespace MVCSample01.Models
{
    public class Content
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        ///
        /// 内容
        /// </summary>
        public int status { get; set; }
        /// <summary>
        ///
        /// 创建时间
        /// </summary>
        public DateTime add_time { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modified_time { get; set; }

    }
}
