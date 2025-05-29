using System;
using Newtonsoft.Json;

namespace MojangApiMonitor
{
    /// <summary>
    /// 表示一个Mojang服务及其状态
    /// </summary>
    public class MojangService
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 服务URL（用于请求检测的实际URL）
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 显示用的URL（简短版本，用于展示）
        /// </summary>
        [JsonProperty("display_url")]
        public string DisplayUrl 
        {
            get
            {
                if (string.IsNullOrEmpty(Url))
                    return string.Empty;
                
                // 提取主域名部分
                Uri uri = new Uri(Url);
                return $"{uri.Scheme}://{uri.Host}";
            }
        }

        /// <summary>
        /// 服务状态
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 最后检查时间
        /// </summary>
        [JsonProperty("last_checked")]
        public DateTime LastChecked { get; set; }

        /// <summary>
        /// 响应时间（毫秒）
        /// </summary>
        [JsonProperty("response_time_ms")]
        public long ResponseTimeMs { get; set; }
    }
} 