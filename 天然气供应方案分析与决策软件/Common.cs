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
        private MathOpt calculator = new MathOpt();
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
        public static void ParameterErrorDetectionTaxiCount(string str1, string str2)
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
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,10000]，请重新输入。");
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
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,10000]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionFlow(string str1, string str2)
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
            if (targetValue1 > 1000000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,1000000]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectioPressure(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0,100]MPa
            if (targetValue1 > 100)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,100]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectioLength(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0,1000]千米
            if (targetValue1 > 1000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,1000]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectioDiameter(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0,10000]毫米
            if (targetValue1 > 10000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,10000]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionRough(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 [0.0001,0.1]
            if (targetValue1 > 0.1 || targetValue1 < 0.0001)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[0.0001,0.1]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionGasWeighRatio(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0.01,2]
            if (targetValue1 > 2 || targetValue1 < 0.01)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[0.01,2]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionAverageTemperature(string str1, string str2)
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

            //参数检测，判断输入是否在规定范围内 [-50,80]
            if (targetValue1 > 80 || targetValue1 < -50)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[-50,80]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionIterations(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0,1000]
            if (targetValue1 > 1000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,1000]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionRelativeError(string str1, string str2)
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
            //参数检测，判断输入是否在规定范围内 （0.000001,0.1]
            if (targetValue1 > 0.1 || targetValue1 < 0.000001)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[0.000001,0.1]，请重新输入。");
            }
        }
        public static void ParameterErrorDetectionLowPressureRatio(string str1, string str2)
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
        }
        public static void ParameterErrorDetectionMiddlePressureRatio(string str1, string str2)
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
        }
        public static void ParameterErrorDetectionIncrementInterval(string str1, string str2)
        {

            //参数检测，判断txtInput2输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");
            }
            double targetValue1 = Convert.ToDouble(str2);
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");
            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 10000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数间隔太大[0,10000]，请重新输入。");
            }
        }
        public double  LowPressureAnalysis(double V1, double V2, double V3, double V4, double V5, double V6, double V7)
        {

            double StdFlow = V1 * 10000;   //标准体积流量   StdFlow
            double UpPre = V2;     //管线上游表压   UpPre
            double Dia = V3;        //管线内径       Dia
            double AbsRough = V4;   //绝对粗糙度     AbsRough
            double Length = V5;     //管线长度       Length
            double GasWeight = V6;  //气体比重       GasWeight
            double Tep = V7;        //气体平均温度 
            double PP1 = UpPre;
            double z1 = calculator.CompressibilityFactor(UpPre, GasWeight, Tep);
            double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
            double PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            while (Math.Abs(PP2 - PP1) > 0.00001)
            {
                PP1 = PP2;
                z1 = calculator.CompressibilityFactor(PP1, GasWeight, Tep);
                PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            }
            double PipelinePressure;
            PipelinePressure = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            return PipelinePressure;
            double bn = calculator.CompressibilityFactorParameter(UpPre, PP1, GasWeight, Tep);

            //txtOutput2.Text = (calculator.MeanVelocity(StdFlow, Dia) * bn).ToString("0.000");
            //txtOutput3.Text = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow).ToString("0.000000");
            //txtOutput4.Text = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough).ToString("0.000000");

        }
        public double FlowSpeed(double V1, double V2, double V3, double V4, double V5, double V6, double V7)
        {

            double StdFlow = V1 * 10000;   //标准体积流量   StdFlow
            double UpPre = V2;     //管线上游表压   UpPre
            double Dia = V3;        //管线内径       Dia
            double AbsRough = V4;   //绝对粗糙度     AbsRough
            double Length = V5;     //管线长度       Length
            double GasWeight = V6;  //气体比重       GasWeight
            double Tep = V7;        //气体平均温度 
            double PP1 = UpPre;
            double z1 = calculator.CompressibilityFactor(UpPre, GasWeight, Tep);
            double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
            double PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            while (Math.Abs(PP2 - PP1) > 0.00001)
            {
                PP1 = PP2;
                z1 = calculator.CompressibilityFactor(PP1, GasWeight, Tep);
                PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            }
            double MeanVelocity;
            //PipelinePressure = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            
            double bn = calculator.CompressibilityFactorParameter(UpPre, PP1, GasWeight, Tep);

            MeanVelocity = (calculator.MeanVelocity(StdFlow, Dia) * bn);
            return MeanVelocity;
            //txtOutput3.Text = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow).ToString("0.000000");
            //txtOutput4.Text = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough).ToString("0.000000");

        }
        public double DaXiXiShuVar(double V1, double V2, double V3, double V4, double V5, double V6, double V7)
        {

            double StdFlow = V1 * 10000;   //标准体积流量   StdFlow
            double UpPre = V2;     //管线上游表压   UpPre
            double Dia = V3;        //管线内径       Dia
            double AbsRough = V4;   //绝对粗糙度     AbsRough
            double Length = V5;     //管线长度       Length
            double GasWeight = V6;  //气体比重       GasWeight
            double Tep = V7;        //气体平均温度 
            double PP1 = UpPre;
            double z1 = calculator.CompressibilityFactor(UpPre, GasWeight, Tep);
            double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
            double PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            while (Math.Abs(PP2 - PP1) > 0.00001)
            {
                PP1 = PP2;
                z1 = calculator.CompressibilityFactor(PP1, GasWeight, Tep);
                PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            }
            double MeanVelocity;
            //PipelinePressure = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            //double bn = calculator.CompressibilityFactorParameter(UpPre, PP1, GasWeight, Tep);

            //MeanVelocity = (calculator.MeanVelocity(StdFlow, Dia) * bn);

            //txtOutput3.Text = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow).ToString("0.000000");
            MeanVelocity= calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);
            return MeanVelocity;

        }
        public double LeiNuoXiShuVar(double V1, double V2, double V3, double V4, double V5, double V6, double V7)
        {

            double StdFlow = V1 * 10000;   //标准体积流量   StdFlow
            double UpPre = V2;     //管线上游表压   UpPre
            double Dia = V3;        //管线内径       Dia
            double AbsRough = V4;   //绝对粗糙度     AbsRough
            double Length = V5;     //管线长度       Length
            double GasWeight = V6;  //气体比重       GasWeight
            double Tep = V7;        //气体平均温度 
            double PP1 = UpPre;
            double z1 = calculator.CompressibilityFactor(UpPre, GasWeight, Tep);
            double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
            double PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            while (Math.Abs(PP2 - PP1) > 0.00001)
            {
                PP1 = PP2;
                z1 = calculator.CompressibilityFactor(PP1, GasWeight, Tep);
                PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
            }
            double MeanVelocity;
            //PipelinePressure = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);

            //double bn = calculator.CompressibilityFactorParameter(UpPre, PP1, GasWeight, Tep);

            //MeanVelocity = (calculator.MeanVelocity(StdFlow, Dia) * bn);

            MeanVelocity= calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow);
            return MeanVelocity;
            //txtOutput4.Text = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough).ToString("0.000000");

        }

    }
}
