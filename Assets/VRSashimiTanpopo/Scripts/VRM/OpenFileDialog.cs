using System.Diagnostics;
using Cysharp.Threading.Tasks;

namespace VRSashimiTanpopo.VRM
{
    public static class OpenFileDialog
    {
        static Process process;
        
        public static async UniTask<string> Open(string filter, string title)
        {
            return await UniTask.Run(() =>
            {
                var script = $@"Add-Type -AssemblyName System.Windows.Forms
$Dialog = New-Object System.Windows.Forms.OpenFileDialog -Property @{{
    Filter = '{filter}'
    Title = '{title}'
}}

if($Dialog.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {{
    Write-Output ($Dialog.FileName)
}}
";
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = "PowerShell.exe", 
                        Arguments = script,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();

                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                UnityEngine.Debug.Log(output);
                return output;
            });
        }
    }
}
