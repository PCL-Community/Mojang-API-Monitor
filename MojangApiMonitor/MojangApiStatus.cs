using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MojangApiMonitor
{
    /// <summary>
    /// 表示Mojang API的整体状态
    /// </summary>
    public class MojangApiStatus
    {
        /// <summary>
        /// 所有Mojang服务的状态列表
        /// </summary>
        [JsonProperty("services")]
        public List<MojangService> Services { get; set; }

        /// <summary>
        /// 状态检查的时间戳
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 初始化一个新的MojangApiStatus实例
        /// </summary>
        public MojangApiStatus()
        {
            Services = new List<MojangService>();
            Timestamp = DateTime.UtcNow;
        }
    }
} 