using System;
using System.Diagnostics;
using System.IO;

namespace LinuxTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 = ffmpeg aufrufen");
            Console.WriteLine("");
            Console.WriteLine("2 = montage aufrufen");
            Console.WriteLine("");
            Console.WriteLine("3 = mp4box aufrufen");
            Console.WriteLine("");
            Console.WriteLine("4 = mediainfo aufrufen");
            Console.WriteLine("");
            Console.WriteLine("Q = Quit");

            while(1 == 1)
            {
                var KeyInfo = Console.ReadKey();

                if (KeyInfo.Key == ConsoleKey.D1)
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.CreateNoWindow = true;
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.UseShellExecute = false;
                    Info.FileName = "ffmpeg";
                    Process Prog = new Process();
                    Prog.StartInfo = Info;
                    Prog.Start();
                }

                if (KeyInfo.Key == ConsoleKey.D2)
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.CreateNoWindow = true;
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.UseShellExecute = false;
                    Info.FileName = "montage";
                    Process Prog = new Process();
                    Prog.StartInfo = Info;
                    Prog.Start();
                }

                if (KeyInfo.Key == ConsoleKey.D3)
                {
                    ProcessStartInfo ProcInfoMP4 = new ProcessStartInfo
                    {
                        FileName = "MP4Box",
                        Arguments = "",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    Process ProcessMP4 = new Process
                    {
                        StartInfo = ProcInfoMP4
                    };
                    ProcessMP4.Start();

                    StreamReader StreamProcessOutput = ProcessMP4.StandardError;
                    string Output = StreamProcessOutput.ReadToEnd();
                    Console.WriteLine($"!!!!ACHTUNG!!!! {Output}");
                }

                if (KeyInfo.Key == ConsoleKey.D4)
                {
                    ProcessStartInfo ProcInfoMP4 = new ProcessStartInfo
                    {
                        FileName = "mediainfo",
                        Arguments = "--Inform=\"Video;%ScanType%\" 00029.MTS",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    Process ProcessMP4 = new Process
                    {
                        StartInfo = ProcInfoMP4
                    };
                    ProcessMP4.Start();

                    StreamReader StreamProcessOutput = ProcessMP4.StandardOutput;
                    string Output = StreamProcessOutput.ReadLine();
                    Console.WriteLine($"!!!!ACHTUNG!!!! {Output}");
                }

                if (KeyInfo.Key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }
    }
}
