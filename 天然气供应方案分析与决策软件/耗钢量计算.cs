using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace 天然气供应方案分析与决策软件
{
    public partial class StellConsumptionCalaulate : Form
    {
        public StellConsumptionCalaulate()
        {
            InitializeComponent();
        }
        private const double mc_dblPai = 3.14159265; //圆周率
        private const int mc_dblStandardAirPressure = 101325; //标准大气压(Pa)
        private const int mc_lngCyclingSumMax = 1000; //最大循环数
        private enum pesCalculateStyle
        {
            pesCalculateStyleNone = -1, //计算类型错误
            pesCalculateStyleLiquidPipelineByGB1997 = 0, //液体管道按GB1997计算
            pesCalculateStyleLiquidPipelineByGB1999 = 1, //液体管道按GB1999计算
            pesCalculateStyleLiquidPipelineByAPI5L = 2, //液体管道按API5L计算
            pesCalculateStyleGasPipelineByGB1997 = 10, //气体管道按GB1997计算
            pesCalculateStyleGasPipelineByGB1999 = 11, //气体管道按GB1999计算
            pesCalculateStyleGasPipelineByAPI5L = 12 //气体管道按API5L计算
        }     // 计算类型
        private bool m_blnEnabled; //本对象是否可用

        private bool m_blnInputPipelineMaterialID; //是否输入了管线材料序号
        private bool m_blnInputFluidTypeID; //是否输入了流体类型序号
        private bool m_blnInputPipelineDesignAbsolutePressure; //是否输入了管线设计绝对压力
        private bool m_blnInputPipelineExternalDiameter; //是否输入了管线外直径
        private bool m_blnInputPipelineLengthIn1Area1Class; //是否输入了一级地区1类长度
        private bool m_blnInputPipelineLengthIn1Area2Class; //是否输入了一级地区2类长度
        private bool m_blnInputPipelineLengthIn2Area; //是否输入了二级地区长度
        private bool m_blnInputPipelineLengthIn3Area; //是否输入了三级地区长度
        private bool m_blnInputPipelineLengthIn4Area; //是否输入了四级地区长度
        private bool m_blnInputPipelineDesignAbsoluteTemperature; //是否输入了管线设计温度

        private int m_intWallThicknessByCalculate; //由计算结果定壁厚
        private int m_intCalculateAndCriterion; //采用计算和规范两种方法
        private int m_intIntensionAndRigidityCheck; //强度和刚度较核

        private int m_intOldSteelPipe; //使用老钢管
        private int m_intHeatSteelPipeline; //使用加热后的钢管

        private bool m_blnRefresh; //对象是否刷新
        private bool m_blnChangeDiameter; //是否改变管径
        private int m_intPipelineMaterialID; //管线材料序号         管材材料
        private double m_dblPipelineDesignAbsolutePressure; //管线设计绝对压力
        //m_dblPipelineDesignAbsolutePressure=Convert.ToDouble(txtinput.text)
        private double m_dblPipelineExternalDiameter; //管线外直径
        private double m_dblPipelineLengthIn1Area1Class; //一级地区1类长度
        private double m_dblPipelineLengthIn1Area2Class; //一级地区2类长度
        private double m_dblPipelineLengthIn2Area; //二级地区长度
        private double m_dblPipelineLengthIn3Area; //三级地区长度
        private double m_dblPipelineLengthIn4Area; //四级地区长度
        private double m_dblPipelineDesignAbsoluteTemperature; //管线设计温度
                     
        private double m_dblPipelineTotallLength; //管线总长度
        private double m_dblPipelineConsumeSteelIn1Area1Class; //一级地区1类耗钢量
        private double m_dblPipelineConsumeSteelIn1Area2Class; //一级地区2类耗钢量
        private double m_dblPipelineConsumeSteelIn2Area; //2级地区耗钢量
        private double m_dblPipelineConsumeSteelIn3Area; //3级地区耗钢量
        private double m_dblPipelineConsumeSteelIn4Area; //4级地区耗钢量
        private double m_dblPipelineConsumeSteelTotal; //总耗钢量

        //定义管线材料相关参数
        const int mc_intPipelineMaterialGB1997Count = 11; //GB1997管线材料数
        private string[] m_astrPipelineMaterialNameGB1997 = new string[mc_intPipelineMaterialGB1997Count - 1 + 1]; //GB1997管线材料名称
        private double[] m_adblMaterialYieldStressMinGB1997 = new double[mc_intPipelineMaterialGB1997Count - 1 + 1]; //GB1997最低屈服强度
        private double[] m_adblMaterialWeldingLineCoefficientGB1997 = new double[mc_intPipelineMaterialGB1997Count - 1 + 1]; //GB1997焊缝系数
        const int mc_intPipelineMaterialGB1999Count = 16; //GB1999管线材料数
        private string[] m_astrPipelineMaterialNameGB1999 = new string[mc_intPipelineMaterialGB1999Count - 1 + 1]; //GB1999管线材料名称
        private double[] m_adblMaterialYieldStressMinGB1999 = new double[mc_intPipelineMaterialGB1999Count - 1 + 1]; //GB1999最低屈服强度
        private double[] m_adblMaterialWeldingLineCoefficientGB1999 = new double[mc_intPipelineMaterialGB1999Count - 1 + 1]; //GB1999最低屈服强度
        const int mc_intPipelineMaterialAPI5LCount = 11; //API5L管线材料数
        private string[] m_astrPipelineMaterialNameAPI5L = new string[mc_intPipelineMaterialAPI5LCount - 1 + 1]; //API5L管线材料名称
        private double[] m_adblMaterialYieldStressMinAPI5L = new double[mc_intPipelineMaterialAPI5LCount - 1 + 1]; //API5L最低屈服强度
        private double[] m_adblMaterialWeldingLineCoefficientAPI5L = new double[mc_intPipelineMaterialAPI5LCount - 1 + 1]; //API5L焊缝系数

        private void Class_Initialize()
        {
            //作用：在初始化时，对各属性用初始化设置
            //接受的参数：
            //输入参数：无
            //输出参数：无
            //返回值：无
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            //变量初始化
            //计算类型
            //m_pesCalculateStyle = pesCalculateStyleNone;
            //标记参数

            // 标记参数不需要


            m_blnInputPipelineDesignAbsolutePressure = false; //没有输入管线设计绝对压力
            m_blnInputPipelineExternalDiameter = false; //没有输入管线外直径
            m_blnInputFluidTypeID = false; //没有输入流体类型序号
            m_blnInputPipelineLengthIn1Area1Class = false; //没有输入一级地区1类长度
            m_blnInputPipelineLengthIn1Area2Class = false; //没有输入一级地区2类长度
            m_blnInputPipelineLengthIn2Area = false; //没有输入二级地区长度
            m_blnInputPipelineLengthIn3Area = false; //没有输入三级地区长度
            m_blnInputPipelineLengthIn4Area = false; //没有输入四级地区长度
            m_blnInputPipelineMaterialID = false; //没有输入了管线材料序号
            m_blnInputPipelineDesignAbsoluteTemperature = false; //没有输入管线设计绝对温度

            //变量参数

            //定义初始变量参数
            
            m_dblPipelineDesignAbsolutePressure = 0; //管线设计绝对压力
            m_dblPipelineExternalDiameter = 0; //管线外直径
            m_dblPipelineLengthIn1Area1Class = 0; //一级地区1类长度
            m_dblPipelineLengthIn1Area2Class = 0; //一级地区2类长度
            m_dblPipelineLengthIn2Area = 0; //二级地区长度
            m_dblPipelineLengthIn3Area = 0; //三级地区长度
            m_dblPipelineLengthIn4Area = 0; //四级地区长度
            m_dblPipelineDesignAbsoluteTemperature = 0; //管线设计温度
            m_intPipelineMaterialID = 0; //管线材料序号
            m_dblPipelineTotallLength = 0; //管线总长度
            m_dblPipelineConsumeSteelIn1Area1Class = 0; //一级地区1类耗钢量
            m_dblPipelineConsumeSteelIn1Area2Class = 0; //一级地区2类耗钢量
            m_dblPipelineConsumeSteelIn2Area = 0; //2级地区耗钢量
            m_dblPipelineConsumeSteelIn3Area = 0; //3级地区耗钢量
            m_dblPipelineConsumeSteelIn4Area = 0; //4级地区耗钢量
            m_dblPipelineConsumeSteelTotal = 0; //总耗钢量

            //计算类型设置      需要认真读懂   
            //m_intWallThicknessByCalculate = System.Convert.ToInt32(vbUnchecked); //由计算结果定壁厚
            //m_intCalculateAndCriterion = System.Convert.ToInt32(vbUnchecked); //采用计算和规范两种方法
            //m_intIntensionAndRigidityCheck = System.Convert.ToInt32(vbUnchecked); //强度和刚度校核

            //m_intOldSteelPipe = System.Convert.ToInt32(vbUnchecked); //使用老钢管
            //m_intHeatSteelPipeline = System.Convert.ToInt32(vbUnchecked); //使用加热后的钢管

            m_blnChangeDiameter = false; //默认条件为不改变管道直径

            //材料相关参数
            //GB1997                    管材类型      国标1997
            #region 


            m_astrPipelineMaterialNameGB1997[0] = "L175";
            m_adblMaterialYieldStressMinGB1997[0] = 175;
            m_adblMaterialWeldingLineCoefficientGB1997[0] = 1;
            m_astrPipelineMaterialNameGB1997[1] = "L210";
            m_adblMaterialYieldStressMinGB1997[1] = 210;
            m_adblMaterialWeldingLineCoefficientGB1997[1] = 1;
            m_astrPipelineMaterialNameGB1997[2] = "L245";
            m_adblMaterialYieldStressMinGB1997[2] = 245;
            m_adblMaterialWeldingLineCoefficientGB1997[2] = 1;
            m_astrPipelineMaterialNameGB1997[3] = "L290";
            m_adblMaterialYieldStressMinGB1997[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1997[3] = 1;
            m_astrPipelineMaterialNameGB1997[4] = "L320";
            m_adblMaterialYieldStressMinGB1997[4] = 320;
            m_adblMaterialWeldingLineCoefficientGB1997[4] = 1;
            m_astrPipelineMaterialNameGB1997[5] = "L360";
            m_adblMaterialYieldStressMinGB1997[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1997[5] = 1;
            m_astrPipelineMaterialNameGB1997[6] = "L390";
            m_adblMaterialYieldStressMinGB1997[6] = 390;
            m_adblMaterialWeldingLineCoefficientGB1997[6] = 1;
            m_astrPipelineMaterialNameGB1997[7] = "L415";
            m_adblMaterialYieldStressMinGB1997[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1997[7] = 1;
            m_astrPipelineMaterialNameGB1997[8] = "L450";
            m_adblMaterialYieldStressMinGB1997[8] = 450;
            m_adblMaterialWeldingLineCoefficientGB1997[8] = 1;
            m_astrPipelineMaterialNameGB1997[9] = "L485";
            m_adblMaterialYieldStressMinGB1997[9] = 485;
            m_adblMaterialWeldingLineCoefficientGB1997[9] = 1;
            m_astrPipelineMaterialNameGB1997[10] = "L555";
            m_adblMaterialYieldStressMinGB1997[10] = 555;
            #endregion
            //GB1999                                                     国标  1999   管材类型
            #region


            m_astrPipelineMaterialNameGB1999[0] = "L245NB";
            m_adblMaterialYieldStressMinGB1999[0] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[0] = 1;
            m_astrPipelineMaterialNameGB1999[1] = "L245MB";
            m_adblMaterialYieldStressMinGB1999[1] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[1] = 1;
            m_astrPipelineMaterialNameGB1999[2] = "L290";
            m_adblMaterialYieldStressMinGB1999[2] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[2] = 1;
            m_astrPipelineMaterialNameGB1999[3] = "L290MB";
            m_adblMaterialYieldStressMinGB1999[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[3] = 1;
            m_astrPipelineMaterialNameGB1999[4] = "L360NB";
            m_adblMaterialYieldStressMinGB1999[4] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[4] = 1;
            m_astrPipelineMaterialNameGB1999[5] = "L360QB";
            m_adblMaterialYieldStressMinGB1999[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[5] = 1;
            m_astrPipelineMaterialNameGB1999[6] = "L360MB";
            m_adblMaterialYieldStressMinGB1999[6] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[6] = 1;
            m_astrPipelineMaterialNameGB1999[7] = "L415NB";
            m_adblMaterialYieldStressMinGB1999[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[7] = 1;
            m_astrPipelineMaterialNameGB1999[8] = "L415QB";
            m_adblMaterialYieldStressMinGB1999[8] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[8] = 1;
            m_astrPipelineMaterialNameGB1999[9] = "L415MB";
            m_adblMaterialYieldStressMinGB1999[9] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[9] = 1;
            m_astrPipelineMaterialNameGB1999[10] = "L450QB";
            m_adblMaterialYieldStressMinGB1999[10] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[10] = 1;
            m_astrPipelineMaterialNameGB1999[11] = "L450MB";
            m_adblMaterialYieldStressMinGB1999[11] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[11] = 1;
            m_astrPipelineMaterialNameGB1999[12] = "L485QB";
            m_adblMaterialYieldStressMinGB1999[12] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[12] = 1;
            m_astrPipelineMaterialNameGB1999[13] = "L485MB";
            m_adblMaterialYieldStressMinGB1999[13] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[13] = 1;
            m_astrPipelineMaterialNameGB1999[14] = "L555QB";
            m_adblMaterialYieldStressMinGB1999[14] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[14] = 1;
            m_astrPipelineMaterialNameGB1999[15] = "L555MB";
            m_adblMaterialYieldStressMinGB1999[15] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[15] = 1;

            #endregion
            //API5L规范                                                  管材类型   API5L
            #region

            m_astrPipelineMaterialNameAPI5L[0] = "A25";
            m_adblMaterialYieldStressMinAPI5L[0] = 172;
            m_adblMaterialWeldingLineCoefficientAPI5L[0] = 1;
            m_astrPipelineMaterialNameAPI5L[1] = "A";
            m_adblMaterialYieldStressMinAPI5L[1] = 207;
            m_adblMaterialWeldingLineCoefficientAPI5L[1] = 1;
            m_astrPipelineMaterialNameAPI5L[2] = "B";
            m_adblMaterialYieldStressMinAPI5L[2] = 241;
            m_adblMaterialWeldingLineCoefficientAPI5L[2] = 1;
            m_astrPipelineMaterialNameAPI5L[3] = "X42";
            m_adblMaterialYieldStressMinAPI5L[3] = 289;
            m_adblMaterialWeldingLineCoefficientAPI5L[3] = 1;
            m_astrPipelineMaterialNameAPI5L[4] = "X46";
            m_adblMaterialYieldStressMinAPI5L[4] = 317;
            m_adblMaterialWeldingLineCoefficientAPI5L[4] = 1;
            m_astrPipelineMaterialNameAPI5L[5] = "X52";
            m_adblMaterialYieldStressMinAPI5L[5] = 358;
            m_adblMaterialWeldingLineCoefficientAPI5L[5] = 1;
            m_astrPipelineMaterialNameAPI5L[6] = "X56";
            m_adblMaterialYieldStressMinAPI5L[6] = 386;
            m_adblMaterialWeldingLineCoefficientAPI5L[6] = 1;
            m_astrPipelineMaterialNameAPI5L[7] = "X60";
            m_adblMaterialYieldStressMinAPI5L[7] = 413;
            m_adblMaterialWeldingLineCoefficientAPI5L[7] = 1;
            m_astrPipelineMaterialNameAPI5L[8] = "X65";
            m_adblMaterialYieldStressMinAPI5L[8] = 448;
            m_adblMaterialWeldingLineCoefficientAPI5L[8] = 1;
            m_astrPipelineMaterialNameAPI5L[9] = "X70";
            m_adblMaterialYieldStressMinAPI5L[9] = 482;
            m_adblMaterialWeldingLineCoefficientAPI5L[9] = 1;
            m_astrPipelineMaterialNameAPI5L[10] = "X80";
            m_adblMaterialYieldStressMinAPI5L[10] = 551;
            m_adblMaterialWeldingLineCoefficientAPI5L[10] = 1;

            //对像标记
         
   
     

            #endregion
        }
    
       private   int SetPipelineMaterialIDByName(string strPipelineMaterialByName)
        {
            //作用：设置管线材料名称设置管线材料ID号
            //接受的参数：
            //输入参数：无
            //输出参数：无
            //返回值：无
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            int intPipelineMaterialID = 0; //管线材料ID号
            int intI = 0; //循环变量

            //对管线材料ID号赋初值
            //GB1997                    管材类型      国标1997
            #region 


            m_astrPipelineMaterialNameGB1997[0] = "L175";
            m_adblMaterialYieldStressMinGB1997[0] = 175;
            m_adblMaterialWeldingLineCoefficientGB1997[0] = 1;
            m_astrPipelineMaterialNameGB1997[1] = "L210";
            m_adblMaterialYieldStressMinGB1997[1] = 210;
            m_adblMaterialWeldingLineCoefficientGB1997[1] = 1;
            m_astrPipelineMaterialNameGB1997[2] = "L245";
            m_adblMaterialYieldStressMinGB1997[2] = 245;
            m_adblMaterialWeldingLineCoefficientGB1997[2] = 1;
            m_astrPipelineMaterialNameGB1997[3] = "L290";
            m_adblMaterialYieldStressMinGB1997[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1997[3] = 1;
            m_astrPipelineMaterialNameGB1997[4] = "L320";
            m_adblMaterialYieldStressMinGB1997[4] = 320;
            m_adblMaterialWeldingLineCoefficientGB1997[4] = 1;
            m_astrPipelineMaterialNameGB1997[5] = "L360";
            m_adblMaterialYieldStressMinGB1997[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1997[5] = 1;
            m_astrPipelineMaterialNameGB1997[6] = "L390";
            m_adblMaterialYieldStressMinGB1997[6] = 390;
            m_adblMaterialWeldingLineCoefficientGB1997[6] = 1;
            m_astrPipelineMaterialNameGB1997[7] = "L415";
            m_adblMaterialYieldStressMinGB1997[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1997[7] = 1;
            m_astrPipelineMaterialNameGB1997[8] = "L450";
            m_adblMaterialYieldStressMinGB1997[8] = 450;
            m_adblMaterialWeldingLineCoefficientGB1997[8] = 1;
            m_astrPipelineMaterialNameGB1997[9] = "L485";
            m_adblMaterialYieldStressMinGB1997[9] = 485;
            m_adblMaterialWeldingLineCoefficientGB1997[9] = 1;
            m_astrPipelineMaterialNameGB1997[10] = "L555";
            m_adblMaterialYieldStressMinGB1997[10] = 555;
            #endregion
            //GB1999                                                     国标  1999   管材类型
            #region


            m_astrPipelineMaterialNameGB1999[0] = "L245NB";
            m_adblMaterialYieldStressMinGB1999[0] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[0] = 1;
            m_astrPipelineMaterialNameGB1999[1] = "L245MB";
            m_adblMaterialYieldStressMinGB1999[1] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[1] = 1;
            m_astrPipelineMaterialNameGB1999[2] = "L290";
            m_adblMaterialYieldStressMinGB1999[2] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[2] = 1;
            m_astrPipelineMaterialNameGB1999[3] = "L290MB";
            m_adblMaterialYieldStressMinGB1999[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[3] = 1;
            m_astrPipelineMaterialNameGB1999[4] = "L360NB";
            m_adblMaterialYieldStressMinGB1999[4] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[4] = 1;
            m_astrPipelineMaterialNameGB1999[5] = "L360QB";
            m_adblMaterialYieldStressMinGB1999[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[5] = 1;
            m_astrPipelineMaterialNameGB1999[6] = "L360MB";
            m_adblMaterialYieldStressMinGB1999[6] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[6] = 1;
            m_astrPipelineMaterialNameGB1999[7] = "L415NB";
            m_adblMaterialYieldStressMinGB1999[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[7] = 1;
            m_astrPipelineMaterialNameGB1999[8] = "L415QB";
            m_adblMaterialYieldStressMinGB1999[8] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[8] = 1;
            m_astrPipelineMaterialNameGB1999[9] = "L415MB";
            m_adblMaterialYieldStressMinGB1999[9] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[9] = 1;
            m_astrPipelineMaterialNameGB1999[10] = "L450QB";
            m_adblMaterialYieldStressMinGB1999[10] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[10] = 1;
            m_astrPipelineMaterialNameGB1999[11] = "L450MB";
            m_adblMaterialYieldStressMinGB1999[11] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[11] = 1;
            m_astrPipelineMaterialNameGB1999[12] = "L485QB";
            m_adblMaterialYieldStressMinGB1999[12] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[12] = 1;
            m_astrPipelineMaterialNameGB1999[13] = "L485MB";
            m_adblMaterialYieldStressMinGB1999[13] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[13] = 1;
            m_astrPipelineMaterialNameGB1999[14] = "L555QB";
            m_adblMaterialYieldStressMinGB1999[14] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[14] = 1;
            m_astrPipelineMaterialNameGB1999[15] = "L555MB";
            m_adblMaterialYieldStressMinGB1999[15] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[15] = 1;

            #endregion
            //API5L规范                                                  管材类型   API5L
            #region

            m_astrPipelineMaterialNameAPI5L[0] = "A25";
            m_adblMaterialYieldStressMinAPI5L[0] = 172;
            m_adblMaterialWeldingLineCoefficientAPI5L[0] = 1;
            m_astrPipelineMaterialNameAPI5L[1] = "A";
            m_adblMaterialYieldStressMinAPI5L[1] = 207;
            m_adblMaterialWeldingLineCoefficientAPI5L[1] = 1;
            m_astrPipelineMaterialNameAPI5L[2] = "B";
            m_adblMaterialYieldStressMinAPI5L[2] = 241;
            m_adblMaterialWeldingLineCoefficientAPI5L[2] = 1;
            m_astrPipelineMaterialNameAPI5L[3] = "X42";
            m_adblMaterialYieldStressMinAPI5L[3] = 289;
            m_adblMaterialWeldingLineCoefficientAPI5L[3] = 1;
            m_astrPipelineMaterialNameAPI5L[4] = "X46";
            m_adblMaterialYieldStressMinAPI5L[4] = 317;
            m_adblMaterialWeldingLineCoefficientAPI5L[4] = 1;
            m_astrPipelineMaterialNameAPI5L[5] = "X52";
            m_adblMaterialYieldStressMinAPI5L[5] = 358;
            m_adblMaterialWeldingLineCoefficientAPI5L[5] = 1;
            m_astrPipelineMaterialNameAPI5L[6] = "X56";
            m_adblMaterialYieldStressMinAPI5L[6] = 386;
            m_adblMaterialWeldingLineCoefficientAPI5L[6] = 1;
            m_astrPipelineMaterialNameAPI5L[7] = "X60";
            m_adblMaterialYieldStressMinAPI5L[7] = 413;
            m_adblMaterialWeldingLineCoefficientAPI5L[7] = 1;
            m_astrPipelineMaterialNameAPI5L[8] = "X65";
            m_adblMaterialYieldStressMinAPI5L[8] = 448;
            m_adblMaterialWeldingLineCoefficientAPI5L[8] = 1;
            m_astrPipelineMaterialNameAPI5L[9] = "X70";
            m_adblMaterialYieldStressMinAPI5L[9] = 482;
            m_adblMaterialWeldingLineCoefficientAPI5L[9] = 1;
            m_astrPipelineMaterialNameAPI5L[10] = "X80";
            m_adblMaterialYieldStressMinAPI5L[10] = 551;
            m_adblMaterialWeldingLineCoefficientAPI5L[10] = 1;

            #endregion


            if (ComInput1.Text == "")
            {
                //传入的管线材料名称为空

                MessageBox.Show("您传入的管线材料名称为空!", /*Constants.vbExclamation,*/  "CPipelineEconomyConsumeSteel" + "-" + "SetPipelineMaterialIDByName(strPipelineMaterialByName As String)");
            }


            else if (radioButtonGb1997.Checked == true)
            {
                //气体管道采用GB1997规范
                for (intI = 0; intI <= mc_intPipelineMaterialGB1997Count - 1; intI++)
                {
                    if (strPipelineMaterialByName == m_astrPipelineMaterialNameGB1997[intI])
                    {
                        //找到对应的材料
                        intPipelineMaterialID = intI;
                        return intPipelineMaterialID;
                        break;
                    }
                }
            }
            else if (radioButtonGb1999.Checked==true )
            {
                //气体管道采用GB1999规范
                for (intI = 0; intI <= mc_intPipelineMaterialGB1999Count - 1; intI++)
                {
                    if (strPipelineMaterialByName == m_astrPipelineMaterialNameGB1999[intI])
                    {
                        //找到对应的材料
                        intPipelineMaterialID = intI;
                        return intPipelineMaterialID;
                        break;
                    }
                }
            }
            else if (radioButtonAPI5L.Checked==true)
            {
                //气体管道采用API5L规范
                for (intI = 0; intI <= mc_intPipelineMaterialAPI5LCount - 1; intI++)
                {
                    if (strPipelineMaterialByName == m_astrPipelineMaterialNameAPI5L[intI])
                    {
                        //找到对应的材料
                        intPipelineMaterialID = intI;
                        return intPipelineMaterialID;
                        break;
                    }
                }
            }
            else
            {
        

                MessageBox.Show("您传入的计算类型目前还没有考虑，不能获取管线材料名称ID号!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            return intPipelineMaterialID;
        }
        #region  垃圾代码 
        //private bool IsValidate()
        //{
        //    bool returnValue = false;
        //    //作用：判断本类是否可用
        //    //接受的参数：
        //    //输入参数：无
        //    //输出参数：无
        //    //返回值：无
        //    //说明:对于所求参数不应该判定是否为0或负数的情况
        //    //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
        //    if (radioButtonGb1997.Checked==false &&radioButtonGb1999.Checked==false&&radioButtonAPI5L.Checked==false)
        //    {
        //        returnValue = false;
        //        MessageBox.Show("您设置的计算类型为无,不能对这种计算类型进行计算,管子尺寸初步计算(经济及可行性)计算对象无效!(正常时不会出现)", /*/*Constants.vbExclamation,*/ "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //        goto PROC_EXIT;
        //    }
        //    else if (radioButtonGb1997.Checked ==true )
        //    {
        //        //求解液体管道(按GB1997)的情况
        //        //判断各参数是否满足大致计算范围

        //        //管线设计绝对压力
        //        if (m_blnInputPipelineDesignAbsolutePressure)
        //        {
        //            //管线设计绝对压力(为正数)
        //            if (m_dblPipelineDesignAbsolutePressure < 0)
        //            {
        //                //管线设计绝对压力为负数
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为负数,管子尺寸初步计算(经济及可行性)计算对象无效!",/* /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineDesignAbsolutePressure == 0)
        //            {
        //                //管线设计绝对压力为零
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*/*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线设计绝对压力参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线设计绝对压力参数未输入,不能进行计算!",/* /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //管线外直径
        //        if (m_blnInputPipelineExternalDiameter)
        //        {
        //            //管线外直径(为正数)
        //            if (m_dblPipelineExternalDiameter < 0)
        //            {
        //                //管线外直径为负数
        //                returnValue = false;
        //                MessageBox.Show("管线外直径负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*/*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineExternalDiameter == 0)
        //            {
        //                //管线外直径为零
        //                returnValue = false;
        //                MessageBox.Show("管线外直径为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*/*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线外直径参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线外直径未输入,不能进行计算!", /*/*Constants.vbInformation,*/ "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区1类长度
        //        if (m_blnInputPipelineLengthIn1Area1Class)
        //        {
        //            //一级地区1类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area1Class < 0)
        //            {
        //                //一级地区1类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区1类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*/*Constants.vbInformation,*/ "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区1类长度参数未输入,不能进行计算!", /*/*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区2类长度
        //        if (m_blnInputPipelineLengthIn1Area2Class)
        //        {
        //            //一级地区2类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area2Class < 0)
        //            {
        //                //一级地区2类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区2类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*/*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区2类长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //二级地区长度
        //        if (m_blnInputPipelineLengthIn2Area)
        //        {
        //            //二级地区长度(为正数)
        //            if (m_dblPipelineLengthIn2Area < 0)
        //            {
        //                //二级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("二级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //二级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("二级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //三级地区长度
        //        if (m_blnInputPipelineLengthIn3Area)
        //        {
        //            //三级地区长度(为正数)
        //            if (m_dblPipelineLengthIn3Area < 0)
        //            {
        //                //三级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("三级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //三级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("三级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //四级地区长度
        //        if (m_blnInputPipelineLengthIn4Area)
        //        {
        //            //四级地区长度(为正数)
        //            if (m_dblPipelineLengthIn4Area < 0)
        //            {
        //                //四级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("四级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //四级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("四级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //检查几何模型各参数间的关系是否错误(无)

        //        //检查物理模型是否在范围内(无)
        //    }
        //    else if (radioButtonGb1999.Checked == true)
        //    {
        //        //求解液体管道(按GB1997)的情况
        //        //判断各参数是否满足大致计算范围

        //        //管线设计绝对压力
        //        if (m_blnInputPipelineDesignAbsolutePressure)
        //        {
        //            //管线设计绝对压力(为正数)
        //            if (m_dblPipelineDesignAbsolutePressure < 0)
        //            {
        //                //管线设计绝对压力为负数
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineDesignAbsolutePressure == 0)
        //            {
        //                //管线设计绝对压力为零
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线设计绝对压力参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线设计绝对压力参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //管线外直径
        //        if (m_blnInputPipelineExternalDiameter)
        //        {
        //            //管线外直径(为正数)
        //            if (m_dblPipelineExternalDiameter < 0)
        //            {
        //                //管线外直径为负数
        //                returnValue = false;
        //                MessageBox.Show("管线外直径负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineExternalDiameter == 0)
        //            {
        //                //管线外直径为零
        //                returnValue = false;
        //                MessageBox.Show("管线外直径为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线外直径参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线外直径未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区1类长度
        //        if (m_blnInputPipelineLengthIn1Area1Class)
        //        {
        //            //一级地区1类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area1Class < 0)
        //            {
        //                //一级地区1类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区1类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区1类长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区2类长度
        //        if (m_blnInputPipelineLengthIn1Area2Class)
        //        {
        //            //一级地区2类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area2Class < 0)
        //            {
        //                //一级地区2类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区2类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区2类长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //二级地区长度
        //        if (m_blnInputPipelineLengthIn2Area)
        //        {
        //            //二级地区长度(为正数)
        //            if (m_dblPipelineLengthIn2Area < 0)
        //            {
        //                //二级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("二级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //二级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("二级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //三级地区长度
        //        if (m_blnInputPipelineLengthIn3Area)
        //        {
        //            //三级地区长度(为正数)
        //            if (m_dblPipelineLengthIn3Area < 0)
        //            {
        //                //三级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("三级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //三级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("三级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //四级地区长度
        //        if (m_blnInputPipelineLengthIn4Area)
        //        {
        //            //四级地区长度(为正数)
        //            if (m_dblPipelineLengthIn4Area < 0)
        //            {
        //                //四级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("四级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //四级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("四级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //检查几何模型各参数间的关系是否错误(无)

        //        //检查物理模型是否在范围内(无)
        //    }
        //    else if (radioButtonAPI5L.Checked==true )
        //    {
        //        //求解液体管道(按GB1999)的情况
        //        //判断各参数是否满足大致计算范围

        //        //管线设计绝对压力
        //        if (m_blnInputPipelineDesignAbsolutePressure)
        //        {
        //            //管线设计绝对压力(为正数)
        //            if (m_dblPipelineDesignAbsolutePressure < 0)
        //            {
        //                //管线设计绝对压力为负数
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineDesignAbsolutePressure == 0)
        //            {
        //                //管线设计绝对压力为零
        //                returnValue = false;
        //                MessageBox.Show("管线设计绝对压力为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线设计绝对压力参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线设计绝对压力参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //管线外直径
        //        if (m_blnInputPipelineExternalDiameter)
        //        {
        //            //管线外直径(为正数)
        //            if (m_dblPipelineExternalDiameter < 0)
        //            {
        //                //管线外直径为负数
        //                returnValue = false;
        //                MessageBox.Show("管线外直径负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else if (m_dblPipelineExternalDiameter == 0)
        //            {
        //                //管线外直径为零
        //                returnValue = false;
        //                MessageBox.Show("管线外直径为零,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //管线外直径参数未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("管线外直径未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区1类长度
        //        if (m_blnInputPipelineLengthIn1Area1Class)
        //        {
        //            //一级地区1类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area1Class < 0)
        //            {
        //                //一级地区1类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区1类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区1类长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //一级地区2类长度
        //        if (m_blnInputPipelineLengthIn1Area2Class)
        //        {
        //            //一级地区2类长度(为正数)
        //            if (m_dblPipelineLengthIn1Area2Class < 0)
        //            {
        //                //一级地区2类长度为负数
        //                returnValue = false;
        //                MessageBox.Show("一级地区2类长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //一级地区1类长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("一级地区2类长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //二级地区长度
        //        if (m_blnInputPipelineLengthIn2Area)
        //        {
        //            //二级地区长度(为正数)
        //            if (m_dblPipelineLengthIn2Area < 0)
        //            {
        //                //二级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("二级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //二级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("二级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //三级地区长度
        //        if (m_blnInputPipelineLengthIn3Area)
        //        {
        //            //三级地区长度(为正数)
        //            if (m_dblPipelineLengthIn3Area < 0)
        //            {
        //                //三级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("三级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //三级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("三级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //四级地区长度
        //        if (m_blnInputPipelineLengthIn4Area)
        //        {
        //            //四级地区长度(为正数)
        //            if (m_dblPipelineLengthIn4Area < 0)
        //            {
        //                //四级地区长度为负数
        //                returnValue = false;
        //                MessageBox.Show("四级地区长度为负数,管子尺寸初步计算(经济及可行性)计算对象无效!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //                goto PROC_EXIT;
        //            }
        //            else
        //            {
        //                //目前尚未考虑的情况
        //            }
        //        }
        //        else
        //        {
        //            //四级地区长度未输入,不能进行计算
        //            returnValue = false;
        //            MessageBox.Show("四级地区长度参数未输入,不能进行计算!", /*Constants.vbInformation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //            goto PROC_EXIT;
        //        }

        //        //检查几何模型各参数间的关系是否错误(无)

        //        //检查物理模型是否在范围内(无)
        //    }
        //  else
        //    {
        //        //目前还没考虑的情况
        //        returnValue = false;
        //        MessageBox.Show("您设置的计算类型目前还没考虑,请添加!(正常时不会出现)", /*Constants.vbExclamation,*/  "CPipelineEconomyConsumeSteel" + "-" + "IsValidate()");
        //        goto PROC_EXIT;
        //    }

        //    //返回函数成功标记
        //    returnValue = true;

        //PROC_EXIT:
        //    return returnValue;

        //PROC_ERR:
        //    returnValue = false;
        //    MessageBox.Show(/*Information.Err().Number, Information.Err().Description,*/ "CPipelineEconomyConsumeSteel", "IsValidate()");
        //    goto PROC_EXIT;

        //    return returnValue;
        //}
        #endregion
        private void Display()
        {
            
            if (radioButtonGb1997.Checked == true)
            {
                ComInput1.Items.Clear();
                ComInput1.Items.AddRange(new object[] {"L175","L210","L245","L290","L320","L360", "L390","L415", "L450","L485","L555"});
            }
            if (radioButtonGb1999.Checked == true)
            {
                ComInput1.Items.Clear();
                ComInput1.Items.AddRange(new object[] { "L245NB", "L245MB", "L290", "L290MB", "L360NB", "L360QB", "L360MB","L415NB", "L415QB","L415MB",
                    "L450QB", "L450MB", "L485QB", "L485MB", "L555QB", "L555MB" });
            }
            if (radioButtonAPI5L.Checked == true)
            {
                ComInput1.Items.Clear();
                ComInput1.Items.AddRange(new object[] { "A25", "A", "B", "X42", "X46", "X52", "X56","X60", "X65","X70",
                    "X80" });   

            }

        }
        private void CalculateStyleGasPipelineByGB1997()
        {
            //作用：有关GasPipelineByGB1997计算的方法
            //接受的参数：
            //输入参数：无  
            //输出参数：无
            //返回值：无
            //说明:1.无
            // On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            double dblCgm = 0; //许用应力
            double dblCgms = 0; //最低屈服强度
            double dblD = 0; //管线外径
            double dblDelta = 0; //管壁厚度
            double dblDeltaCalculate = 0; //计算壁厚
            double dblDeltaFindTable = 0; //查表壁厚
          

            double dblDFindTable = 0; //查表的管径
            double dblFai = 0; //焊缝系数
            double dblL = 0; //管线总长度

            double dblDeltaW1C1 = 0;    //一级地区1类壁厚
            double dblDeltaW1C2 = 0;    //一级地区2类壁厚
            double dblDeltaW2 = 0;      //二级地区
            double dblDeltaW3 = 0;      //三级地区
            double dblDeltaW4 = 0;      //四级地区

            double dblL1C1 = 0; //一级地区1类长度
            double dblL1C2 = 0; //一级地区2类长度
            double dblL2 = 0; //二级地区长度
            double dblL3 = 0; //三级地区长度
            double dblL4 = 0; //四级地区长度
            double dblF11 = 0.8; //一级地区1类设计系数
            double dblF12 = 0.72; //一级地区2类设计系数
            double dblF2 = 0.6; //二级地区设计系数
            double dblF3 = 0.5; //三级地区设计系数
            double dblF4 = 0.4; //四级地区设计系数
            double dblP = 0; //管线设计压力
            double dblT = 0; //管线设计温度


            double dbltCoefficient = 0; //温度折减系数
            double dblW = 0; //总耗钢量
            double dblW1C1 = 0; //一级地区1类耗钢量
            double dblW1C1Calculate = 0; //一级地区1类耗钢量(计算)
            double dblW1C1FindTable = 0; //一级地区1类耗钢量(查表)
            double dblW1C2 = 0; //一级地区2类耗钢量
            double dblW1C2Calculate = 0; //一级地区2类耗钢量(计算)
            double dblW1C2FindTable = 0; //一级地区2类耗钢量(查表)
            double dblW2 = 0; //二级地区耗钢量
            double dblW2Calculate = 0; //二级地区耗钢量(计算)
            double dblW2FindTable = 0; //二级地区耗钢量(查表)
            double dblW3 = 0; //三级地区耗钢量
            double dblW3Calculate = 0; //三级地区耗钢量(计算)
            double dblW3FindTable = 0; //三级地区耗钢量(查表)
            double dblW4 = 0; //四级地区耗钢量
            double dblW4Calculate = 0; //四级地区耗钢量(计算)
            double dblW4FindTable = 0; //四级地区耗钢量(查表)

            double dblWPerL = 0; //单位长度重量
            double dblWPerLCalculate = 0; //单位长度重量(计算)
            double dblWPerLFindTable = 0; //单位长度重量(查表)
     

                 Display();     //  管线类型选择

            //GB1997                    管材类型      国标1997
            #region 


            m_astrPipelineMaterialNameGB1997[0] = "L175";
            m_adblMaterialYieldStressMinGB1997[0] = 175;
            m_adblMaterialWeldingLineCoefficientGB1997[0] = 1;
            m_astrPipelineMaterialNameGB1997[1] = "L210";
            m_adblMaterialYieldStressMinGB1997[1] = 210;
            m_adblMaterialWeldingLineCoefficientGB1997[1] = 1;
            m_astrPipelineMaterialNameGB1997[2] = "L245";
            m_adblMaterialYieldStressMinGB1997[2] = 245;
            m_adblMaterialWeldingLineCoefficientGB1997[2] = 1;
            m_astrPipelineMaterialNameGB1997[3] = "L290";
            m_adblMaterialYieldStressMinGB1997[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1997[3] = 1;
            m_astrPipelineMaterialNameGB1997[4] = "L320";
            m_adblMaterialYieldStressMinGB1997[4] = 320;
            m_adblMaterialWeldingLineCoefficientGB1997[4] = 1;
            m_astrPipelineMaterialNameGB1997[5] = "L360";
            m_adblMaterialYieldStressMinGB1997[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1997[5] = 1;
            m_astrPipelineMaterialNameGB1997[6] = "L390";
            m_adblMaterialYieldStressMinGB1997[6] = 390;
            m_adblMaterialWeldingLineCoefficientGB1997[6] = 1;
            m_astrPipelineMaterialNameGB1997[7] = "L415";
            m_adblMaterialYieldStressMinGB1997[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1997[7] = 1;
            m_astrPipelineMaterialNameGB1997[8] = "L450";
            m_adblMaterialYieldStressMinGB1997[8] = 450;
            m_adblMaterialWeldingLineCoefficientGB1997[8] = 1;
            m_astrPipelineMaterialNameGB1997[9] = "L485";
            m_adblMaterialYieldStressMinGB1997[9] = 485;
            m_adblMaterialWeldingLineCoefficientGB1997[9] = 1;
            m_astrPipelineMaterialNameGB1997[10] = "L555";
            m_adblMaterialYieldStressMinGB1997[10] = 555;
            #endregion

            int Num = SetPipelineMaterialIDByName(ComInput1.Text);
            //if (IsValidate())
            //{
            //用GB1997规范求解气体管道耗钢量
            //对局部变量赋值
                dblP = Convert.ToDouble(txtInput1.Text);  //  绝对压力
                dblD = Convert.ToDouble(txtInput2.Text);   //  管线外直径
                dblL1C1 = Convert.ToDouble(txtInput3.Text);  // 一类地区1类长度
                dblL1C2 = Convert.ToDouble(txtInput4.Text);  // 一类地区2类长度
                dblL2 = Convert.ToDouble(txtInput5.Text);    // 二类地区
                dblL3 = Convert.ToDouble(txtInput6.Text);  // 三类地区
                dblL4 = Convert.ToDouble(txtInput7.Text);  // 四类地区
                dblT = Convert.ToDouble(txtInput8.Text);    // 管线设计温度

                //求解过程
                //**********************以下为核心代码**************************
                //按<输气管道工程设计规范>的要求转换单位制
                dblL1C1 = dblL1C1 / 1000; //(km)
                dblL1C2 = dblL1C2 / 1000; //(km)
                dblL2 = dblL2 / 1000; //(km)
                dblL3 = dblL3 / 1000; //(km)
                dblL4 = dblL4 / 1000; //(km)
                dblP = dblP / 1000000; //(MPa)
                dblD = dblD * 1000; //(mm)
                //dblT = dblT - 273.15; //(℃)

                ////强度刚度校核参数
                //intIntensionAndRigidityCheck = m_intIntensionAndRigidityCheck;

                //开始计算     总长度
                dblL = dblL1C1 + dblL1C2 + dblL2 + dblL3 + dblL4;
              


                //最低屈服强度
              

                if (PipelineAnalyChecked5.Checked==true)
                {
                    //曾经加热大于等于300℃（焊接除外）的钢管许用应力
                    dblCgms = m_adblMaterialYieldStressMinGB1997[Num ] * 0.75;
                }
                else
                {
                    dblCgms = m_adblMaterialYieldStressMinGB1997[Num ];
                }

                //确定焊缝系数
                dblFai = 1;

                //温度折减系数
                if (dblT > 120)
                {
                    //管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!
                    MessageBox.Show("管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!", "提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //对象不能使用,修改对象标记
                    m_blnRefresh = true;
                    m_blnEnabled = false;
                    goto PROC_EXIT;
                }
                else
                {
                    dbltCoefficient = 1;
                }

                //确定壁厚的方法
                if (PipelineAnalyChecked1.Checked == true)
                {
                    //使用计算壁厚为所需壁厚
                    if (PipelineAnalyChecked2.Checked == true )
                    {
                        //同时显示用计算和查表方法计算的内容
                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblCgms * dblFai * dblF11 * dbltCoefficient;
               
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        ////查表确定壁厚(显示提问)
                        if (!GetWallThicknessByGB1997(true, dblDFindTable, dblDeltaFindTable))
                      {
                          //不能由GB1997查到对应的壁厚，不能计算耗钢量
                           //对象不能使用,修改对象标记
                         m_blnRefresh = true;
                         m_blnEnabled = false;
                         goto PROC_EXIT;
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                          if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

     
                        //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C1Calculate = dblWPerLCalculate * dblL1C1 * 1000;
                        dblW1C1FindTable = dblWPerLFindTable * dblL1C1 * 1000;
                    

                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
           
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;
                    
                        //查表确定壁厚
                        if (!GetWallThicknessByGB1997(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1997查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                       }
                       //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C2Calculate = dblWPerLCalculate * dblL1C2 * 1000;
                        dblW1C2FindTable = dblWPerLFindTable * dblL1C2 * 1000;
         


                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
          
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW2 = dblDelta; //二级地区壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByGB1997(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1997查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                
                        //二级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //二级地区总耗钢
                        dblW2Calculate = dblWPerLCalculate * dblL2 * 1000;
                        dblW2FindTable = dblWPerLFindTable * dblL2 * 1000;
                    

                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
             
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW3 = dblDelta; //三级地区壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByGB1997(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1997查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //三级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //三级地区总耗钢
                        dblW3Calculate = dblWPerLCalculate * dblL3 * 1000;
                        dblW3FindTable = dblWPerLFindTable * dblL3 * 1000;
        


                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW4 = dblDelta; //四级地区壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByGB1997(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1997查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

         
                        //四级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //四级地区总耗钢
                        dblW4Calculate = dblWPerLCalculate * dblL4 * 1000;
                        dblW4FindTable = dblWPerLFindTable * dblL4 * 1000;

                        //用于显示的各部分壁厚
                        txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                        txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                        txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                        txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                        txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚

                        //用于显示的各部分耗钢量
                        dblW1C1 = dblW1C1Calculate; //一级地区1类耗钢量
                        dblW1C2 = dblW1C2Calculate; //一级地区2类耗钢量
                        dblW2 = dblW2Calculate; //2级地区耗钢量
                        dblW3 = dblW3Calculate; //3级地区耗钢量
                        dblW4 = dblW4Calculate; //4级地区耗钢量                   
                    }
                    else
                    {
                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //一级地区1类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区1类总耗钢
                        dblW1C1 = dblWPerL * dblL1C1 * 1000;


                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //一级地区2类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区2类总耗钢
                        dblW1C2 = dblWPerL * dblL1C2 * 1000;
        

                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble((10 * dblDelta + 1) / 10);
                        dblDeltaW2 = dblDelta; //二级地区壁厚
                   
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //二级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //二级地区总耗钢
                        dblW2 = dblWPerL * dblL2 * 1000;
         

                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW3 = dblDelta; //三级地区壁厚
                  
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //三级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //三级地区总耗钢
                        dblW3 = dblWPerL * dblL3 * 1000;
         

                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW4 = dblDelta; //四级地区壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked==true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //四级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //四级总耗钢
                        dblW4 = dblWPerL * dblL4 * 1000;
      
                    }
                }
                else
                {
                    //直接采用查规范的方法确定壁厚

                    //一级地区1类的许用应力
                    dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区1类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    //查表确定壁厚
                    dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                    if (!GetWallThicknessByGB1997(true, dblD, dblDelta))
                    {
                        //不能由GB1997查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked==true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //一级地区1类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区1类总耗钢
                    dblW1C1 = dblWPerL * dblL1C1 * 1000;


                    //一级地区2类的许用应力
                    dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区2类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1997(false, dblD, dblDelta))
                    {
                        //不能由GB1997查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }
                    dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked==true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //一级地区2类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区2类总耗钢
                    dblW1C2 = dblWPerL * dblL1C2 * 1000;
       

                    //二级地区的许用应力
                    dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                    //二级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    //查表确定壁厚
                    dblDeltaW2 = dblDelta; //二级地区壁厚
                    if (!GetWallThicknessByGB1997(false, dblD, dblDelta))
                    {
                        //不能由GB1997查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked==true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //二级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //二级地区总耗钢
                    dblW2 = dblWPerL * dblL2 * 1000;
             
                    //三级地区的许用应力
                    dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                    //三级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    //查表确定壁厚
                    dblDeltaW3 = dblDelta; //三级地区壁厚
                    if (!GetWallThicknessByGB1997(false, dblD, dblDelta))
                    {
                        //不能由GB1997查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked==true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //三级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //三级地区总耗钢
                    dblW3 = dblWPerL * dblL3 * 1000;
                    //激发获取计算参数的事件
                
                    //四级地区的许用应力
                    dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                    //四级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW4 = dblDelta; //四级地区壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1997(false, dblD, dblDelta))
                    {
                        //不能由GB1997查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1997查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked==true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //四级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //四级总耗钢
                    dblW4 = dblWPerL * dblL4 * 1000;
                    //激发获取计算参数的事件
                   
                }

                //按<输气管道工程设计规范>的要求恢复单位制到国际单位
                dblL = dblL * 1000;
                dblW = dblW1C1 + dblW1C2 + dblW2 + dblW3 + dblW4;

                //**********************以上为核心代码**************************
                txtOutput1.Text = dblL.ToString();       //管线总长度
                txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚

                txtOutput7.Text = dblW1C1.ToString(); //一级地区1类耗钢量
                txtOutput8.Text = dblW1C2.ToString(); //一级地区2类耗钢量
                txtOutput9.Text = dblW2.ToString(); //2级地区耗钢量
                txtOutput10.Text = dblW3.ToString(); //3级地区耗钢量
                txtOutput11.Text = dblW4.ToString(); //4级地区耗钢量
                txtOutput12.Text = dblW.ToString(); //总耗钢量

                //修改对象标记
                m_blnRefresh = true;
            //    m_blnEnabled = true;
            //}
            //else
            //{
            //    //对象不能使用,修改对象标记
            //    m_blnRefresh = true;
            //    m_blnEnabled = false;
            //}

        PROC_EXIT:
            return;


        }
        private void CalculateStyleGasPipelineByGB1999()
        {
            //作用：有关GasPipelineByGB1999计算的方法
            //接受的参数：
            //输入参数：无
            //输出参数：无
            //返回值：无
            //说明:1.无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            double dblCgm = 0; //许用应力
            double dblCgms = 0; //最低屈服强度
            double dblD = 0; //管线外径
            double dblDelta = 0; //管壁厚度
            double dblDeltaCalculate = 0; //计算壁厚
            double dblDeltaFindTable = 0; //查表壁厚
          

        
            double dblDFindTable = 0; //查表的管径
            double dblFai = 0; //焊缝系数
            double dblDeltaW1C1 = 0;    //一级地区1类壁厚
            double dblDeltaW1C2 = 0;    //一级地区2类壁厚
            double dblDeltaW2 = 0;      //二级地区
            double dblDeltaW3 = 0;      //三级地区
            double dblDeltaW4 = 0;      //四级地区

            double dblL = 0; //管线总长度
            double dblL1C1 = 0; //一级地区1类长度
            double dblL1C2 = 0; //一级地区2类长度
            double dblL2 = 0; //二级地区长度
            double dblL3 = 0; //三级地区长度
            double dblL4 = 0; //四级地区长度

            double dblF11 = 0; //一级地区1类设计系数
            double dblF12 = 0; //一级地区2类设计系数
            double dblF2 = 0; //二级地区设计系数
            double dblF3 = 0; //三级地区设计系数
            double dblF4 = 0; //四级地区设计系数

            double dblP = 0; //管线设计压力
            double dblT = 0; //管线设计温度
            double dbltCoefficient = 0; //温度折减系数

            double dblW = 0; //总耗钢量

            double dblW1C1 = 0; //一级地区1类耗钢量
            double dblW1C1Calculate = 0; //一级地区1类耗钢量(计算)
            double dblW1C1FindTable = 0; //一级地区1类耗钢量(查表)
            double dblW1C2 = 0; //一级地区2类耗钢量
            double dblW1C2Calculate = 0; //一级地区2类耗钢量(计算)
            double dblW1C2FindTable = 0; //一级地区2类耗钢量(查表)

            double dblW2 = 0; //二级地区耗钢量
            double dblW2Calculate = 0; //二级地区耗钢量(计算)
            double dblW2FindTable = 0; //二级地区耗钢量(查表)

            double dblW3 = 0; //三级地区耗钢量
            double dblW3Calculate = 0; //三级地区耗钢量(计算)
            double dblW3FindTable = 0; //三级地区耗钢量(查表)

            double dblW4 = 0; //四级地区耗钢量
            double dblW4Calculate = 0; //四级地区耗钢量(计算)
            double dblW4FindTable = 0; //四级地区耗钢量(查表)

       

            double dblWPerL = 0; //单位长度重量
            double dblWPerLCalculate = 0; //单位长度重量(计算)
            double dblWPerLFindTable = 0; //单位长度重量(查表)

            int intIntensionAndRigidityCheck; //强度刚度校核选项
             
            Display();

            #region


            m_astrPipelineMaterialNameGB1999[0] = "L245NB";
            m_adblMaterialYieldStressMinGB1999[0] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[0] = 1;
            m_astrPipelineMaterialNameGB1999[1] = "L245MB";
            m_adblMaterialYieldStressMinGB1999[1] = 245;
            m_adblMaterialWeldingLineCoefficientGB1999[1] = 1;
            m_astrPipelineMaterialNameGB1999[2] = "L290";
            m_adblMaterialYieldStressMinGB1999[2] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[2] = 1;
            m_astrPipelineMaterialNameGB1999[3] = "L290MB";
            m_adblMaterialYieldStressMinGB1999[3] = 290;
            m_adblMaterialWeldingLineCoefficientGB1999[3] = 1;
            m_astrPipelineMaterialNameGB1999[4] = "L360NB";
            m_adblMaterialYieldStressMinGB1999[4] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[4] = 1;
            m_astrPipelineMaterialNameGB1999[5] = "L360QB";
            m_adblMaterialYieldStressMinGB1999[5] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[5] = 1;
            m_astrPipelineMaterialNameGB1999[6] = "L360MB";
            m_adblMaterialYieldStressMinGB1999[6] = 360;
            m_adblMaterialWeldingLineCoefficientGB1999[6] = 1;
            m_astrPipelineMaterialNameGB1999[7] = "L415NB";
            m_adblMaterialYieldStressMinGB1999[7] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[7] = 1;
            m_astrPipelineMaterialNameGB1999[8] = "L415QB";
            m_adblMaterialYieldStressMinGB1999[8] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[8] = 1;
            m_astrPipelineMaterialNameGB1999[9] = "L415MB";
            m_adblMaterialYieldStressMinGB1999[9] = 415;
            m_adblMaterialWeldingLineCoefficientGB1999[9] = 1;
            m_astrPipelineMaterialNameGB1999[10] = "L450QB";
            m_adblMaterialYieldStressMinGB1999[10] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[10] = 1;
            m_astrPipelineMaterialNameGB1999[11] = "L450MB";
            m_adblMaterialYieldStressMinGB1999[11] = 450;
            m_adblMaterialWeldingLineCoefficientGB1999[11] = 1;
            m_astrPipelineMaterialNameGB1999[12] = "L485QB";
            m_adblMaterialYieldStressMinGB1999[12] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[12] = 1;
            m_astrPipelineMaterialNameGB1999[13] = "L485MB";
            m_adblMaterialYieldStressMinGB1999[13] = 485;
            m_adblMaterialWeldingLineCoefficientGB1999[13] = 1;
            m_astrPipelineMaterialNameGB1999[14] = "L555QB";
            m_adblMaterialYieldStressMinGB1999[14] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[14] = 1;
            m_astrPipelineMaterialNameGB1999[15] = "L555MB";
            m_adblMaterialYieldStressMinGB1999[15] = 555;
            m_adblMaterialWeldingLineCoefficientGB1999[15] = 1;

            #endregion  
            int Num = SetPipelineMaterialIDByName(ComInput1.Text);


            //if (IsValidate())
            //{
            //用GB1999规范求解气体管道耗钢量
            //对局部变量赋值
                dblP = Convert.ToDouble(txtInput1.Text);  //  绝对压力
                dblD = Convert.ToDouble(txtInput2.Text);   //  管线外直径
                dblL1C1 = Convert.ToDouble(txtInput3.Text);  // 一类地区1类长度
                dblL1C2 = Convert.ToDouble(txtInput4.Text);  // 一类地区2类长度
                dblL2 = Convert.ToDouble(txtInput5.Text);    // 二类地区
                dblL3 = Convert.ToDouble(txtInput6.Text);  // 三类地区
                dblL4 = Convert.ToDouble(txtInput7.Text);  // 四类地区
                dblT = Convert.ToDouble(txtInput8.Text);    // 管线设计温度

                //求解过程
                //**********************以下为核心代码**************************
                //按<输气管道工程设计规范>的要求转换单位制
                dblL1C1 = dblL1C1 / 1000; //(km)
                dblL1C2 = dblL1C2 / 1000; //(km)
                dblL2 = dblL2 / 1000; //(km)
                dblL3 = dblL3 / 1000; //(km)
                dblL4 = dblL4 / 1000; //(km)
                dblP = dblP / 1000000; //(MPa)
                dblD = dblD * 1000; //(mm)
                dblT = dblT - 273.15; //(℃)

                //强度刚度校核参数
                intIntensionAndRigidityCheck = m_intIntensionAndRigidityCheck;

                //开始计算
                dblL = dblL1C1 + dblL1C2 + dblL2 + dblL3 + dblL4;

                ////激发获取计算参数的事件
                //m_strCalculateProcess = "Get Yield Stress...";
                //if (ShowCalculateProcessEvent != null)
                //    ShowCalculateProcessEvent(m_strCalculateProcess);


            

                //确定许用应力

                //    确定最低屈服强度

               if (PipelineAnalyChecked5.Checked == true )
                {
                    //曾经加热大于等于300℃（焊接除外）的钢管许用应力
                    dblCgms = m_adblMaterialYieldStressMinGB1999[Num] * 0.75;
                }
                else
                {
                  dblCgms = m_adblMaterialYieldStressMinGB1999[Num ];
                }

                //确定焊缝系数
                dblFai = 1;

                //设计系数
             
                        //天然气
                        //一级地区1类设计系数
                        dblF11 = 0.8;
                        //一级地区2类设计系数
                        dblF12 = 0.72;
                        //二级地区设计系数
                        dblF2 = 0.6;
                        //三级地区设计系数
                        dblF3 = 0.5;
                        //四级地区设计系数
                        dblF4 = 0.4;
                      

                //温度折减系数

                if (dblT > 120)
                {
                    //管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!
                    MessageBox.Show("管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!", "提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //对象不能使用,修改对象标记
                    m_blnRefresh = true;
                    m_blnEnabled = false;
                    goto PROC_EXIT;
                }
                else
                {
                    dbltCoefficient = 1;
                }

                //确定壁厚的方法
                if( PipelineAnalyChecked1.Checked == true)    //    复选框
                {
                    //使用计算壁厚为所需壁厚
                    if (PipelineAnalyChecked2.Checked == true)
                    {
                        //同时显示用计算和查表方法计算的内容

                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                        
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1                                 
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;
                        //查表确定壁厚(显示提问)
                        if (!GetWallThicknessByGB1999(true, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1999查到对应的壁厚，不能计算耗钢量
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }


                        //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C1Calculate = dblWPerLCalculate * dblL1C1 * 1000;
                        dblW1C1FindTable = dblWPerLFindTable * dblL1C1 * 1000;
             

                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                     
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;
                        //查表确定壁厚
                        if (!GetWallThicknessByGB1999(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1999查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                      
                        //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C2Calculate = dblWPerLCalculate * dblL1C2 * 1000;
                        dblW1C2FindTable = dblWPerLFindTable * dblL1C2 * 1000;
                  


                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                        
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW2 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByGB1999(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1999查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                      
                        //二级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //二级地区总耗钢
                        dblW2Calculate = dblWPerLCalculate * dblL2 * 1000;
                        dblW2FindTable = dblWPerLFindTable * dblL2 * 1000;
             


                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
               
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW3 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByGB1999(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1999查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                   
                        //三级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //三级地区总耗钢
                        dblW3Calculate = dblWPerLCalculate * dblL3 * 1000;
                        dblW3FindTable = dblWPerLFindTable * dblL3 * 1000;
                   


                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                 
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW4 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;
                        //查表确定壁厚
                        if (!GetWallThicknessByGB1999(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由GB1999查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //四级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //四级地区总耗钢
                        dblW4Calculate = dblWPerLCalculate * dblL4 * 1000;
                        dblW4FindTable = dblWPerLFindTable * dblL4 * 1000;

                        txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                        txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                        txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                        txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                        txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚

                        //用于显示的各部分耗钢量              用于显示耗钢量计算过程      
                        dblW1C1 = dblW1C1Calculate; //一级地区1类耗钢量
                        dblW1C2 = dblW1C2Calculate; //一级地区2类耗钢量
                        dblW2 = dblW2Calculate; //2级地区耗钢量
                        dblW3 = dblW3Calculate; //3级地区耗钢量
                        dblW4 = dblW4Calculate; //4级地区耗钢量
                    }
                    else
                    {
                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                        //一级地区1类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区1类总耗钢
                        dblW1C1 = dblWPerL * dblL1C1 * 1000;
     

                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //一级地区2类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区2类总耗钢
                        dblW1C2 = dblWPerL * dblL1C2 * 1000;
                       

                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW2 = dblDelta; //一级地区1类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //二级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //二级地区总耗钢
                        dblW2 = dblWPerL * dblL2 * 1000;
                       

                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW3 = dblDelta; //一级地区1类壁厚

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //三级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //三级地区总耗钢
                        dblW3 = dblWPerL * dblL3 * 1000;
                    

                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW4 = dblDelta; //一级地区1类壁厚

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //四级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //四级总耗钢
                        dblW4 = dblWPerL * dblL4 * 1000;
                       
                    }
                }
                else
                {
                    //直接采用查规范的方法确定壁厚

                    //一级地区1类的许用应力
                    dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区1类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1999(true, dblD, dblDelta))
                    {
                        //不能由GB1999查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //一级地区1类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区1类总耗钢
                    dblW1C1 = dblWPerL * dblL1C1 * 1000;
                

                    //一级地区2类的许用应力
                    dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区2类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1999(false, dblD, dblDelta))
                    {
                        //不能由GB1999查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //一级地区2类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区2类总耗钢
                    dblW1C2 = dblWPerL * dblL1C2 * 1000;
                    

                    //二级地区的许用应力
                    dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                    //二级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW2 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1999(false, dblD, dblDelta))
                    {
                        //不能由GB1999查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //二级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //二级地区总耗钢
                    dblW2 = dblWPerL * dblL2 * 1000;
                   

                    //三级地区的许用应力
                    dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                    //三级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW3= dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1999(false, dblD, dblDelta))
                    {
                        //不能由GB1999查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //三级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //三级地区总耗钢
                    dblW3 = dblWPerL * dblL3 * 1000;
                 

                    //四级地区的许用应力
                    dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                    //四级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW4 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByGB1999(false, dblD, dblDelta))
                    {
                        //不能由GB1999查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由GB1999查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //四级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //四级总耗钢
                    dblW4 = dblWPerL * dblL4 * 1000;
            
                }

                //按<输油管道工程设计规范>的要求恢复单位制到国际单位
                dblL = dblL * 1000;
                dblW = dblW1C1 + dblW1C2 + dblW2 + dblW3 + dblW4;

                //**********************以上为核心代码**************************
                txtOutput1.Text = dblL.ToString();       //管线总长度
                txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚

                txtOutput7.Text = dblW1C1.ToString(); //一级地区1类耗钢量
                txtOutput8.Text = dblW1C2.ToString(); //一级地区2类耗钢量
                txtOutput9.Text = dblW2.ToString(); //2级地区耗钢量
                txtOutput10.Text = dblW3.ToString(); //3级地区耗钢量
                txtOutput11.Text = dblW4.ToString(); //4级地区耗钢量
                txtOutput12.Text = dblW.ToString(); //总耗钢量

                //修改对象标记
                m_blnRefresh = true;
                m_blnEnabled = true;
            //}
            //else
            //{
            //    //对象不能使用,修改对象标记
            //    m_blnRefresh = true;
            //    m_blnEnabled = false;
            //}

        PROC_EXIT:
            return;
            
            MessageBox.Show(/*Information.Err().Number, Information.Err().Description,*/ "CPipelineEconomyConsumeSteel", "CalculateStyleGasPipelineByGB1999()");
            goto PROC_EXIT;

        }

                    
        private void CalculateStyleGasPipelineByAPI5L()
        {
            //作用：有关GasPipelineByAPI5L计算的方法
            //接受的参数：
            //输入参数：无
            //输出参数：无
            //返回值：无
            //说明:1.无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            double dblCgm = 0; //许用应力
            double dblCgms = 0; //最低屈服强度
            double dblD = 0; //管线外径
            double dblDelta = 0; //管壁厚度
            double dblDeltaCalculate = 0; //计算壁厚
            double dblDeltaFindTable = 0; //查表壁厚
           

            double dblDFindTable = 0; //查表的管径
            double dblFai = 0; //焊缝系数
            double dblL = 0; //管线总长度
            double dblDeltaW1C1 = 0;    //一级地区1类壁厚
            double dblDeltaW1C2 = 0;    //一级地区2类壁厚
            double dblDeltaW2 = 0;      //二级地区
            double dblDeltaW3 = 0;      //三级地区
            double dblDeltaW4 = 0;      //四级地区
            double dblL1C1 = 0; //一级地区1类长度
            double dblL1C2 = 0; //一级地区2类长度
            double dblL2 = 0; //二级地区长度
            double dblL3 = 0; //三级地区长度
            double dblL4 = 0; //四级地区长度
            double dblF11 = 0; //一级地区1类设计系数
            double dblF12 = 0; //一级地区2类设计系数
            double dblF2 = 0; //二级地区设计系数
            double dblF3 = 0; //三级地区设计系数
            double dblF4 = 0; //四级地区设计系数
            double dblP = 0; //管线设计压力
            double dblT = 0; //管线设计温度
            double dbltCoefficient = 0; //温度折减系数
            double dblW = 0; //总耗钢量
            double dblW1C1 = 0; //一级地区1类耗钢量
            double dblW1C1Calculate = 0; //一级地区1类耗钢量(计算)
            double dblW1C1FindTable = 0; //一级地区1类耗钢量(查表)
            double dblW1C2 = 0; //一级地区2类耗钢量
            double dblW1C2Calculate = 0; //一级地区2类耗钢量(计算)
            double dblW1C2FindTable = 0; //一级地区2类耗钢量(查表)
            double dblW2 = 0; //二级地区耗钢量
            double dblW2Calculate = 0; //二级地区耗钢量(计算)
            double dblW2FindTable = 0; //二级地区耗钢量(查表)
            double dblW3 = 0; //三级地区耗钢量
            double dblW3Calculate = 0; //三级地区耗钢量(计算)
            double dblW3FindTable = 0; //三级地区耗钢量(查表)
            double dblW4 = 0; //四级地区耗钢量
            double dblW4Calculate = 0; //四级地区耗钢量(计算)
            double dblW4FindTable = 0; //四级地区耗钢量(查表)

            double dblWPerL = 0; //单位长度重量
            double dblWPerLCalculate = 0; //单位长度重量(计算)
            double dblWPerLFindTable = 0; //单位长度重量(查表)
            int intIntensionAndRigidityCheck; //强度刚度校核选项

            Display();
                                              //API5L规范                                                  管材类型   API5L
            #region

            m_astrPipelineMaterialNameAPI5L[0] = "A25";
            m_adblMaterialYieldStressMinAPI5L[0] = 172;
            m_adblMaterialWeldingLineCoefficientAPI5L[0] = 1;
            m_astrPipelineMaterialNameAPI5L[1] = "A";
            m_adblMaterialYieldStressMinAPI5L[1] = 207;
            m_adblMaterialWeldingLineCoefficientAPI5L[1] = 1;
            m_astrPipelineMaterialNameAPI5L[2] = "B";
            m_adblMaterialYieldStressMinAPI5L[2] = 241;
            m_adblMaterialWeldingLineCoefficientAPI5L[2] = 1;
            m_astrPipelineMaterialNameAPI5L[3] = "X42";
            m_adblMaterialYieldStressMinAPI5L[3] = 289;
            m_adblMaterialWeldingLineCoefficientAPI5L[3] = 1;
            m_astrPipelineMaterialNameAPI5L[4] = "X46";
            m_adblMaterialYieldStressMinAPI5L[4] = 317;
            m_adblMaterialWeldingLineCoefficientAPI5L[4] = 1;
            m_astrPipelineMaterialNameAPI5L[5] = "X52";
            m_adblMaterialYieldStressMinAPI5L[5] = 358;
            m_adblMaterialWeldingLineCoefficientAPI5L[5] = 1;
            m_astrPipelineMaterialNameAPI5L[6] = "X56";
            m_adblMaterialYieldStressMinAPI5L[6] = 386;
            m_adblMaterialWeldingLineCoefficientAPI5L[6] = 1;
            m_astrPipelineMaterialNameAPI5L[7] = "X60";
            m_adblMaterialYieldStressMinAPI5L[7] = 413;
            m_adblMaterialWeldingLineCoefficientAPI5L[7] = 1;
            m_astrPipelineMaterialNameAPI5L[8] = "X65";
            m_adblMaterialYieldStressMinAPI5L[8] = 448;
            m_adblMaterialWeldingLineCoefficientAPI5L[8] = 1;
            m_astrPipelineMaterialNameAPI5L[9] = "X70";
            m_adblMaterialYieldStressMinAPI5L[9] = 482;
            m_adblMaterialWeldingLineCoefficientAPI5L[9] = 1;
            m_astrPipelineMaterialNameAPI5L[10] = "X80";
            m_adblMaterialYieldStressMinAPI5L[10] = 551;
            m_adblMaterialWeldingLineCoefficientAPI5L[10] = 1;

            #endregion

            int Num = SetPipelineMaterialIDByName(ComInput1.Text);


            //if (IsValidate())
            //{
            //用API5L规范求解液体管道耗钢量
            //对局部变量赋值
            dblP = Convert.ToDouble(txtInput1.Text);  //  绝对压力
                dblD = Convert.ToDouble(txtInput2.Text);   //  管线外直径
                dblL1C1 = Convert.ToDouble(txtInput3.Text);  // 一类地区1类长度
                dblL1C2 = Convert.ToDouble(txtInput4.Text);  // 一类地区2类长度
                dblL2 = Convert.ToDouble(txtInput5.Text);    // 二类地区
                dblL3 = Convert.ToDouble(txtInput6.Text);  // 三类地区
                dblL4 = Convert.ToDouble(txtInput7.Text);  // 四类地区
                dblT = Convert.ToDouble(txtInput8.Text);    // 管线设计温度

                //求解过程
                //**********************以下为核心代码**************************
                //按<输气管道工程设计规范>的要求转换单位制
                dblL1C1 = dblL1C1 / 1000; //(km)
                dblL1C2 = dblL1C2 / 1000; //(km)
                dblL2 = dblL2 / 1000; //(km)
                dblL3 = dblL3 / 1000; //(km)
                dblL4 = dblL4 / 1000; //(km)
                dblP = dblP / 1000000; //(MPa)
                dblD = dblD * 1000; //(mm)
             

                //强度刚度校核参数
                intIntensionAndRigidityCheck = m_intIntensionAndRigidityCheck;

                //开始计算
                dblL = dblL1C1 + dblL1C2 + dblL2 + dblL3 + dblL4;


               if (PipelineAnalyChecked5.Checked == true)
                {
                    //曾经加热大于等于300℃（焊接除外）的钢管许用应力
                    dblCgms = m_adblMaterialYieldStressMinAPI5L[Num] * 0.75;
                }
                else
                {
                    dblCgms = m_adblMaterialYieldStressMinAPI5L[Num];
                }

                //确定焊缝系数
                dblFai = 1;

                //设计系数
               
                        //天然气
                        //一级地区1类设计系数
                        dblF11 = 0.8;
                        //一级地区2类设计系数
                        dblF12 = 0.72;
                        //二级地区设计系数
                        dblF2 = 0.6;
                        //三级地区设计系数
                        dblF3 = 0.5;
                        //四级地区设计系数
                        dblF4 = 0.4;
                   

                //温度折减系数
                if (dblT > 120)
                {
                    //管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!
                    MessageBox.Show("管线设计温度>120℃，《输气管道工程设计规范》中不包含这种情况!", "提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //对象不能使用,修改对象标记
                    m_blnRefresh = true;
                    m_blnEnabled = false;
                    goto PROC_EXIT;
                }
                else
                {
                    dbltCoefficient = 1;
                }

                //确定壁厚的方法
                if (PipelineAnalyChecked1.Checked == true)
                {
                    //使用计算壁厚为所需壁厚
                    if (PipelineAnalyChecked2.Checked == true)
                    {
                        //同时显示用计算和查表方法计算的内容

                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                    
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C1 = dblDelta; //一级地区1类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚(显示提问)
                        if (!GetWallThicknessByAPI5L(true, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由API5L查到对应的壁厚，不能计算耗钢量
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C1Calculate = dblWPerLCalculate * dblL1C1 * 1000;
                        dblW1C1FindTable = dblWPerLFindTable * dblL1C1 * 1000;
            

                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                      
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByAPI5L(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由API5L查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                
                        //一级地区1类单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //一级地区1类总耗钢
                        dblW1C2Calculate = dblWPerLCalculate * dblL1C2 * 1000;
                        dblW1C2FindTable = dblWPerLFindTable * dblL1C2 * 1000;
                    


                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                      
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW2 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByAPI5L(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由API5L查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //二级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //二级地区总耗钢
                        dblW2Calculate = dblWPerLCalculate * dblL2 * 1000;
                        dblW2FindTable = dblWPerLFindTable * dblL2 * 1000;
   


                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                       
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW3= dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByAPI5L(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由API5L查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

          
                        //三级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //三级地区总耗钢
                        dblW3Calculate = dblWPerLCalculate * dblL3 * 1000;
                        dblW3FindTable = dblWPerLFindTable * dblL3 * 1000;
               


                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
     
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaCalculate = dblDelta; //计算壁厚
                        dblDeltaW4 = dblDelta; //一级地区2类壁厚
                        dblDFindTable = dblD;
                        dblDeltaFindTable = dblDelta;

                        //查表确定壁厚
                        if (!GetWallThicknessByAPI5L(false, dblDFindTable, dblDeltaFindTable))
                        {
                            //不能由API5L查到对应的壁厚，不能计算耗钢量
                            if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //当前壁厚为所需壁厚
                            }
                            else
                            {
                                //对象不能使用,修改对象标记
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDeltaCalculate))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }

                            //查表壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }

                       
                        //四级地区单位长度耗钢量
                        dblWPerLCalculate = System.Convert.ToDouble(0.0246615 * (dblD - dblDeltaCalculate) * dblDeltaCalculate);
                        dblWPerLFindTable = System.Convert.ToDouble(0.0246615 * (dblDFindTable - dblDeltaFindTable) * dblDeltaFindTable);
                        //四级地区总耗钢
                        dblW4Calculate = dblWPerLCalculate * dblL4 * 1000;
                        dblW4FindTable = dblWPerLFindTable * dblL4 * 1000;


                        //用于显示的各部分壁厚
                        txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                        txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                        txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                        txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                        txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚


                        //用于显示的各部分耗钢量
                        dblW1C1 = dblW1C1Calculate; //一级地区1类耗钢量
                        dblW1C2 = dblW1C2Calculate; //一级地区2类耗钢量
                        dblW2 = dblW2Calculate; //2级地区耗钢量
                        dblW3 = dblW3Calculate; //3级地区耗钢量
                        dblW4 = dblW4Calculate; //4级地区耗钢量
                    }
                    else
                    {
                        //仅显示计算壁厚为所需壁厚
                        //一级地区1类的许用应力
                        dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区1类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C1= dblDelta; //一级地区2类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }
                        //一级地区1类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区1类总耗钢
                        dblW1C1 = dblWPerL * dblL1C1 * 1000;
                    

                        //一级地区2类的许用应力
                        dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                        //一级地区2类的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }
                        //一级地区2类单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //一级地区2类总耗钢
                        dblW1C2 = dblWPerL * dblL1C2 * 1000;
            

                        //二级地区的许用应力
                        dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                        //二级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW2 = dblDelta; //一级地区2类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }
                        //二级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //二级地区总耗钢
                        dblW2 = dblWPerL * dblL2 * 1000;
                   
                        //三级地区的许用应力
                        dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                        //三级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW3 = dblDelta; //一级地区2类壁厚
                        //强度刚度校核 
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }
                        //三级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //三级地区总耗钢
                        dblW3 = dblWPerL * dblL3 * 1000;
                      

                        //四级地区的许用应力
                        dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                        //四级地区的壁厚
                        dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                        //修正到最接近的0.1
                        dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                        dblDeltaW4 = dblDelta; //一级地区2类壁厚
                        //强度刚度校核
                        if (PipelineAnalyChecked3.Checked == true)
                        {
                            //计算壁厚
                            //最小壁厚校核
                            if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                            {
                                m_blnRefresh = true;
                                m_blnEnabled = false;
                                goto PROC_EXIT;
                            }
                        }
                        //四级地区单位长度耗钢量
                        dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                        //四级总耗钢
                        dblW4 = dblWPerL * dblL4 * 1000;
                 
                    }
                }
                else
                {
                    //直接采用查规范的方法确定壁厚

                    //一级地区1类的许用应力
                    dblCgm = dblF11 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区1类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW1C1 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByAPI5L(true, dblD, dblDelta))
                    {
                        //不能由API5L查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //一级地区1类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区1类总耗钢
                    dblW1C1 = dblWPerL * dblL1C1 * 1000;
            

                    //一级地区2类的许用应力
                    dblCgm = dblF12 * dblCgms * dblFai * dbltCoefficient;
                    //一级地区2类的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW1C2 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByAPI5L(false, dblD, dblDelta))
                    {
                        //不能由API5L查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblDFindTable, dblDeltaFindTable))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }
                    //一级地区2类单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //一级地区2类总耗钢
                    dblW1C2 = dblWPerL * dblL1C2 * 1000;
                  

                    //二级地区的许用应力
                    dblCgm = dblF2 * dblCgms * dblFai * dbltCoefficient;
                    //二级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW2 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByAPI5L(false, dblD, dblDelta))
                    {
                        //不能由API5L查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //二级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //二级地区总耗钢
                    dblW2 = dblWPerL * dblL2 * 1000;
                

                    //三级地区的许用应力
                    dblCgm = dblF3 * dblCgms * dblFai * dbltCoefficient;
                    //三级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW3 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByAPI5L(false, dblD, dblDelta))
                    {
                        //不能由API5L查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //三级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //三级地区总耗钢
                    dblW3 = dblWPerL * dblL3 * 1000;
                  

                    //四级地区的许用应力
                    dblCgm = dblF4 * dblCgms * dblFai * dbltCoefficient;
                    //四级地区的壁厚
                    dblDelta = System.Convert.ToDouble((dblP * dblD) / (2 * dblCgm));
                    //修正到最接近的0.1
                    dblDelta = System.Convert.ToDouble(((10 * dblDelta + 1)) / 10);
                    dblDeltaW4 = dblDelta; //一级地区2类壁厚
                    //查表确定壁厚
                    if (!GetWallThicknessByAPI5L(false, dblD, dblDelta))
                    {
                        //不能由API5L查到对应的壁厚，不能计算耗钢量
                        if (MessageBox.Show("不能由API5L查到对应的壁厚，是否直接用计算结果确定耗钢量!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //当前壁厚为所需壁厚
                        }
                        else
                        {
                            //对象不能使用,修改对象标记
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //强度刚度校核
                    if (PipelineAnalyChecked3.Checked == true)
                    {
                        //查表壁厚
                        //最小壁厚校核
                        if (!IsLogicalGasPiplineMinWallThickness(dblD, dblDelta))
                        {
                            m_blnRefresh = true;
                            m_blnEnabled = false;
                            goto PROC_EXIT;
                        }
                    }

                    //四级地区单位长度耗钢量
                    dblWPerL = System.Convert.ToDouble(0.0246615 * (dblD - dblDelta) * dblDelta);
                    //四级总耗钢
                    dblW4 = dblWPerL * dblL4 * 1000;
               
                }

                //按<输油管道工程设计规范>的要求恢复单位制到国际单位
                dblL = dblL * 1000;
                dblW = dblW1C1 + dblW1C2 + dblW2 + dblW3 + dblW4;

                //**********************以上为核心代码**************************
                txtOutput1.Text = dblL.ToString();       //管线总长度
                txtOutput2.Text = dblDeltaW1C1.ToString(); //一级地区1类壁厚
                txtOutput3.Text = dblDeltaW1C1.ToString(); //一级地区2类壁厚
                txtOutput4.Text = dblDeltaW1C1.ToString(); //二级地区壁厚
                txtOutput5.Text = dblDeltaW1C1.ToString(); //三级地区壁厚
                txtOutput6.Text = dblDeltaW1C1.ToString(); //四级地区壁厚

                txtOutput7.Text = dblW1C1.ToString(); //一级地区1类耗钢量
                txtOutput8.Text = dblW1C2.ToString(); //一级地区2类耗钢量
                txtOutput9.Text = dblW2.ToString(); //2级地区耗钢量
                txtOutput10.Text = dblW3.ToString(); //3级地区耗钢量
                txtOutput11.Text = dblW4.ToString(); //4级地区耗钢量
                txtOutput12.Text = dblW.ToString(); //总耗钢量


                //修改对象标记
                m_blnRefresh = true;
                m_blnEnabled = true;
            //}
            //else
            //{
            //    //对象不能使用,修改对象标记
            //    m_blnRefresh = true;
            //    m_blnEnabled = false;
            //}

        PROC_EXIT:
            return;

    
            MessageBox.Show(/*Information.Err().Number, Information.Err().Description,*/ "CPipelineEconomyConsumeSteel", "CalculateStyleGasPipelineByAPI5L()");
            goto PROC_EXIT;

        }
        private bool IsLogicalGasPiplineMinWallThickness(double dblExternalDiameter, double dblWallThickness)
        {
            bool returnValue = false;
            //作用：气体管道的最小壁厚校核
            //接受的参数：
            //输入参数：dblExternalDiameter--管道外直径(m)
            //          dblWallThickness--壁厚(m)
            //输出参数：无
            //返回值：无
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            bool blnFindDiameter = false; //找到管径
            const int c_intPipelineDiameterCount = 27; //直径总数
            double dblD; //管道外直径(m)
            double dblDelta; //管道的公称壁厚(m)
            double[,] dblWallThicknessMin = new double[28, 2]; //最小壁厚系列(第一列为公称直径，第二列为最小壁厚)
            int intDiameterID = 0; //管径序号
            int intI = 0; //循环变量
            

            //考察输入各变量是否合理
            //外径
            if (dblExternalDiameter < 0)
            {
                //您输入的外径<0，不能进行液体管道的强度较核
                MessageBox.Show("您输入的外径<0，不能进行液体管道的强度较核!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else if (dblExternalDiameter == 0)
            {
                //您输入的外径=0，不能进行液体管道的强度较核
                MessageBox.Show("您输入的外径=0，不能进行液体管道的强度较核!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else
            {
                //内径符合要求
            }

            //壁厚
            if (dblWallThickness < 0)
            {
                //您输入的管道壁厚<0，不能进行液体管道的强度较核
                MessageBox.Show("您输入的管道壁厚<0，不能进行液体管道的强度较核!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else if (dblWallThickness == 0)
            {
                //您输入的管道壁厚=0，不能进行液体管道的强度较核
                MessageBox.Show("您输入的管道壁厚<0，不能进行液体管道的强度较核!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else
            {
                //管道壁厚符合要求
            }

            //开始最小壁厚校核
            //对各变量赋值
            dblD = dblExternalDiameter;
            dblDelta = dblWallThickness;

            dblWallThicknessMin[0, 0] = 100;
            dblWallThicknessMin[0, 1] = 2.5;

            dblWallThicknessMin[1, 0] = 150;
            dblWallThicknessMin[1, 1] = 2.5;

            dblWallThicknessMin[2, 0] = 200;
            dblWallThicknessMin[2, 1] = 3.5;

            dblWallThicknessMin[3, 0] = 250;
            dblWallThicknessMin[3, 1] = 4;

            dblWallThicknessMin[4, 0] = 300;
            dblWallThicknessMin[4, 1] = 4.5;

            dblWallThicknessMin[5, 0] = 350;
            dblWallThicknessMin[5, 1] = 5;                  //代码  这一项有错

            dblWallThicknessMin[6, 0] = 400;
            dblWallThicknessMin[6, 1] = 5;

            dblWallThicknessMin[7, 0] = 450;
            dblWallThicknessMin[7, 1] = 5;

            dblWallThicknessMin[8, 0] = 500;
            dblWallThicknessMin[8, 1] = 6;

            dblWallThicknessMin[9, 0] = 550;
            dblWallThicknessMin[9, 1] = 6;

            dblWallThicknessMin[10, 0] = 600;
            dblWallThicknessMin[10, 1] = 6.5;

            dblWallThicknessMin[11, 0] = 650;
            dblWallThicknessMin[11, 1] = 6.5;

            dblWallThicknessMin[12, 0] = 700;
            dblWallThicknessMin[12, 1] = 6.5;

            dblWallThicknessMin[13, 0] = 750;
            dblWallThicknessMin[13, 1] = 6.5;

            dblWallThicknessMin[14, 0] = 800;
            dblWallThicknessMin[14, 1] = 6.5;

            dblWallThicknessMin[15, 0] = 850;
            dblWallThicknessMin[15, 1] = 6.5;

            dblWallThicknessMin[16, 0] = 900;
            dblWallThicknessMin[16, 1] = 6.5;

            dblWallThicknessMin[17, 0] = 950;
            dblWallThicknessMin[17, 1] = 8;

            dblWallThicknessMin[18, 0] = 1000;
            dblWallThicknessMin[18, 1] = 8;

            dblWallThicknessMin[19, 0] = 1050;
            dblWallThicknessMin[19, 1] = 9;

            dblWallThicknessMin[20, 0] = 1110;
            dblWallThicknessMin[20, 1] = 9;

            dblWallThicknessMin[21, 0] = 1150;
            dblWallThicknessMin[21, 1] = 9;

            dblWallThicknessMin[22, 0] = 1200;
            dblWallThicknessMin[22, 1] = 9;

            dblWallThicknessMin[23, 0] = 1300;
            dblWallThicknessMin[23, 1] = 11.5;

            dblWallThicknessMin[24, 0] = 1400;
            dblWallThicknessMin[24, 1] = 11.5;

            dblWallThicknessMin[25, 0] = 1500;
            dblWallThicknessMin[25, 1] = 13;

            dblWallThicknessMin[26, 0] = 1600;
            dblWallThicknessMin[26, 1] = 13;

            //最小壁厚校核
            if (dblExternalDiameter < dblWallThicknessMin[0, 0])
            {
                //管径小于最小壁厚系列中的管径最小值
                MessageBox.Show("管径小于最小壁厚系列中的管径最小值,无法确定壁厚是否在最小壁厚范围内!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else if (dblExternalDiameter > dblWallThicknessMin[c_intPipelineDiameterCount - 1, 0])
            {
                //管径大于最小壁厚系列中的管径最大值
                MessageBox.Show("管径大于最小壁厚系列中的管径最大值,无法确定壁厚是否在最小壁厚范围内!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else if (dblWallThicknessMin[0, 0] < dblExternalDiameter)
            {
                //管径为最小壁厚系列中的最小值
                if (dblWallThicknessMin[0, 1] > dblWallThickness)
                {
                    //气体管道的壁厚过小
                    MessageBox.Show("气体管道的壁厚过小!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                    goto PROC_EXIT;
                }
                else
                {
                    //气体管道的壁厚合适
                    returnValue = true;
                    goto PROC_EXIT;
                }
            }
            else
            {
                //管径在最小壁厚系列内
                blnFindDiameter = false;
                intDiameterID = -1;
                for (intI = 1; intI <= c_intPipelineDiameterCount - 1; intI++)
                {
                    if (dblWallThicknessMin[intI - 1, 0] < dblExternalDiameter && dblExternalDiameter <= dblWallThicknessMin[intI, 0])
                    {
                        blnFindDiameter = true;
                        intDiameterID = intI;
                        break;
                    }
                }

                if (blnFindDiameter)
                {
                    //找到对应壁厚
                    if (dblWallThicknessMin[intDiameterID, 1] > dblWallThickness)
                    {
                        //气体管道的壁厚过小
                        MessageBox.Show("气体管道的壁厚过小!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        returnValue = false;
                        goto PROC_EXIT;
                    }
                    else
                    {
                        //气体管道的壁厚合适
                        returnValue = true;
                        goto PROC_EXIT;
                    }
                }
                else
                {
                    //在最小壁厚系列中找不到对应的管径（正常情况下不可能）
                    MessageBox.Show("在最小壁厚系列中找不到对应的管径（正常情况下不可能）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                    goto PROC_EXIT;
                }
            }

            //通过强度较核
            returnValue = true;

        PROC_EXIT:
            return returnValue;

        }
        private bool GetWallThicknessByGB1997(bool blnShowQuestion, double dblExternalDiameter, double dblWallThickness)
        {
            bool returnValue = false;
            //作用：由GB1997获取管壁厚度
            //接受的参数：
            //输入参数：blnShowQuestion--是否显示问题(true--显示问题，false--不显示问题)
            //          dblWallThickness --管线壁厚
            //          dblExternalDiameter --管线外直径
            //输出参数：dblWallThickness--管线壁厚
            //返回值：获取成功后返回True,返回失败返回False
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            const int c_intPipelineDiameterGB1997 = 36; //管线外径系列数GB1997
            const int c_intPipelineWallThicknessMaxGB1997 = 26; //管线壁厚系列数最大值GB1997
            const int c_dblWallThicknessError = -1; //错误的壁厚
            double[] adblPipelineDiameterGB1997 = new double[c_intPipelineDiameterGB1997 - 1 + 1]; //直径系列数据（mm）
            double[,] adblPipelineDiameterWallThicknessGB1997 = new double[c_intPipelineDiameterGB1997 - 1 + 1, c_intPipelineWallThicknessMaxGB1997 - 1 + 1]; //直径,壁厚数据(行-直径，列-壁厚)
            double intI = 0; //循环变量
            double intJ = 0; //循环变量
            bool blnFindDiameter = false; // 找到管径
            bool blnFindWallThickness = false; //找到壁厚
            int intDiameterID = 0; //管径序号
            int intWallThicknessMaxID = 0; //最大壁厚的位置

            //初始化函数返回值
            returnValue = false;

            //对GB1997系列赋值
            //清空数组内容
            for (intI = 0; intI <= c_intPipelineDiameterGB1997 - 1; intI++)
            {
                adblPipelineDiameterGB1997[(int)intI] = c_dblWallThicknessError;
                for (intJ = 0; intJ <= c_intPipelineWallThicknessMaxGB1997 - 1; intJ++)
                {
                    adblPipelineDiameterWallThicknessGB1997[(int)intI, (int)intJ] = c_dblWallThicknessError;
                }
            }

            //GB1997壁厚系列(mm)

            adblPipelineDiameterGB1997[0] = 60.3;
            adblPipelineDiameterWallThicknessGB1997[0, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[0, 1] = 2.8;
            adblPipelineDiameterWallThicknessGB1997[0, 2] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[0, 3] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[0, 4] = 3.9;
            adblPipelineDiameterWallThicknessGB1997[0, 5] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[0, 6] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[0, 7] = 5.5;
            adblPipelineDiameterWallThicknessGB1997[0, 8] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[0, 9] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[0, 10] = 11.1;

            adblPipelineDiameterGB1997[1] = 73;
            adblPipelineDiameterWallThicknessGB1997[1, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[1, 1] = 2.8;
            adblPipelineDiameterWallThicknessGB1997[1, 2] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[1, 3] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[1, 4] = 4;
            adblPipelineDiameterWallThicknessGB1997[1, 5] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[1, 6] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[1, 7] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[1, 8] = 5.5;
            adblPipelineDiameterWallThicknessGB1997[1, 9] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[1, 10] = 7;
            adblPipelineDiameterWallThicknessGB1997[1, 11] = 14;

            adblPipelineDiameterGB1997[2] = 88.9;
            adblPipelineDiameterWallThicknessGB1997[2, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[2, 1] = 2.8;
            adblPipelineDiameterWallThicknessGB1997[2, 2] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[2, 3] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[2, 4] = 4;
            adblPipelineDiameterWallThicknessGB1997[2, 5] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[2, 6] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[2, 7] = 5.5;
            adblPipelineDiameterWallThicknessGB1997[2, 8] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[2, 9] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[2, 10] = 7.6;
            adblPipelineDiameterWallThicknessGB1997[2, 11] = 15.2;

            adblPipelineDiameterGB1997[3] = 101.6;
            adblPipelineDiameterWallThicknessGB1997[3, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[3, 1] = 2.8;
            adblPipelineDiameterWallThicknessGB1997[3, 2] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[3, 3] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[3, 4] = 4;
            adblPipelineDiameterWallThicknessGB1997[3, 5] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[3, 6] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[3, 7] = 5.7;
            adblPipelineDiameterWallThicknessGB1997[3, 8] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[3, 9] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[3, 10] = 8.1;

            adblPipelineDiameterGB1997[4] = 114.3;
            adblPipelineDiameterWallThicknessGB1997[4, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[4, 1] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[4, 2] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[4, 3] = 4;
            adblPipelineDiameterWallThicknessGB1997[4, 4] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[4, 5] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[4, 6] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[4, 7] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[4, 8] = 6;
            adblPipelineDiameterWallThicknessGB1997[4, 9] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[4, 10] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[4, 11] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[4, 12] = 8.6;
            adblPipelineDiameterWallThicknessGB1997[4, 13] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[4, 14] = 13.5;
            adblPipelineDiameterWallThicknessGB1997[4, 15] = 17.1;

            adblPipelineDiameterGB1997[5] = 141.3;
            adblPipelineDiameterWallThicknessGB1997[5, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[5, 1] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[5, 2] = 4;
            adblPipelineDiameterWallThicknessGB1997[5, 3] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[5, 4] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[5, 5] = 6.6;
            adblPipelineDiameterWallThicknessGB1997[5, 6] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[5, 7] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[5, 8] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[5, 9] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[5, 10] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[5, 11] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[5, 12] = 19.1;

            adblPipelineDiameterGB1997[6] = 168.3;
            adblPipelineDiameterWallThicknessGB1997[6, 0] = 2.1;
            adblPipelineDiameterWallThicknessGB1997[6, 1] = 2.8;
            adblPipelineDiameterWallThicknessGB1997[6, 2] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[6, 3] = 3.6;
            adblPipelineDiameterWallThicknessGB1997[6, 4] = 4;
            adblPipelineDiameterWallThicknessGB1997[6, 5] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[6, 6] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[6, 7] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[6, 8] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[6, 9] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[6, 10] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[6, 11] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[6, 12] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[6, 13] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[6, 14] = 11;
            adblPipelineDiameterWallThicknessGB1997[6, 15] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[6, 16] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[6, 17] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[6, 18] = 18.3;
            adblPipelineDiameterWallThicknessGB1997[6, 19] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[6, 20] = 22.2;

            adblPipelineDiameterGB1997[7] = 219.1;
            adblPipelineDiameterWallThicknessGB1997[7, 0] = 3.2;
            adblPipelineDiameterWallThicknessGB1997[7, 1] = 4;
            adblPipelineDiameterWallThicknessGB1997[7, 2] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[7, 3] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[7, 4] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[7, 5] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[7, 6] = 7;
            adblPipelineDiameterWallThicknessGB1997[7, 7] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[7, 8] = 8.2;
            adblPipelineDiameterWallThicknessGB1997[7, 9] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[7, 10] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[7, 11] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[7, 12] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[7, 13] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[7, 14] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[7, 15] = 18.3;
            adblPipelineDiameterWallThicknessGB1997[7, 16] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[7, 17] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[7, 18] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[7, 19] = 25.4;

            adblPipelineDiameterGB1997[8] = 273.1;
            adblPipelineDiameterWallThicknessGB1997[8, 0] = 4;
            adblPipelineDiameterWallThicknessGB1997[8, 1] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[8, 2] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[8, 3] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[8, 4] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[8, 5] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[8, 6] = 7.8;
            adblPipelineDiameterWallThicknessGB1997[8, 7] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[8, 8] = 9.3;
            adblPipelineDiameterWallThicknessGB1997[8, 9] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[8, 10] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[8, 11] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[8, 12] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[8, 13] = 18.3;
            adblPipelineDiameterWallThicknessGB1997[8, 14] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[8, 15] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[8, 16] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[8, 17] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[8, 18] = 31.8;

            adblPipelineDiameterGB1997[9] = 323.9;
            adblPipelineDiameterWallThicknessGB1997[9, 0] = 4.4;
            adblPipelineDiameterWallThicknessGB1997[9, 1] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[9, 2] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[9, 3] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[9, 4] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[9, 5] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[9, 6] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[9, 7] = 8.4;
            adblPipelineDiameterWallThicknessGB1997[9, 8] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[9, 9] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[9, 10] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[9, 11] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[9, 12] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[9, 13] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[9, 14] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[9, 15] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[9, 16] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[9, 17] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[9, 18] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[9, 19] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[9, 20] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[9, 21] = 27;
            adblPipelineDiameterWallThicknessGB1997[9, 22] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[9, 23] = 31.8;

            adblPipelineDiameterGB1997[10] = 355.6;
            adblPipelineDiameterWallThicknessGB1997[10, 0] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[10, 1] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[10, 2] = 5.3;
            adblPipelineDiameterWallThicknessGB1997[10, 3] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[10, 4] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[10, 5] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[10, 6] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[10, 7] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[10, 8] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[10, 9] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[10, 10] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[10, 11] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[10, 12] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[10, 13] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[10, 14] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[10, 15] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[10, 16] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[10, 17] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[10, 18] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[10, 19] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[10, 20] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[10, 21] = 27;
            adblPipelineDiameterWallThicknessGB1997[10, 22] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[10, 23] = 31.8;

            adblPipelineDiameterGB1997[11] = 406.4;
            adblPipelineDiameterWallThicknessGB1997[11, 0] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[11, 1] = 5.2;
            adblPipelineDiameterWallThicknessGB1997[11, 2] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[11, 3] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[11, 4] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[11, 5] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[11, 6] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[11, 7] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[11, 8] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[11, 9] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[11, 10] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[11, 11] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[11, 12] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[11, 13] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[11, 14] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[11, 15] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[11, 16] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[11, 17] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[11, 18] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[11, 19] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[11, 20] = 27;
            adblPipelineDiameterWallThicknessGB1997[11, 21] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[11, 22] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[11, 23] = 31.8;

            adblPipelineDiameterGB1997[12] = 457;
            adblPipelineDiameterWallThicknessGB1997[12, 0] = 4.8;
            adblPipelineDiameterWallThicknessGB1997[12, 1] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[12, 2] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[12, 3] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[12, 4] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[12, 5] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[12, 6] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[12, 7] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[12, 8] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[12, 9] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[12, 10] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[12, 11] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[12, 12] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[12, 13] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[12, 14] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[12, 15] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[12, 16] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[12, 17] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[12, 18] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[12, 19] = 27;
            adblPipelineDiameterWallThicknessGB1997[12, 20] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[12, 21] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[12, 22] = 31.8;

            adblPipelineDiameterGB1997[13] = 508;
            adblPipelineDiameterWallThicknessGB1997[13, 0] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[13, 1] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[13, 2] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[13, 3] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[13, 4] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[13, 5] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[13, 6] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[13, 7] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[13, 8] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[13, 9] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[13, 10] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[13, 11] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[13, 12] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[13, 13] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[13, 14] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[13, 15] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[13, 16] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[13, 17] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[13, 18] = 27;
            adblPipelineDiameterWallThicknessGB1997[13, 19] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[13, 20] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[13, 21] = 31.8;
            adblPipelineDiameterWallThicknessGB1997[13, 22] = 33.3;
            adblPipelineDiameterWallThicknessGB1997[13, 23] = 34.9;

            adblPipelineDiameterGB1997[14] = 559;
            adblPipelineDiameterWallThicknessGB1997[14, 0] = 5.6;
            adblPipelineDiameterWallThicknessGB1997[14, 1] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[14, 2] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[14, 3] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[14, 4] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[14, 5] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[14, 6] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[14, 7] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[14, 8] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[14, 9] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[14, 10] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[14, 11] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[14, 12] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[14, 13] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[14, 14] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[14, 15] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[14, 16] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[14, 17] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[14, 18] = 27;
            adblPipelineDiameterWallThicknessGB1997[14, 19] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[14, 20] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[14, 21] = 31.8;
            adblPipelineDiameterWallThicknessGB1997[14, 22] = 33.3;
            adblPipelineDiameterWallThicknessGB1997[14, 23] = 34.9;
            adblPipelineDiameterWallThicknessGB1997[14, 24] = 36.5;
            adblPipelineDiameterWallThicknessGB1997[14, 25] = 38.1;

            adblPipelineDiameterGB1997[15] = 610;
            adblPipelineDiameterWallThicknessGB1997[15, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[15, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[15, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[15, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[15, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[15, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[15, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[15, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[15, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[15, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[15, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[15, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[15, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[15, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[15, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[15, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[15, 16] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[15, 17] = 27;
            adblPipelineDiameterWallThicknessGB1997[15, 18] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[15, 19] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[15, 20] = 31.8;
            adblPipelineDiameterWallThicknessGB1997[15, 21] = 33.3;
            adblPipelineDiameterWallThicknessGB1997[15, 22] = 34.9;
            adblPipelineDiameterWallThicknessGB1997[15, 23] = 36.5;
            adblPipelineDiameterWallThicknessGB1997[15, 24] = 38.1;
            adblPipelineDiameterWallThicknessGB1997[15, 25] = 39.7;

            adblPipelineDiameterGB1997[16] = 660;
            adblPipelineDiameterWallThicknessGB1997[16, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[16, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[16, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[16, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[16, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[16, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[16, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[16, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[16, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[16, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[16, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[16, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[16, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[16, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[16, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[16, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[16, 16] = 25.4;

            adblPipelineDiameterGB1997[17] = 711;
            adblPipelineDiameterWallThicknessGB1997[17, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[17, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[17, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[17, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[17, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[17, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[17, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[17, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[17, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[17, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[17, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[17, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[17, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[17, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[17, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[17, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[17, 16] = 25.4;

            adblPipelineDiameterGB1997[18] = 762;
            adblPipelineDiameterWallThicknessGB1997[18, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[18, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[18, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[18, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[18, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[18, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[18, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[18, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[18, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[18, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[18, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[18, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[18, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[18, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[18, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[18, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[18, 16] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[18, 17] = 27;
            adblPipelineDiameterWallThicknessGB1997[18, 18] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[18, 19] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[18, 20] = 31.8;

            adblPipelineDiameterGB1997[19] = 813;
            adblPipelineDiameterWallThicknessGB1997[19, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[19, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[19, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[19, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[19, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[19, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[19, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[19, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[19, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[19, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[19, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[19, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[19, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[19, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[19, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[19, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[19, 16] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[19, 17] = 27;
            adblPipelineDiameterWallThicknessGB1997[19, 18] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[19, 19] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[19, 20] = 31.8;

            adblPipelineDiameterGB1997[20] = 864;
            adblPipelineDiameterWallThicknessGB1997[20, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[20, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[20, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[20, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[20, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[20, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[20, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[20, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[20, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[20, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[20, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[20, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[20, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[20, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[20, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[20, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[20, 16] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[20, 17] = 27;
            adblPipelineDiameterWallThicknessGB1997[20, 18] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[20, 19] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[20, 20] = 31.8;

            adblPipelineDiameterGB1997[21] = 914;
            adblPipelineDiameterWallThicknessGB1997[21, 0] = 6.4;
            adblPipelineDiameterWallThicknessGB1997[21, 1] = 7.1;
            adblPipelineDiameterWallThicknessGB1997[21, 2] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[21, 3] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[21, 4] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[21, 5] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[21, 6] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[21, 7] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[21, 8] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[21, 9] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[21, 10] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[21, 11] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[21, 12] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[21, 13] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[21, 14] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[21, 15] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[21, 16] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[21, 17] = 27;
            adblPipelineDiameterWallThicknessGB1997[21, 18] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[21, 19] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[21, 20] = 31.8;

            adblPipelineDiameterGB1997[22] = 965;
            adblPipelineDiameterWallThicknessGB1997[22, 0] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[22, 1] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[22, 2] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[22, 3] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[22, 4] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[22, 5] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[22, 6] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[22, 7] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[22, 8] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[22, 9] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[22, 10] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[22, 11] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[22, 12] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[22, 13] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[22, 14] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[22, 15] = 27;
            adblPipelineDiameterWallThicknessGB1997[22, 16] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[22, 17] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[22, 18] = 31.8;

            adblPipelineDiameterGB1997[23] = 1016;
            adblPipelineDiameterWallThicknessGB1997[23, 0] = 7.9;
            adblPipelineDiameterWallThicknessGB1997[23, 1] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[23, 2] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[23, 3] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[23, 4] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[23, 5] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[23, 6] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[23, 7] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[23, 8] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[23, 9] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[23, 10] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[23, 11] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[23, 12] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[23, 13] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[23, 14] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[23, 15] = 27;
            adblPipelineDiameterWallThicknessGB1997[23, 16] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[23, 17] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[23, 18] = 31.8;

            adblPipelineDiameterGB1997[24] = 1067;
            adblPipelineDiameterWallThicknessGB1997[24, 0] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[24, 1] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[24, 2] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[24, 3] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[24, 4] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[24, 5] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[24, 6] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[24, 7] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[24, 8] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[24, 9] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[24, 10] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[24, 11] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[24, 12] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[24, 13] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[24, 14] = 27;
            adblPipelineDiameterWallThicknessGB1997[24, 15] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[24, 16] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[24, 17] = 31.8;

            adblPipelineDiameterGB1997[25] = 1118;
            adblPipelineDiameterWallThicknessGB1997[25, 0] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[25, 1] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[25, 2] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[25, 3] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[25, 4] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[25, 5] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[25, 6] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[25, 7] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[25, 8] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[25, 9] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[25, 10] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[25, 11] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[25, 12] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[25, 13] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[25, 14] = 27;
            adblPipelineDiameterWallThicknessGB1997[25, 15] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[25, 16] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[25, 17] = 31.8;

            adblPipelineDiameterGB1997[26] = 1168;
            adblPipelineDiameterWallThicknessGB1997[26, 0] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[26, 1] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[26, 2] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[26, 3] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[26, 4] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[26, 5] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[26, 6] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[26, 7] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[26, 8] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[26, 9] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[26, 10] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[26, 11] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[26, 12] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[26, 13] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[26, 14] = 27;
            adblPipelineDiameterWallThicknessGB1997[26, 15] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[26, 16] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[26, 17] = 31.8;

            adblPipelineDiameterGB1997[27] = 1219;
            adblPipelineDiameterWallThicknessGB1997[27, 0] = 8.7;
            adblPipelineDiameterWallThicknessGB1997[27, 1] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[27, 2] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[27, 3] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[27, 4] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[27, 5] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[27, 6] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[27, 7] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[27, 8] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[27, 9] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[27, 10] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[27, 11] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[27, 12] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[27, 13] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[27, 14] = 27;
            adblPipelineDiameterWallThicknessGB1997[27, 15] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[27, 16] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[27, 17] = 31.8;

            adblPipelineDiameterGB1997[28] = 1321;
            adblPipelineDiameterWallThicknessGB1997[28, 0] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[28, 1] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[28, 2] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[28, 3] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[28, 4] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[28, 5] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[28, 6] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[28, 7] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[28, 8] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[28, 9] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[28, 10] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[28, 11] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[28, 12] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[28, 13] = 27;
            adblPipelineDiameterWallThicknessGB1997[28, 14] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[28, 15] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[28, 16] = 31.8;

            adblPipelineDiameterGB1997[29] = 1422;
            adblPipelineDiameterWallThicknessGB1997[29, 0] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[29, 1] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[29, 2] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[29, 3] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[29, 4] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[29, 5] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[29, 6] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[29, 7] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[29, 8] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[29, 9] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[29, 10] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[29, 11] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[29, 12] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[29, 13] = 27;
            adblPipelineDiameterWallThicknessGB1997[29, 14] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[29, 15] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[29, 16] = 31.8;

            adblPipelineDiameterGB1997[30] = 1524;
            adblPipelineDiameterWallThicknessGB1997[30, 0] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[30, 1] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[30, 2] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[30, 3] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[30, 4] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[30, 5] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[30, 6] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[30, 7] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[30, 8] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[30, 9] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[30, 10] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[30, 11] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[30, 12] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[30, 13] = 27;
            adblPipelineDiameterWallThicknessGB1997[30, 14] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[30, 15] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[30, 16] = 31.8;

            adblPipelineDiameterGB1997[31] = 1626;
            adblPipelineDiameterWallThicknessGB1997[31, 0] = 9.5;
            adblPipelineDiameterWallThicknessGB1997[31, 1] = 10.3;
            adblPipelineDiameterWallThicknessGB1997[31, 2] = 11.1;
            adblPipelineDiameterWallThicknessGB1997[31, 3] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[31, 4] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[31, 5] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[31, 6] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[31, 7] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[31, 8] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[31, 9] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[31, 10] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[31, 11] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[31, 12] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[31, 13] = 27;
            adblPipelineDiameterWallThicknessGB1997[31, 14] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[31, 15] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[31, 16] = 31.8;

            adblPipelineDiameterGB1997[32] = 1727;
            adblPipelineDiameterWallThicknessGB1997[32, 0] = 11.9;
            adblPipelineDiameterWallThicknessGB1997[32, 1] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[32, 2] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[32, 3] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[32, 4] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[32, 5] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[32, 6] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[32, 7] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[32, 8] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[32, 9] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[32, 10] = 27;
            adblPipelineDiameterWallThicknessGB1997[32, 11] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[32, 12] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[32, 13] = 31.8;

            adblPipelineDiameterGB1997[33] = 1829;
            adblPipelineDiameterWallThicknessGB1997[33, 0] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[33, 1] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[33, 2] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[33, 3] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[33, 4] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[33, 5] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[33, 6] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[33, 7] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[33, 8] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[33, 9] = 27;
            adblPipelineDiameterWallThicknessGB1997[33, 10] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[33, 11] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[33, 12] = 31.8;

            adblPipelineDiameterGB1997[34] = 1930;
            adblPipelineDiameterWallThicknessGB1997[34, 0] = 12.7;
            adblPipelineDiameterWallThicknessGB1997[34, 1] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[34, 2] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[34, 3] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[34, 4] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[34, 5] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[34, 6] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[34, 7] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[34, 8] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[34, 9] = 27;
            adblPipelineDiameterWallThicknessGB1997[34, 10] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[34, 11] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[34, 12] = 31.8;

            adblPipelineDiameterGB1997[35] = 2032;
            adblPipelineDiameterWallThicknessGB1997[35, 0] = 14.3;
            adblPipelineDiameterWallThicknessGB1997[35, 1] = 15.9;
            adblPipelineDiameterWallThicknessGB1997[35, 2] = 17.5;
            adblPipelineDiameterWallThicknessGB1997[35, 3] = 19.1;
            adblPipelineDiameterWallThicknessGB1997[35, 4] = 20.6;
            adblPipelineDiameterWallThicknessGB1997[35, 5] = 22.2;
            adblPipelineDiameterWallThicknessGB1997[35, 6] = 23.8;
            adblPipelineDiameterWallThicknessGB1997[35, 7] = 25.4;
            adblPipelineDiameterWallThicknessGB1997[35, 8] = 27;
            adblPipelineDiameterWallThicknessGB1997[35, 9] = 28.6;
            adblPipelineDiameterWallThicknessGB1997[35, 10] = 30.2;
            adblPipelineDiameterWallThicknessGB1997[35, 11] = 31.8;

            //按输入的直径和壁厚查找壁厚
            if (dblExternalDiameter < adblPipelineDiameterGB1997[0] || dblExternalDiameter > adblPipelineDiameterGB1997[c_intPipelineDiameterGB1997 - 1])
            {
                //您传入的管外径超过GB1997规范的范围，不能确定管壁厚度
                MessageBox.Show("您传入的管外径超过GB1997规范的范围，不能确定管壁厚度!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                returnValue = false;
                goto PROC_EXIT;
            }
            else
            {
                //管径达到范围内
                blnFindDiameter = false;
                for (intI = 0; intI <= c_intPipelineDiameterGB1997 - 1; intI++)
                {
                    if (dblExternalDiameter == adblPipelineDiameterGB1997[(int)intI])
                    {
                        //找到对应的管径
                        blnFindDiameter = true;
                        intDiameterID = (int)intI;
                        break;
                    }
                }

                if (blnFindDiameter)
                {
                    //找到对应的管径
                }
                else
                {
                    //未找到对应的管径
                    if (blnShowQuestion)
                    {
                        //您输入的管外径不是GB1997中的标准系列，是否继续计算（取上限）？
                        if (MessageBox .Show("您输入的管外径不是GB1997中的标准系列，是否继续计算（取上限）" + "？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //继续计算，查找合理的管径
                            blnFindDiameter = false;
                            for (intI = 1; intI <= c_intPipelineDiameterGB1997 - 1; intI++)
                            {
                                if (dblExternalDiameter > adblPipelineDiameterGB1997[(int)intI - 1] && dblExternalDiameter <= adblPipelineDiameterGB1997[(int)intI])
                                {
                                    //找到对应的管径，退出
                                    blnFindDiameter = true;
                                    intDiameterID = (int)intI;
                                    m_blnChangeDiameter = true; //改变管径
                                    m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterGB1997[(int)intI] / 1000); //化为国际单位
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //找不到合理的管径，不能继续计算
                            dblWallThickness = c_dblWallThicknessError;
                            returnValue = false;
                            goto PROC_EXIT;
                        }
                    }
                    else
                    {
                        blnFindDiameter = false;
                        for (intI = 1; intI <= c_intPipelineDiameterGB1997 - 1; intI++)
                        {
                            if (dblExternalDiameter > adblPipelineDiameterGB1997[(int)intI - 1] && dblExternalDiameter <= adblPipelineDiameterGB1997[(int)intI])
                            {
                                //找到对应的管径，退出
                                blnFindDiameter = true;
                                intDiameterID = (int)intI;
                                m_blnChangeDiameter = true; //改变管径
                                m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterGB1997[(int)intI] / 1000); //化为国际单位
                                break;
                            }
                        }
                    }
                }

                //按传入的壁厚在规范中查找对应的壁厚
                //查找最大壁厚的位置
                intWallThicknessMaxID = -1;
                for (intJ = 0; intJ <= c_intPipelineWallThicknessMaxGB1997 - 1; intJ++)
                {
                    if (adblPipelineDiameterWallThicknessGB1997[intDiameterID, (int)intJ] < 0)
                    {
                        intWallThicknessMaxID = (int)(intJ - 1);
                        break;
                    }
                }

                if (intWallThicknessMaxID < 0)
                {
                    //可能一个都没有找到（所有的数据都大于0），此时用壁厚最大值序号用壁厚数组的最大列数
                    intWallThicknessMaxID = c_intPipelineWallThicknessMaxGB1997 - 1;
                }

                blnFindWallThickness = false;
                if (dblWallThickness <= adblPipelineDiameterWallThicknessGB1997[intDiameterID, 0])
                {
                    //采用最小壁厚
                    dblWallThickness = adblPipelineDiameterWallThicknessGB1997[intDiameterID, 0];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness == adblPipelineDiameterWallThicknessGB1997[intDiameterID, intWallThicknessMaxID])
                {
                    //找到对应壁厚
                    dblWallThickness = adblPipelineDiameterWallThicknessGB1997[intDiameterID, intWallThicknessMaxID];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness > adblPipelineDiameterWallThicknessGB1997[intDiameterID, intWallThicknessMaxID])
                {
                    //您所需的壁厚超过规范中的范围，不能获的壁厚!
                    MessageBox.Show("您所需的壁厚超过规范中的范围，不能获得壁厚!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //dblWallThickness = c_dblWallThicknessError
                    //用户认为查找壁厚失败时仍能计算结果，故查找失败时直接返回传入的参数值
                    dblWallThickness = dblWallThickness;
                    blnFindWallThickness = false;
                    returnValue = false;
                    goto PROC_EXIT;
                }
                else
                {
                    for (intJ = 1; intJ <= intWallThicknessMaxID; intJ++)
                    {
                        if (adblPipelineDiameterWallThicknessGB1997[intDiameterID, (int)intJ - 1] < dblWallThickness && dblWallThickness <= adblPipelineDiameterWallThicknessGB1997[intDiameterID, (int)intJ])
                        {
                            //找到对应的壁厚（取大值）
                            dblWallThickness = adblPipelineDiameterWallThicknessGB1997[intDiameterID, (int)intJ];
                            blnFindWallThickness = true;
                            break;
                        }
                    }

                    if (!blnFindWallThickness)
                    {
                        //找不到合理的壁厚（正常情况下不可能，请检查程序）!
                        MessageBox.Show("找不到合理的管径（正常情况下不可能，请检查程序）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dblWallThickness = c_dblWallThicknessError;
                        returnValue = false;
                        goto PROC_EXIT;
                    }
                }
                //返回函数值
                returnValue = true;

            }

        PROC_EXIT:
            return returnValue;

 
        }
        private bool GetWallThicknessByGB1999(bool blnShowQuestion, double dblExternalDiameter, double dblWallThickness)
        {
            bool returnValue = false;
            //作用：由GB1999获取管壁厚度
            //接受的参数：
            //输入参数：blnShowQuestion--是否显示问题(true--显示问题，false--不显示问题)
            //          dblWallThickness --管线壁厚
            //          dblExternalDiameter --管线外直径
            //输出参数：dblWallThickness--管线壁厚
            //返回值：获取成功后返回True,返回失败返回False
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            const int c_intPipelineWallThicknessCountGB1999 = 27; //管线壁厚数GB1999总数
            const int c_intPipelineDiameterCountGB1999 = 31; //管线直径总数GB1999
            const int c_dblWallThicknessError = -1; //错误的壁厚
            double[] adblPipelineWallThicknessGB1999 = new double[c_intPipelineWallThicknessCountGB1999 - 1 + 1]; //(mm)
            double[,] adblPipelineDiameterSeries = new double[c_intPipelineDiameterCountGB1999 - 1 + 1, 3]; //管线系列（管外经，最小壁厚，最大壁厚）(mm)
            double intI = 0; //循环变量
            double intJ = 0; //循环变量
            bool blnFindDiameter = false; // 找到管径
            bool blnFindWallThickness = false; //找到壁厚
            int intDiameterID = 0; //管径序号
            int intWallThicknessMaxID; //最大壁厚的位置

            //初始化函数返回值
            returnValue = false;

            //对GB1999系列赋值
            //清空数组内容
            for (intI = 0; intI <= c_intPipelineWallThicknessCountGB1999 - 1; intI++)
            {
                adblPipelineWallThicknessGB1999[(int)intI] = c_dblWallThicknessError;
            }

            for (intI = 0; intI <= c_intPipelineDiameterCountGB1999 - 1; intI++)
            {
                adblPipelineDiameterSeries[(int)intI, 0] = c_dblWallThicknessError;
                adblPipelineDiameterSeries[(int)intI, 1] = c_dblWallThicknessError;
                adblPipelineDiameterSeries[(int)intI, 2] = c_dblWallThicknessError;
            }

            //GB1999壁厚系列(mm)
            adblPipelineWallThicknessGB1999[0] = 2.3;
            adblPipelineWallThicknessGB1999[1] = 2.6;
            adblPipelineWallThicknessGB1999[2] = 2.9;
            adblPipelineWallThicknessGB1999[3] = 3.2;
            adblPipelineWallThicknessGB1999[4] = 3.6;
            adblPipelineWallThicknessGB1999[5] = 4;
            adblPipelineWallThicknessGB1999[6] = 4.5;
            adblPipelineWallThicknessGB1999[7] = 5;
            adblPipelineWallThicknessGB1999[8] = 5.6;
            adblPipelineWallThicknessGB1999[9] = 6.3;
            adblPipelineWallThicknessGB1999[10] = 7.1;
            adblPipelineWallThicknessGB1999[11] = 8;
            adblPipelineWallThicknessGB1999[12] = 8.8;
            adblPipelineWallThicknessGB1999[13] = 10;
            adblPipelineWallThicknessGB1999[14] = 11;
            adblPipelineWallThicknessGB1999[15] = 12.5;
            adblPipelineWallThicknessGB1999[16] = 14.2;
            adblPipelineWallThicknessGB1999[17] = 16;
            adblPipelineWallThicknessGB1999[18] = 17.5;
            adblPipelineWallThicknessGB1999[19] = 20;
            adblPipelineWallThicknessGB1999[20] = 22.5;
            adblPipelineWallThicknessGB1999[21] = 25;
            adblPipelineWallThicknessGB1999[22] = 28;
            adblPipelineWallThicknessGB1999[23] = 30;
            adblPipelineWallThicknessGB1999[24] = 32;
            adblPipelineWallThicknessGB1999[25] = 36;
            adblPipelineWallThicknessGB1999[26] = 40;

            //管线系列（管外经，最小壁厚，最大壁厚）
            adblPipelineDiameterSeries[0, 0] = 33.7;
            adblPipelineDiameterSeries[0, 1] = 2.3;
            adblPipelineDiameterSeries[0, 2] = 10;

            adblPipelineDiameterSeries[1, 0] = 42.4;
            adblPipelineDiameterSeries[1, 1] = 2.3;
            adblPipelineDiameterSeries[1, 2] = 10;

            adblPipelineDiameterSeries[2, 0] = 48.3;
            adblPipelineDiameterSeries[2, 1] = 2.3;
            adblPipelineDiameterSeries[2, 2] = 12.5;

            adblPipelineDiameterSeries[3, 0] = 60.3;
            adblPipelineDiameterSeries[3, 1] = 2.3;
            adblPipelineDiameterSeries[3, 2] = 14.2;

            adblPipelineDiameterSeries[4, 0] = 88.9;
            adblPipelineDiameterSeries[4, 1] = 2.3;
            adblPipelineDiameterSeries[4, 2] = 20;

            adblPipelineDiameterSeries[5, 0] = 114.3;
            adblPipelineDiameterSeries[5, 1] = 2.3;
            adblPipelineDiameterSeries[5, 2] = 20;

            adblPipelineDiameterSeries[6, 0] = 168.3;
            adblPipelineDiameterSeries[6, 1] = 2.9;
            adblPipelineDiameterSeries[6, 2] = 40;

            adblPipelineDiameterSeries[7, 0] = 219.1;
            adblPipelineDiameterSeries[7, 1] = 3.2;
            adblPipelineDiameterSeries[7, 2] = 40;

            adblPipelineDiameterSeries[8, 0] = 273;
            adblPipelineDiameterSeries[8, 1] = 3.6;
            adblPipelineDiameterSeries[8, 2] = 40;

            adblPipelineDiameterSeries[9, 0] = 323.9;
            adblPipelineDiameterSeries[9, 1] = 4;
            adblPipelineDiameterSeries[9, 2] = 40;

            adblPipelineDiameterSeries[10, 0] = 355.6;
            adblPipelineDiameterSeries[10, 1] = 4.5;
            adblPipelineDiameterSeries[10, 2] = 40;

            adblPipelineDiameterSeries[11, 0] = 406.4;
            adblPipelineDiameterSeries[11, 1] = 4.5;
            adblPipelineDiameterSeries[11, 2] = 40;

            adblPipelineDiameterSeries[12, 0] = 457;
            adblPipelineDiameterSeries[12, 1] = 5;
            adblPipelineDiameterSeries[12, 2] = 40;

            adblPipelineDiameterSeries[13, 0] = 508;
            adblPipelineDiameterSeries[13, 1] = 5.6;
            adblPipelineDiameterSeries[13, 2] = 40;

            adblPipelineDiameterSeries[14, 0] = 559;
            adblPipelineDiameterSeries[14, 1] = 5.6;
            adblPipelineDiameterSeries[14, 2] = 40;

            adblPipelineDiameterSeries[15, 0] = 610;
            adblPipelineDiameterSeries[15, 1] = 5.6;
            adblPipelineDiameterSeries[15, 2] = 40;

            adblPipelineDiameterSeries[16, 0] = 660;
            adblPipelineDiameterSeries[16, 1] = 6.3;
            adblPipelineDiameterSeries[16, 2] = 40;

            adblPipelineDiameterSeries[17, 0] = 711;
            adblPipelineDiameterSeries[17, 1] = 6.3;
            adblPipelineDiameterSeries[17, 2] = 36;

            adblPipelineDiameterSeries[18, 0] = 762;
            adblPipelineDiameterSeries[18, 1] = 6.3;
            adblPipelineDiameterSeries[18, 2] = 36;

            adblPipelineDiameterSeries[19, 0] = 813;
            adblPipelineDiameterSeries[19, 1] = 6.3;
            adblPipelineDiameterSeries[19, 2] = 36;

            adblPipelineDiameterSeries[20, 0] = 864;
            adblPipelineDiameterSeries[20, 1] = 2.3;
            adblPipelineDiameterSeries[20, 2] = 10;

            adblPipelineDiameterSeries[21, 0] = 914;
            adblPipelineDiameterSeries[21, 1] = 6.3;
            adblPipelineDiameterSeries[21, 2] = 36;

            adblPipelineDiameterSeries[22, 0] = 1016;
            adblPipelineDiameterSeries[22, 1] = 8;
            adblPipelineDiameterSeries[22, 2] = 36;

            adblPipelineDiameterSeries[23, 0] = 1067;
            adblPipelineDiameterSeries[23, 1] = 8;
            adblPipelineDiameterSeries[23, 2] = 36;

            adblPipelineDiameterSeries[24, 0] = 1118;
            adblPipelineDiameterSeries[24, 1] = 8.8;
            adblPipelineDiameterSeries[24, 2] = 36;

            adblPipelineDiameterSeries[25, 0] = 1168;
            adblPipelineDiameterSeries[25, 1] = 8.8;
            adblPipelineDiameterSeries[25, 2] = 36;

            adblPipelineDiameterSeries[26, 0] = 1219;
            adblPipelineDiameterSeries[26, 1] = 8.8;
            adblPipelineDiameterSeries[26, 2] = 36;

            adblPipelineDiameterSeries[27, 0] = 1321;
            adblPipelineDiameterSeries[27, 1] = 8.8;
            adblPipelineDiameterSeries[27, 2] = 36;

            adblPipelineDiameterSeries[28, 0] = 1422;
            adblPipelineDiameterSeries[28, 1] = 8.8;
            adblPipelineDiameterSeries[28, 2] = 36;

            adblPipelineDiameterSeries[29, 0] = 1524;
            adblPipelineDiameterSeries[29, 1] = 8.8;
            adblPipelineDiameterSeries[29, 2] = 36;

            adblPipelineDiameterSeries[30, 0] = 1626;
            adblPipelineDiameterSeries[30, 1] = 8.8;
            adblPipelineDiameterSeries[30, 2] = 36;

            //按输入的直径和壁厚查找壁厚
            if (dblExternalDiameter < adblPipelineDiameterSeries[0, 0] || dblExternalDiameter > adblPipelineDiameterSeries[c_intPipelineDiameterCountGB1999 - 1, 0])
            {
                //您传入的管外径超过GB1999规范的范围，不能确定管壁厚度
                MessageBox.Show("您传入的管外径超过GB1999规范的范围，不能确定管壁厚度!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else
            {
                //管径达到范围内
                blnFindDiameter = false;
                for (intI = 0; intI <= c_intPipelineDiameterCountGB1999 - 1; intI++)
                {
                    if (dblExternalDiameter == adblPipelineDiameterSeries[(int)intI, 0])
                    {
                        //找到对应的管径
                        blnFindDiameter = true;
                        intDiameterID = (int)intI;
                        break;
                    }
                }

                if (blnFindDiameter)
                {
                    //找到对应的管径
                }
                else
                {
                    //未找到对应的管径
                    if (blnShowQuestion)
                    {
                        //您输入的管外径不是GB1999中的标准系列，是否继续计算（取上限）？
                        if (MessageBox.Show("您输入的管外径不是GB1999中的标准系列，是否继续计算（取上限）" + "？",  "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //继续计算，查找合理的管径
                            blnFindDiameter = false;
                            for (intI = 1; intI <= c_intPipelineDiameterCountGB1999 - 1; intI++)
                            {
                                if (dblExternalDiameter > adblPipelineDiameterSeries[(int)intI - 1, 0] && dblExternalDiameter <= adblPipelineDiameterSeries[(int)intI, 0])
                                {
                                    //找到对应的管径，退出
                                    blnFindDiameter = true;
                                    intDiameterID = (int)intI;
                                    m_blnChangeDiameter = true; //改变管径
                                    m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterSeries[(int)intI, 0] / 1000); //化为国际单位
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //找不到合理的管径，不能继续计算
                            dblWallThickness = c_dblWallThicknessError;
                            returnValue = false;
                            goto PROC_EXIT;
                        }
                    }
                    else
                    {
                        blnFindDiameter = false;
                        for (intI = 1; intI <= c_intPipelineDiameterCountGB1999 - 1; intI++)
                        {
                            if (dblExternalDiameter > adblPipelineDiameterSeries[(int)intI - 1, 0] && dblExternalDiameter <= adblPipelineDiameterSeries[(int)intI, 0])
                            {
                                //找到对应的管径，退出
                                blnFindDiameter = true;
                                intDiameterID = (int)intI;
                                m_blnChangeDiameter = true; //改变管径
                                m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterSeries[(int)intI, 0] / 1000); //化为国际单位
                                break;
                            }
                        }
                    }
                }

                //按传入的壁厚在规范中查找对应的壁厚
                blnFindWallThickness = false;
                if (dblWallThickness <= adblPipelineWallThicknessGB1999[0])
                {
                    //采用最小壁厚
                    dblWallThickness = adblPipelineWallThicknessGB1999[0];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness == adblPipelineWallThicknessGB1999[c_intPipelineWallThicknessCountGB1999 - 1])
                {
                    //找到对应壁厚
                    dblWallThickness = adblPipelineWallThicknessGB1999[c_intPipelineWallThicknessCountGB1999 - 1];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness > adblPipelineWallThicknessGB1999[c_intPipelineWallThicknessCountGB1999 - 1])
                {
                    //您所需的壁厚超过规范中的范围，不能获的壁厚!
                    MessageBox.Show("您所需的壁厚超过规范中的范围，不能获得壁厚!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //dblWallThickness = c_dblWallThicknessError
                    //用户认为查找壁厚失败时仍能计算结果，故查找失败时直接返回传入的参数值
                    dblWallThickness = dblWallThickness;
                    blnFindWallThickness = false;
                    returnValue = false;
                    goto PROC_EXIT;
                }
                else
                {
                    for (intJ = 1; intJ <= c_intPipelineWallThicknessCountGB1999 - 1; intJ++)
                    {
                        if (adblPipelineWallThicknessGB1999[(int )intJ - 1] < dblWallThickness && dblWallThickness <= adblPipelineWallThicknessGB1999[(int)intJ])
                        {
                            //找到对应的壁厚（取大值）
                            dblWallThickness = adblPipelineWallThicknessGB1999[(int)intJ];
                            blnFindWallThickness = true;
                            break;
                        }
                    }

                    if (!blnFindWallThickness)
                    {
                        //找不到合理的壁厚（正常情况下不可能，请检查程序）!
                        MessageBox.Show("找不到合理的管径（正常情况下不可能，请检查程序）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dblWallThickness = c_dblWallThicknessError;
                        returnValue = false;
                        goto PROC_EXIT;
                    }
                }

                //检查是否在推荐的范围内
                if (adblPipelineDiameterSeries[intDiameterID, 1] <= dblWallThickness && dblWallThickness <= adblPipelineDiameterSeries[intDiameterID, 2])
                {
                    //选定的壁厚在优先选用的范围内
                }
                else if (adblPipelineDiameterSeries[intDiameterID, 1] > dblWallThickness)
                {
                    //选定的管径小于优先选用的最小壁厚
                    if (blnShowQuestion)
                    {
                        //您输入的管外径不是GB1999中优先选用的标准系列，是否继续计算？
                        if (MessageBox.Show("您输入的管外径不是GB1999中优先选用的标准系列，是否继续计算" + "？",  "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //继续计算
                            dblWallThickness = adblPipelineDiameterSeries[intDiameterID, 1];
                        }
                        else
                        {
                            //找不到合理的壁厚，不能继续计算
                            dblWallThickness = c_dblWallThicknessError;
                            returnValue = false;
                            goto PROC_EXIT;
                        }
                    }
                    else
                    {
                        //继续计算
                        dblWallThickness = adblPipelineDiameterSeries[intDiameterID, 1];
                    }
                }
                else
                {
                    //选定的管径大于优先选用的最大壁厚，取实际壁厚
                    if (blnShowQuestion)
                    {
                        //您输入的管外径不是GB1999中优先选用的标准系列，是否继续计算？
                        if (MessageBox.Show("您输入的管外径不是GB1999中优先选用的标准系列，是否继续计算" + "？",  "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //继续计算
                            dblWallThickness = dblWallThickness;
                        }
                        else
                        {
                            //找不到合理的壁厚，不能继续计算
                            dblWallThickness = c_dblWallThicknessError;
                            returnValue = false;
                            goto PROC_EXIT;
                        }
                    }
                    else
                    {
                        //继续计算
                        dblWallThickness = dblWallThickness;
                    }
                }

                //返回函数值
                returnValue = true;

            }

        PROC_EXIT:
            return returnValue;

        }
        private bool GetWallThicknessByAPI5L(bool blnShowQuestion, double dblExternalDiameter, double dblWallThickness)
        {
            bool returnValue = false;
            //作用：由API5L获取管壁厚度
            //接受的参数：
            //输入参数：blnShowQuestion--是否显示问题(true--显示问题，false--不显示问题)
            //          dblWallThickness --管线壁厚
            //          dblExternalDiameter --管线外直径
            //输出参数：dblWallThickness--管线壁厚
            //返回值：获取成功后返回True,返回失败返回False
            //说明:无
            //On Error Goto PROC_ERR VBConversions Warning: could not be converted to try/catch - logic too complex
            const int c_intPipelineDiameterAPI5L = 44; //管线外径系列数API5L
            const int c_intPipelineWallThicknessMaxAPI5L = 26; //管线壁厚系列数最大值API5L
            const int c_dblWallThicknessError = -1; //错误的壁厚
            double[] adblPipelineDiameterAPI5L = new double[c_intPipelineDiameterAPI5L - 1 + 1]; //直径系列数据（mm）
            double[,] adblPipelineDiameterWallThicknessAPI5L = new double[c_intPipelineDiameterAPI5L - 1 + 1, c_intPipelineWallThicknessMaxAPI5L - 1 + 1]; //直径,壁厚数据(行-直径，列-壁厚)
            double intI = 0; //循环变量
            double intJ = 0; //循环变量
            bool blnFindDiameter = false; // 找到管径
            bool blnFindWallThickness = false; //找到壁厚
            int intDiameterID = 0; //管径序号
            int intWallThicknessMaxID = 0; //最大壁厚的位置

            //初始化函数返回值
            returnValue = false;

            //对API5L系列赋值
            //清空数组内容
            for (intI = 0; intI <= c_intPipelineDiameterAPI5L - 1; intI++)
            {
                adblPipelineDiameterAPI5L[(int)intI] = c_dblWallThicknessError;
                for (intJ = 0; intJ <= c_intPipelineWallThicknessMaxAPI5L - 1; intJ++)
                {
                    adblPipelineDiameterWallThicknessAPI5L[(int)intI, (int)intJ] = c_dblWallThicknessError;
                }
            }

            //API5L壁厚系列(mm)
            adblPipelineDiameterAPI5L[0] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[0, 0] = 1.7;
            adblPipelineDiameterWallThicknessAPI5L[0, 1] = 2.4;

            adblPipelineDiameterAPI5L[1] = 13.7;
            adblPipelineDiameterWallThicknessAPI5L[1, 0] = 2.2;
            adblPipelineDiameterWallThicknessAPI5L[1, 1] = 3;

            adblPipelineDiameterAPI5L[2] = 17.1;
            adblPipelineDiameterWallThicknessAPI5L[2, 0] = 2.3;
            adblPipelineDiameterWallThicknessAPI5L[2, 1] = 3.2;

            adblPipelineDiameterAPI5L[3] = 21.3;
            adblPipelineDiameterWallThicknessAPI5L[3, 0] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[3, 1] = 3.7;
            adblPipelineDiameterWallThicknessAPI5L[3, 2] = 7.5;

            adblPipelineDiameterAPI5L[4] = 26.7;
            adblPipelineDiameterWallThicknessAPI5L[4, 0] = 2.9;
            adblPipelineDiameterWallThicknessAPI5L[4, 1] = 3.9;
            adblPipelineDiameterWallThicknessAPI5L[4, 2] = 7.8;

            adblPipelineDiameterAPI5L[5] = 33.4;
            adblPipelineDiameterWallThicknessAPI5L[5, 0] = 3.4;
            adblPipelineDiameterWallThicknessAPI5L[5, 1] = 4.5;
            adblPipelineDiameterWallThicknessAPI5L[5, 2] = 9.1;

            adblPipelineDiameterAPI5L[6] = 42.2;
            adblPipelineDiameterWallThicknessAPI5L[6, 0] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[6, 1] = 4.9;
            adblPipelineDiameterWallThicknessAPI5L[6, 2] = 9.7;

            adblPipelineDiameterAPI5L[7] = 48.3;
            adblPipelineDiameterWallThicknessAPI5L[7, 0] = 3.7;
            adblPipelineDiameterWallThicknessAPI5L[7, 1] = 5.1;
            adblPipelineDiameterWallThicknessAPI5L[7, 2] = 10.2;

            adblPipelineDiameterAPI5L[8] = 60.3;
            adblPipelineDiameterWallThicknessAPI5L[8, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[8, 1] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[8, 2] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[8, 3] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[8, 4] = 3.9;
            adblPipelineDiameterWallThicknessAPI5L[8, 5] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[8, 6] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[8, 7] = 5.5;
            adblPipelineDiameterWallThicknessAPI5L[8, 8] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[8, 9] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[8, 10] = 11.1;

            adblPipelineDiameterAPI5L[9] = 73;
            adblPipelineDiameterWallThicknessAPI5L[9, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[9, 1] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[9, 2] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[9, 3] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[9, 4] = 4;
            adblPipelineDiameterWallThicknessAPI5L[9, 5] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[9, 6] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[9, 7] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[9, 8] = 5.5;
            adblPipelineDiameterWallThicknessAPI5L[9, 9] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[9, 10] = 7;
            adblPipelineDiameterWallThicknessAPI5L[9, 11] = 14;

            adblPipelineDiameterAPI5L[10] = 88.9;
            adblPipelineDiameterWallThicknessAPI5L[10, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[10, 1] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[10, 2] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[10, 3] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[10, 4] = 4;
            adblPipelineDiameterWallThicknessAPI5L[10, 5] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[10, 6] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[10, 7] = 5.5;
            adblPipelineDiameterWallThicknessAPI5L[10, 8] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[10, 9] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[10, 10] = 7.6;
            adblPipelineDiameterWallThicknessAPI5L[10, 11] = 15.2;

            adblPipelineDiameterAPI5L[11] = 101.6;
            adblPipelineDiameterWallThicknessAPI5L[11, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[11, 1] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[11, 2] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[11, 3] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[11, 4] = 4;
            adblPipelineDiameterWallThicknessAPI5L[11, 5] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[11, 6] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[11, 7] = 5.7;
            adblPipelineDiameterWallThicknessAPI5L[11, 8] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[11, 9] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[11, 10] = 8.1;

            adblPipelineDiameterAPI5L[12] = 114.3;
            adblPipelineDiameterWallThicknessAPI5L[12, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[12, 1] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[12, 2] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[12, 3] = 4;
            adblPipelineDiameterWallThicknessAPI5L[12, 4] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[12, 5] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[12, 6] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[12, 7] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[12, 8] = 6;
            adblPipelineDiameterWallThicknessAPI5L[12, 9] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[12, 10] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[12, 11] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[12, 12] = 8.6;
            adblPipelineDiameterWallThicknessAPI5L[12, 13] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[12, 14] = 13.5;
            adblPipelineDiameterWallThicknessAPI5L[12, 15] = 17.1;

            adblPipelineDiameterAPI5L[13] = 141.3;
            adblPipelineDiameterWallThicknessAPI5L[13, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[13, 1] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[13, 2] = 4;
            adblPipelineDiameterWallThicknessAPI5L[13, 3] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[13, 4] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[13, 5] = 6.6;
            adblPipelineDiameterWallThicknessAPI5L[13, 6] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[13, 7] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[13, 8] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[13, 9] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[13, 10] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[13, 11] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[13, 12] = 19.1;

            adblPipelineDiameterAPI5L[14] = 168.3;
            adblPipelineDiameterWallThicknessAPI5L[14, 0] = 2.1;
            adblPipelineDiameterWallThicknessAPI5L[14, 1] = 2.8;
            adblPipelineDiameterWallThicknessAPI5L[14, 2] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[14, 3] = 3.6;
            adblPipelineDiameterWallThicknessAPI5L[14, 4] = 4;
            adblPipelineDiameterWallThicknessAPI5L[14, 5] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[14, 6] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[14, 7] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[14, 8] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[14, 9] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[14, 10] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[14, 11] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[14, 12] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[14, 13] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[14, 14] = 11;
            adblPipelineDiameterWallThicknessAPI5L[14, 15] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[14, 16] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[14, 17] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[14, 18] = 18.3;
            adblPipelineDiameterWallThicknessAPI5L[14, 19] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[14, 20] = 22.2;

            adblPipelineDiameterAPI5L[15] = 219.1;
            adblPipelineDiameterWallThicknessAPI5L[15, 0] = 3.2;
            adblPipelineDiameterWallThicknessAPI5L[15, 1] = 4;
            adblPipelineDiameterWallThicknessAPI5L[15, 2] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[15, 3] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[15, 4] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[15, 5] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[15, 6] = 7;
            adblPipelineDiameterWallThicknessAPI5L[15, 7] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[15, 8] = 8.2;
            adblPipelineDiameterWallThicknessAPI5L[15, 9] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[15, 10] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[15, 11] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[15, 12] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[15, 13] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[15, 14] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[15, 15] = 18.3;
            adblPipelineDiameterWallThicknessAPI5L[15, 16] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[15, 17] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[15, 18] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[15, 19] = 25.4;

            adblPipelineDiameterAPI5L[16] = 273.1;
            adblPipelineDiameterWallThicknessAPI5L[16, 0] = 4;
            adblPipelineDiameterWallThicknessAPI5L[16, 1] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[16, 2] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[16, 3] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[16, 4] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[16, 5] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[16, 6] = 7.8;
            adblPipelineDiameterWallThicknessAPI5L[16, 7] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[16, 8] = 9.3;
            adblPipelineDiameterWallThicknessAPI5L[16, 9] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[16, 10] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[16, 11] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[16, 12] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[16, 13] = 18.3;
            adblPipelineDiameterWallThicknessAPI5L[16, 14] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[16, 15] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[16, 16] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[16, 17] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[16, 18] = 31.8;

            adblPipelineDiameterAPI5L[17] = 323.9;
            adblPipelineDiameterWallThicknessAPI5L[17, 0] = 4.4;
            adblPipelineDiameterWallThicknessAPI5L[17, 1] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[17, 2] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[17, 3] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[17, 4] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[17, 5] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[17, 6] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[17, 7] = 8.4;
            adblPipelineDiameterWallThicknessAPI5L[17, 8] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[17, 9] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[17, 10] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[17, 11] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[17, 12] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[17, 13] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[17, 14] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[17, 15] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[17, 16] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[17, 17] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[17, 18] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[17, 19] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[17, 20] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[17, 21] = 27;
            adblPipelineDiameterWallThicknessAPI5L[17, 22] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[17, 23] = 31.8;

            adblPipelineDiameterAPI5L[18] = 355.6;
            adblPipelineDiameterWallThicknessAPI5L[18, 0] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[18, 1] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[18, 2] = 5.3;
            adblPipelineDiameterWallThicknessAPI5L[18, 3] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[18, 4] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[18, 5] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[18, 6] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[18, 7] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[18, 8] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[18, 9] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[18, 10] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[18, 11] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[18, 12] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[18, 13] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[18, 14] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[18, 15] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[18, 16] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[18, 17] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[18, 18] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[18, 19] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[18, 20] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[18, 21] = 27;
            adblPipelineDiameterWallThicknessAPI5L[18, 22] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[18, 23] = 31.8;

            adblPipelineDiameterAPI5L[19] = 406.4;
            adblPipelineDiameterWallThicknessAPI5L[19, 0] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[19, 1] = 5.2;
            adblPipelineDiameterWallThicknessAPI5L[19, 2] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[19, 3] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[19, 4] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[19, 5] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[19, 6] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[19, 7] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[19, 8] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[19, 9] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[19, 10] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[19, 11] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[19, 12] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[19, 13] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[19, 14] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[19, 15] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[19, 16] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[19, 17] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[19, 18] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[19, 19] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[19, 20] = 27;
            adblPipelineDiameterWallThicknessAPI5L[19, 21] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[19, 22] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[19, 23] = 31.8;

            adblPipelineDiameterAPI5L[20] = 457;
            adblPipelineDiameterWallThicknessAPI5L[20, 0] = 4.8;
            adblPipelineDiameterWallThicknessAPI5L[20, 1] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[20, 2] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[20, 3] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[20, 4] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[20, 5] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[20, 6] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[20, 7] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[20, 8] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[20, 9] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[20, 10] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[20, 11] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[20, 12] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[20, 13] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[20, 14] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[20, 15] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[20, 16] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[20, 17] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[20, 18] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[20, 19] = 27;
            adblPipelineDiameterWallThicknessAPI5L[20, 20] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[20, 21] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[20, 22] = 31.8;

            adblPipelineDiameterAPI5L[21] = 508;
            adblPipelineDiameterWallThicknessAPI5L[21, 0] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[21, 1] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[21, 2] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[21, 3] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[21, 4] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[21, 5] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[21, 6] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[21, 7] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[21, 8] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[21, 9] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[21, 10] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[21, 11] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[21, 12] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[21, 13] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[21, 14] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[21, 15] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[21, 16] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[21, 17] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[21, 18] = 27;
            adblPipelineDiameterWallThicknessAPI5L[21, 19] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[21, 20] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[21, 21] = 31.8;
            adblPipelineDiameterWallThicknessAPI5L[21, 22] = 33.3;
            adblPipelineDiameterWallThicknessAPI5L[21, 23] = 34.9;

            adblPipelineDiameterAPI5L[22] = 559;
            adblPipelineDiameterWallThicknessAPI5L[22, 0] = 5.6;
            adblPipelineDiameterWallThicknessAPI5L[22, 1] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[22, 2] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[22, 3] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[22, 4] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[22, 5] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[22, 6] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[22, 7] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[22, 8] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[22, 9] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[22, 10] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[22, 11] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[22, 12] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[22, 13] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[22, 14] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[22, 15] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[22, 16] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[22, 17] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[22, 18] = 27;
            adblPipelineDiameterWallThicknessAPI5L[22, 19] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[22, 20] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[22, 21] = 31.8;
            adblPipelineDiameterWallThicknessAPI5L[22, 22] = 33.3;
            adblPipelineDiameterWallThicknessAPI5L[22, 23] = 34.9;
            adblPipelineDiameterWallThicknessAPI5L[22, 24] = 36.5;
            adblPipelineDiameterWallThicknessAPI5L[22, 25] = 38.1;

            adblPipelineDiameterAPI5L[23] = 610;
            adblPipelineDiameterWallThicknessAPI5L[23, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[23, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[23, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[23, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[23, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[23, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[23, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[23, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[23, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[23, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[23, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[23, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[23, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[23, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[23, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[23, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[23, 16] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[23, 17] = 27;
            adblPipelineDiameterWallThicknessAPI5L[23, 18] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[23, 19] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[23, 20] = 31.8;
            adblPipelineDiameterWallThicknessAPI5L[23, 21] = 33.3;
            adblPipelineDiameterWallThicknessAPI5L[23, 22] = 34.9;
            adblPipelineDiameterWallThicknessAPI5L[23, 23] = 36.5;
            adblPipelineDiameterWallThicknessAPI5L[23, 24] = 38.1;
            adblPipelineDiameterWallThicknessAPI5L[23, 25] = 39.7;

            adblPipelineDiameterAPI5L[24] = 660;
            adblPipelineDiameterWallThicknessAPI5L[24, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[24, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[24, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[24, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[24, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[24, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[24, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[24, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[24, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[24, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[24, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[24, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[24, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[24, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[24, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[24, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[24, 16] = 25.4;

            adblPipelineDiameterAPI5L[25] = 711;
            adblPipelineDiameterWallThicknessAPI5L[25, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[25, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[25, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[25, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[25, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[25, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[25, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[25, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[25, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[25, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[25, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[25, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[25, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[25, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[25, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[25, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[25, 16] = 25.4;

            adblPipelineDiameterAPI5L[26] = 762;
            adblPipelineDiameterWallThicknessAPI5L[26, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[26, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[26, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[26, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[26, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[26, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[26, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[26, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[26, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[26, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[26, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[26, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[26, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[26, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[26, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[26, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[26, 16] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[26, 17] = 27;
            adblPipelineDiameterWallThicknessAPI5L[26, 18] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[26, 19] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[26, 20] = 31.8;

            adblPipelineDiameterAPI5L[27] = 813;
            adblPipelineDiameterWallThicknessAPI5L[27, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[27, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[27, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[27, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[27, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[27, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[27, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[27, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[27, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[27, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[27, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[27, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[27, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[27, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[27, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[27, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[27, 16] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[27, 17] = 27;
            adblPipelineDiameterWallThicknessAPI5L[27, 18] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[27, 19] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[27, 20] = 31.8;

            adblPipelineDiameterAPI5L[28] = 864;
            adblPipelineDiameterWallThicknessAPI5L[28, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[28, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[28, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[28, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[28, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[28, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[28, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[28, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[28, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[28, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[28, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[28, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[28, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[28, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[28, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[28, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[28, 16] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[28, 17] = 27;
            adblPipelineDiameterWallThicknessAPI5L[28, 18] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[28, 19] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[28, 20] = 31.8;

            adblPipelineDiameterAPI5L[29] = 914;
            adblPipelineDiameterWallThicknessAPI5L[29, 0] = 6.4;
            adblPipelineDiameterWallThicknessAPI5L[29, 1] = 7.1;
            adblPipelineDiameterWallThicknessAPI5L[29, 2] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[29, 3] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[29, 4] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[29, 5] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[29, 6] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[29, 7] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[29, 8] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[29, 9] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[29, 10] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[29, 11] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[29, 12] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[29, 13] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[29, 14] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[29, 15] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[29, 16] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[29, 17] = 27;
            adblPipelineDiameterWallThicknessAPI5L[29, 18] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[29, 19] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[29, 20] = 31.8;

            adblPipelineDiameterAPI5L[30] = 965;
            adblPipelineDiameterWallThicknessAPI5L[30, 0] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[30, 1] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[30, 2] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[30, 3] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[30, 4] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[30, 5] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[30, 6] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[30, 7] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[30, 8] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[30, 9] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[30, 10] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[30, 11] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[30, 12] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[30, 13] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[30, 14] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[30, 15] = 27;
            adblPipelineDiameterWallThicknessAPI5L[30, 16] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[30, 17] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[30, 18] = 31.8;

            adblPipelineDiameterAPI5L[31] = 1016;
            adblPipelineDiameterWallThicknessAPI5L[31, 0] = 7.9;
            adblPipelineDiameterWallThicknessAPI5L[31, 1] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[31, 2] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[31, 3] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[31, 4] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[31, 5] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[31, 6] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[31, 7] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[31, 8] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[31, 9] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[31, 10] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[31, 11] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[31, 12] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[31, 13] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[31, 14] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[31, 15] = 27;
            adblPipelineDiameterWallThicknessAPI5L[31, 16] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[31, 17] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[31, 18] = 31.8;

            adblPipelineDiameterAPI5L[32] = 1067;
            adblPipelineDiameterWallThicknessAPI5L[32, 0] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[32, 1] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[32, 2] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[32, 3] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[32, 4] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[32, 5] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[32, 6] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[32, 7] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[32, 8] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[32, 9] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[32, 10] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[32, 11] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[32, 12] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[32, 13] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[32, 14] = 27;
            adblPipelineDiameterWallThicknessAPI5L[32, 15] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[32, 16] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[32, 17] = 31.8;

            adblPipelineDiameterAPI5L[33] = 1118;
            adblPipelineDiameterWallThicknessAPI5L[33, 0] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[33, 1] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[33, 2] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[33, 3] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[33, 4] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[33, 5] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[33, 6] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[33, 7] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[33, 8] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[33, 9] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[33, 10] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[33, 11] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[33, 12] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[33, 13] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[33, 14] = 27;
            adblPipelineDiameterWallThicknessAPI5L[33, 15] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[33, 16] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[33, 17] = 31.8;

            adblPipelineDiameterAPI5L[34] = 1168;
            adblPipelineDiameterWallThicknessAPI5L[34, 0] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[34, 1] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[34, 2] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[34, 3] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[34, 4] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[34, 5] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[34, 6] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[34, 7] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[34, 8] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[34, 9] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[34, 10] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[34, 11] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[34, 12] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[34, 13] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[34, 14] = 27;
            adblPipelineDiameterWallThicknessAPI5L[34, 15] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[34, 16] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[34, 17] = 31.8;

            adblPipelineDiameterAPI5L[35] = 1219;
            adblPipelineDiameterWallThicknessAPI5L[35, 0] = 8.7;
            adblPipelineDiameterWallThicknessAPI5L[35, 1] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[35, 2] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[35, 3] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[35, 4] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[35, 5] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[35, 6] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[35, 7] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[35, 8] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[35, 9] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[35, 10] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[35, 11] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[35, 12] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[35, 13] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[35, 14] = 27;
            adblPipelineDiameterWallThicknessAPI5L[35, 15] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[35, 16] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[35, 17] = 31.8;

            adblPipelineDiameterAPI5L[36] = 1321;
            adblPipelineDiameterWallThicknessAPI5L[36, 0] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[36, 1] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[36, 2] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[36, 3] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[36, 4] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[36, 5] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[36, 6] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[36, 7] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[36, 8] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[36, 9] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[36, 10] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[36, 11] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[36, 12] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[36, 13] = 27;
            adblPipelineDiameterWallThicknessAPI5L[36, 14] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[36, 15] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[36, 16] = 31.8;

            adblPipelineDiameterAPI5L[37] = 1422;
            adblPipelineDiameterWallThicknessAPI5L[37, 0] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[37, 1] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[37, 2] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[37, 3] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[37, 4] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[37, 5] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[37, 6] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[37, 7] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[37, 8] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[37, 9] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[37, 10] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[37, 11] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[37, 12] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[37, 13] = 27;
            adblPipelineDiameterWallThicknessAPI5L[37, 14] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[37, 15] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[37, 16] = 31.8;

            adblPipelineDiameterAPI5L[38] = 1524;
            adblPipelineDiameterWallThicknessAPI5L[38, 0] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[38, 1] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[38, 2] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[38, 3] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[38, 4] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[38, 5] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[38, 6] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[38, 7] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[38, 8] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[38, 9] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[38, 10] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[38, 11] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[38, 12] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[38, 13] = 27;
            adblPipelineDiameterWallThicknessAPI5L[38, 14] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[38, 15] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[38, 16] = 31.8;

            adblPipelineDiameterAPI5L[39] = 1626;
            adblPipelineDiameterWallThicknessAPI5L[39, 0] = 9.5;
            adblPipelineDiameterWallThicknessAPI5L[39, 1] = 10.3;
            adblPipelineDiameterWallThicknessAPI5L[39, 2] = 11.1;
            adblPipelineDiameterWallThicknessAPI5L[39, 3] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[39, 4] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[39, 5] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[39, 6] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[39, 7] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[39, 8] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[39, 9] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[39, 10] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[39, 11] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[39, 12] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[39, 13] = 27;
            adblPipelineDiameterWallThicknessAPI5L[39, 14] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[39, 15] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[39, 16] = 31.8;

            adblPipelineDiameterAPI5L[40] = 1727;
            adblPipelineDiameterWallThicknessAPI5L[40, 0] = 11.9;
            adblPipelineDiameterWallThicknessAPI5L[40, 1] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[40, 2] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[40, 3] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[40, 4] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[40, 5] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[40, 6] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[40, 7] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[40, 8] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[40, 9] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[40, 10] = 27;
            adblPipelineDiameterWallThicknessAPI5L[40, 11] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[40, 12] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[40, 13] = 31.8;

            adblPipelineDiameterAPI5L[41] = 1829;
            adblPipelineDiameterWallThicknessAPI5L[41, 0] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[41, 1] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[41, 2] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[41, 3] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[41, 4] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[41, 5] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[41, 6] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[41, 7] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[41, 8] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[41, 9] = 27;
            adblPipelineDiameterWallThicknessAPI5L[41, 10] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[41, 11] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[41, 12] = 31.8;

            adblPipelineDiameterAPI5L[42] = 1930;
            adblPipelineDiameterWallThicknessAPI5L[42, 0] = 12.7;
            adblPipelineDiameterWallThicknessAPI5L[42, 1] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[42, 2] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[42, 3] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[42, 4] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[42, 5] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[42, 6] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[42, 7] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[42, 8] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[42, 9] = 27;
            adblPipelineDiameterWallThicknessAPI5L[42, 10] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[42, 11] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[42, 12] = 31.8;

            adblPipelineDiameterAPI5L[43] = 2032;
            adblPipelineDiameterWallThicknessAPI5L[43, 0] = 14.3;
            adblPipelineDiameterWallThicknessAPI5L[43, 1] = 15.9;
            adblPipelineDiameterWallThicknessAPI5L[43, 2] = 17.5;
            adblPipelineDiameterWallThicknessAPI5L[43, 3] = 19.1;
            adblPipelineDiameterWallThicknessAPI5L[43, 4] = 20.6;
            adblPipelineDiameterWallThicknessAPI5L[43, 5] = 22.2;
            adblPipelineDiameterWallThicknessAPI5L[43, 6] = 23.8;
            adblPipelineDiameterWallThicknessAPI5L[43, 7] = 25.4;
            adblPipelineDiameterWallThicknessAPI5L[43, 8] = 27;
            adblPipelineDiameterWallThicknessAPI5L[43, 9] = 28.6;
            adblPipelineDiameterWallThicknessAPI5L[43, 10] = 30.2;
            adblPipelineDiameterWallThicknessAPI5L[43, 11] = 31.8;

            //按输入的直径和壁厚查找壁厚
            if (dblExternalDiameter < adblPipelineDiameterAPI5L[0] || dblExternalDiameter > adblPipelineDiameterAPI5L[c_intPipelineDiameterAPI5L - 1])
            {
                //您传入的管外径超过API5L规范的范围，不能确定管壁厚度
                MessageBox.Show("您传入的管外径超过API5L规范的范围，不能确定管壁厚度!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
                goto PROC_EXIT;
            }
            else
            {
                //管径达到范围内
                blnFindDiameter = false;
                for (intI = 0; intI <= c_intPipelineDiameterAPI5L - 1; intI++)
                {
                    if (dblExternalDiameter == adblPipelineDiameterAPI5L[(int)intI])
                    {
                        //找到对应的管径
                        blnFindDiameter = true;
                        intDiameterID = (int)intI;
                        break;
                    }
                }

                if (blnFindDiameter)
                {
                    //找到对应的管径
                }
                else
                {
                    //未找到对应的管径
                    if (blnShowQuestion)
                    {
                        //您输入的管外径不是API5L中的标准系列，是否继续计算（取上限）？
                        if (MessageBox.Show("您输入的管外径不是API5L中的标准系列，是否继续计算（取上限）" + "？",  "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //继续计算，查找合理的管径
                            blnFindDiameter = false;
                            for (intI = 1; intI <= c_intPipelineDiameterAPI5L - 1; intI++)
                            {
                                if (dblExternalDiameter > adblPipelineDiameterAPI5L[(int)intI - 1] && dblExternalDiameter <= adblPipelineDiameterAPI5L[(int)intI])
                                {
                                    //找到对应的管径，退出
                                    blnFindDiameter = true;
                                    intDiameterID = (int)intI;
                                    m_blnChangeDiameter = true; //改变管径
                                    m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterAPI5L[(int)intI] / 1000); //化为国际单位
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //找不到合理的管径，不能继续计算
                            dblWallThickness = c_dblWallThicknessError;
                            returnValue = false;
                            goto PROC_EXIT;
                        }
                    }
                    else
                    {
                        blnFindDiameter = false;
                        for (intI = 1; intI <= c_intPipelineDiameterAPI5L - 1; intI++)
                        {
                            if (dblExternalDiameter > adblPipelineDiameterAPI5L[(int)intI - 1] && dblExternalDiameter <= adblPipelineDiameterAPI5L[(int)intI])
                            {
                                //找到对应的管径，退出
                                blnFindDiameter = true;
                                intDiameterID = (int)intI;
                                m_blnChangeDiameter = true; //改变管径
                                m_dblPipelineExternalDiameter = System.Convert.ToDouble(adblPipelineDiameterAPI5L[(int)intI] / 1000); //化为国际单位
                                break;
                            }
                        }
                    }
                }

                //按传入的壁厚在规范中查找对应的壁厚
                //查找最大壁厚的位置
                intWallThicknessMaxID = -1;
                for (intJ = 0; intJ <= c_intPipelineWallThicknessMaxAPI5L - 1; intJ++)
                {
                    if (adblPipelineDiameterWallThicknessAPI5L[intDiameterID, (int)intJ] < 0)
                    {
                        intWallThicknessMaxID = (int)(intJ - 1);
                        break;
                    }
                }

                if (intWallThicknessMaxID < 0)
                {
                    //可能一个都没有找到（所有的数据都大于0），此时用壁厚最大值序号用壁厚数组的最大列数
                    intWallThicknessMaxID = c_intPipelineWallThicknessMaxAPI5L - 1;
                }

                blnFindWallThickness = false;
                if (dblWallThickness <= adblPipelineDiameterWallThicknessAPI5L[intDiameterID, 0])
                {
                    //采用最小壁厚
                    dblWallThickness = adblPipelineDiameterWallThicknessAPI5L[intDiameterID, 0];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness == adblPipelineDiameterWallThicknessAPI5L[intDiameterID, intWallThicknessMaxID])
                {
                    //找到对应壁厚
                    dblWallThickness = adblPipelineDiameterWallThicknessAPI5L[intDiameterID, intWallThicknessMaxID];
                    blnFindWallThickness = true;
                }
                else if (dblWallThickness > adblPipelineDiameterWallThicknessAPI5L[intDiameterID, intWallThicknessMaxID])
                {
                    //您所需的壁厚超过规范中的范围，不能获的壁厚!
                    MessageBox.Show("您所需的壁厚超过规范中的范围，不能获得壁厚!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //dblWallThickness = c_dblWallThicknessError
                    //用户认为查找壁厚失败时仍能计算结果，故查找失败时直接返回传入的参数值
                    dblWallThickness = dblWallThickness;
                    blnFindWallThickness = false;
                    returnValue = false;
                    goto PROC_EXIT;
                }
                else
                {
                    for (intJ = 1; intJ <= intWallThicknessMaxID; intJ++)
                    {
                        if (adblPipelineDiameterWallThicknessAPI5L[intDiameterID, (int)intJ - 1] < dblWallThickness && dblWallThickness <= adblPipelineDiameterWallThicknessAPI5L[intDiameterID, (int)intJ])
                        {
                            //找到对应的壁厚（取大值）
                            dblWallThickness = adblPipelineDiameterWallThicknessAPI5L[intDiameterID, (int)intJ];
                            blnFindWallThickness = true;
                            break;
                        }
                    }

                    if (!blnFindWallThickness)
                    {
                        //找不到合理的壁厚（正常情况下不可能，请检查程序）!
                        MessageBox.Show("找不到合理的管径（正常情况下不可能，请检查程序）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dblWallThickness = c_dblWallThicknessError;
                        returnValue = false;
                        goto PROC_EXIT;
                    }
                }
                //返回函数值
                returnValue = true;

            }

        PROC_EXIT:
            return returnValue;


      
        }
        private void Clear()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtInput3.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtInput5.Text = "";
            txtInput6.Text = "";
            txtInput7.Text = "";
            txtInput8.Text = "";
            txtOutput1.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";
            txtOutput6.Text = "";
            txtOutput7.Text = "";
            txtOutput8.Text = "";
            txtOutput9.Text = "";
            txtOutput10.Text = "";
            txtOutput11.Text = "";
            txtOutput12.Text = "";
     

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblOutput6_Click(object sender, EventArgs e)
        {

        }

        private void lblOutput2_Click(object sender, EventArgs e)
        {

        }

        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButtonGb1997.Checked == true)
            {
                CalculateStyleGasPipelineByGB1997();
            }
            else if (radioButtonGb1999.Checked == true)
            {
                CalculateStyleGasPipelineByGB1999();
            }
            else if (radioButtonAPI5L.Checked == true)
            {
                CalculateStyleGasPipelineByAPI5L();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void radioButtonGb1997_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void radioButtonGb1999_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void radioButtonAPI5L_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }
    }
}
