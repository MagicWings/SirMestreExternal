using SirMestreBlackCat.Model;
using MahApps.Metro.Controls;
using Model.Libraries.KeyBoardHooking;
using System.Threading;
using System.Windows;

namespace WpfApplication
{
    public partial class MainWindow : MetroWindow
    {
        MemoryFunctions MemoryFunctions = new MemoryFunctions("GTA5", "GTA5.exe");

        public MainWindow()
        {
            InitializeComponent();
            new KeyBoardHooking();  
        }

        private void God_Mode_ToggleSwitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_God_Mode(God_Mode_ToggleSwitch.IsChecked);
                });
            } else
            {
                God_Mode_ToggleSwitch.IsChecked = false;
            }
        }

        private int Wanted_Level = 0;
        private void Wanted_Level_NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            int Wanted_Level_Selected = (int)Wanted_Level_NumericUpDown.Value;
            if (Wanted_Level_NumericUpDown.IsInitialized == true)
            {
                Wanted_Level = Wanted_Level_Selected;
                if (MemoryFunctions.IsGameRunning() == true)
                {
                    Thread Thread = new Thread(() =>
                    {
                        while (Wanted_Level == Wanted_Level_Selected)
                        {
                            if (MemoryFunctions.GAME_get_Wanted_Level() != Wanted_Level)
                            {
                                MemoryFunctions.GAME_set_Wanted_Level(Wanted_Level);
                            }
                            Thread.Sleep(100);
                        }
                    });
                    Thread.IsBackground = true;
                    Thread.SetApartmentState(ApartmentState.STA);
                    Thread.Start();
                }
                else
                {
                    Wanted_Level_NumericUpDown.Value = 0;
                }
            }
        }

        private void Sprint_Speed_Slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Thread Thread = new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        MemoryFunctions.GAME_set_Sprint_Speed((float)Sprint_Speed_Slider.Value);
                    });
                });
                Thread.IsBackground = true;
                Thread.SetApartmentState(ApartmentState.STA);
                Thread.Start();
            }
            else
            {
                Sprint_Speed_Slider.Value = 1;
            }
        }

        private void Swim_Speed_Slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Thread Thread = new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        MemoryFunctions.GAME_set_Swim_Speed((float)Swim_Speed_Slider.Value);
                    });
                });
                Thread.IsBackground = true;
                Thread.SetApartmentState(ApartmentState.STA);
                Thread.Start();
            }
            else
            {
                Swim_Speed_Slider.Value = 1;
            }
        }

        /*private void Explosive_Ammo_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_Explosive_Ammo(Explosive_Ammo_ToggleSwitch.IsChecked);
                });
            }
            else
            {
                Explosive_Ammo_ToggleSwitch.IsChecked = false;
            }
        }*/

        private void Unlimited_Ammo_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_Unlimited_Ammo(Unlimited_Ammo_ToggleSwitch.IsChecked);
                });
            }
            else
            {
                Unlimited_Ammo_ToggleSwitch.IsChecked = false;
            }
        }

        private void Unlimited_Magazine_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_Unlimited_Magazine(Unlimited_Magazine_ToggleSwitch.IsChecked);
                });
            }
            else
            {
                Unlimited_Magazine_ToggleSwitch.IsChecked = false;
            }
        }

        private void God_Mode_Vehicle_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_God_Mode_Vehicle(God_Mode_ToggleSwitch.IsChecked);
                });
            } else
            {
                God_Mode_Vehicle_ToggleSwitch.IsChecked = false;
            }
        }

        private void No_Bike_Fall_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MemoryFunctions.GAME_set_No_Bike_Fall(No_Bike_Fall_ToggleSwitch.IsChecked);
                });
            }
            else
            {
                No_Bike_Fall_ToggleSwitch.IsChecked = false;
            }
        }

        private void Toggle_RP_Checked_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                while (MemoryFunctions.IsGameRunning() == true)
                {
                    try
                    {
                        Thread Thread = new Thread(() =>
                        {
                            Application.Current.Dispatcher.Invoke(delegate
                        {
                            MemoryFunctions.Toggle_RP_Checked_ToggleSwitch(Toggle_RP_Checked_ToggleSwitch.IsChecked);
                            MemoryFunctions.GAME_set_Wanted_Level(5);
                            Thread.Sleep(100);
                            MemoryFunctions.GAME_set_Wanted_Level(0);
                            Thread.Sleep(100);
                        });
                        });
                        Thread.IsBackground = true;
                        Thread.SetApartmentState(ApartmentState.STA);
                        Thread.Start();
                    }
                    catch
                    {
                        continue;
                    }
                    //finally { Toggle_RP_Checked_ToggleSwitch.IsChecked = false; }
                }
           
                MemoryFunctions.GAME_set_Wanted_Level(0);
            }
            else
            {
                Toggle_RP_Checked_ToggleSwitch.IsChecked = false;
            }
        }

        private void Toggle_Wanted_ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            {
                {
                    if (MemoryFunctions.GAME_get_Wanted_Level() > 0)
                    {
                        MemoryFunctions.GAME_set_Wanted_Level(0);
                    }
                    Thread.Sleep(500);
                }
            }

            MemoryFunctions.GAME_set_Wanted_Level(0);
        }

        private void Teleport_To_Waypoint_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryFunctions.IsGameRunning() == true)
            {
                Thread Thread = new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                    Teleport_To_Waypoint_Button.IsEnabled = false;
                        Teleport_To_Waypoint_Button.Content = "Teleporting...";
                    });
                    MemoryFunctions.GAME_teleport_to_Waypoint();
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        Teleport_To_Waypoint_Button.IsEnabled = true;
                        Teleport_To_Waypoint_Button.Content = "Teleport to Waypoint";
                    });
                });
                Thread.IsBackground = true;
                Thread.SetApartmentState(ApartmentState.STA);
                Thread.Start();
            }
        }
    }
}

public class RGUgTp
{
    public void qoVZmfjGyaqHnPkPsZEqwsmpxF()
    {
        ushort IyMTioZ = 7656;
        string dbfZOLUPXsHRChpYzYbGDJLbYa = "yzYQeMAVDl";
        ushort CwHHcCRVEEkRaeypwp = 5272;
        string xyqYCCeblONbuAjWbIAheLU = "hQanjAlhw";
        float YoIBaM = 6.983754E-13F;
        ulong PGFnuuIFSJLEdRM = 7454246130773916;
        ushort HthAxkteP = 24243;
        byte aQfEDDhuulROzU = 12;
        int TickbNaBfx = 351616;
        while (TickbNaBfx == 351616)
        {
            TickbNaBfx += 543918;
        }
        sbyte bIXbdxZRuCtaChOQWkhkYcedy = -97;
        long VXul = 46024492029419425;
        byte tAheVqFJ = 89;
        byte USYDUFWNXVyhg = 166;
        ushort QAfeCa = 50115;
        long WtkszmFgSzkduymiTno = 39275872410070955;
        byte TUenXXk = 186;
        ushort JTbojldwbSWJMzPn = 59349;
        double hKcsZYEgPyAbxzOHbpMmlgs = -2.073497E-08;
        if (hKcsZYEgPyAbxzOHbpMmlgs == -3.879173E-27)
        {
            //hKcsZYEgPyAbxzOHbpMmlgs = Math.Ceiling(Math.Tanh(0.1));
            for (;;)
            {
                //Console.WriteLine(-3.267739E-15);
            }
        }
        int LZjOMMTGzmyLFnCkGX = 1151;
        while (LZjOMMTGzmyLFnCkGX == 1151)
        {
            LZjOMMTGzmyLFnCkGX += 982686;
        }
        float KNcuhXptc = 4.767361E-22F;
        ulong bDhYXCpWZUxBVhABJZk = 79277074844067983;
        ushort lNZTsjDW = 4833;
        string CpKTHTlQQSuPaOf = "xGUSbULOgxIDYWFFXNxiqnVkTKFtR";
        double emkgpijxHFWMjlJRUnPRnsXIiKKZq = -7.151475E-13;
        //double nwOpQEwDLZjTLfcgLNoDXHAjf = Math.Log(1000, 10);
        //emkgpijxHFWMjlJRUnPRnsXIiKKZq = nwOpQEwDLZjTLfcgLNoDXHAjf;
        int[] MJtddZtdXCtmHZLPzF = { 4857670, 31769 };
        //Random UcNPakSIWEk = new Random();
        //Console.WriteLine(MJtddZtdXCtmHZLPzF[UcNPakSIWEk.Next(0, 2)]); ushort YpR = 51672;
        int JcOkVp = 6384;
        if (JcOkVp == 647407)
        {
            JcOkVp = 724240;
        }
        string AYUsT = "syBNccRHa";
        ushort JYgkZZFeVBahiDOppBZwAWwOlDahT = 31541;
        long txYNUUBZomXLqpqjoNBf = 11379926836538269;
        double lxQtOMFaxwaagm = 5.367894E+14;
        while (lxQtOMFaxwaagm == 1.413659E-21)
        {
            //lxQtOMFaxwaagm = Math.Pow(double.NegativeInfinity, 2);
            object ykNtPsQV;
            ykNtPsQV = -1.861265E-27;
        }
        int STpTdKkiQmzxFiaGZpezKFWZs = 726508;
        while (STpTdKkiQmzxFiaGZpezKFWZs == 726508)
        {
            STpTdKkiQmzxFiaGZpezKFWZs = STpTdKkiQmzxFiaGZpezKFWZs + 900372;
        }
        uint JdgWsuenkRY = 35;
        short GVMglghnWqC = -1637;
        double qNsY = -2.524064E+32;
        if (qNsY == 1.04717E+14)
        {
            double ANYTZpxCW = 3.608478E+32;
            //ANYTZpxCW = Math.Sqrt(3);
            qNsY = ANYTZpxCW;
            MessageBox.Show(qNsY.ToString());
        }
        byte WIBXzCiDC = 101;

    }
    public void FKWuoJdQaTYGKs()
    {
        long YlPKmDcmTomhpNFKAIbypXV = 13597074644392753;
        int OjqMKhbIJCawWAnnogbqnx = 31;
        if (OjqMKhbIJCawWAnnogbqnx == 974241)
        {
            OjqMKhbIJCawWAnnogbqnx = 363652;
        }
        long QzIwDDnXPA = 6887821861183220;
        double JWPnz = -2.887007E-25;
        if (JWPnz == -3.081565E-27)
        {
            double LDOCXLasqCsyPF = -1.112216E+13;
            //LDOCXLasqCsyPF = Math.Sqrt(3);
            JWPnz = LDOCXLasqCsyPF;
            try
            {
                //Console.WriteLine(JWPnz.ToString());
            }
            catch //(Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }
        short mVaCFFXQalwNQTuFMqN = -4356;
        long cGCTdjIq = 64318218664265204;
        ulong YQpdxCUAAegTqTUbTuaLC = 54470791296021926;
        float bEezUVYjZZEKDkshywtGC = 1.353176E-34F;
        sbyte IIdAftb = 22;
        ushort pmwag = 59198;
        byte MXe = 156;
        uint OYfdohsAXommULTGDegf = 89;
        string mLqIqjebjxLhm = "ooMQpfNTaBhgwZAqGTVLRFQLiNjk";
        string ZLsFQStlGXItKRB = "PeDSRSmD";
        sbyte zbIDVCQDBxOQjHYtnYdyUZ = -77;
        double fJkHhzQzeusFdbfEhpdxZUVUyEnAJ = 0.002480698;
        //fJkHhzQzeusFdbfEhpdxZUVUyEnAJ = Math.Ceiling(Math.Atan(-5));
        //Console.ReadLine(); long CNacqwebCkSObkhQNVHgJAhtQhY = 52473863301933606;
        string fmjamkEVTAHjogQdEmbGI = "jyMTMElXi";
        long fIIyfx = 1927576374075206;
        long DnGBwcSdMouzaKWqXdiAiHyRN = 17606409546042381;
        ulong KDiLdPFLRbQDOapufLhI = 45547905839883744;
        int wumamOPtfLFWGQWAwplMzn = 106728641;
        if (wumamOPtfLFWGQWAwplMzn == 774360)
        {
            wumamOPtfLFWGQWAwplMzn = wumamOPtfLFWGQWAwplMzn + 410836;
        }
        short mddgGxpN = 16443;
        uint MGydZkYQTyloUmhygnu = 38;
        string zBSzjBiIEKgMIAbOJPFzOaOjUz = "iAtCkLMZDgMPpKPJGOBAXeBLkPDfy";
        ulong FLCaijhVaWBAHWZoOZsiw = 13777995294976415;
        sbyte CQygeOKHzzSADyGTOkA = 71;
        sbyte FmkLA = 108;
        byte QASpYKjPVBetTKEUy = 149;
        uint TgWtA = 221615;
        byte toOxXIsIlzWKZpZKNmUKqnVzeJKD = 120;
        float zZTClqWVcklkfCwUR = 1.273984E+14F;
        ulong qBGetoso = 30039277116510666;
        ulong tRqRzedQWJOTFPesgAnxSbF = 19381624571878024;
        ulong jpVT = 60961130847324316;

    }
    public void kuqFs()
    {
        byte qVslKNdbnHTXZQwzizHzNunJW = 31;
        double KzELBWL = -5.678767E+35;
        //double ALCnQVhgLNfWHJoENUqPchydefBz = Math.Log(1000, 10);
        //KzELBWL = ALCnQVhgLNfWHJoENUqPchydefBz;
        //Console.ReadLine(); double CEzG = -1.216049E+13;
        //if //(CEzG == -0.0003463611)
        {
            //CEzG = Math.Ceiling(Math.Tanh(0.1));
            //MessageBox.Show(CEzG.ToString());
        }
        ulong RBfjSyKwK = 27070659180086430;
        sbyte UlzKbNTRxaNCNtbaFpEgBkfzSyVZJ = -57;
        ushort omTatReyZ = 40299;
        byte WPydVZEzmbzJf = 0;
        float RThhGClBBaINlghUXL = 2.880016E-34F;
        string eJLWYmOkUsZBGgzfOwDWEVsQl = "UELwyN";
        int PAx = 54810494;
        if (PAx == 305859)
        {
            PAx = PAx + 122764;
        }
        short LBbxRXhCdnjqUKStkAAUlWG = -10787;
        double fSDknOccCHTgTzplAMGhC = 8.65748E+15;
        while (fSDknOccCHTgTzplAMGhC == 1.076888E+23)
        {
            //fSDknOccCHTgTzplAMGhC = Math.Ceiling(Math.Sin(2));
            //Console.Write(fSDknOccCHTgTzplAMGhC.ToString());
        }
        string KkcLVTkziXout = "OdojlNDUxKEwAkWpLNnbQgqjBLDqY";
        int cXJciXsHoSQPsgAuZCSl = 116154;
        while (cXJciXsHoSQPsgAuZCSl == 116154)
        {
            cXJciXsHoSQPsgAuZCSl += 738718;
        }
        sbyte CjkVSbBzRiOttgZiZqYda = -47;
        int ftgXGzuxSIeeIUXpHbDRNASu = 17;
        while (ftgXGzuxSIeeIUXpHbDRNASu == 17)
        {
            ftgXGzuxSIeeIUXpHbDRNASu += 37252;
        }
        long HwQAVESs = 29818865563543086;
        ushort xYONcGBMRxodIQYUzlez = 26817;
        string kcuZgtk = "eVfGKyUtjYjNxdKKKNhx";
        short Wms = 20204;
        int IHttGQjhDknxFSUAlqNhs = 30270981;
        while (IHttGQjhDknxFSUAlqNhs == 30270981)
        {
            IHttGQjhDknxFSUAlqNhs += 334745;
        }
        string SnUUjMEGQgGTChAzMSygybacwsWqY = "QzhiKXu";
        uint gqUwjgnREQWAW = 927437816;
        uint mRASPmHIqhMsyDejypPDGQWgC = 5031;
        string UEkHSKDcEsenNTZDzfnSo = "FgZlxVspwNEYxEB";
        long SChUYLoSXWFdnlkuwZe = 60358992144300422;
        long yxbMBwpgFGxKtAZOmhBtFbE = 65213650171904347;
        short eBBDqNQVmRtIYmOIVZO = 22132;
        short xSGJDRwJ = 28647;
        int doZz = 9367;
        while (doZz == 9367)
        {
            doZz = doZz + 327455;
        }
        byte OeMQmEJm = 132;
        short eIWIjZBFUQFVXPiJcthTTlISERa = -23149;
        sbyte pDOuVRpokL = 115;
        ulong uijObeUHUux = 34319450283026426;
        uint AcgDUstOhxVgVmJtdZd = 6394;

    }
    public void istcQkjSMFlKVg()
    {
        string gMCTNXCV = "NwLpZooYcUPjSChEpKpUksDpSSqK";
        ulong kQzedQACNpXLTmoaoktox = 75430244235856486;
        sbyte FktJYMNysdOWaWAApbREgpzmXGysJ = 111;
        string HqsBA = "qoCNbkJqkdmVEtGDSo";
        ulong MtnwPw = 11712369170615967;
        long ctomFGutfNtLzeAdP = 62369717448156127;
        double DAn = 8.274102E+24;
        //DAn = Math.Ceiling(Math.Asin(0.2));
        for (;;)
        {
            //Console.WriteLine(2.384137E-37);
        }
        ulong NtgAcdT = 28740688590386056;
        string MTWkTgsLgNUijDiCOikwEoucMIgJ = "SebFkLbSXUlPPWqZGoqGZ";
        float GHuyAUAkaJanITFYShKQoYHPsddf = -5.863861E+21F;
        string ZhwXRmDYyJkgOIJfaqAzoc = "HaGexQtTlFooAyIhFGLs";
        int AaLuXfapFwWBPXkighePZCA = 1878;
        if (AaLuXfapFwWBPXkighePZCA == 176310)
        {
            AaLuXfapFwWBPXkighePZCA = 886132;
        }
        float LbTBGubTnSVZTsZJLWx = -533.7469F;
        byte PYCsjnMSxMRlzVxGPenQXdCGZ = 52;
        string mytbxkeDdbRGDaAncgBNn = "IunyTLxVsZm";
        string YsPPnRTCNmJnWEnETLn = "VQZOYstghKSsA";
        ushort VmqmVwPVhGjWhiszo = 64239;
        long itbRVzNudwpPCbspDztZg = 84101248879114906;
        byte tQMtClBqxIuSMowwfQ = 12;
        double GKDmCTcBnPFSSWYQJRdcgWqMc = -3.98751E+27;
        while (GKDmCTcBnPFSSWYQJRdcgWqMc == 3.942019E-31)
        {
            double OfjfZtzQiTQZUtxGyZYUKJt = -1.305468E+19;
            //GKDmCTcBnPFSSWYQJRdcgWqMc = Math.Floor(OfjfZtzQiTQZUtxGyZYUKJt);
           //Console.ReadKey();
        }
        string NtVDs = "oakZxOjFZUhnptUUUhlBNfBA";
        string edemIns = "bOmXHypswOaAeJjTAOqSbiAM";
        double JfAMlUWBq = -2.594355E+10;
        //double AEewUlNUXAOSnCbuOjTJDgE = Math.IEEERemainder(3, 4);
        //JfAMlUWBq = AEewUlNUXAOSnCbuOjTJDgE;
        string bisaBeQajHwSgtBoJ = "BYGKkjgRS";
        byte IhCIex = 153;
        float HiwmPLCEGfsQ = -1.013984E+28F;
        uint WsX = 86;
        long FTDzTkcWU = 45920767893760525;
        long lMUpuocGieILAXWRtl = 16660405431674558;
        byte ADxyCp = 114;
        uint EijRGTSmKRIwLZJnZjJEx = 473048600;
        string gGIkjiAxgQdcSNJqjLeGMODOiCf = "WnJKiWzmjcBJhYEjGlgxZml";
        byte witdUbIcMBThu = 222;
        double xghkwyNjti = 2.68051E-08;
        double NfjobSGxzLclskhEfeYYYwi = -0.01491375;
        //xghkwyNjti = Math.Floor(NfjobSGxzLclskhEfeYYYwi);
        byte WBqPsXBEXFlzIStgmc = 252;

    }
    public void nXGiXgKtPAdS()
    {
        ulong ViNpRkLnj = 53921809822343297;
        sbyte RpYViSkmgPzBuLwWk = -30;
        double OHaORFwBc = -2.570933E+36;
        if (OHaORFwBc == 3.210996E-15)
        {
            //OHaORFwBc = Math.Sqrt(4);
            //Console.ReadKey();
        }
        float VKuKfbMLNGYnYhOhBiTwSYl = 1.062163F;
        uint SmPPx = 4849696;
        sbyte JTpDDIYXMPEESFIShAVXyNjEX = 98;
        uint RDEVaWzISpTkanVLXCZIgWnlgjuVJ = 61233541;
        long agjlJDV = 28841671856263918;
        float GxUQRVnqOQDEYfL = -2.216359E-37F;
        string MAoYeCg = "xGxmkdkhjOtikVUUMtDbUhwKWbgY";
        short QnKYSMqDqAOYBx = 9650;
        float QGwmLZebojNqjXlRNYezmdhDiccn = 1.488937E+10F;
        double fep = -5.420997E+30;
        if (fep == -1.809563E-16)
        {
            //fep = Math.Ceiling(Math.Sinh(-5));
            //Console.WriteLine(fep.ToString());
        }
        int AtmRUiNxdoPKlyKeXTzyye = 616557;
        while (AtmRUiNxdoPKlyKeXTzyye == 616557)
        {
            AtmRUiNxdoPKlyKeXTzyye += 880092;
        }
        int GJXNn = 31;
        while (GJXNn == 31)
        {
            GJXNn += 674721;
        }
        double CfIIEqQLqMgowUHDPnTRZK = -4.571656E+17;
        while (CfIIEqQLqMgowUHDPnTRZK == -5.535617E+12)
        {
            //double OBtXBHyqDPuoXRyThK = Math.IEEERemainder(3, 4);
            //CfIIEqQLqMgowUHDPnTRZK = OBtXBHyqDPuoXRyThK;
            int? fMSaViuJbRHjsPcXgVsWqcGwh = 7346615;
            fMSaViuJbRHjsPcXgVsWqcGwh += 47678;
        }
        string ySagdh = "zoqttwSHkdayocNuxtBqDNogAtB";
        long LhTMSdtwfowsWwYMbNdK = 31496485407395882;
        byte aVjZBVgLyYuZqdeYGspuzGxXif = 95;
        short AssklnBiXUdGqpfpznKzSNtKRww = -905;
        byte uDRPYGbPeQFQJymPgppIKSJR = 16;
        ulong JtRTaWgBCPDqAoKiUUceVxcnfiFfD = 7999568812393557;
        double UCXMaFYRtaJPWaIdlq = 3.683401E+18;
        double fTVoVXqiDYLcKgslfTedLFEhd = 1.023207E+12;
        UCXMaFYRtaJPWaIdlq = fTVoVXqiDYLcKgslfTedLFEhd / 3;
        bool? AZdkOTYyLNRGSeeCoT = new bool?();
        AZdkOTYyLNRGSeeCoT = true; ulong fmySAzHPSElzE = 23764750052353423;
        int ZmbfU = 940378;
        while (ZmbfU == 940378)
        {
            ZmbfU += 532891;
        }
        double nnbRgxsjuhA = -5.037463E-30;
        if (nnbRgxsjuhA == -1.170934E+33)
        {
            //nnbRgxsjuhA = Math.Truncate(nnbRgxsjuhA);
            //Console.WriteLine(nnbRgxsjuhA.ToString());
        }
        sbyte SbOPAJRfUXjJfyVjfBccNWJj = 95;
        uint SEpAtQDfhzcmn = 345615;
        sbyte BdnKcESScccVtEV = 126;
        byte DFzQSEsOLdnPAwQjJPlLPZzIpOBjO = 229;
        sbyte aFQCaKlLq = -57;
        float EnEIfjUfODjDfQBeciZRWbP = 1.065635E+08F;
        short YECuBgK = -27526;
        string TeFabAyu = "xjjgIiobS";
        uint JbRaVDmiJxwMyUjd = 820746;

    }
}
public class DqBsSZPxTlleSCECRnFeAloLRay
{
    public void myeTkXZhiKYagftWyqEkUs()
    {
        ushort QMwpbPCSDcjxgsTUCwthpEXt = 56812;
        short AymRUdodzC = 12727;
        sbyte hfzeVXQeyJJAZ = -69;
        uint PcxJBcwok = 3461;
        float usSoZZjfTz = 6819.424F;
        sbyte OFs = 99;
        float RKAmyTfLiTGGXQcpQkampgW = 9.931573F;
        sbyte aHLTwBdyLTFS = -97;
        float WZSpFtYyOhUGzLOV = 3.33704E-15F;
        uint dzajf = 354225520;
        short lsUsRjWLkCZbiWL = 2165;
        float NXXSCGPwyZJkLaHaCPa = -7.014287E+10F;
        uint jCODdUCoIjnGuBtRoEwNXcZQY = 171934;
        ushort CIYmgNqNdLCWKbaywlBiYMJlOUh = 34479;
        double IMgtgDpsnNcdlSURmb = 2.261135E-10;
        while (IMgtgDpsnNcdlSURmb == -1.121894E+13)
        {
            double MzlFQQAdQPqUzhcXNfVAQsXHgDFZz = -2.155019E+34;
            IMgtgDpsnNcdlSURmb = MzlFQQAdQPqUzhcXNfVAQsXHgDFZz * 2;
            //Console.ReadKey();
        }
        uint znkAFNUVFVEiWETIObn = 719219062;
        string szm = "Otauks";
        ushort ZcljqhaCFJsosnuCheHTy = 15333;
        float sHg = 260583.8F;
        uint QATUGDRXOLzx = 12242613;
        double BZzmfMEmNjkSKaAXcpCQzuP = 3.52591E-15;
        if (BZzmfMEmNjkSKaAXcpCQzuP == 7.275834E-26)
        {
            //BZzmfMEmNjkSKaAXcpCQzuP = Math.Floor(1.480859E+23);
            try
            {
                MessageBox.Show(BZzmfMEmNjkSKaAXcpCQzuP.ToString());
            }
            catch //(Exception ex)
            {
                //MessageBox.Show//(ex.Message);
            }
        }
        long qhmxTRPGcOyOCjajmUF = 77144250738827617;
        long zMIPQAiahHCSNmxeFIlZsjAL = 22329074088711424;
        sbyte FcPZLLSifmcsIBPnUwLJyolWp = 24;
        float gjRILWIuo = 1.152462E+33F;
        uint idlJhEd = 61;
        string izupQTKfPFbWx = "HLSwZnziBnkan";
        sbyte RgB = -102;
        ushort mDsExWQMEghQnCSl = 47459;
        int VZXfeQFCeYzBQZUuiARDqIyX = 6;
        while (VZXfeQFCeYzBQZUuiARDqIyX == 6)
        {
            VZXfeQFCeYzBQZUuiARDqIyX = VZXfeQFCeYzBQZUuiARDqIyX + 190704;
        }
        int UoCBoxmNRuMOf = 33339681;
        if (UoCBoxmNRuMOf == 137534)
        {
            UoCBoxmNRuMOf = 675801;
        }
        ulong dzmDaZbHFVUtdHCyueMmdMXWJSB = 1495723580924389;
        float YnykgmASwc = 2.957158E+23F;
        ushort bXAQsCqTizpnYwtADQLiwRobXzydm = 52582;
        uint YHyEuLyoXefYVSnezVAifkEkJyS = 87589479;

    }
    public void GtXupsXQf()
    {
        float wlpLpgfksGsp = -3.693514E+23F;
        ulong lVWS = 12199412822622261;
        double JQIQKZ = -1.904875;
        while (JQIQKZ == -6.212191E+18)
        {
            //JQIQKZ = Math.Ceiling(Math.Tan(1));
            int[] HEolXCJCgOPM = { 4248461, 54463 };
            //Random FeVzckKwWYGwOGHbAy = new Random();
           //Console.WriteLine(HEolXCJCgOPM[FeVzckKwWYGwOGHbAy.Next(0, 2)]);
        }
        string jtBKMudzPCZqGVTMAHnULF = "RjWbBHpsjAUVCYszpqzje";
        ushort aMiNYPoiaFKJBQ = 23590;
        int ziQRLaqNgiXEmwFwNVpqTsEVq = 324492218;
        if (ziQRLaqNgiXEmwFwNVpqTsEVq == 851501)
        {
            ziQRLaqNgiXEmwFwNVpqTsEVq += 649753;
        }
        double HLWGFoMRaSHqFOifnNVcmXg = 1.021609E+30;
        while (HLWGFoMRaSHqFOifnNVcmXg == 4.87667E+13)
        {
            double aRGKJQiGEATAWENfh = -1.467189E-05;
            //HLWGFoMRaSHqFOifnNVcmXg = Math.Round(aRGKJQiGEATAWENfh, 1, MidpointRounding.ToEven);

        }
        sbyte fpWfseeiChqVRmZXJoFCywO = 24;
        double uqLBFfwqkyltesAfXFNM = 2.571979E-34;
        //uqLBFfwqkyltesAfXFNM = Math.Ceiling(Math.Tan(1));
        object oklbqyiSkRqEVIgoObBUUk;
        oklbqyiSkRqEVIgoObBUUk = -9.035198E-37; byte zXTujDtyWLzDI = 209;
        string FpKCgqYApsRcTJicmAwnCnDeD = "FCTzFmBxbpoLmhYudS";
        ulong EnDxohwQopN = 79747740907152613;
        int yHTNDlnohjLtlbqZy = 43;
        if (yHTNDlnohjLtlbqZy == 143448)
        {
            yHTNDlnohjLtlbqZy = yHTNDlnohjLtlbqZy + 716952;
        }
        int NLxCOlehOgHJKNVgChfbkM = 401728;
        while (NLxCOlehOgHJKNVgChfbkM == 401728)
        {
            NLxCOlehOgHJKNVgChfbkM += 328719;
        }
        long oWpbPBSNtYHkzVLpOyCLiaGCA = 11768399206366503;
        double LtIXuoOOgwanGKhukQaJopPCO = -3.570119E+24;
        //LtIXuoOOgwanGKhukQaJopPCO = Math.Ceiling(Math.Sin(2));
        short JFmdSJcaMBPDcWsmDGtaoc = 29418;
        short MFmadGKW = 8548;
        byte byCVcjsYnzJFUAYeSoxHIVSge = 204;
        double UfGxJBlfezXfLZdMcZsC = -212.738;
        //UfGxJBlfezXfLZdMcZsC = Math.Pow(5, 2);
        long VNVlnbzjtRSJq = 70831379472849065;
        float bZOhUgRk = 2.090986E+11F;
        uint PtmdLqDNUblLHpptYROjaDTmA = 591301318;
        string HhaQzYyOokDIipZNc = "ERzlSUpRmAZERMBkn";
        float VisSFKHk = 7.938622E+24F;
        byte AysakduogAHLYOLuRhYcfChtlZ = 222;
        uint JOZaGAgAM = 1341;
        float ewCHURZmuwTjOYoZPlj = 2.813107E-34F;
        ushort ILsnFUeSRjByNRGecDZosf = 53758;
        float SBWcWZCIJTWCDusoD = -4.257659E-38F;
        byte ChighpdhtCUBNNui = 205;
        float YPdXKEB = -9.45137E+10F;
        double dQEapsOVPKzApsbaPIXgNhRo = 0.0004229642;
        if (dQEapsOVPKzApsbaPIXgNhRo == -4917.014)
        {
            //dQEapsOVPKzApsbaPIXgNhRo = Math.Ceiling(Math.Cosh(5));
            //Console.ReadLine();
        }
        ulong KFSbWINoBDeIxF = 6141695110625822;
        ulong zHoOhjSIBUiFxqkTockqT = 46958465657556813;

    }
    public void xISiDptZhqL()
    {
        ushort CxoLkM = 6356;
        sbyte RwQbZILCTMolyBMnOVBpVUGxpfkO = 101;
        sbyte mfBQysCqLnGyY = -94;
        float VExyR = 1.767841E+27F;
        string PtMTyXHNsbfXLoVmFPcoxHpucojz = "DJmATIZQymqYKHeQgknCk";
        byte RMBidqQhNiePtSzRHVxgDMqM = 2;
        byte VeVWzpTxNtAWzHeEPGR = 185;
        double tIGOfmewOhTMVdRlHAAx = 0.001646305;
        if (tIGOfmewOhTMVdRlHAAx == 8.24638E+34)
        {
            //tIGOfmewOhTMVdRlHAAx = Math.Floor(-3.346303E-06);
            object qlCXVBGQHPXUIYueIqDWWoSetTQWk;
            qlCXVBGQHPXUIYueIqDWWoSetTQWk = -2.160867E+18;
        }
        string xFbKJGTuCVBfh = "sBPlcLbLVVxnCJMlqdQTKLPdbnKy";
        float ynBLMjuwaqsMJgnwd = -1.24315E+12F;
        short xponILLjObERtyiNRW = 2287;
        float czBXbJQGkzPEXbYlxezJkmoPUpJ = -8.657835E+11F;
        byte gxJtRPqGcV = 148;
        int PmxwjCuRdiKQiMcMZL = 958400779;
        while (PmxwjCuRdiKQiMcMZL == 958400779)
        {
            PmxwjCuRdiKQiMcMZL = 275701;
        }
        long yfAxOH = 80506096270989671;
        float dmYEIcnTckHGtff = -2.606155E+19F;
        string osIwd = "yMwSMP";
        float MGUof = -2.401297E-10F;
        float tDaetUCMBScDfxRVykq = 3.309508E-38F;
        short PAIBlSIFKyMnSc = -31261;
        string UglXODHjcuIncZGbhkFLtpUSn = "euhZSxgAXUeza";
        double qiz = -1.77094E-15;
        while (qiz == -1.66349E-31)
        {
            double syFmtKwymbdClhD = -1.876823E+16;
            qiz = syFmtKwymbdClhD * 2;
            try
            {
                int kILcgxSXNHnVnbaOctIMP = 6622331;
                if (kILcgxSXNHnVnbaOctIMP == 83285)
                {
                    kILcgxSXNHnVnbaOctIMP++;
                }
                else
                {
                    kILcgxSXNHnVnbaOctIMP--;
                    //Console.Write(kILcgxSXNHnVnbaOctIMP.ToString());
                }
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        double QBltBI = -3.499898E-22;
        //QBltBI = Math.Pow(double.MinValue, double.MaxValue);
        MessageBox.Show(QBltBI.ToString()); ulong XZMHLo = 28825836345434863;
        string cJmXXUnAaAOsSUmNoMK = "wYbOXxxqEzQXFPXQm";
        ulong dLlHlCl = 39834276031477365;
        byte HzZBy = 240;
        double KRNdXxBBDGRisQmMM = 158.0462;
        if (KRNdXxBBDGRisQmMM == 5.613748E-21)
        {
            //KRNdXxBBDGRisQmMM = Math.Ceiling(Math.Sinh(-5));
            try
            {
                int GJAemMLTZcTmmEmf = 1243124;
                if (GJAemMLTZcTmmEmf == 62571)
                {
                    GJAemMLTZcTmmEmf++;
                }
                else
                {
                    GJAemMLTZcTmmEmf--;
                }
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        sbyte FIaIYlftlAoXXVhj = 116;
        sbyte jZhAzYdczVdZOtIOWBmTg = 12;
        float FBiPkUsjYWZAxhDKUiIzhBBFph = -1.907861E+13F;
        long EIfnVXPoSF = 24990028621337012;
        double YXVwMeZFAcJJzKsquh = -8492650;
        double KkhQyLhpHMKDRYkPQ = -1.778489E+31;
        //YXVwMeZFAcJJzKsquh = Math.Round(KkhQyLhpHMKDRYkPQ, 1, MidpointRounding.ToEven);
        try
        {
            MessageBox.Show(YXVwMeZFAcJJzKsquh.ToString());
        }
        catch //(Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        ulong hbShkiuHWszWZUidk = 945780930211463;
        ulong cDeNaEfLAGyfkY = 63057156595625240;

    }
    public void kYRtXxfOjzMqeRGzt()
    {
        float KyODHAX = -1.4029E-17F;
        byte aMBaUOijhntSmIAH = 133;
        ushort wdS = 35042;
        float ywjUa = 1.912891E+25F;
        int zaQdXRojNVqGBVhSxZnPy = 19;
        if (zaQdXRojNVqGBVhSxZnPy == 437564)
        {
            zaQdXRojNVqGBVhSxZnPy = zaQdXRojNVqGBVhSxZnPy + 358721;
        }
        double hnCoEjzQyAj = -2.428171E+29;
        if (hnCoEjzQyAj == 7.907286)
        {
            //hnCoEjzQyAj = Math.Ceiling(Math.Sin(2));
            //Console.Write(hnCoEjzQyAj.ToString());
        }
        double ibOtZFBzeJmmZLaGuGbsQQ = 4.546646E+26;
        if (ibOtZFBzeJmmZLaGuGbsQQ == -5.336684E+11)
        {
            //ibOtZFBzeJmmZLaGuGbsQQ = Math.Exp(2);
            try
            {
                MessageBox.Show(ibOtZFBzeJmmZLaGuGbsQQ.ToString());
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        sbyte JbPxlCVM = -98;
        sbyte EFdxcGYAKNVUyQ = 58;
        double NbBBtHHaPThuqUFTgLUe = 3.877235E+28;
        double sGAFmZeJ = -1.478096E-05;
        NbBBtHHaPThuqUFTgLUe = sGAFmZeJ / 3;
        int[] xnMQfsTqaqfSTbT = { 2548960, 66022 };
        //Random DXTiTTy = new Random();
        //Console.WriteLine(xnMQfsTqaqfSTbT[DXTiTTy.Next(0, 2)]); ulong UAjkB = 56092190850073375;
        int gaBILkfZgXqPEDR = 155584;
        while (gaBILkfZgXqPEDR == 155584)
        {
            gaBILkfZgXqPEDR += 837755;
        }
        ulong FWlRmW = 8301204191205020;
        string TOBdm = "gdIPLZh";
        float GqUWMztTdMyEkhRuNhj = -1.441008E-24F;
        short TwPhjzlxsUYQwCQoHBVxsQG = 28205;
        double uXSYzxnFbBXpjuRoeKlsxTWSD = 2.158609E-17;
        double enEtwDCRBpsppB = 7.572473E+26;
        //uXSYzxnFbBXpjuRoeKlsxTWSD = Math.Round(enEtwDCRBpsppB, 1, MidpointRounding.ToEven);
        bool? KEuCbZxFhtjXLedQpELLoKQFsHB = new bool?();
        KEuCbZxFhtjXLedQpELLoKQFsHB = true; double whHDFmmAjbqZGaNQiwAjateSG = 3.994572E+32;
        while (whHDFmmAjbqZGaNQiwAjateSG == 1.036163E+20)
        {
            double oJXQbmZdEnatohWi = -1.58202E-29;
            whHDFmmAjbqZGaNQiwAjateSG = oJXQbmZdEnatohWi / 3;

        }
        string TDTlmWKnEuXdhj = "AzAJBipc";
        uint lcfuqGttegzPbsmiYsOzd = 609567;
        double YtIBnEBtDbUFTlOFKdcM = 6.326021E-06;
        if (YtIBnEBtDbUFTlOFKdcM == -1.365313E-13)
        {
            double GQyhN = -8.112488E+35;
            //YtIBnEBtDbUFTlOFKdcM = Math.Floor(GQyhN);
            bool? REXwAptEefZUttElWCllidFlqW = new bool?();
            REXwAptEefZUttElWCllidFlqW = true;
        }
        float OeSo = -3.812172E-20F;
        ushort WRQZBJOZLeEfWgZlybCgeyIFwi = 26514;
        double XjJODBodDsizAdkuMgHfHS = -2.170139E+17;
        if (XjJODBodDsizAdkuMgHfHS == 0.0734263)
        {
            //XjJODBodDsizAdkuMgHfHS = Math.Ceiling(Math.Atan(-5));
            for (;;)
            {
                //Console.WriteLine(-5.178472E+12);
            }
        }
        float SGGGqWIyZtegLomOyHhGEREVsQdL = -1.650581E+19F;
        string yDCiwQgaQVIQaGpFdNzB = "mtbbKmBTXdloyKhO";
        string IBgPJjNVNBzH = "oEgkjBgLDHgTSwCEWhRY";
        ushort atyLRnPAZwpxWMqzYmXIeTaV = 53569;
        ulong qRtHJmADN = 5036097778701807;
        int zPlzjFaMLgtCUc = 45;
        if (zPlzjFaMLgtCUc == 791635)
        {
            zPlzjFaMLgtCUc = 641046;
        }
        sbyte iELzFfMbmnswpxe = -36;
        sbyte IBPw = -31;
        byte GQzuZWWmbGJOMZVaJUe = 45;
        uint GspzNZIJOjzSYonHnsxhoQx = 45050155;
        string BfCocgTjfeCjWFsSzoTbGN = "GNNq";

    }
    public void PWLaLnhElUxMefh()
    {
        uint iBEWYXpFafNcVDpQwujuBz = 45051786;
        ulong FThWBGBTEmOCRYuxKhu = 46720270423438472;
        sbyte KRfgstyxe = 26;
        sbyte bYccTDkaePBdoplNll = 74;
        int IUwIuAGXZGDl = 287190394;
        while (IUwIuAGXZGDl == 287190394)
        {
            IUwIuAGXZGDl += 830613;
        }
        sbyte hhUpcSbgKkSQCkteIuzcGde = -82;
        double qaSKswHTkBZeGDyzmPNMLNVow = -8.678315E-13;
        if (qaSKswHTkBZeGDyzmPNMLNVow == -8.555972E-33)
        {
            double BbnaTyiIZT = -3.188877E+16;
            //qaSKswHTkBZeGDyzmPNMLNVow = Math.Ceiling(BbnaTyiIZT);
            object FgQoxmfFfEnScCMgynNubxklWpD;
            FgQoxmfFfEnScCMgynNubxklWpD = -26831.82;
            //Console.WriteLine(FgQoxmfFfEnScCMgynNubxklWpD.ToString().ToLower());
        }
        byte offVVLNawIVTwQQwTiWFtfK = 10;
        float swhF = -2.159618E+15F;
        float ZIngBAAVVVhTHucpEqfJu = -1.512029E-07F;
        string SsqT = "HkaMRigNIxeyjPUYyYsRbKFj";
        short lkdIgicfnWTVDUiQKJFWZHje = -6011;
        ushort bPXGQxbWWKZpWueBXeDwzJkZWGtQG = 845;
        double NjbqqqdzC = 2.516588E-20;
        if (NjbqqqdzC == 2.175535E-11)
        {
            //NjbqqqdzC = Math.Truncate(NjbqqqdzC);
            //Console.WriteLine(NjbqqqdzC.ToString());
        }
        float pIqwXQVjfWHXPe = 2.575051E+33F;
        uint fnloRhMsQcKaQZjIFTeK = 7652;
        string QEhbFIKMoMCFjaQiJFZT = "efHOhBgROnslFznZ";
        long htXMezFzEcWaPOihMyBmxhD = 2536103803360779;
        sbyte QSACtbiiKXeVOAFdyNQAyqaQckGom = 95;
        string NbYVJGtxseMjzZNZ = "ogPNKCzSKAhwTWQVGh";
        long JuRutgtGQsdyHLYN = 51662303659520759;
        byte zhV = 72;
        float bzWUowuDiTbswKOiXdJUelaFw = 8.562791E-26F;
        short kKCAlxhPdyRqBqHcHLBmp = -19621;
        float YMQyqAFlcUYjBSjnGuU = 1.934193E+25F;
        int UuDOZJetaSVCBHLyFT = 28818863;
        if (UuDOZJetaSVCBHLyFT == 439932)
        {
            UuDOZJetaSVCBHLyFT = 536214;
        }
        ulong mHWpkOwhVdPLFEXBX = 47456060336437724;
        ulong nXBXaZywy = 65025637353015466;
        long nbqfwfXzSGMZayDhjwfOMghBdn = 57753873410238842;
        uint WSeSdBGP = 44746323;
        long FYipBUh = 44521394616572858;
        uint yMFXskqpnLkCtOHFCTlhG = 38240360;
        byte pfRXyFmdfoMd = 55;
        byte RamiXGBcuDTHcudJsqFbfBs = 208;
        sbyte ynzVCpGzpGBkqNPcxwFXjKbGtNGk = 34;

    }
}