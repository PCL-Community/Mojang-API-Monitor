using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MojangApiMonitor
{
    /// <summary>
    /// Mojang API监控器
    /// </summary>
    public class MojangMonitor
    {
        private readonly HttpClient _httpClient;
        private readonly List<MojangService> _services;

        /// <summary>
        /// 初始化一个新的MojangMonitor实例
        /// </summary>
        public MojangMonitor()
        {
            // 创建一个HttpClientHandler并配置
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            
            // 添加用户代理头，模拟浏览器
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            
            // 初始化要监控的所有9种Mojang服务列表
            _services = new List<MojangService>
            {
                new MojangService { Name = "Minecraft.net", Url = "https://minecraft.net/" },
                new MojangService { Name = "Session Minecraft", Url = "http://session.minecraft.net/" },
                new MojangService { Name = "Account Mojang", Url = "http://account.mojang.com/" },
                new MojangService { Name = "Auth Mojang", Url = "https://auth.mojang.com/" },
                new MojangService { Name = "Skins Minecraft", Url = "http://skins.minecraft.net/" },
                new MojangService { Name = "Authserver Mojang", Url = "https://authserver.mojang.com/" },
                new MojangService { Name = "Sessionserver Mojang", Url = "https://sessionserver.mojang.com/" },
                new MojangService { Name = "API Mojang", Url = "https://api.mojang.com/" },
                new MojangService { Name = "Textures Minecraft", Url = "http://textures.minecraft.net/" }
            };
        }

        /// <summary>
        /// 检查所有Mojang服务的状态
        /// </summary>
        /// <returns>包含所有服务状态的MojangApiStatus对象</returns>
        public async Task<MojangApiStatus> CheckServicesStatusAsync()
        {
            var status = new MojangApiStatus();

            foreach (var service in _services)
            {
                await CheckServiceStatusAsync(service);
                status.Services.Add(service);
            }

            return status;
        }

        /// <summary>
        /// 检查单个服务的状态
        /// </summary>
        /// <param name="service">要检查的服务</param>
        private async Task CheckServiceStatusAsync(MojangService service)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            try
            {
                // 创建请求消息
                var request = new HttpRequestMessage(HttpMethod.Get, service.Url);
                
                // 添加特定的头信息
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Headers.Add("Connection", "keep-alive");
                
                // 发送请求
                var response = await _httpClient.SendAsync(request);
                stopwatch.Stop();
                
                service.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                
                // 无论返回什么状态码，只要服务器响应了，我们就认为它是在线的
                // 这是因为Mojang的某些服务即使正常工作也可能返回非200状态码
                service.Status = "ONLINE";
                
                // 记录HTTP状态码，用于调试
                Console.WriteLine($"{service.Name} 返回状态码: {(int)response.StatusCode} {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop();
                service.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                
                // 检查是否是DNS解析错误，如果是，尝试使用不同的URL格式
                if (ex.Message.Contains("不知道这样的主机") || ex.Message.Contains("找不到请求的类型的数据"))
                {
                    try
                    {
                        // 尝试使用HTTPS而不是HTTP
                        string alternativeUrl = service.Url.Replace("http://", "https://");
                        var alternativeRequest = new HttpRequestMessage(HttpMethod.Get, alternativeUrl);
                        var alternativeResponse = await _httpClient.SendAsync(alternativeRequest);
                        
                        service.Status = "ONLINE";
                        Console.WriteLine($"{service.Name} 使用替代URL {alternativeUrl} 成功连接");
                    }
                    catch
                    {
                        service.Status = "OFFLINE";
                        Console.WriteLine($"错误检查 {service.Name}: {ex.Message}");
                    }
                }
                else
                {
                    service.Status = "OFFLINE";
                    Console.WriteLine($"错误检查 {service.Name}: {ex.Message}");
                }
            }
            catch (TaskCanceledException)
            {
                // 超时
                stopwatch.Stop();
                service.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                service.Status = "OFFLINE";
                Console.WriteLine($"{service.Name} 检查超时");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                service.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                service.Status = "OFFLINE";
                Console.WriteLine($"检查 {service.Name} 时发生未知错误: {ex.Message}");
            }

            service.LastChecked = DateTime.UtcNow;
        }

        /// <summary>
        /// 将服务状态导出为JSON文件
        /// </summary>
        /// <param name="status">服务状态对象</param>
        /// <param name="filePath">输出文件路径</param>
        public void ExportToJson(MojangApiStatus status, string filePath)
        {
            var json = JsonConvert.SerializeObject(status, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
} 