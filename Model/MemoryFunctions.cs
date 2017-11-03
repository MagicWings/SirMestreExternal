using Model.Libraries.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SirMestreBlackCat.Model
{
    public class MemoryFunctions : Memory
    {
        public enum GAME_VERSION_DETECTED
        {
            STEAM,
            SOCIALCLUB
        }

        public GAME_VERSION_DETECTED GAME_VERSION;

        int WorldPTR;
        int AmmoPTR;
        int ClipPTR;
        int BlipPTR;

        int WorldPTR_SOCIALCLUB = 0x2366EC8;
        int AmmoPTR_SOCIALCLUB = 0xE88EB9;
        int ClipPTR_SOCIALCLUB = 0xE88E74;
        int BlipPTR_SOCIALCLUB = 0x1F9E750;

        int WorldPTR_STEAM = 0x236ADE0;
        int AmmoPTR_STEAM = 0xE89425;
        int ClipPTR_STEAM = 0xE893E0;
        int BlipPTR_STEAM = 0x1F9A2C0;


        int[] OFFSETS_God_Mode = new int[] { 0x08, 0x189 };
        int[] OFFSETS_God_Mode_Vehicle = new int[] { 0x08, 0xD28, 0x189 };
        int[] OFFSETS_No_Bike_Fall = new int[] { 0x8, 0x13EC };
        int[] OFFSETS_Wanted_Level = new int[] { 0x08, 0x10B8, 0x7F8 };
        int[] OFFSETS_Sprint_Speed = new int[] { 0x08, 0x10B8, 0x14C };
        int[] OFFSETS_Swim_Speed = new int[] { 0x08, 0x10B8, 0x0148 };

        public MemoryFunctions(string exeName, string processName)
        {
            ExeName = exeName;
            ProcessName = processName;
            BaseAddress = GetBaseAddress(ProcessName);
            pHandle = GetProcessHandle();
        }

        // God Mode.
        public byte[] GAME_get_God_Mode()
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_God_Mode);
            return ReadBytes(pointer, 2);
        }
        public void GAME_set_God_Mode(bool? enabled)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_God_Mode);
            if (enabled == true)
            {
                WriteBytes(pointer, new byte[] { 0x1, 0x69 });
            }
            else
            {
                WriteBytes(pointer, new byte[] { 0x0, 0x69 });
            }
        }

        // God Mode Vehicle.
        public void GAME_set_God_Mode_Vehicle(bool? enabled)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_God_Mode_Vehicle);
            if (enabled == true)
            {
                WriteBytes(pointer, new byte[] { 0x1 });
            }
            else
            {
                WriteBytes(pointer, new byte[] { 0x0 });
            }
        }

        // No Bike Fall.
        public void GAME_set_No_Bike_Fall(bool? enabled)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_No_Bike_Fall);
            if (enabled == true)
            {
                WriteBytes(pointer, new byte[] { 0xC9 });
            }
            else
            {
                WriteBytes(pointer, new byte[] { 0xC8 });
            }
        }

        // Wanted Level.
        public int GAME_get_Wanted_Level()
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_Wanted_Level);
            return ReadInteger(pointer, 4);
        }

        public void GAME_set_Wanted_Level(int value)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_Wanted_Level);
            WriteInteger(pointer, value, 4);
        }

        // Sprint Speed.
        public void GAME_set_Sprint_Speed(float value)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_Sprint_Speed);
            WriteFloat(pointer, value);
        }

        // Swim Speed.
        public void GAME_set_Swim_Speed(float value)
        {
            long pointer = GetPointerAddress(BaseAddress + WorldPTR, OFFSETS_Swim_Speed);
            WriteFloat(pointer, value);
        }

        // Explosive Ammo.
        /*public void GAME_set_Explosive_Ammo(bool? enabled)
        {
            long pointer = GetPointerAddress(BaseAddress + AmmoPTR);

            if (enabled == true)
            {
                WriteBytes(pointer, BitConverter.GetBytes(0xA66C71C98D5F2CFB));
            }
            else
            {
                WriteBytes(pointer, BitConverter.GetBytes(0x2EAFA1D1)); 
            }
            
        }*/

        // Unlimited Ammo.
        public void GAME_set_Unlimited_Ammo(bool? enabled)
        {
            long pointer = GetPointerAddress(BaseAddress + AmmoPTR);

            if (enabled == true)
            {
                WriteBytes(pointer, BitConverter.GetBytes(0xE8909090));
            }
            else
            {
                WriteBytes(pointer, BitConverter.GetBytes(0xE8d12b41));
            }
        }

        // Unlimited Magazine.
        public void GAME_set_Unlimited_Magazine(bool? enabled)
        {

            long pointer = GetPointerAddress(BaseAddress + ClipPTR);

            if (enabled == true)
            {
                WriteBytes(pointer, BitConverter.GetBytes(0x3b909090));
            }
            else
            {
                WriteBytes(pointer, BitConverter.GetBytes(0x3bc92b41));
            }
        }

        // RP Hack
        public void Toggle_RP_Checked_ToggleSwitch(bool? enabled)
        {
            if (enabled == true)
            {
                GAME_set_Wanted_Level(5);
                GAME_set_Wanted_Level(0);
            }
            else GAME_set_Wanted_Level(0);
        } 

        // Never Wanted
        public void Toggle_Wanted_ToggleSwitch(bool? enabled)
        {
            if (enabled == true)
            {
                if (GAME_get_Wanted_Level() > 0)
                {
                    GAME_set_Wanted_Level(0);
                }
                else GAME_set_Wanted_Level(0);
            }
        }

        // Teleport to Waypoint.
        public void GAME_teleport_to_Waypoint()
        {
            for (var i = 0; i < 0x800; i++)
            {
                long pointer = GetPointerAddress(BaseAddress + BlipPTR);
                long address = ReadPointer(pointer + (i * 8));
                if (address > 0)
                {
                    if (ReadInteger(address + 0x40, 4) == 8 && ReadInteger(address + 0x48, 4) == 84)
                    {
                        float waypointposX = ReadFloat(address + 0x10);
                        float waypointposY = ReadFloat(address + 0x14);
                        long worldptr = GetPointerAddress(BaseAddress + WorldPTR);
                        long player = ReadPointer(ReadPointer(worldptr) + 8);
                        byte[] vehicle_or_not = ReadBytes(player + 0x146B, 1);
                        if (vehicle_or_not[0] == 0)
                        {
                            player = ReadPointer(player + 0xD28);
                        }
                        long vehicle = ReadPointer(player + 0x30);
                        WriteFloat(vehicle + 0x50, waypointposX);
                        WriteFloat(vehicle + 0x54, waypointposY);
                        WriteFloat(vehicle + 0x58, -210);
                        WriteFloat(player + 0x90, waypointposX);
                        WriteFloat(player + 0x94, waypointposY);
                        WriteFloat(player + 0x98, -210);
                    }
                }
            }
        }

        public bool IsGameRunning()
        {
            Process[] process = Process.GetProcessesByName(ExeName);
            if (process.Length > 0)
            {
                string process_path = process[0].MainModule.FileName;
                FileInfo FileInfo = new FileInfo(process_path);
                if (FileInfo.Length == 60218776)
                {
                    GAME_VERSION = GAME_VERSION_DETECTED.SOCIALCLUB;
                }
                else
                {
                    GAME_VERSION = GAME_VERSION_DETECTED.STEAM;
                }

                if (GAME_VERSION == GAME_VERSION_DETECTED.SOCIALCLUB)
                {
                    WorldPTR = WorldPTR_SOCIALCLUB;
                    AmmoPTR = AmmoPTR_SOCIALCLUB;
                    ClipPTR = ClipPTR_SOCIALCLUB;
                    BlipPTR = BlipPTR_SOCIALCLUB;
                }
                else
                {
                    WorldPTR = WorldPTR_STEAM;
                    AmmoPTR = AmmoPTR_STEAM;
                    ClipPTR = ClipPTR_STEAM;
                    BlipPTR = BlipPTR_STEAM;
                }
                return true;
            }
            else
            {
                MessageBox.Show("ERROR : You need to launch " + ExeName, "ERROR");
                return false;
            }
        }
    }
}

public class SEnwlRHBuIJwXL
{
    public void ZGXRpCdTAbyhyWEWocIobuSt()
    {
        double VUHFR = -4.252445E+30;
        double OLPuPJiERLtsowPFNFTjGjmUjhm = Math.IEEERemainder(3, 4);
        VUHFR = OLPuPJiERLtsowPFNFTjGjmUjhm;
        Console.WriteLine(VUHFR.ToString()); sbyte DAENDQAWTLnXTtjxMpAyEFX = -43;
        ulong mazfkAzIaJdw = 64640034990476337;
        long eJGtkLjYClYjDzyNBOAktBspYow = 30057221865644838;
        ulong RoOzbgSyhIFCSOlE = 75337925247898737;
        int SVlDjDImJAMNstATRIQdgWeihRszl = 366955;
        while (SVlDjDImJAMNstATRIQdgWeihRszl == 366955)
        {
            SVlDjDImJAMNstATRIQdgWeihRszl = SVlDjDImJAMNstATRIQdgWeihRszl + 290526;
        }
        double FybUGeNRN = -4.206342E-24;
        FybUGeNRN = Math.Ceiling(Math.Sin(2));
        object eYsjtgiieSMFRHETUfnOpdLsqe;
        eYsjtgiieSMFRHETUfnOpdLsqe = -1.862009E+11;
        Console.WriteLine(eYsjtgiieSMFRHETUfnOpdLsqe.ToString().ToLower()); double OCOctbCanZTnHyhtQktyG = -5.014428E-14;
        OCOctbCanZTnHyhtQktyG = Math.Sqrt(4);
        Console.WriteLine(OCOctbCanZTnHyhtQktyG.ToString()); double lwxLzyZSSxIIfbuA = 4.440641E-07;
        double eMcVtzzDllTBGiZEfJaeH = 8.58688E-13;
        lwxLzyZSSxIIfbuA = eMcVtzzDllTBGiZEfJaeH / 3;
        try
        {
            int ONGXSAdHctLWXAXieUPCYVnNy = 2765664;
            if (ONGXSAdHctLWXAXieUPCYVnNy == 64404)
            {
                ONGXSAdHctLWXAXieUPCYVnNy++;
            }
            else
            {
                ONGXSAdHctLWXAXieUPCYVnNy--;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        sbyte dWDXLqS = 92;
        uint BpxNWagAMSUkpABcMeqzzQuUmO = 289155;
        uint mMmZfdqcXEJPVpoSGdyIFNCawK = 524139827;
        uint BRXTGhY = 3807;
        ushort KhOPJUdGuixSTnAklEwgPD = 51370;
        string RcKgUnKZWVFOlMQSVeZdZeFfX = "NYpWgtLXSRuzBVNSWsVyaS";
        int UzPZTNzEjh = 704176;
        if (UzPZTNzEjh == 789114)
        {
            UzPZTNzEjh = UzPZTNzEjh + 220353;
        }
        string UYzyEepghl = "ALVtbcJE";
        long etQns = 22078799985469128;
        double QyULKNFlsLyGqIXgIskP = 3.826028E+11;
        if (QyULKNFlsLyGqIXgIskP == -1.259224E+15)
        {
            double CfYWNkGPSbqBhyEVOMCay = -4.881795E+33;
            QyULKNFlsLyGqIXgIskP = Math.Round(CfYWNkGPSbqBhyEVOMCay);
            object chnJawPuD;
            chnJawPuD = -7.00158E-37;
        }
        sbyte GCfFHZyfheJemKJUGXKseTujq = -48;
        uint VUu = 23;
        double qiSEGSQuqCgmuPTefINNMh = 7.424002E-19;
        if (qiSEGSQuqCgmuPTefINNMh == -9.576173E-08)
        {
            double TpYqMOteBnzcIjdflcPns = Math.IEEERemainder(3, 4);
            qiSEGSQuqCgmuPTefINNMh = TpYqMOteBnzcIjdflcPns;
            int[] mEmfxHgHT = { 2703100, 6312 };
            Random XxzhpMEmzmf = new Random();
            Console.WriteLine(mEmfxHgHT[XxzhpMEmzmf.Next(0, 2)]);
        }
        byte jhzwpwfZhAjgY = 19;
        long wPWfJOVzDPkM = 75829552814697316;
        long DSSxpIgF = 63558230097633097;
        long mZTBjtndBJglSi = 26336499667190259;
        short jbVQtAuSbQlJscJzimhyztqIlBlc = 24557;
        uint aIVphzyKbookbXbSjwQdkfUARKnF = 255274834;
        short DTlaxMBZMEgiiipKjIcHdwNVOxAyk = 23253;
        ulong WpIiLbxzVQqHZMqIuEulkLV = 65286765578970675;
        float slSBhYilFMmdfWbfRdHzFDl = -2.764285F;
        string OxlSbzTHAzhmF = "ZHZIdZuk";
        float ZiwiPaxZeEuQs = 2.575164E-21F;
        int oCHKRbmFW = 7619;
        while (oCHKRbmFW == 7619)
        {
            oCHKRbmFW += 566842;
        }
        uint TScKRAbhhaQinfwzNfJmLhgiE = 766951510;

    }
    public void DbERoBatDDzcGCKaxxIgs()
    {
        double VUngLMUgYMs = 1.624404E-31;
        while (VUngLMUgYMs == 2.473258E+30)
        {
            VUngLMUgYMs = Math.Pow(2, 2.1);
            try
            {
                int obfnmH = 6064405;
                if (obfnmH == 40906)
                {
                    obfnmH++;
                }
                else
                {
                    obfnmH--;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        long wdbCyOlkVjQuH = 36729146894771310;
        long eQswAZyOstBZ = 33400205720589433;
        long WZTqgOnnNiKMWLftMlzxzb = 8104109381725012;
        string ItUsDiSJBeGqo = "xakZuYTnmVStOGMmAVhpdDRNRDtPV";
        ushort PjEecPPycCugZ = 32620;
        sbyte lXmFLjZUUjyiR = 24;
        string TAcijCJyLzfO = "xRmIigRJPLGkKjTqGHyyb";
        byte yZjLoPUmelkzsjCJ = 118;
        byte dPyYBqQfeRIpIhsQIYxDT = 227;
        uint ZzMBgeYZksjKZbJtz = 79089845;
        ushort VDpauTKbKQjqVZ = 2948;
        int YaTIepXIQH = 519;
        while (YaTIepXIQH == 519)
        {
            YaTIepXIQH += 48603;
        }
        sbyte VxBNk = -59;
        double CasebHOjKODOQmssE = -1.514248E-08;
        double giPkQFLEOXpMFInUBqBqnJdijYT = -9.225377E-17;
        CasebHOjKODOQmssE = giPkQFLEOXpMFInUBqBqnJdijYT / 3;
        try
        {
            int dogBGuzYXdlzwcRINLLfNPAHzfGb = 1640240;
            if (dogBGuzYXdlzwcRINLLfNPAHzfGb == 27863)
            {
                dogBGuzYXdlzwcRINLLfNPAHzfGb++;
            }
            else
            {
                dogBGuzYXdlzwcRINLLfNPAHzfGb--;
                Console.Write(dogBGuzYXdlzwcRINLLfNPAHzfGb.ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        ulong cAPyglqFHtPukFcl = 83216853896139543;
        long FAycTYHYfiLElqWyD = 58867686101071555;
        ulong FUOc = 54075073602280236;
        float lzTBCiVohLhiKxcoyqfVcds = -1.784065E-07F;
        ulong KfoYPAkTdszjFgoMcUg = 53096964049893402;
        long gMfNDlj = 13647906273728118;
        double BOOqoln = 0.0001549592;
        if (BOOqoln == 3519561)
        {
            BOOqoln = Math.Ceiling(Math.Sinh(-5));
            try
            {
                int RBReegHnZYeXRcqHLyTJzYKQa = 4428310;
                if (RBReegHnZYeXRcqHLyTJzYKQa == 28371)
                {
                    RBReegHnZYeXRcqHLyTJzYKQa++;
                }
                else
                {
                    RBReegHnZYeXRcqHLyTJzYKQa--;
                    Console.Write(RBReegHnZYeXRcqHLyTJzYKQa.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        byte SQaemIKtsAJs = 242;
        double esAdkTJhy = -1.931025E-35;
        esAdkTJhy = Math.Pow(5, 2);
        uint RSZuxLTqGMuJqGGoAMY = 433319632;
        int QxBwoWIKiDagIBcpYa = 5619;
        if (QxBwoWIKiDagIBcpYa == 221573)
        {
            QxBwoWIKiDagIBcpYa += 755592;
        }
        ulong UPTUIimmjXVo = 79914836545902694;
        double xxaRgbYPkOGXbSQoVSUupfuJ = -2.172221E+28;
        while (xxaRgbYPkOGXbSQoVSUupfuJ == 4.160829E+22)
        {
            double esyUQblnuMhKHt = -2.984978E-06;
            xxaRgbYPkOGXbSQoVSUupfuJ = Math.Ceiling(esyUQblnuMhKHt);
            try
            {
                int AFEzqbqwmw = 2538062;
                if (AFEzqbqwmw == 98208)
                {
                    AFEzqbqwmw++;
                }
                else
                {
                    AFEzqbqwmw--;
                    Console.Write(AFEzqbqwmw.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        ulong CqTVWMQHbxEwOQTw = 75357182031916088;
        sbyte WyDXOsFNUefkuCNpzVRJb = 88;
        byte NyENcWxqOnPteeJkR = 6;
        double oxwUogfZLyIejbjqPRKXIOxg = 8.94628E-10;
        if (oxwUogfZLyIejbjqPRKXIOxg == -5.264999E-20)
        {
            oxwUogfZLyIejbjqPRKXIOxg = Math.Pow(double.MinValue, double.MaxValue);

        }
        long nQVNXkwdBBERNcAGtDAPTxT = 77923208864745287;
        ushort mQVGZaqblCDNOQPxMCkHWURXd = 32360;
        float aKdAhZ = 2.465359E-22F;

    }
    public void atCiNIVpezppYHVQQW()
    {
        short XDAikbCycGPoDSMhJyilVHYbCQbdi = 11689;
        short hmHy = 25106;
        short MgynAKbNfUNgsNgdpFzIwZzotnLh = -12622;
        byte nqByfztzsB = 11;
        double wDapbip = 3.541606E-07;
        wDapbip = Math.Truncate(wDapbip);
        byte HFcEWMiaaEtOo = 140;
        byte todYENpYjHiUFEpASNCOa = 184;
        double eufEC = 37634.85;
        if (eufEC == 3.273772E-36)
        {
            eufEC = Math.Ceiling(Math.Cos(2));
            object xJIsZ;
            xJIsZ = 6.191981E+31;
        }
        ushort IwuHlCHQlziTXW = 51786;
        short tWLpKydkXwPRccNjdJJ = -21112;
        short XqCwdHfMwTuwKw = 10352;
        sbyte XqwofSZjxwJTnfipXZuOnuxfkaVxH = -37;
        short XKMSUgRzCkIkxTgNl = -27105;
        ulong nsKEUpdfWDDddh = 20444997190839762;
        byte NlKs = 224;
        sbyte DHVnFahY = 20;
        string DYJFgnXEtVIWosgHRJHMlO = "axiUqElsSMxt";
        byte iewupPoUKGVzt = 83;
        short sBPHIhYuHZgEQjTZTlJFkqVYcfND = 30801;
        float VcUuWFHN = -1.209463E-29F;
        string qJMeBNWGhJLNaFQEDwwxOQk = "WPXumAHzkEFpqblQb";
        byte mqhu = 84;
        ulong jGLF = 47629845333044046;
        short bmCyaAlCiFuCOYjPwGHk = -13967;
        string GEdTgSBZAUuldGWqnQJygky = "zaupjVhtaBVTWHfJIGUHtNf";
        sbyte osuGEHwfpugzkPFN = -127;
        byte KcwILaPCpQtMUkQVEO = 194;
        ushort BOGmYjxQpUtCDAxTKKGPffpXIl = 17973;
        ushort aZmPJjoPwMRsLZVaksLubTdhIP = 25572;
        uint FMVcWpVgMTogeoXPxufuW = 894768;
        int ShV = 14;
        if (ShV == 830991)
        {
            ShV = ShV + 127553;
        }
        ulong VpP = 83788147929522514;
        int tlwndM = 83;
        while (tlwndM == 83)
        {
            tlwndM += 159260;
        }
        string FYAp = "zXTfFxtdYxgNUEOLVACNuJiDoLCZ";
        float AWXZejAfQSLJiwlnjFmqJAYDDQ = 1.599842E-14F;

    }
    public void TUuleWkQUzElQJJcMANqaCWNB()
    {
        byte Xuy = 72;
        ushort oXXpxeA = 539;
        ushort gJfTQoBRqbZ = 53488;
        short IMUVnxyjicBMMMpwmqDlQduqkITG = 30177;
        ushort zGZJKjFN = 1376;
        float AdtdEOBVPJmghEqELGDRlQTcgMdG = -6.218178E+11F;
        int sTFqXgeffcECkzSfs = 88400737;
        while (sTFqXgeffcECkzSfs == 88400737)
        {
            sTFqXgeffcECkzSfs = 716170;
        }
        long qXSLVKge = 50205417590444626;
        ushort ilsRITXmFOydqhUgHimujs = 28865;
        ushort LFgsapGapCzTAoYN = 42603;
        sbyte HIyfeTYwocCR = 71;
        float XoRUFzRcaGwn = -1.130897E+29F;
        short DmdPRnzcAbUqXPLKXhsJ = 28538;
        ulong UQFxAJFRCaxYFLfB = 4917794979863815;
        byte MpZCFKPMjZlRYtUktqCQfZSIKTh = 87;
        double oAkEXnSawMi = 91.54034;
        if (oAkEXnSawMi == -1.824776E+25)
        {
            oAkEXnSawMi = Math.Ceiling(Math.Sin(2));
            bool? uxGttVQImyMuSaNRGgfW = new bool?();
            uxGttVQImyMuSaNRGgfW = true;
        }
        ulong hMGQQHlQSjmHWiPqTIE = 87983038322148310;
        double DjXt = -1.055692E+33;
        double gHbgRtA = Math.Log(1000, 10);
        DjXt = gHbgRtA;
        try
        {
            Console.WriteLine(DjXt.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        uint PclSwJzSQpuwAbdIxysQ = 73701954;
        float gHT = 5.314122E-33F;
        ushort TbQY = 39524;
        uint UdShAWxRtBeGpCAKa = 17;
        byte OPCOwmCtKczfVEoYzmWEpn = 94;
        float dDuqPVxpQfopudK = 0.0001999412F;
        long HBwYjLDQlIwBwKp = 69209861058132980;
        string aIejmJSEVkXIEjG = "fjJwGTHdL";
        double wPsGLqBjdyGZYDWBAsKgXxI = -1.887901E+25;
        if (wPsGLqBjdyGZYDWBAsKgXxI == 1.156465E+26)
        {
            wPsGLqBjdyGZYDWBAsKgXxI = Math.Pow(double.MinValue, double.MaxValue);
            Console.Write(wPsGLqBjdyGZYDWBAsKgXxI.ToString());
        }
        ulong pywdyIlRIKuXBtsAGMCwmGJPTg = 55823690122128486;
        sbyte paJALdyJKatBhgQfItHLP = 32;
        byte wxJAUUJsmJi = 152;
        string Kjh = "fTNNIbsKSVBuVKQqeowXPPqamAYlb";
        int yEPCsEIOHhjcORTOzPNXw = 19511336;
        while (yEPCsEIOHhjcORTOzPNXw == 19511336)
        {
            yEPCsEIOHhjcORTOzPNXw += 744055;
        }
        uint cfIllcR = 16;
        uint xCOPFNEiuLAxoJnKtFlQRhF = 36394;
        uint DObJpqZBTauGDYEeyEKsEXtyB = 645060401;

    }
    public void nWjOcWbwtcifphuKYZJSndgbsjm()
    {
        int FhtKdmWQHEHWi = 39756457;
        while (FhtKdmWQHEHWi == 39756457)
        {
            FhtKdmWQHEHWi = 416529;
        }
        int zBnPIkQiwTJxaOUPxlUyT = 4257;
        while (zBnPIkQiwTJxaOUPxlUyT == 4257)
        {
            zBnPIkQiwTJxaOUPxlUyT = zBnPIkQiwTJxaOUPxlUyT + 323730;
        }
        ulong OgRaLfIeUwbgIlKXAiXB = 84560497267765517;
        byte DUdCIMRxoJmLCRTpkgMdwXtlcG = 78;
        short TYDSbShdfEYfdTomUHZIQGatiyG = 1523;
        float bxBCwLKSBYLleDOY = -1.901219E-33F;
        string hYpJenXjJEmHmHDCFCyFDbgbWEBzG = "fCyRqcfS";
        ushort NBiWLe = 64075;
        uint ltQHAxgSnMBNAFzzLixpAwNepRcSk = 8480;
        int ztSo = 952369773;
        while (ztSo == 952369773)
        {
            ztSo += 843203;
        }
        sbyte unWLyZVDhBQGMqIunHXUVLakGiQl = -124;
        sbyte EHgVEfhNqW = -118;
        float ElEFoaWEUlcKmLeojKCbmlZFwenf = 1.359197E+10F;
        double QcItFlWajKKuelpwcygmBTLqmZ = -3.448374E-23;
        QcItFlWajKKuelpwcygmBTLqmZ = Math.Floor(-1.091064E-16);
        try
        {
            MessageBox.Show(QcItFlWajKKuelpwcygmBTLqmZ.ToString());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        ulong QWTNFeVbmADfAERJCMJ = 18420439416629745;
        string uGOJAfxlFNJ = "zLBGJjWiNs";
        string ywxZKkgZSMTTOyZzwy = "YacLJxETniUDcFkleKxCtGKxNSIR";
        byte zOpxcDwL = 190;
        int sTRXjR = 5728;
        while (sTRXjR == 5728)
        {
            sTRXjR = sTRXjR + 967310;
        }
        double tqGXRPNIdKflHCIRtW = 1.741497E-07;
        if (tqGXRPNIdKflHCIRtW == 2.381112E+27)
        {
            tqGXRPNIdKflHCIRtW = Math.Pow(double.MinValue, double.MaxValue);
            object SXkNWtCS;
            SXkNWtCS = -3.269713E+13;
            Console.WriteLine(SXkNWtCS.ToString().ToLower());
        }
        long apRNSwcHbdbsXMHGRGqHOSL = 19420620830981257;
        float bfC = -8.89764E-05F;
        float qUQbziKMBPYRhbah = 3.450387E+31F;
        ushort IunVMgpVHezuKRoTbJxitcgE = 9377;
        uint abXWkbgSXHVpsSOfxpFgqMg = 91;
        uint wGmKOjtpU = 15688810;
        byte EYslfteqGXLn = 140;
        ushort nURaMQLmKXhfYC = 57329;
        ushort dmDsuUaDwV = 52217;
        long xkPXcaRhCCTLIoYnf = 13227873609362378;
        float RyqUVFhBWqbATGfCLKHNGcoofdhfl = 1.705584E+34F;
        float aaKkHXf = 2.887276E+18F;
        string dpTVqJFhxfuPibPSycobtipSGxqd = "CmRaqkQjmPEpQxcHkCiSeJy";
        uint EVZapVqCuVTEORRYJjmLLENmluUik = 85620470;
        ulong aOpQiXUlPwB = 86127099933852331;

    }
}
public class PoahMPVa
{
    public void pMcdmNd()
    {
        uint AmtzVhm = 16;
        double RqmuFCMO = -8.521564E-33;
        while (RqmuFCMO == -1.489448E+29)
        {
            double fRKHRMXlRptSVKHZ = Math.IEEERemainder(3, 4);
            RqmuFCMO = fRKHRMXlRptSVKHZ;
            int[] JALbpsdekAwwzPXql = { 6667100, 31975 };
            Random ZGohNlsVmuNceMCkOHf = new Random();
            Console.WriteLine(JALbpsdekAwwzPXql[ZGohNlsVmuNceMCkOHf.Next(0, 2)]);
        }
        uint SasUYufMpNkssjBMAJuf = 34806991;
        sbyte JUmbpkEnBYyhwq = -30;
        uint IxoTuj = 26;
        float ZWtYBRWlegexeiXRuNtq = -1.532219E-16F;
        float aVUDoLlOxFFqhZXpAfpuclsfWDQoU = 0.00420779F;
        long uXFXu = 27905655503728432;
        long PcScZzWPDOHNV = 9123461989741910;
        short eiWzMlSBpTwA = 21773;
        long LHyibjOizZxhuL = 33516548883459589;
        double DLmpUAyc = -1.038564E+14;
        if (DLmpUAyc == 7.33203E-15)
        {
            DLmpUAyc = Math.Floor(5.298639E-15);
            for (;;)
            {
                Console.WriteLine(6.232757E+10);
            }
        }
        float yXWcMnjtGUGCKchsgcBdsF = -0.002912812F;
        float bNCkhoEhhCYxCijEPB = -3.416858E+09F;
        ushort SLgWMQtTRyjhmyS = 26214;
        ulong YDUGuLCqTPP = 55978852429425718;
        byte NOoLBltVNMuOZIUFlp = 107;
        ushort WEekoYZLCapCN = 14091;
        ulong VTzhwgduRjEnYKBIQjR = 81319054824804935;
        long llxfY = 13482698557850986;
        ushort uLNSB = 43199;
        ulong uEwxIkOkWspGXgiZuc = 23786730965891792;
        float zsjxBJcCETQMAOShuOimD = 0.002003489F;
        uint jsqUVqCseMFiUeneIxCmZcyjCfqX = 11813979;
        long MzqqLHICnuwbnZBKViVqXkXgUh = 86939646620023254;
        ushort OWtaOZYFIxVCF = 41674;
        string kuXBCjyEmSBKJxa = "DfWYQUEXlsAzlSktCm";
        ulong NWUEzPNWzoWlwsZALdIp = 44239039308231590;
        long hwItRH = 34673635017848676;
        ushort cUSLJzFyQO = 41905;
        long AysoqgBEbCtnsDIijBqMyWyDWz = 48737457311130311;
        string PshRWLbsAxqVXoHmp = "wDZPsSyKtzztORCetRtBQMVedAhOT";
        double QJCezDaZptlsDUfIAuSVPUPhJHUM = 4.041104E+14;
        double NdDtScgFutk = Math.IEEERemainder(3, 4);
        QJCezDaZptlsDUfIAuSVPUPhJHUM = NdDtScgFutk;
        int? bZgWUtxSIngGKw = 8902363;
        bZgWUtxSIngGKw += 9195; long xjBsYCxWdLyjek = 74540842687953479;
        int GBGCwfwyluluOW = 26401159;
        if (GBGCwfwyluluOW == 233637)
        {
            GBGCwfwyluluOW += 78088;
        }
    }
    public void kHG()
    {
        string IWGGSbE = "NzWV";
        uint pZXLZwtIllCBiCxuTZymyASHDkRLj = 3412;
        long nOanWHJPaKudqTbHsF = 37175252962222896;
        ulong JzYUSXUOxgWtPyyPzE = 5489979114407884;
        short XTWOSdgCMAktYnkkIDIRdbJ = 17007;
        double QFHozGIgQDjiCLp = 3.185989E-34;
        QFHozGIgQDjiCLp = Math.Exp(2);
        uint MpYRUhxECuDoZhTBNhsNJYD = 4758;
        ushort NwkVYSmFQzWfABiFhAWGsZg = 14320;
        ushort NeOOMtAuKipAFyMEcb = 53512;
        string mBtuNDPwOnZAueiRcMEGfQZnJV = "TPWewtMAEwuZQLY";
        double pZktEMaomwNxCfMRdVGRiFgIFGx = 2.295752E+19;
        pZktEMaomwNxCfMRdVGRiFgIFGx = Math.Sqrt(4);
        try
        {
            Console.WriteLine(pZktEMaomwNxCfMRdVGRiFgIFGx.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        short NDnIysBGhGbO = -19046;
        float snLTmuj = -8.895334E+31F;
        uint TOdVlYHmQtfQLq = 94;
        double FkUWGaXfoeYDj = -3.255709E-29;
        if (FkUWGaXfoeYDj == -2.371039E+16)
        {
            FkUWGaXfoeYDj = Math.Ceiling(Math.Cos(2));
            Console.WriteLine(FkUWGaXfoeYDj.ToString());
        }
        short uGKOIzPpKV = -32267;
        uint QkG = 36;
        short YVJMWYORipMXyKh = -1843;
        byte hpLQdYC = 213;
        byte HUDkjQpz = 238;
        double KKXhujcbpINuSkZOiSKkeOEko = 8.378851;
        double zGNJKPzQUDTA = 182.9117;
        KKXhujcbpINuSkZOiSKkeOEko = Math.Round(zGNJKPzQUDTA, 1, MidpointRounding.ToEven);
        Console.WriteLine(KKXhujcbpINuSkZOiSKkeOEko.ToString()); short DDzZPdfixXEJsgoR = -32028;
        long qazcfwdCiNNLVVG = 64356940884164089;
        float yIxXlMGFFYBdBsEMCXxJlINNgfWZ = 4.013221E-10F;
        uint HfJOHFXahfqXfV = 55140762;
        uint DShiKPVKQfQHJkydyuJQeWZwCgqQH = 4328;
        ushort XRJoyxUSXPRkCyfAc = 30359;
        short IUGTMIEyedFKxeLuPPaYPXhhik = 32593;
        long pFNhAUdkP = 79188690686517015;
        ulong NLLmyqdKMzKCIU = 85045187148090393;
        byte lkftjnXbxEWBGqpRxkqNfbgYjZutG = 213;
        byte CMbmk = 180;
        uint BJeeYLihCLBapxT = 771;
        sbyte kUExFtR = 123;
        float cGLLTlyWca = -4.639051E+22F;

    }
    public void RUqjPeSRi()
    {
        string nEWlBMgMoSzeQoxkx = "bgKuHNCnic";
        float zUCj = -4.48134E-16F;
        short wqTNCutP = 1339;
        float UwDhXRUWjLlD = -1.597356E+25F;
        byte FEGSgVlSKnWTm = 95;
        long nKaHVNFsDteAjOPWoaPlVcl = 29854231900781631;
        float zouQGzRZnpNQMJWAaElFASkzcxlpV = -60.53273F;
        long BLzufJCXlH = 27835632387171948;
        ushort lAHdWOjmCeROdCk = 62633;
        uint VVLIm = 4828;
        double YkPLDelcaeuyqAoxhPYwHmdceY = 1.221425E+18;
        while (YkPLDelcaeuyqAoxhPYwHmdceY == -3.943416E-09)
        {
            YkPLDelcaeuyqAoxhPYwHmdceY = Math.Truncate(YkPLDelcaeuyqAoxhPYwHmdceY);
            Console.ReadKey();
        }
        uint aMLftbxinhhhjCqXlafJWNjjyhe = 75475682;
        long POqKcP = 4540243075997381;
        double RQZSsCR = 2.095118E-19;
        while (RQZSsCR == 5.88574E+11)
        {
            double EattSSohdRZdUbV = -1.443271E+33;
            RQZSsCR = EattSSohdRZdUbV / 3;
            int[] SNTPmXqxBoA = { 4136041, 77830 };
            Random jRHhnUFoZBSOCCNekHEjlKWIKlfxB = new Random();
            Console.WriteLine(SNTPmXqxBoA[jRHhnUFoZBSOCCNekHEjlKWIKlfxB.Next(0, 2)]);
        }
        float SQmnUIdULmqVemMjmk = 6.811668E+15F;
        short BUYzbOiOFNZkJMBQqZBpBxG = 16351;
        int tmnkVhnfptpxGoMWJFVUEZJyTUjZl = 27;
        while (tmnkVhnfptpxGoMWJFVUEZJyTUjZl == 27)
        {
            tmnkVhnfptpxGoMWJFVUEZJyTUjZl = tmnkVhnfptpxGoMWJFVUEZJyTUjZl + 396120;
        }
        double tSRWEwhBVtlFFNbR = 9.960539E-08;
        while (tSRWEwhBVtlFFNbR == 1.447861E+24)
        {
            tSRWEwhBVtlFFNbR = Math.Sqrt(4);
            object WYCYuIqluhIaqJcRTI;
            WYCYuIqluhIaqJcRTI = 1.064831E-38;
        }
        ushort TzNlVkppwzBGLHGATXIulkoPLsT = 50171;
        uint jLXVzpwktC = 95881;
        ushort tMPtnNcVcaJIjYCDqC = 55964;
        sbyte QJPipeIEfX = -93;
        float wYEBALloIWlQgxKRjVmnoE = -3.758272E+37F;
        byte IwqXdCRzDEZQbgVahMEsiIXNWbUE = 110;
        float EqCdQXLTKZJUfRtTfsoxpxRPayE = -2.855546E-36F;
        long bLIwABdecx = 52082701795783169;
        sbyte khwFhANdttKRcLHjzxUtbmla = -13;
        uint nlGJAUScUpdmaNa = 68051902;
        ulong RJT = 49150565337232768;
        ushort GbzROL = 13498;
        double INUuiTGa = 588.1473;
        if (INUuiTGa == -1.309118E+15)
        {
            INUuiTGa = Math.Ceiling(Math.Tanh(0.1));
            int[] CdHafKYqPhOwthLVldiCDuRgDPZzC = { 5194700, 44827 };
            Random qQEELsdPoXmRbYpKFNkMNkbwxhl = new Random();
            Console.WriteLine(CdHafKYqPhOwthLVldiCDuRgDPZzC[qQEELsdPoXmRbYpKFNkMNkbwxhl.Next(0, 2)]);
        }
        short BQVQXRPxsSxPkoLtTQTkmn = 18606;
        sbyte OihEfD = -40;
        double NbghsSPpS = 4.509732E-14;
        while (NbghsSPpS == 1.333395E-28)
        {
            NbghsSPpS = Math.Ceiling(Math.Acos(0));
            try
            {
                MessageBox.Show(NbghsSPpS.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        float BPoEddxxmh = 8.919751E-24F;

    }
    public void nJSwyWC()
    {
        double hhWJLEpIsYLw = -4.677216E-35;
        double qTfOVsSzRsqphIgIwtSfFQGAO = Math.Log(1000, 10);
        hhWJLEpIsYLw = qTfOVsSzRsqphIgIwtSfFQGAO;
        ushort FbeHRTRCEH = 41611;
        ushort aeIukoobYW = 36895;
        short ajTgAHMlbt = -31825;
        byte SVZlMaQmmCtnCDimGf = 136;
        short SHIxhZKcYMHhHxtcN = -20548;
        int bOJBTXeEZwnzVba = 3305;
        if (bOJBTXeEZwnzVba == 12881)
        {
            bOJBTXeEZwnzVba = 123277;
        }
        byte PMgUgPjTVZ = 217;
        uint XgEjV = 6;
        int hlGxeMXGDsRATPM = 94;
        if (hlGxeMXGDsRATPM == 536123)
        {
            hlGxeMXGDsRATPM = hlGxeMXGDsRATPM + 352573;
        }
        uint BWqj = 3847;
        sbyte hLsEVoLLzWpxBKFesRgXtnbfIieAN = 7;
        ulong ytKOCkRO = 50476936705599342;
        short RtmhHTKlV = 15345;
        byte XXjeZhaQ = 140;
        sbyte tKp = 116;
        ushort FGnh = 32892;
        long YoMQeooeEcft = 84279347663264855;
        ushort yqVniqgWdymRgLwkMmTVPIjFYf = 34943;
        byte HQbZeSuUQgYYIYI = 33;
        int saGPyQyfPgqpcPWyxIkRcSapW = 90;
        if (saGPyQyfPgqpcPWyxIkRcSapW == 683085)
        {
            saGPyQyfPgqpcPWyxIkRcSapW += 547208;
        }
        uint oijTSskDYcnE = 74890;
        sbyte kDZV = 71;
        ulong ZoSqUBYfFTWNGNFF = 40522544272922085;
        string VWhDDFBJjEZbqcaXKKEpDo = "XKVBLLmDwnhFbYbqsmX";
        int FnJ = 521;
        if (FnJ == 53224)
        {
            FnJ += 862059;
        }
        ulong GpjSHdPY = 68791257223124283;
        long ldGMGtwlnJTj = 64336757631031430;
        long yYHYaCBPdS = 71015489007473021;
        ushort QgfMCbkjymlzI = 62304;
        int fHsUMzFaPQVTufbXMpyq = 888359501;
        while (fHsUMzFaPQVTufbXMpyq == 888359501)
        {
            fHsUMzFaPQVTufbXMpyq = fHsUMzFaPQVTufbXMpyq + 328552;
        }
        short HXbKcWOczDJqCeXMfqPsloMsA = -14100;
        ulong ktEuhNuS = 71924077999209211;
        long POWKTZ = 10819244403680872;
        sbyte SfZnjPLfYUBqbmQDK = -33;

    }
    public void ZoBBESihcHdbcpOf()
    {
        short tWadRMYA = -18511;
        double CIZRninsFwxb = 6.401041E+11;
        while (CIZRninsFwxb == 3.099529E+28)
        {
            double ysqyAWzP = -3.846758E-38;
            CIZRninsFwxb = Math.Round(ysqyAWzP, 1, MidpointRounding.ToEven);
            for (;;)
            {
                Console.WriteLine(1.174834E-33);
            }
        }
        ulong JUMtKEtDgBfXybxADxJxR = 65621739543664330;
        uint SbxtpZSuMo = 643882349;
        ulong GFpsqpLRuSJabhXgkAImn = 61653770046581497;
        sbyte tha = 27;
        sbyte QMjDbI = 18;
        long XKyFdqaAwKD = 42657676844955330;
        short aDQawNyLmnyXRDaGsdJtJGWFA = 6307;
        float ThwMwyXhHFJFDPiwfK = -3.168883E-18F;
        int CjiW = 41;
        if (CjiW == 502675)
        {
            CjiW += 808507;
        }
        float ReTMCtGTYOueQUxBAQkHaQHlDz = -237.149F;
        uint eSMJYdDthaJMfN = 8802722;
        sbyte YfNKFkTazIi = -115;
        sbyte qDKD = -40;
        sbyte tYfIkXoLTyzsYDxJumCQXFVkfVF = -27;
        int mMZuG = 63;
        while (mMZuG == 63)
        {
            mMZuG = 579348;
        }
        short jKLhBUUooRqYOdUDpsU = -22498;
        ulong MtFXobsT = 5634630656546120;
        string IPYVJwSdNBeVKhQZ = "qEEFSXwEesPtthDnuAalRLieloSOt";
        uint iLDdjBVQSDsFcdL = 3607;
        int pIQOhHuztEeIkDUGIQd = 7545;
        while (pIQOhHuztEeIkDUGIQd == 7545)
        {
            pIQOhHuztEeIkDUGIQd = pIQOhHuztEeIkDUGIQd + 694652;
        }
        uint fbCVAk = 23405532;
        ulong XwxQcClNUgci = 42753883644804948;
        long JBnx = 70998168103323548;
        long HDLXzwQsdWFAs = 41114179121540642;
        double GLEgyJMfiqiIIiUBm = -2.670378E-14;
        while (GLEgyJMfiqiIIiUBm == -5.124814E+25)
        {
            GLEgyJMfiqiIIiUBm = Math.Ceiling(Math.Cos(2));
            Console.Write(GLEgyJMfiqiIIiUBm.ToString());
        }
        double dIGYWIeBxjhTdWtUglll = 3.592321E-14;
        dIGYWIeBxjhTdWtUglll = Math.Ceiling(Math.Sin(2));
        for (;;)
        {
            Console.WriteLine(5.545813E-08);
        }
        short cXqwtUUTeOddORhM = 11132;
        int GRXSRMSVLOewKnBCFo = 13;
        while (GRXSRMSVLOewKnBCFo == 13)
        {
            GRXSRMSVLOewKnBCFo = 414725;
        }
        ushort zJZYmaXhMKefBKCZif = 55;
        short QnoDyEzihuCzn = -15658;
        sbyte MBdzCDRxzPwGwoqwbiIwLwQyzYE = -55;
        int lxsdyFWTTmySeJiTOSdg = 94019;
        if (lxsdyFWTTmySeJiTOSdg == 198100)
        {
            lxsdyFWTTmySeJiTOSdg = lxsdyFWTTmySeJiTOSdg + 318026;
        }
        long aDYCTRDZmqSNAhpwYSq = 28465742166366308;

    }
}