using System;
using System.Threading.Tasks;

namespace MojangApiMonitor
{
    /// <summary>
    /// 示例类，展示如何使用MojangMonitor
    /// </summary>
    public static class MojangApiMonitorExample
    {
        /// <summary>
        /// 运行示例
        /// </summary>
        /// <param name="outputPath">JSON输出文件路径</param>
        /// <returns>异步任务</returns>
        public static async Task RunExampleAsync(string outputPath = "mojang_status.json")
        {
            Console.WriteLine("开始检查Mojang API状态...");
            Console.WriteLine("正在检查，请稍候...");
            Console.WriteLine();
            
            var monitor = new MojangMonitor();
            var status = await monitor.CheckServicesStatusAsync();
            
            // 输出到控制台
            Console.WriteLine("===== Mojang 服务状态 =====");
            Console.WriteLine($"检查时间: {status.Timestamp}");
            Console.WriteLine();
            
            Console.WriteLine("+---------------------+-----------------------------+------------+----------------+");
            Console.WriteLine("| 服务名称            | URL                         | 状态       | 响应时间       |");
            Console.WriteLine("+---------------------+-----------------------------+------------+----------------+");
            
            foreach (var service in status.Services)
            {
                string statusText = service.Status == "ONLINE" ? "在线" : "离线";
                string statusColor = service.Status == "ONLINE" ? "在线" : "离线";
                
                Console.WriteLine($"| {service.Name,-19} | {service.DisplayUrl,-27} | {statusText,-10} | {service.ResponseTimeMs,10}ms |");
            }
            
            Console.WriteLine("+---------------------+-----------------------------+------------+----------------+");
            Console.WriteLine();
            
            // 导出到JSON文件
            monitor.ExportToJson(status, outputPath);
            Console.WriteLine($"状态已导出到文件: {outputPath}");
        }
    }
} 