using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 天然气供应方案分析与决策软件
{
    class Common
    {
        public static string path = null;
        public static void ParameterErrorDetectionCompressureFacator(string str1, string str2)
        {

            //参数检测，判断txtInput2输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断txtInput2输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 1.5 || targetValue1 < 0.1)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[0.1,1.5]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionDailyWorkTime(string str1, string str2)
        {
            //参数检测，判断输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 24)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,24]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionTaxiCount(string str1,string str2)
        {
            //参数检测，判断输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,10000]
            if (targetValue1 >10000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,24]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionBusCount(string str1, string str2)
        {
            //参数检测，判断输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,10000]
            if (targetValue1 > 10000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,24]，请重新输入。");
            }
        }

       // todoadd
    }
}
