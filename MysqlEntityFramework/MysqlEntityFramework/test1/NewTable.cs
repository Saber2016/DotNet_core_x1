using System;
using System.Collections.Generic;

namespace MysqlEntityFramework.test1
{
    public partial class NewTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public byte? Gender { get; set; }
        public decimal? Hight { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? Time { get; set; }
    }
}
