using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TestHaris.Models;

namespace TestHaris
{
    public partial class Form1 : Form
    {
        public class FarsiNumberConverter
        {
            // Dictionary to map Farsi digits to their corresponding strings
            private static readonly Dictionary<int, string> FarsiDigits = new Dictionary<int, string>
            {
                [0] = "صفر",
                [1] = "یک",
                [2] = "دو",
                [3] = "سه",
                [4] = "چهار",
                [5] = "پنج",
                [6] = "شش",
                [7] = "هفت",
                [8] = "هشت",
                [9] = "نه",
                [10] = "ده"
            };

            // Dictionary to map Farsi tens to their corresponding strings
            private static readonly Dictionary<int, string> FarsiTens = new Dictionary<int, string>
            {
                [10] = "ده",
                [20] = "بیست",
                [30] = "سی",
                [40] = "چهل",
                [50] = "پنجاه",
                [60] = "شصت",
                [70] = "هفتاد",
                [80] = "هشتاد",
                [90] = "نود"
            };

            // Dictionary to map Farsi teens (11-19) to their corresponding strings
            private static readonly Dictionary<int, string> FarsiTeens = new Dictionary<int, string>
            {
                [11] = "یازده",
                [12] = "دوازده",
                [13] = "سیزده",
                [14] = "چهارده",
                [15] = "پانزده",
                [16] = "شانزده",
                [17] = "هفده",
                [18] = "هجده",
                [19] = "نوزده"
            };
            // Dictionary to map Farsi hundreds to their corresponding strings
            private static readonly Dictionary<int, string> FarsiHundreds = new Dictionary<int, string>() {
                [1] = "صد",
                [2] = "دویست",
                [3] = "سیصد",
                [4] = "چهارصد",
                [5] = "پونصد",
                [6] = "ششصد",
                [7] = "هفتصد",
                [8] = "هشتصد",
                [9] = "نه صد",
            };
            // Dictionary to map Farsi thousands to their corresponding strings
            private static readonly Dictionary<E_PublicUnits.E_Segment, string> FarsiThousands = new Dictionary<E_PublicUnits.E_Segment, string>
            {
                [E_PublicUnits.E_Segment.Hundred] = "",
                [E_PublicUnits.E_Segment.Thousand] = "هزار",
                [E_PublicUnits.E_Segment.Million] = "میلیون",
                [E_PublicUnits.E_Segment.Billion] = "میلیارد",
                [E_PublicUnits.E_Segment.Trillion] = "تیلیارد"
            };

            // Separator used to join segments of the converted number
            private static readonly string FarsiSeparator = " و ";

            // Method to convert a number to its Farsi representation
            public static string ConvertToFarsi(string number)
            {
                var result = new List<string>();
                // در این مرحله چک کردن ورودی داریم
                if (!long.TryParse(number, out long numericValue))
                    return "ورودی را فقط عدد وارد کنید";
                // اگر صفر بود بنویس صفر
                if (numericValue == 0)
                    return FarsiDigits['0'];
                // اگر عدد منفی بود اولش بنویس منفی
                if (numericValue < 0)
                {
                    result.Add("منفی");
                    numericValue = Math.Abs(numericValue);
                }

                // تبدیل عدد به بخش های سه تایی
                // با مدل M_Segment
                List<M_Segment> listSegments = SplitNumberIntoSegments(numericValue);

                // تبدیل سه تایی ها به حروف فارسی
                for (int i = listSegments.Count - 1; i >= 0; i--)
                {
                    M_Segment segment = listSegments[i];
                    var segmentText = ConvertSegmentToFarsi(segment);
                    string thousandSuffix = "";
                    //if (segment == listSegments.LastOrDefault())
                    thousandSuffix = FarsiThousands[segment.Segment];
                    result.Add(segmentText + " " + thousandSuffix);
                }

                // Join the segments with the Farsi separator, and remove any empty strings
                return string.Join(FarsiSeparator, result.Where(s => !string.IsNullOrEmpty(s)));
            }

            /// <summary>
            /// متد تبدیل عدد به بخش های 3 رقمی
            /// </summary>
            /// <param name="number">عدد ورودی کاربر</param>
            /// <returns>لیست با عضو های سه رقمی</returns>
            private static List<M_Segment> SplitNumberIntoSegments(long number)
            {
                List<M_Segment> Result = new List<M_Segment>();
                int SegmentCounter = 1;

                while (number > 0)
                {
                    long seg = number % 1_000;
                    // تنظیم سِگمِنت
                    M_Segment segment = new M_Segment()
                    {
                        Numbers = (int)seg
                    };
                    // تنظیم واحد شمارش
                    switch (SegmentCounter)
                    {
                        case 2:
                            segment.Segment = E_PublicUnits.E_Segment.Thousand;
                            break;
                        case 3:
                            segment.Segment = E_PublicUnits.E_Segment.Million;
                            break;
                        case 4:
                            segment.Segment = E_PublicUnits.E_Segment.Billion;
                            break;
                        case 5:
                            segment.Segment = E_PublicUnits.E_Segment.Trillion;
                            break;
                        default:
                            segment.Segment = E_PublicUnits.E_Segment.Hundred;
                            break;
                    }
                    Result.Add(segment);
                    number /= 1_000;
                    SegmentCounter++;
                }

                return Result;
            }

            /// <summary>
            /// تبدیل هر قسمت از عدد (سِگمِنت) به فارسی
            /// </summary>
            /// <param name="segment"></param>
            /// <returns></returns>
            private static string ConvertSegmentToFarsi(M_Segment segment)
            {
                List<string> result = new List<string>();

                // اضافه کردن صد
                if (segment.Numbers >= 100)
                {
                    int hundredsDigit = segment.Numbers / 100;
                    string hundredsText = FarsiHundreds[hundredsDigit];
                    result.Add(hundredsText);
                    segment.Numbers %= 100;
                }

                // If the segment has a tens digit, add its Farsi representation
                if (segment.Numbers >= 20)
                {
                    int tensDigit = segment.Numbers / 10;
                    string tensText = FarsiTens[tensDigit * 10];
                    result.Add(tensText);
                    segment.Numbers %= 10;
                }
                // If the segment is in the teens (11-19), add its Farsi representation
                else if (segment.Numbers >= 11)
                {
                    string teensText = FarsiTeens[segment.Numbers];
                    result.Add(teensText);
                    segment.Numbers = 0;
                }

                // If the segment has a units digit, add its Farsi representation
                if (segment.Numbers > 0)
                {
                    string unitsText = FarsiDigits[segment.Numbers];
                    result.Add(unitsText);
                }

                // Join the Farsi representations with the Farsi separator
                return string.Join(FarsiSeparator, result);
            }
        }
        public Form1()
        {
            InitializeComponent();
            lblResult.Text = "New Label Text";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string number = textBox1.Text;
            string farsiText = FarsiNumberConverter.ConvertToFarsi(number);
            lblResult.Text = farsiText;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
