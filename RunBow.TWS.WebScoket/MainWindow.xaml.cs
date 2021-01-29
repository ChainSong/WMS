using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunBow.TWS.WebScoket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string PortName = System.Configuration.ConfigurationSettings.AppSettings["PortName"].ToString();//端口名称

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Border_Title.MouseDown += new MouseButtonEventHandler(Border_Title_MouseDown);//定义可拖动的全局border
        }

        //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

        private Boolean Stem = false;

        private static SerialPort serialPort1 = new SerialPort();//串口控件

        private ScoketCommunication scoket = new ScoketCommunication();//通讯类


        public delegate void HandleInterfaceUpdataDelegate(string text);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;

        System.Threading.Timer TimeResult;

        Thread thread;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            setOrgComb();//初始化秤属性    
            scoket.InitializationWebScoket();//初始化scoket

            interfaceUpdataHandle = new HandleInterfaceUpdataDelegate(UpdateTextBox);//实例化委托对象 
            scoket.OnReceive += ReceiveMsg;
            scoket.OnConnect += ScoketConnect;
            OpenSerialPort();//打开电子秤
                             //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                             //thread = new Thread(Read);//多线程去读
                             //thread.Start();

            TimeResult = new System.Threading.Timer(new TimerCallback(MyDelegate));//时间控件去读
            TimeResult.Change(0, 500);
        }


        /// <summary>
        /// 初始化串口
        /// </summary>
        private void setOrgComb()
        {
            serialPort1.PortName = !string.IsNullOrEmpty(PortName) ? PortName : "COM1";     //端口名称
            serialPort1.BaudRate = 9600;            //波特率
            serialPort1.Parity = Parity.None;       //奇偶效验
            serialPort1.StopBits = StopBits.One;    //效验
            serialPort1.DataBits = 8;               //每个字节的数据位长度
            serialPort1.StopBits = StopBits.One;
            //serialPort1.RtsEnable = true;

            serialPort1.ReadTimeout = 500;
        }

        /// <summary>
        /// 打开串口秤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenSerialPort()
        {
            try
            {
                //serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                //serialPort1.DataReceived -= serialPort1_DataReceived;
                Stem = false;
                //Thread.Sleep(100);
                if (serialPort1.IsOpen)
                {
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    serialPort1.Close();

                    //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    //serialPort1.DataReceived += serialPort1_DataReceived;
                    serialPort1.Open();
                    Stem = true;
                }
                else
                {
                    //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    //serialPort1.DataReceived += serialPort1_DataReceived;
                    serialPort1.Open();
                    Stem = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败：" + ex.Message.ToString());
                return;
            }
        }

        /// <summary>
        /// 自己读
        /// </summary>
        public void Read()
        {
            while (true)
            {
                while (Stem)
                {
                    try
                    {
                        Thread.Sleep(200);
                        if (serialPort1.IsOpen)//如果电子秤是打开状态
                        {

                            string weight = "";
                            string message = serialPort1.ReadLine();
                            if (message.Length == 17)
                            {
                                weight = message.Substring(8, 6).Trim();
                            }
                            else
                            {
                                weight = "00.000";
                            }

                            //委托把重量传给界面
                            if (!string.IsNullOrEmpty(weight))
                            {
                                Dispatcher.Invoke(interfaceUpdataHandle, weight.ToString());
                                scoket.SendMessage(weight);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("电子秤未连接，请检查！");
                        Stem = false;
                    }
                }
            }

        }

        /// <summary>
        /// 自己读2
        /// </summary>
        public void Read2()
        {
            try
            {
                //Thread.Sleep(200);
                if (serialPort1.IsOpen)//如果电子秤是打开状态
                {

                    string weight = "";
                    string message = serialPort1.ReadLine();
                    if (message.Length == 17)
                    {
                        weight = message.Substring(8, 6).Trim();
                    }
                    else
                    {
                        weight = "00.000";
                    }

                    //委托把重量传给界面
                    if (!string.IsNullOrEmpty(weight))
                    {
                        Dispatcher.Invoke(interfaceUpdataHandle, weight.ToString());
                        scoket.SendMessage(weight);
                    }

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("电子秤未连接，请检查！");
                Stem = false;
            }
        }
        delegate void UpdateTimer();
        void MyDelegate(object state)
        {
            this.Dispatcher.BeginInvoke(new UpdateTimer(MyEventFunc));
        }

        void MyEventFunc()
        {
            Read2();
        }



        /// <summary>
        /// 串口电子秤接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {



            //while (Stem)
            //{
            //    if (serialPort1.IsOpen)//如果电子秤是打开状态
            //    {
            //        try
            //        {
            //            string weight = "";

            //            #region 耀华
            //            //string message = serialPort1.ReadExisting();                         
            //            //Match match = Regex.Match(message, "=(?<weightDecimal>[0-9]*).(?<weightInteger>[0-9]*)", RegexOptions.RightToLeft); //获取匹配命名组
            //            //string Decimal = match.Groups["weightDecimal"].Value.ToString();
            //            //string Integer = match.Groups["weightInteger"].Value.ToString();
            //            ////逆序
            //            //char[] DecimalChar = Decimal.ToCharArray();                        
            //            //Array.Reverse(DecimalChar);
            //            //Decimal = new string(DecimalChar);                       
            //            //char[] IntegerChar = Integer.ToCharArray();
            //            //Array.Reverse(IntegerChar);
            //            //Integer = new string(IntegerChar);

            //            //if (Decimal == "" && Integer == "")
            //            //{
            //            //    weight = "00.000";                       
            //            //}
            //            //else
            //            //{
            //            //    weight = string.Format("{0:D2}", Integer.TrimStart('0') == "" ? "0" : Integer.TrimStart('0')) + "." + string.Format("{0:D3}", Decimal);
            //            //}
            //            #endregion

            //            #region 英展

            //            string message = serialPort1.ReadLine();

            //            if (message.Length == 17)
            //            {                           
            //                weight = message.Substring(8, 6).Trim();
            //            }
            //            else
            //            {
            //                weight = "00.000";
            //            }
            //            #endregion                       
            //            //委托把重量传给界面
            //            if (!string.IsNullOrEmpty(weight))
            //            {
            //                Dispatcher.Invoke(interfaceUpdataHandle, weight.ToString()); 
            //                //scoket.SendMessage(weight);                          
            //            }
            //            Thread.Sleep(200);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("读取失败:" + ex.Message.ToString());
            //            Stem = false;
            //        }
            //    }
            //}
        }




        /// <summary>
        /// 更新文本框
        /// </summary>
        /// <param name="text"></param>
        private void UpdateTextBox(string text)
        {
            txtweight.Text = text;
        }



        /// <summary>
        /// 重连电子秤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //setOrgComb();

            OpenSerialPort();
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void CloseCom()
        {

            try
            {
                Stem = false;
                if (serialPort1 != null)
                {
                    //serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    //serialPort1.DataReceived -= serialPort1_DataReceived;
                }

                if (serialPort1.IsOpen)
                {
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    serialPort1.Close();
                    serialPort1.Dispose();

                }
            }
            catch (Exception e)
            {

            }
        }


        #region 

        /// <summary>
        /// 拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Border_Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseCom();//关掉秤            
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }
        #endregion

        /// <summary>
        /// 模拟发送数据到前台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            scoket.SendMessage(txtweight.Text);
        }

        Random r = new Random();

        /// <summary>
        /// 测使用
        /// </summary>
        public void UpdateWeight()
        {
            while (true)
            {
                Thread.Sleep(1000);
                int a = r.Next(10, 200);
                string weight = (Convert.ToDecimal(a) / 100).ToString();
                //txtMsg.Text = weight;
                GetTextvalue gt1 = new GetTextvalue(GetDisable);
                this.Dispatcher.Invoke(gt1, weight);
                //scoket.SendMessage(weight);
            }
        }

        //定义个无参无返回值的委托
        private delegate void GetTextvalue(string msg);
        /// <summary>
        /// 禁用按钮，以及提示
        /// </summary>
        private void GetDisable(string msg)
        {
            txtMsg.Text = msg;
        }

        /// <summary>
        /// 前台网页发过来的数据
        /// </summary>
        /// <param name="s"></param>
        /// <param name="msg"></param>
        public void ReceiveMsg(object s, string msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// 前端网页发出的连接请求
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void ScoketConnect(object s, EventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
