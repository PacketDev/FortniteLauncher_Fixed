using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using FortniteLauncher.FortniteLauncher.Logs;
using FortniteLauncher.FortniteLauncher.Json;
using FortniteLauncher.FortniteLauncher.Auth;

namespace FortniteLauncher
{
    internal class Program
    {
        static readonly string binPath = @"C:\Program Files\Epic Games\Fortnite\FortniteGame\Binaries\Win64\";
        static readonly string launcherExe = $"{binPath}FortniteLauncher.exe";
        static readonly string shippingExe = $"{binPath}FortniteClient-Win64-Shipping.exe";
        static readonly string eacShippingExe = $"{binPath}FortniteClient-Win64-Shipping_EAC.exe";
        public static readonly string beShippingExe = $"{binPath}FortniteClient-Win64-Shipping_BE.exe";
        static readonly string obfuscationid = "o0qvpbVMAZh6AwSBOk6Y_8D5TvJyOA";
        static Process _fnProcess;
        static Process _fnEacProcess;
        static Process _fnLauncher;
        static string token = "unused";
        static string exchange = "unused";
        static string authType = "unused";
        public static string sslBypassDLL = @".\ServerV2.dll";

        static void Main(string[] args)
        {
            Log.Logs("FortniteLauncher by !Sky");
            if (!File.Exists(launcherExe) | !File.Exists(shippingExe) | !File.Exists(eacShippingExe))
            {
                Log.Error("Something is wrong with your Fortnite installation or can't find your installation");
                Thread.Sleep(3000);
                Environment.Exit(2);
            }
            Log.Logs("Do you want to authenticate? Y | N");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine("Authorization requiered:");
                token = Auth.GetToken(Console.ReadLine());
                exchange = Auth.GetExchange(token);
                authType = "exchangecode";
            }

            var launchArgs = $"-obfuscationid={obfuscationid} -AUTH_LOGIN=unused -AUTH_PASSWORD=a8bcfa232e974a5d9a8895ae2157a180 -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -EpicPortal -steamimportavailable -epicusername=FNBR Sky -epicuserid=c44d2da4a43d4871a766096644281a51 -epiclocale=en -epicsandboxid=fn -nobe -fromfl=eac -fltoken=41h55957c3e674816c873826";
            _fnLauncher = new Process
            {
                StartInfo =
                {
                    FileName = launcherExe,
                    Arguments = launchArgs

                }
            };
            _fnLauncher.Start();
            foreach (ProcessThread thread in _fnLauncher.Threads)
            {
                Win32.SuspendThread(Win32.OpenThread(0x0002, false, thread.Id));
            }

            _fnEacProcess = new Process
            {
                StartInfo =
                {
                    FileName = eacShippingExe,
                    Arguments = launchArgs

                }
            };
            _fnEacProcess.Start();
            foreach (ProcessThread thread in _fnEacProcess.Threads)
            {
                Win32.SuspendThread(Win32.OpenThread(0x0002, false, thread.Id));
            }
            _fnProcess = new Process
            {
                StartInfo =
                {
                    FileName = shippingExe,
                    Arguments = launchArgs,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            _fnProcess.Start();
            AsyncStreamReader asyncOutputReader = new AsyncStreamReader(_fnProcess.StandardOutput);

            asyncOutputReader.DataReceived += delegate (object sender, string data)
            {
                Console.WriteLine(data);
            };

            asyncOutputReader.Start();

            Injector.InjectDll(_fnProcess.Id, sslBypassDLL);

            _fnProcess.WaitForExit();
            _fnLauncher.Kill();
            _fnEacProcess.Kill();
        }
    }
}
