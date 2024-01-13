using System;
using System.Collections.Generic;
using System.Text;

namespace TestHaris.Models
{
    public class M_Segment
    {
        /// <summary>
        /// عدد سه رقمی
        /// </summary>
        public int Numbers { get; set; }
        /// <summary>
        /// عنوان عدد
        /// مثال : صد هزار ، صد میلیون ، صد میلیارد
        /// </summary>
        public E_PublicUnits.E_Segment Segment { get; set; }
    }
}
