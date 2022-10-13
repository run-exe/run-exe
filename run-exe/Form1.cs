using System.Net;
using System.Text;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
namespace SpiderNextBoot;
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        // https://learn.microsoft.com/ja-jp/dotnet/api/system.componentmodel.backgroundworker?view=net-6.0
        backgroundWorker1.WorkerReportsProgress = true;
        backgroundWorker1.WorkerSupportsCancellation = true;
        if (backgroundWorker1.IsBusy != true)
        {
            // Start the asynchronous operation.
            backgroundWorker1.RunWorkerAsync();
        }
    }
    private void CancelSample()
    {
        if (backgroundWorker1.WorkerSupportsCancellation == true)
        {
            // Cancel the asynchronous operation.
            backgroundWorker1.CancelAsync();
        }
    }
    private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        Console.WriteLine("Location: " + Assembly.GetExecutingAssembly().Location);
        DirectoryInfo fi = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        string parent = fi.Parent.FullName;
        string jsonPath = $"{parent}\\spider-next-boot.json";
        string json;
        if (File.Exists(jsonPath))
        {
            Console.WriteLine($"Reading File: {jsonPath}");
            json = File.ReadAllText(jsonPath, Encoding.UTF8);
        }
        else
        {
            var jsonUrl = "https://github.com/spider-explorer/spider-next/releases/download/64bit/spider-next-boot.json";
            Console.WriteLine($"Downloading File: {jsonUrl}");
            json = GetStringFromUrl(jsonUrl);
        }
        var root = System.Text.Json.JsonDocument.Parse(json).RootElement;
        var len = root.GetArrayLength();
        Console.WriteLine(len);
        listBox1.DisplayMember = "name";
        //listBox1.ValueMember = "url";
        for (int i = 0; i < len; i++)
        {
            var name = root[i].GetProperty("name").GetString();
            var url = root[i].GetProperty("url").GetString();
            listBox1.Items.Add(new
            {
                name = name,
                url = url
            });
        }
    }
    private void listBox1_DoubleClick(object sender, EventArgs e)
    {
        if (listBox1.SelectedIndex < 0) return;
        Program.JsonUrl = ((dynamic)listBox1.SelectedItem).url;
        this.Close();
    }
    private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled == true)
        {
            //resultLabel.Text = "Canceled!";
        }
        else if (e.Error != null)
        {
            //resultLabel.Text = "Error: " + e.Error.Message;
        }
        else
        {
            //resultLabel.Text = "Done!";
        }
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
}