using System;
using System.Threading.Tasks;
using MojangApiMonitor;

namespace MojangApiMonitorTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // 设置输出文件路径
            string outputPath = "mojang_status.json";
            if (args.Length > 0)
            {
                outputPath = args[0];
            }
            
            // 运行示例
            await MojangApiMonitorExample.RunExampleAsync(outputPath);
            
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
