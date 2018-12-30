using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 天然气供应方案分析与决策软件
{
    public partial class CompressorCalculate : Form
    {
        public CompressorCalculate()
        {
            InitializeComponent();
        }
        CompressorStation ComStation = new CompressorStation();
        private const double mc_dblPai = 3.14159265; //圆周率
        private const int mc_lngCyclingSumMax = 1000; //最大循环数
        private const int mc_dblStandardAirPressure = 101325; //标准大气压(Pa)
                                                              //私有变量
                                                              //计算类型
        private enum pesCalculateStyle       //  没用 计算类型错误
        {
            pesCalculateStyleNone = -1, //计算类型错误
            pesCalculateStyleNoWasteLevel = 0 //无耗气水平管
        }

      
        private double m_dblMaximumPipelineAbsolutePressure; //最大管线绝对压力
        private double m_dblActualVolumeFlowRate; //实际体积流量
        private double m_dblStandardVolumeFlowRate; //标准体积流量

        private double m_dblInsideDiameter; //管线内径
        private double m_dblGasSpecificGravity; //气体比重
        private double m_dblAbsoluteRoughness; //管线绝对粗糙度
        private double m_dblFirstStationCompressRatio; //首站压比
        private double m_dblMiddleStationCompressRatio; //中间站压比
        private double m_dblEndStationCompressRatio; //末站压比
        private int m_intCompressionStationsCount; //压缩机站总数
        private double m_dblGeneralCompressionStationsPower; //压缩机站总功率
        private double m_dblAverageVelocity; //平均流速
        private double m_dblReyonldsNumber; //雷诺数
        private double m_dblDarcyFrictionFactor; //达西摩阻系数
        private pesCalculateStyle m_pesCalculateStyle; //计算类型
        private string m_strCalculateProcess; //计算过程字符串
        private bool m_blnEnabled; //本对象是否可用
        private bool m_blnRefresh; //对象是否刷新
                                   //公有变量(无)
        public void Calculate()   //  有用 重点看  脑壳疼  。。。。。........。。。。。
        {try { 
            //作用：有关计算的方法
            //接受的参数：
            //输入参数：无
            //输出参数：无
            //返回值：无
            //说明:1.所有局部变量的命名参照<管道工程电算程序集>
            //     2.求解方法遵循从特殊到一般的方法
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - Math .Logic too complex
            const double c_dblAbsoluteErrorLimitFC = 0.0001; //求解层流摩阻系数过程的FC误差限
            const int c_intCycleCountMax = 200; //最大循环数

            double[] adblHP = null; //各压缩机站所需功率
            double[] adblMP = null; //各压缩机站的位置
            double[] adblP = null; //沿线压力
            double dblC3 = 0; //比重系数
            double dblC5 = 0; //超压缩系数
            double dblC6 = 0; //直径系数
            double dblC7 = 0; //湍流摩擦系数第一分量
            double dblC8 = 0; //湍流摩擦系数第二分量
            double dblC9 = 0; //湍流摩擦系数
            double dblC11 = 0; //层流摩擦系数
            double dblCR = 0; //压缩比
            double dblD = 0; //管道内径
            double dblF = 0; //层流摩阻系数的估计值
            double dblFC = 0; //层流摩阻系数的计算中间值
            double dblFF = 0; //层流摩阻系数的计算值
            double dblG = 0; //气体比重
            double dblHP = 0; //压缩机功率
            double dblKE = 0; //管子绝对粗糙度
            double dblL = 0; //当前计算长度(英里)
            double dblLC = 0; //两压缩机站之间的长度
            double dblLP = 0; //管道长度(英里)
            double dblNC = 0; //临界雷诺数
            double dblLR = 0; //剩余长度(英里)
            double dblPM = 0; //管道最大运行压力(磅/英寸^2)
            double dblPU = 0; //上游绝对压力
            double dblPA = 0; //平均压力
            double dblPD = 0; //下游压力
            double dblRE = 0; //雷诺数
            double dblT = 0; //气体平均温度
            double dblZ = 0; //压缩因子
            double dblQB = 0; //标准体积流量(千(英尺)^3/日)
            double dblQM = 0; //标准体积流量(百万(英尺)^3/日)
            double dblPI = 0; //管道入口站输送压力(磅/英寸^2)
            double dblPO = 0; //管道出口站输送压力(磅/英寸^2)
            int intI = 0; //循环变量
            int intJ = 0; //循环变量
                          //Dim intNS As Integer '压缩机站编号
            int intM = 0; //程序计数控制变量（《管道工程电算程序》中使用）



            
            //if (IsValidate())
            //{
            //按计算类型分类计算
            //if (m_pesCalculateStyle ==   pesCalculateStyleNoWasteLevel)
            //{
            //求解无耗气水平管线(不存在特殊的情况)
            //对局部变量赋值
            dblQB = Convert.ToDouble(txtInput1.Text); //标准体积流量
            dblPI = Convert.ToDouble(txtInput2.Text); //上游压力
            dblPO = Convert.ToDouble(txtInput3.Text);//下游压力
            dblG = Convert.ToDouble(ComStation.PipeGasWeight); //气体比重
            dblT = Convert.ToDouble(ComStation.PipeTep)+273.15; //气体温度
            dblKE = Convert.ToDouble(ComStation.PipeRough); //管线绝对粗糙度
            dblD = Convert.ToDouble(txtInput4.Text); //管线内径
            dblLP = Convert.ToDouble(txtInput5.Text); //管线长度
            dblPM = Convert.ToDouble(txtInput6.Text); //最大管线绝对压力

            //求解过程
            //**********************以下为核心代码**************************
            //按<管道工程电算程序集>的要求转换单位制
            dblQB = dblQB * (35.3147 * 3600 * 24);  //流量
            dblQM = dblQB / 1000000;     //流量换算
            dblPI = dblPI / 6894.76;  //上游压力
            dblPO = dblPO / 6894.76;//下游压力
            dblLP = dblLP / 1609.344;  //长度
            dblD = dblD / 0.0254;   //内径
            dblKE = dblKE / 0.0254; //<电算程序集中所用的绝对粗糙度实际单位是英寸>    粗糙度
            dblPM = dblPM / 6894.76;   //最大压力
            dblT = dblT * ((double)9 / 5); //(兰式温度)      // 绝对温度转兰式温度     绝对温度=摄氏度+273.15;

            //开始计算
            dblPU = dblPI;   //上游压力转换为上游绝对压力
            dblL = dblLP;  //长度转换为英里
                           //求解比重系数
            dblC3 = System.Convert.ToDouble(Math.Pow((0.6 / dblG), 0.5));
            //求解直径系数
            dblC6 = Math.Pow(dblD, (2.5));
            //求解湍流摩阻系数
            dblC7 = System.Convert.ToDouble(4 * (Math.Log(3.7 * dblD) / Math.Log(10)));
            dblC8 = System.Convert.ToDouble(4 * (Math.Log(1 / dblKE) / Math.Log(10)));
            dblC9 = dblC7 + dblC8;    //湍流摩阻系数
                                      //临界雷诺数
            dblNC = System.Convert.ToDouble(((20.912 * dblD) / dblKE) * (Math.Log((3.7 * dblD) / dblKE) / Math.Log(10)));
            #region 
            dblPA = dblPU;
            for (intI = 1; intI <= 5; intI++)
            {
                //求解压缩因子和超压缩系数
                dblZ = CalculateCompressFactor(dblPA * 6894.76, dblG,  dblT); //接口参数为国际单位
                if (dblZ <= 0)
                {
                    //求解压缩因子失败,气体管道工艺计算对象无效!
                    m_blnEnabled = false;
                    MessageBox.Show("求解压缩因子失败,气体管道工艺计算对象无效!", "CGasTechnicsCompressionSchedule" + "-" + "Calculate()");
                    Close();
                }
                dblC5 = System.Convert.ToDouble(Math.Sqrt(1 / dblZ));

                if ((dblL * (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2))) > Math.Pow(dblPU, 2))
                {
                    dblPD = 0;
                    break;
                }
                else
                {
                    dblPD = Convert.ToDouble(Math.Sqrt(Math.Pow(dblPU, 2) - (dblL * Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2))));
                }

                dblPA = System.Convert.ToDouble((dblPU + dblPD) / 2);

            }
            #endregion
            #region

            if (dblPD >= dblPO)
            {
                //不需要压缩机
                m_intCompressionStationsCount = 0; //压缩机站总数
                m_dblGeneralCompressionStationsPower = 0; //压缩机站总功率
                                                          //确定实际体积流量
                m_dblActualVolumeFlowRate = m_dblStandardVolumeFlowRate * ((mc_dblStandardAirPressure * dblZ) / (dblPA * 6894.76 * 1));
                m_dblAverageVelocity = System.Convert.ToDouble((4 * m_dblActualVolumeFlowRate) / (mc_dblPai * m_dblInsideDiameter * m_dblInsideDiameter)); //平均速度
                dblRE = System.Convert.ToDouble(477.5 * ((dblQB * dblG) / (8 * dblD)) * (14.73 / 520));

                if (dblRE > dblNC)
                {
                    //气体处于紊流区
                    //获取摩阻系数(用于显示)
                    dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
                }
                else
                {
                    //气体处于层流区
                    //层流时摩阻系数初值
                    dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
                                                                              //求解层流摩阻系数
                    for (intI = 0; intI <= mc_lngCyclingSumMax - 1; intI++)
                    {
                        dblFC = System.Convert.ToDouble((4 * (Math.Log(dblRE / Math.Sqrt(1 / dblF)) / Math.Log(10))) - 0.6);
                        dblFF = System.Convert.ToDouble(1 / (Math.Pow(dblFC, 2)));
                        dblF = System.Convert.ToDouble((dblF + dblFF) / 2);
                        if (Math.Abs(dblF - dblFF) < c_dblAbsoluteErrorLimitFC)
                        {
                            //求解摩阻的精度达到要求,退出本层循环
                            break;
                        }
                    }

                    if (intI >= mc_lngCyclingSumMax)
                    {
                        //求解层流区摩阻系数不收敛
                        m_blnEnabled = false;
                        MessageBox.Show("在已知上游压力和流量的条件下,求解层流区摩阻系数不收敛!");
                        Close();
                    }

                    dblC11 = dblFC * 0.965;
                }

                m_dblReyonldsNumber = dblRE; //雷诺数
                m_dblDarcyFrictionFactor = dblF; //达西摩阻系数
                #region
                //激发获取计算参数的事件
                //m_strCalculateProcess = "No Compression Required.";
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);
                //m_strCalculateProcess = "Calculations Terminated.";
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);
                ////显示具体参数
                //m_strCalculateProcess = "f_DarcyFrictionFactor=" + Str(m_dblDarcyFrictionFactor).Trim();
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);
                //m_strCalculateProcess = "V_AverageVelocity=" + Str(m_dblAverageVelocity).Trim();
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);
                //m_strCalculateProcess = "Q_StandardVolumeFlowRate=" + Str(m_dblStandardVolumeFlowRate).Trim();
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);
                //m_strCalculateProcess = "Q_ActualVolumeFlowRate=" + Str(m_dblActualVolumeFlowRate).Trim();
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);

                ////修改对象标记
                //m_blnRefresh = true;
                //m_blnEnabled = true;
                //goto PROC_EXIT;
                #endregion
            }
            #endregion
            #region

            //判断需要一个压缩机的情况
            dblPU = dblPM;
            dblL = dblLP;

            dblPA = dblPU;
            for (intI = 1; intI <= 5; intI++)
            {
                //求解压缩因子和超压缩系数
                dblZ = CalculateCompressFactor(dblPA * 6894.76, dblG,  dblT); //接口参数为国际单位
                if (dblZ <= 0)
                {
                    //求解压缩因子失败,气体管道工艺计算对象无效!
                    m_blnEnabled = false;
                    MessageBox.Show("求解压缩因子失败,气体管道工艺计算对象无效!",


                        "CGasTechnicsCompressionSchedule" + "-" + "Calculate()");
                    Close();
                }
                dblC5 = System.Convert.ToDouble(Math.Sqrt(1 / dblZ));

                if ((dblL * (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2))) > Math.Pow(dblPU, 2))
                {
                    dblPD = 0;
                    break;
                }
                else
                {
                    dblPD = System.Convert.ToDouble(Math.Sqrt(Math.Pow(dblPU, 2) - dblL * (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2))));
                    dblPA = System.Convert.ToDouble((dblPU + dblPD) / 2);
                }
            }
            #endregion
            #region 

            if (dblPD >= dblPO)
            {
                //在入口需要一个压缩机
                dblPD = dblPO;
                dblL = dblLP;

                dblPA = dblPD;
                for (intI = 1; intI <= 5; intI++)
                {
                    //求解压缩因子和超压缩系数
                    dblZ = CalculateCompressFactor(dblPA * 6894.76, dblG,  dblT); //接口参数为国际单位
                    if (dblZ <= 0)
                    {
                        //求解压缩因子失败,气体管道工艺计算对象无效!
                        m_blnEnabled = false;
                        MessageBox.Show("求解压缩因子失败,气体管道工艺计算对象无效!"/*, Constants.vbInformation*/, "CGasTechnicsCompressionSchedule" + "-" + "Calculate()");
                        Close();
                    }
                    dblC5 = System.Convert.ToDouble(Math.Sqrt(1 / dblZ));

                    dblPU = System.Convert.ToDouble(Math.Sqrt(Math.Pow(dblPD, 2) + (dblL * (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2)))));
                    dblPA = System.Convert.ToDouble((dblPU + dblPD) / 2);
                }

                dblCR = dblPU / dblPI;
                dblHP = System.Convert.ToDouble((210.2 * ((Math.Pow(dblCR, 0.231)) - 1) + 3.5) * dblQM);

                m_intCompressionStationsCount = 1; //压缩机站总数
                                                   //恢复参数为国际单位
                m_dblGeneralCompressionStationsPower = dblHP * 745.7; //压缩机站总功率

                //实际体积流量=P0*Q0*Z1/P1*Z0(0-标准状态,1-真实状态)
                m_dblActualVolumeFlowRate = m_dblStandardVolumeFlowRate * ((mc_dblStandardAirPressure * dblZ) / (dblPA * 6894.76 * 1));
                m_dblAverageVelocity = System.Convert.ToDouble((4 * m_dblActualVolumeFlowRate) / (mc_dblPai * m_dblInsideDiameter * m_dblInsideDiameter)); //平均速度
                dblRE = System.Convert.ToDouble(477.5 * ((dblQB * dblG) / (8 * dblD)) * (14.73 / 520));

                if (dblRE > dblNC)
                {
                    //气体处于紊流区
                    //获取摩阻系数(用于显示)
                    dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
                }
                else
                {
                    //气体处于层流区
                    //层流时摩阻系数初值
                    dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
                                                                              //求解层流摩阻系数
                    for (intI = 0; intI <= mc_lngCyclingSumMax - 1; intI++)
                    {
                        dblFC = System.Convert.ToDouble((4 * (Math.Log(dblRE / Math.Sqrt(1 / dblF)) / Math.Log(10))) - 0.6);
                        dblFF = System.Convert.ToDouble(1 / (Math.Pow(dblFC, 2)));
                        dblF = System.Convert.ToDouble((dblF + dblFF) / 2);
                        if (Math.Abs(dblF - dblFF) < c_dblAbsoluteErrorLimitFC)
                        {
                            //求解摩阻的精度达到要求,退出本层循环
                            break;
                        }
                    }

                    if (intI >= mc_lngCyclingSumMax)
                    {
                        //求解层流区摩阻系数不收敛
                        m_blnEnabled = false;
                        MessageBox.Show("在已知上游压力和流量的条件下,求解层流区摩阻系数不收敛!"/*, Constants.vbExclamation*/);
                        Close();
                    }

                    dblC11 = dblFC * 0.965;
                }

                m_dblReyonldsNumber = dblRE; //雷诺数
                m_dblDarcyFrictionFactor = dblF; //达西摩阻系数
               
            }
            #endregion
            #region 
            //有一个以上的压缩机站
            dblPU = dblPM;
            dblPD = dblPM / m_dblMiddleStationCompressRatio; //中间站压比       'dblPD = dblPM / 1.35 '首站压比(原来代码)
            m_dblFirstStationCompressRatio = dblPM / dblPI;
            dblPA = System.Convert.ToDouble((dblPU + dblPD) / 2);

            //求解压缩因子和超压缩系数
            dblZ = CalculateCompressFactor(dblPA * 6894.76, dblG,  dblT); //接口参数为国际单位
            if (dblZ <= 0)
            {
                //求解压缩因子失败,气体管道工艺计算对象无效!
                m_blnEnabled = false;
                MessageBox.Show("求解压缩因子失败,气体管道工艺计算对象无效!", /*Constants.vbInformation,*/ "CGasTechnicsCompressionSchedule" + "-" + "Calculate()");
                Close();
            }
            dblC5 = System.Convert.ToDouble(Math.Sqrt(1 / dblZ));

            //临界雷诺数
            dblNC = System.Convert.ToDouble(((20.912 * dblD) / dblKE) * (Math.Log((3.7 * dblD) / dblKE) / Math.Log(10)));
            //雷诺数
            dblRE = System.Convert.ToDouble(477.5 * ((dblQB * dblG) / (8 * dblD)) * (14.73 / 520));

            if (dblRE > dblNC)
            {
                //气体处于紊流区
                //求解管长(此时流量单位为(千立方英尺/天))
                dblLC = System.Convert.ToDouble(((Math.Pow(dblPU, 2)) - (Math.Pow(dblPD, 2))) / (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2)));
                //获取摩阻系数(用于显示)
                dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
            }
            else
            {
                //气体处于层流区
                //层流时摩阻系数初值
                dblF = System.Convert.ToDouble(1 / (Math.Pow(dblC9, 2))); //由<管道工程电算程序集>反推得到
                                                                          //求解层流摩阻系数
                for (intJ = 0; intJ <= mc_lngCyclingSumMax - 1; intJ++)
                {
                    dblFC = System.Convert.ToDouble((4 * (Math.Log(dblRE / Math.Sqrt(1 / dblF)) / Math.Log(10))) - 0.6);
                    dblFF = System.Convert.ToDouble(1 / (Math.Pow(dblFC, 2)));
                    dblF = System.Convert.ToDouble((dblF + dblFF) / 2);
                    if (Math.Abs(dblF - dblFF) < c_dblAbsoluteErrorLimitFC)
                    {
                        //求解摩阻的精度达到要求,退出本层循环
                        break;
                    }
                }

                if (intJ >= mc_lngCyclingSumMax)
                {
                    //求解层流区摩阻系数不收敛
                    m_blnEnabled = false;
                    MessageBox.Show("在已知上游压力和流量的条件下,求解层流区摩阻系数不收敛!"/*, Constants.vbExclamation*/);
                    Close();
                }

                dblC11 = dblFC * 0.965;
                //求解求解管长
                dblLC = System.Convert.ToDouble(((Math.Pow(dblPU, 2)) - (Math.Pow(dblPD, 2))) / (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC11)), 2)));
            }
            #endregion
            #region
            //intNS = Int(dblL / dblLC)'此为《管道工程电算程序集》中内容，无实际
            //重新定义压缩机站位置和功率
            intM = 1;
            adblHP = new double[intM + 1]; //各站所需功率
            adblMP = new double[intM + 1 + 1]; //各站的位置
            adblP = new double[2 * (intM + 1) + 1 + 1]; //沿线压力

            dblCR = dblPM / dblPI;
            adblHP[0] = System.Convert.ToDouble((210.2 * ((Math.Pow((dblCR), 0.231)) - 1) + 3.5) * dblQM);
            adblMP[0] = 0;
            adblP[0] = dblPI;
            adblP[intM] = dblPM;
            dblLR = dblLP - dblLC;

            for (intI = 0; intI <= c_intCycleCountMax - 1; intI++)
            {
                dblL = dblLR;
                dblPD = dblPO;

                dblPA = dblPD;
                for (intJ = 1; intJ <= 5; intJ++)
                {
                    //求解压缩因子和超压缩系数
                    dblZ = CalculateCompressFactor(dblPA * 6894.76, dblG,  dblT); //接口参数为国际单位
                    if (dblZ <= 0)
                    {
                        //求解压缩因子失败,气体管道工艺计算对象无效!
                        m_blnEnabled = false;
                        MessageBox.Show("求解压缩因子失败,气体管道工艺计算对象无效!"/*, Constants.vbInformation*/, "CGasTechnicsCompressionSchedule" + "-" + "Calculate()");
                        Close();
                    }
                    dblC5 = System.Convert.ToDouble(Math.Sqrt(1 / dblZ));

                    dblPU = System.Convert.ToDouble(Math.Sqrt(Math.Pow(dblPD, 2) + (dblL * (Math.Pow((dblQB / (77.5 * dblC3 * dblC5 * dblC6 * dblC9)), 2)))));
                    dblPA = System.Convert.ToDouble((dblPU + dblPD) / 2);
                }

                if (dblPU <= dblPM)
                {
                    //最后一个压缩机站，退出循环
                    dblCR = dblPU / (dblPM / m_dblMiddleStationCompressRatio);
                    m_dblEndStationCompressRatio = dblCR;
                    adblHP[intM] = System.Convert.ToDouble((210.2 * ((Math.Pow((dblCR), 0.231)) - 1) + 3.5) * dblQM);
                    adblMP[intM] = intM * dblLC;
                    adblP[2 * intM] = (int)(dblPM / m_dblMiddleStationCompressRatio); //最后一个压缩机站的入口压力
                    adblP[2 * intM + 1] = (int)dblPU;
                    adblMP[intM + 1] = (int)dblLP;
                    adblP[2 * intM + 2] = (int)dblPO;
                    adblP[2 * intM + 3] = (int)dblPO;
                    break;
                }
                else
                {
                    dblCR = m_dblMiddleStationCompressRatio;
                    adblHP[intM] = System.Convert.ToDouble((210.2 * ((Math.Pow((dblCR), 0.231)) - 1) + 3.5) * dblQM);
                    adblMP[intM] = intM * dblLC;
                    adblP[2 * intM] = (int)(dblPM / m_dblMiddleStationCompressRatio);
                    adblP[2 * intM + 1] = (int)dblPM;
                    intM++;
                    dblLR = dblLP - (intM * dblLC);
                    //添加了压缩机站，更新压缩机站数组
                    Array.Resize(ref adblHP, intM + 1); //各站所需功率
                    Array.Resize(ref adblMP, intM + 1 + 1); //各站的位置
                    Array.Resize(ref adblP, 2 * (intM + 1) + 1 + 1); //沿线压力
                }
            }

            if (intI > c_intCycleCountMax)
            {
                m_blnEnabled = false;
                MessageBox.Show("布置压缩机站的过程不收敛，不能继续计算!"/*, Constants.vbExclamation*/);
                Close();
            }
            //    以下是代码核心求取过程
            #endregion



            m_intCompressionStationsCount = intM + 1; //压缩机站总数    整数取整+1
                                                      //总功率
            dblHP = 0;
            for (intI = 0; intI <= intM; intI++)
            {
                dblHP = dblHP + adblHP[intI];
            }
            //恢复参数为国际单位
            m_dblGeneralCompressionStationsPower = dblHP * 745.7; //压缩机站总功率

            //实际体积流量=P0*Q0*Z1/P1*Z0(0-标准状态,1-真实状态)
            m_dblActualVolumeFlowRate = m_dblStandardVolumeFlowRate * ((mc_dblStandardAirPressure * dblZ) / (dblPA * 6894.76 * 1));
            m_dblAverageVelocity = System.Convert.ToDouble((4 * m_dblActualVolumeFlowRate) / (mc_dblPai * m_dblInsideDiameter * m_dblInsideDiameter)); //平均速度
            m_dblReyonldsNumber = dblRE; //雷诺数
            m_dblDarcyFrictionFactor = dblF; //达西摩阻系数;
            txtOutput5.Text = m_dblDarcyFrictionFactor.ToString("0.000000");
            txtOutput4.Text = m_dblReyonldsNumber.ToString("0.000000");
            txtOutput3.Text = m_dblAverageVelocity.ToString("0.000000");
            txtOutput2.Text = m_dblGeneralCompressionStationsPower.ToString("0.000000");
            txtOutput1.Text = m_intCompressionStationsCount.ToString("0.000000");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private double CalculateCompressFactor(double dblAbsolutePressure, double dblSpecificGravity, double dblTemperature)
        {
            double returnValue = 0;
            //作用：气体压缩因子
            //接受的参数：
            //输入参数：dblAbsolutePressure-气体绝对温度
            //          dblSpecificGravity-气体比重
            //          dblTemperature-气体温度
            //输出参数：无
            //返回值：气体压缩因子
            //说明:1.所有输入参数参数采用国际单位
            //     2.中间参数单位,T-兰氏温度,P-磅/英寸^2
            //     3.没有考虑压缩因子的使用范围
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - Math .Logic too complex
            const int c_dblCompressFactorError = -1; //压缩因子错误时的返回值
            const int c_intCycleCount = 5; //循环变量
            double dblP = 0; //压力
            double dblT = 0; //温度
            double dblG = 0; //比重
            int intI = 0; //循环变量
            double dblTC = 0; //临界温度
            double dblPC = 0; //临界压力
            double dblZ = 0; //压缩因子
            double dblTR = 0; //对比温度
            double dblRR = 0; //计算比
            double dblPR = 0; //对比压力

            if (dblAbsolutePressure < 0)
            {
                //绝对压力值小于0,不能计算气体压缩因子
                returnValue = c_dblCompressFactorError;
                MessageBox.Show("绝对压力值小于0,不能计算气体压缩因子!"/*, Constants.vbInformation*/);
                Close();
            }
            else if (dblAbsolutePressure == 0)
            {
                //绝对压力值等于0,不能计算气体压缩因子
                returnValue = c_dblCompressFactorError;
                MessageBox.Show("绝对压力值等于0,不能计算气体压缩因子!"/*, Constants.vbInformation*/);
                Close();
            }
            else
            {
                dblP = dblAbsolutePressure;
                dblP = dblP / 6894.76;
            }

            if (dblSpecificGravity < 0)
            {
                //比重小于0,不能计算气体压缩因子
                returnValue = c_dblCompressFactorError;
                MessageBox.Show("比重小于0,不能计算气体压缩因子!");
                Close();
            }
            else if (dblSpecificGravity == 0)
            {
                //比重等于0,不能计算气体压缩因子

                MessageBox.Show("比重等于0,不能计算气体压缩因子!");
                Close();
            }
            else
            {
                dblG = dblSpecificGravity;
            }

            if (dblTemperature <= 0)
            {
                //温度小于0,不能计算气体压缩因子
                returnValue = c_dblCompressFactorError;
                MessageBox.Show("温度小于0,不能计算气体压缩因子!");
                Close();
            }
            else if (dblTemperature == 0)
            {
                //温度等于0,不能计算气体压缩因子
                returnValue = c_dblCompressFactorError;
                MessageBox.Show("温度等于0,不能计算气体压缩因子!"/*, Constants.vbInformation*/);
                Close();
            }
            else
            {
                dblT = dblTemperature;
                dblT = System.Convert.ToDouble(((double)9 / 5) * dblT);
            }

            //求解气体压缩因子
            if (dblG < 0.7)
            {
                dblTC = System.Convert.ToDouble(166 + (318 * dblG));
                dblPC = System.Convert.ToDouble(693 - (36 * dblG));
            }
            else
            {
                dblTC = System.Convert.ToDouble(166 + (318 * dblG));
                dblPC = System.Convert.ToDouble(708 - (56 * dblG));
            }

            dblZ = 1;
            dblTR = dblT / dblTC;
            dblPR = dblP / dblPC;
            for (intI = 0; intI <= c_intCycleCount - 1; intI++)
            {
                dblRR = System.Convert.ToDouble((0.27 * dblPR) / (dblZ * dblTR));
                dblZ = System.Convert.ToDouble(1 + (0.31506 - (1.0467 / dblTR) - (0.5783 / (Math.Pow(dblTR, 3)))) * dblRR + (0.5353 - (0.6123 / dblTR)) * (Math.Pow(dblRR, 2)) + (0.6815 * ((Math.Pow(dblRR, 2)) / (Math.Pow(dblTR, 3)))));
            }

            //返回函数值
            returnValue = dblZ;


            return returnValue;
        }

        private  void Claear()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtInput3.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtInput6.Text = "";
            txtInput7.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";

        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Claear();
        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Calculate();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }
    }
}
