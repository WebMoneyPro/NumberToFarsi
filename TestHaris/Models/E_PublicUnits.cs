using System;
using System.Collections.Generic;
using System.Text;

namespace TestHaris.Models
{
    /// <summary>
    /// کلاسی که در آن تمام اینام ها تعریف میشود
    /// </summary>
    public class E_PublicUnits
    {
        /// <summary>
        /// واحد گذاری هر قطعه از لیست اعداد
        /// </summary>
        public enum E_Segment
        {
            /// <summary>
            /// سه رقمی / صد به پائین
            /// </summary>
            Hundred,
            /// <summary>
            /// صد هزار تا نه صد هزار
            /// </summary>
            Thousand,
            /// <summary>
            /// صد میلیون تا نه صد میلیون
            /// </summary>
            Million,
            /// <summary>
            /// صد میلیارد تا نه صد میلیارد
            /// </summary>
            Billion,
            /// <summary>
            /// صد تریلیون تا نه صد تریلیون
            /// </summary>
            Trillion
        }
    }
}
