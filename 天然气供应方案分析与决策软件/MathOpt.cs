using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 天然气供应方案分析与决策软件
{
   public  class MathOpt
    {

        #region 计算达西摩阻系数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="T1">气体平均温度</param>
        /// <param name="Q1">气体比重</param>
        /// <param name="D1">管线内径</param>
        /// <param name="F1">标准体积流量</param>
        /// <param name=K1">绝对粗糙度</param>
        /// <returns></returns>

        public double DaXiXiShu(double T1, double Q1, double D1, double F1, double K1)
        {
            double T2 = (T1 + 273) / 191.16;                             //注意温度单位是℉而不是摄氏度，需要单位换算
            double N1 = 1.0009 * Math.Pow(10, -5) * Math.Pow(Q1, 0.5); //N1表示临界温度T下的动力粘度
            double N2;                                                 //N2 表示为实际温度下T下的动力粘度；
            //double R1=0.039685;
            double R2;
            if (T2 <= 1)         //注意是T2不是T1!!
            {
                N2 = N1 * Math.Pow(T2, 0.005);
            }
            else
            {
                N2 = N1 * Math.Pow(T2, 0.71 + 0.29 * 191.16 / (T1 + 273));
            }
            double Re1 = 0.01777 * Q1 * F1 / (D1 * N2);
            double BL1;   //BL1,BL2 均为中间变量，方便计算
            double BL2;
            if (Re1 <= 4000)
            {
                if (Re1 <= 2000)
                {
                    R2 = 64 / Re1;
                }
                else
                {
                    R2 = 0.0025 * Math.Pow(Re1, 1 / 3.0);
                }
            }
            else
            {
                BL1 = K1 / (3.71 * D1) + 5.7385 / (Math.Pow(Re1, 0.9));
                BL2 = Math.Log(BL1);
                R2 = 1.33036 / Math.Pow(BL2, 2);   //  由于求解的达西系数为隐函数，不方面计算，这里参考文献 <<似牛顿浆体几个摩阻系数计算公式的比较>> 求解其系数
            }
            return R2;
        }
        #endregion
        #region 计算雷诺数
        /// <summary>
        /// 代码重复了很多，主要是为了后面直接计算雷诺数提供方便(已检验)
        /// </summary>
        /// <param name="T3">气体平均温度</param>
        /// <param name="Q2">气体比重</param>
        /// <param name="D2">管线内径</param>
        /// <param name="F2">标准体积流量</param>
        /// <returns></returns>
        public double LeiNuoXiShu(double T3, double Q2, double D2, double F2)
        {
            double T4 = (T3 + 273) / 191.16;
            double N3 = 1.0009 * Math.Pow(10, -5) * Math.Pow(Q2, 0.5); //N1表示临界温度T下的动力粘度
            double N4;                                                 //N2 表示为实际温度下T下的动力粘度；
            if (T4 <= 1)
            {
                N4 = N3 * Math.Pow(T4, 0.005);
            }
            else
            {
                N4 = N3 * Math.Pow(T4, 0.71 + 0.29 * 191.16 / (T3 + 273));
            }
            double Re1 = 0.01777* Q2 * F2 / (D2 * N4 );
            return Re1;
        }
        #endregion
        #region   计算平均流速
        /// <summary>
        /// 计算平均流速（已检验）
        /// </summary>
        /// <param name="F2">标准体积流量</param>
        /// <param name="D2">管线内径</param>
        /// <returns></returns>
        public double MeanVelocity(double F2, double D2)
        {
            double BL3 =  4 * Math.Pow(10, 6) * F2 *0.101325;    //BL3,BL4 均为中间变量，计算方便，无实义
            double BL4 = 24 * 3600 * Math.PI * D2 * D2*0.10325;
            double V;
            V = BL3 / BL4;
            return V;
        }
        #endregion
        #region   计算压缩因子系数(需要迭代)
        /// <summary>
        /// 在此部分不实现迭代过程，主要无实参传进来（已检验）
        /// </summary>
        /// <param name="P1">管线上游表压</param>
        /// <param name="Q3">气体比重</param>
        /// <param name="T5">气体平均温度</param>
        /// <returns></returns>
        public double CompressibilityFactor(double P1, double Q3, double T5)
        {
            double T6 = T5 + 273;
            double PK1 = P1;                                     //选取表压初值
            double BL4 = P1 + PK1 * PK1 / (PK1 + P1);
            double PJ1 = 2 / 3.0 * BL4;   //计算管线平均表压
            double BL5 = 5072000 * PJ1 * Math.Pow(10, 1.785 * Q3); //BL5,BL6 均为中间变量，计算方便，无实义  
            double BL6 = BL5 / (Math.Pow(T6, 3.825));
            double Z1 = 1.0 / (1 + BL6);    // Calculate Compressibility Fcator 
            return Z1;
            //现在开始进入迭代过程
        }
        #endregion
        #region  计算管线下游表压
        /// <summary>
        /// 此过程代码需要改进，编程技术需要提高(已检验)
        /// </summary>
        /// <param name="P2">管线上游表压</param>
        /// <param name="F3">标准体积流量</param>
        /// <param name="R2">达西摩阻系数</param>
        /// <param name="Z2">压缩因子系数</param>
        /// <param name="Q4">气体比重系数</param>
        /// <param name="T6">气体的平均温度</param>
        /// <param name="L1">管线长度</param>
        /// <param name="D3">管线内径</param>
        /// <returns></returns>
        public double PipelinePressure(double P2, double F3, double R2, double Z2,
                                       double Q4, double T7, double L1, double D3)
        {
            double T8 = T7 + 273;
            double PP1;
            double C = 3.32355;//PP1 为管线下游表压                  
            double BL7 = F3 * F3 / (C * C);                       //BL7,BL8,BL9 BL10 均为中间变量，计算方便，无实义
            double BL8 = R2 * Z2 * Q4 * T8 * L1;            //注意温度单位是℉而不是摄氏度，需要单位换算
            double BL9 = BL7 * BL8 / (Math.Pow(D3, 5));
            double BL10 = P2 * P2 - BL9;
            PP1 = Math.Pow(BL10, 0.5);
            return PP1; 
        }
        #endregion
        #region 计算管线上游表压
        /// <summary>
        /// 此代码和上面代码，存在很多相似，需要改进(已检验)
        /// </summary>
        /// <param name="P3">管线下游表压</param>
        /// <param name="F4">标准体积流量</param>
        /// <param name="R3">达西摩阻系数</param>
        /// <param name="Z3">压缩因子系数</param>
        /// <param name="Q5">气体比重系数</param>
        /// <param name="T7">气体的平均温度</param>
        /// <param name="L2">管线长度</param>
        /// <param name="D4">管线内径</param>
        /// <returns></returns>
        public double PipelinePressure1(double P3, double F4, double R3, double Z3,
                                     double Q5, double T9, double L2, double D4)
        {
            double T10 = T9 + 273;                           //摄氏温度转化为华式温度
            double PP2;                                      //PP2 为管线上游表压
            double C = 3.32355;    //PP1 为管线下游表压  
            double BL11 = F4 * F4 / (C * C);                  //BL11,BL12,BL13,BL14 均为中间变量，计算方便，无实义
            double BL12 = R3 * Z3 * Q5 * T10 * L2;          //注意温度单位是℉而不是摄氏度，需要单位换算
            double BL13 = BL11 * BL12 / (Math.Pow(D4, 5));
            double BL14 = P3 * P3 + BL13;
            PP2 = Math.Pow(BL14, 0.5);
            return PP2;
        }

        #endregion
        #region 计算流体体积流量
        /// <summary>
        /// 计算流体体积流量（已检验）
        /// </summary>
        /// <param name="P4">管线上游表压</param>
        /// <param name="P5">管线下游表压</param>
        /// <param name="D5">管线内径</param>
        /// <param name="R4">达西摩阻系数</param>
        /// <param name="Z4">压缩因子系数</param>
        /// <param name="Q6">气体比重系数</param>
        /// <param name="T11">气体平均温度</param>
        /// <param name="L3">管线长度</param>
        /// <returns></returns>
        public double StandardVolumeFlow(double P4, double P5, double D5, double R4, double Z4, double Q6, double T11, double L3)
        {
            double T12 = T11 + 273;                    //摄氏温度转化为华式温度
            double C = 3.32355;
            double BL15 = P4 * P4 - P5 * P5;
            double BL16 = R4 * Z4 * Q6 * T12 * L3;    //注意温度单位是℉而不是摄氏度，需要单位换算
            double BN = Math.Pow(D5, 5);
            double BL17 = BL15 * Math.Pow(D5, 5) / BL16;

            double StdFlow = C * Math.Pow(BL17, 0.5)/10000;
            return StdFlow;
        }
        #endregion  
        #region    计算压缩因子系数（与标准体积流量无关，可直接计算得出）
        /// <summary>
        /// 计算压缩因子系数
        /// </summary>
        /// <param name="P6">管线上游表压</param>
        /// <param name="P7">管线下游表压</param>
        /// <param name="Q4">气体比重</param>
        /// <param name="T13">气体平均温度</param>
        /// <returns></returns>
        public double CompressibilityFactor1(double P6, double P7, double Q4, double T13)
        {
            double T14 = T13 + 273;
            double BL30 = P6 + P7 * P7 / (P6 + P7);
            double PJ2 =(2/3.0)*BL30;   //计算管线平均表压
            double BL5 = 5072000 * PJ2 * Math.Pow(10, 1.785 * Q4); //BL5,BL6 均为中间变量，计算方便，无实义  
            double BL6 = BL5 / (Math.Pow(T14, 3.825));
            double Z1 = 1.0 / (1 + BL6);    // Calculate Compressibility Fcator 
            return Z1;
        }
        #endregion

        #region    计算压缩因子系数12（与标准体积流量无关，可直接计算得出）
        /// <summary>
        /// 计算压缩因子系数
        /// </summary>
        /// <param name="P6">管线上游表压</param>
        /// <param name="P7">管线下游表压</param>
        /// <param name="Q4">气体比重</param>
        /// <param name="T13">气体平均温度</param>
        /// <returns></returns>
        public double CompressibilityFactorParameter(double P6, double P7, double Q4, double T13)
        {
            double T14 = T13 + 273;
            double BL30 = P6 + P7 * P7 / (P6 + P7);
            double PJ2 = (2 / 3.0) * BL30;   //计算管线平均表压
            double BL5 = 5072000 * PJ2 * Math.Pow(10, 1.785 * Q4); //BL5,BL6 均为中间变量，计算方便，无实义  
            double BL6 = BL5 / (Math.Pow(T14, 3.825));
            double Z1 = 1.0 / (1 + BL6);    // Calculate Compressibility Fcator 
            double BN = Z1 / PJ2;
            return BN;
        }
        #endregion

        #region   计算管长
        /// <summary>
        /// 计算管长
        /// </summary>
        /// <param name="P8">管线上游表压</param>
        /// <param name="P9">管线下游表压</param>
        /// <param name="D6">管线内径</param>
        /// <param name="F5">标准体积流量</param>
        /// <param name="R5">达西摩阻系数</param>
        /// <param name="Z5">压缩因子系数</param>
        /// <param name="Q7">气体比重</param>
        /// <param name="T15">气体平均温度</param>
        /// <returns></returns>
        public double PipelineLength(double P8, double P9, double D6, double F5, double R5, double Z5, double Q7, double T15)
        {
            double T16 = T15 + 273;
            double BL18 = P8 * P8 - P9 * P9;
            double BL19 = F5 * F5 / (3.32*3.32);
            double BL20 = BL19 * R5 * Z5 * Q7 * T16;
            double BL21 = BL18 * Math.Pow(D6, 5);
            double L = BL21 / BL20;
            return L;
        }
        #endregion
        #region 计算管道直径
        /// <summary>
        /// 
        /// </summary>
        /// <param name="F6">标准体积流量</param>
        /// <param name="R6">达西摩阻系数</param>
        /// <param name="Z6">压缩因子系数</param>
        /// <param name="Q8">气体比重</param>
        /// <param name="T17">气体平均温度</param>
        /// <param name="L4">管道长度</param>
        /// <param name="P10">管道上线压力</param>
        /// <param name="P11">管道下线压力</param>
        /// <returns></returns>
        public double PipelineDiameter(double stdflow, double R6, double Z6, double Q8, double T17, double L4, double uppre, double downpre)
        {
            double T18 = T17 + 273;
            double C = 3.32355;
            double BL21 = stdflow * stdflow / (C * C);
            double BL22 = R6 * Z6 * Q8 * T18 * L4;
            double BL23 = uppre * uppre - downpre * downpre;
            double BL24 = BL21 * BL22 / BL23;
            double PipeDia = Math.Pow(BL24, 0.2);
            return PipeDia;
        }
        #endregion
        #region  计算CNG标准站压缩机压比
        /// <summary>
        /// 计算CNG标准站的压缩机压比
        /// </summary>
        /// <param name="P1">压缩机入口压力</param>
        /// <param name="P2">压缩机出口压力</param>
        /// <returns></returns>
        public double CompressorPreRat(double P1, double P2)
        {
            double PreRatio = P2 / P1;
            return PreRatio;
        }
        #endregion
        #region 计算CNG标准站压缩机台数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Flow">标准站体积流量</param>
        /// <param name="T">每日工作时长</param>
        /// <param name="PreRat">压比</param>  p2/p1    p2  出口压力  p1 入口压力
        ///  <param name="Z>压缩因子</param>
        /// <returns></returns>
        public double CompressorPower( double Z,double Flow, double t, double P1, double P2)
        {
            double PreRat = P2 / P1;
            double R = Math.Pow(PreRat, 0.333) - 1;
            double n = Math.Ceiling(4.3 * Z * Flow * R / (t * 8160));
            return n;
        }
        #endregion
        #region   计算压缩机组台数
        /// <summary>  
        /// 计算压缩机组台数
        /// </summary>
        /// <param name="No2">压缩机组总功率</param>
        /// <returns></returns>
        public double CompressorNumber(double No2)
        {
            double Num = No2 / 200000;
            return Num;
        }
        #endregion
        #region    计算CNG子站的加气机数量
        /// <summary  
        /// 计算CNG子站的加气机数量
        /// </summary>
        /// <param name="n1">出租车数量</param>
        /// <param name="n2">公交车数量</param>
        /// <param name="t">每日工作时长</param>
        /// <returns></returns>
        public double CNGNum(double n1, double n2, double t)
        {
            double t1 = 0.1 * n1 + 0.2 * n2;
            double N = t1 / (2 * t);
            return N;
        }
        #endregion
        #region 计算低压区储气容积
        /// <summary>
        /// 计算低压区储气容积
        /// </summary>
        /// <param name="VolPro1">低压区容积比例</param>
        /// <param name="VolPro2">中压区容积比例</param>
        /// <param name="Flow1">设计加气量</param>
        /// <param name="ComTime">压缩机补气时间</param>
        /// <returns></returns>
        public double LowPer(double VolPro1, double VolPro2, double Flow1, double ComTime)
        {
            double MidVar = 1;
            if (ComTime == 5)
            {
                MidVar = 0.74;
            }
            else if (ComTime == 6)
            {
                MidVar = 0.645;
            }
            else if (ComTime == 7)
            {
                MidVar = 0.55;
            }
            double n = MidVar * 1;
            double Vg = MidVar * Flow1 / (VolPro1 + VolPro2 + 1);
            return Vg;
        }
        #endregion
        #region  计算CNG子母站槽车配置数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StdFlow">子站设计气量</param>
        /// <param name="t1">运输车运途来回时间</param>
        /// <param name="t2">车载容器充气时间</param>
        /// <param name="t3">车载容器供气时间</param>
        /// <param name="FreightVol">单车运输量</param>
        /// <returns></returns>
        public double TankCarNum(double StdFlow, double L )
        {
            double BL = StdFlow * (2 * L / 70 + 3.5);
            double n = Math.Ceiling(BL/9600);
            return n;
        }
        #endregion
    }
}
