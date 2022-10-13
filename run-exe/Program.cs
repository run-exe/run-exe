using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
namespace SpiderNextBoot;
public static class Program
{
    ////public static string JsonUrl = null;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        Console.Error.WriteLine(args.Length);
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Please specify program name.");
            Environment.Exit(1);
        }
        ArraySegment<string> arySeg = new ArraySegment<string>(args, 2, args.Length-2);
        string[] argsSlice = arySeg.ToArray();
        //Application.Run(new Form1());
        //if (JsonUrl == null) return;
        string JsonUrl = $"https://github.com/spider-explorer/spider-next/releases/download/64bit/{args[1]}.json";
        //                 https://github.com/spider-explorer/spider-next/releases/download/64bit/spider-next.json
        RunSelectedProgram(JsonUrl, argsSlice);
    }
    static void RunSelectedProgram(string jsonUrl, string[] args)
    {
        var appName = GetFileBaseNameFromUrl(jsonUrl);
        Console.Error.WriteLine(appName);
        var json = GetStringFromUrl(jsonUrl);
        var root = System.Text.Json.JsonDocument.Parse(json).RootElement;
        var version = root.GetProperty("version").GetString();
        var url = root.GetProperty("url").GetString();
        var mainDll = root.GetProperty("main_dll").GetString();
        var mainClass = root.GetProperty("main_class").GetString();
        //var console = root.GetProperty("console").GetBoolean();
        Console.Error.WriteLine(version);
        Console.Error.WriteLine(url);
        var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.Error.WriteLine(profilePath);
        var installPath = $"{profilePath}\\.javacommons\\.software\\{appName}\\{version}";
        Console.Error.WriteLine(installPath);
        if (!Directory.Exists(installPath))
        {
            Console.Error.WriteLine($"{installPath} が存在しません");
            DirectoryInfo di = new DirectoryInfo(installPath);
            DirectoryInfo diParent = di.Parent;
            string parent = diParent.FullName;
            Console.Error.WriteLine($"{parent} を準備します");
            Directory.CreateDirectory(parent);
            string destinationPath = $"{parent}\\{version}.zip";
            FileInfo fi = new FileInfo(destinationPath);
            if (!fi.Exists)
            {
                Console.Error.WriteLine($"{destinationPath} にダウンロードします");
                DownloadBinaryFromUrl(url, destinationPath);
                Console.Error.WriteLine($"{destinationPath} にダウンロードが完了しました");
            }
            Console.Error.WriteLine($"{installPath} に展開します");
            ZipFile.ExtractToDirectory(destinationPath, installPath);
            Console.Error.WriteLine($"{installPath} に展開しました");
        }
        Console.Error.WriteLine($"{mainClass} を起動します");
        Thread.Sleep(1000);
        StartAssembly($"{installPath}\\{mainDll}", mainClass, version, args);
    }
    static string GetFileBaseNameFromUrl(string url)
    {
        var list = url.Split("/");
        var fileName = list[list.Length - 1];
        var baseName = Path.GetFileNameWithoutExtension(fileName);
        return baseName;
    }
    static string GetStringFromUrl(string url)
    {
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        WebHeaderCollection header = response.Headers;
        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }
    static void DownloadBinaryFromUrl(string url, string destinationPath)
    {
        WebRequest objRequest = System.Net.HttpWebRequest.Create(url);
        var objResponse = objRequest.GetResponse();
        byte[] buffer = new byte[32768];
        using (Stream input = objResponse.GetResponseStream())
        {
            using (FileStream output = new FileStream(destinationPath, FileMode.CreateNew))
            {
                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
    static void StartAssembly(string path, string mainClass, string version, string[] args)
    {
        Assembly test01Dll = Assembly.LoadFrom(path);
        var appType = test01Dll.GetType(mainClass);
        if (appType == null)
        {
            Console.Error.WriteLine("(appType == null)");
            return;
        }
        var setVersion = appType.GetMethod("SetVersion", BindingFlags.Public | BindingFlags.Static);
        if (setVersion != null)
        {
            setVersion.Invoke(null, new object[] { version });
        }
        var main = appType.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
        if (main == null) Console.Error.WriteLine("(main == null)");
        //if (!console) FreeConsole();
        main.Invoke(null, new object[] { args });
#if false
        if (console)
        {
            Console.Error.WriteLine("プログラムが終了しました。何かキーを押して下さい: ");
            Console.ReadKey();
        }
#endif
    }
    public static void FreeConsole()
    {
        Console.SetOut(TextWriter.Null);
        NativeMethods.FreeConsole();
    }
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern bool FreeConsole();
    }
}