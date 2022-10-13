using System;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Windows.Forms;
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
        Console.WriteLine(args.Length);
        if (args.Length != 2) Environment.Exit(1);
        //Application.Run(new Form1());
        //if (JsonUrl == null) return;
        string JsonUrl = $"https://github.com/spider-explorer/spider-next/releases/download/64bit/{args[1]}.json";
        //                 https://github.com/spider-explorer/spider-next/releases/download/64bit/spider-next.json
        RunSelectedProgram(JsonUrl);
    }
    static void RunSelectedProgram(string jsonUrl)
    {
        var appName = GetFileBaseNameFromUrl(jsonUrl);
        Console.WriteLine(appName);
        var json = GetStringFromUrl(jsonUrl);
        var root = System.Text.Json.JsonDocument.Parse(json).RootElement;
        var version = root.GetProperty("version").GetString();
        var url = root.GetProperty("url").GetString();
        var mainDll = root.GetProperty("main_dll").GetString();
        var mainClass = root.GetProperty("main_class").GetString();
        var console = root.GetProperty("console").GetBoolean();
        Console.WriteLine(version);
        Console.WriteLine(url);
        var profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.WriteLine(profilePath);
        var installPath = $"{profilePath}\\.javacommons\\.software\\{appName}\\{version}";
        Console.WriteLine(installPath);
        if (!Directory.Exists(installPath))
        {
            Console.WriteLine($"{installPath} が存在しません");
            DirectoryInfo di = new DirectoryInfo(installPath);
            DirectoryInfo diParent = di.Parent;
            string parent = diParent.FullName;
            Console.WriteLine($"{parent} を準備します");
            Directory.CreateDirectory(parent);
            string destinationPath = $"{parent}\\{version}.zip";
            FileInfo fi = new FileInfo(destinationPath);
            if (!fi.Exists)
            {
                Console.WriteLine($"{destinationPath} にダウンロードします");
                DownloadBinaryFromUrl(url, destinationPath);
                Console.WriteLine($"{destinationPath} にダウンロードが完了しました");
            }
            Console.WriteLine($"{installPath} に展開します");
            ZipFile.ExtractToDirectory(destinationPath, installPath);
            Console.WriteLine($"{installPath} に展開しました");
        }
        Console.WriteLine($"{mainClass} を起動します");
        Thread.Sleep(1000);
        StartAssembly($"{installPath}\\{mainDll}", mainClass, version, console);
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
    static void StartAssembly(string path, string mainClass, string version, bool console)
    {
        Assembly test01Dll = Assembly.LoadFrom(path);
        var appType = test01Dll.GetType(mainClass);
        if (appType == null)
        {
            Console.WriteLine("(appType == null)");
            return;
        }
        var setVersion = appType.GetMethod("SetVersion", BindingFlags.Public | BindingFlags.Static);
        if (setVersion != null)
        {
            setVersion.Invoke(null, new object[] { version });
        }
        var main = appType.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
        if (main == null) Console.WriteLine("(main == null)");
        if (!console) FreeConsole();
        main.Invoke(null, new object[] { });
#if false
        if (console)
        {
            Console.WriteLine("プログラムが終了しました。何かキーを押して下さい: ");
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