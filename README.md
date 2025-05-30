# Mojang API 监控器

这是一个 .NET Standard 2.0 类库，用于监控 Mojang API 的状态。当玩家在 Minecraft 中遇到登录错误时，可以使用此工具检查是否为 Mojang 服务器的问题。

## 功能

- 监控 9 个 Mojang 服务的状态
- 检测服务是否在线
- 测量响应时间
- 将结果导出为 JSON 文件

## 监控的服务

| # | 服务名称 | URL |
|---|---------|-----|
| 1 | Minecraft.net | https://minecraft.net/ |
| 2 | Session Minecraft | http://session.minecraft.net/ |
| 3 | Account Mojang | http://account.mojang.com/ |
| 4 | Auth Mojang | https://auth.mojang.com/ |
| 5 | Skins Minecraft | http://skins.minecraft.net/ |
| 6 | Authserver Mojang | https://authserver.mojang.com/ |
| 7 | Sessionserver Mojang | https://sessionserver.mojang.com/ |
| 8 | API Mojang | https://api.mojang.com/ |
| 9 | Textures Minecraft | http://textures.minecraft.net/ |

## 项目结构

- `MojangApiMonitor/` - 主要类库项目
  - `MojangService.cs` - 表示一个 Mojang 服务及其状态
  - `MojangApiStatus.cs` - 表示所有 Mojang 服务的状态
  - `MojangMonitor.cs` - 核心功能类，负责检查服务状态
  - `MojangApiMonitorExample.cs` - 示例代码
- `MojangApiMonitorTest/` - 控制台应用程序，用于测试类库

## 使用方法

### 安装

将此项目添加为您的项目的引用。

### 基本用法

```csharp
// 创建监控器实例
var monitor = new MojangMonitor();

// 检查所有服务的状态
var status = await monitor.CheckServicesStatusAsync();

// 导出结果到JSON文件
monitor.ExportToJson(status, "mojang_status.json");
```

### JSON输出格式

```json
{
  "services": [
    {
      "name": "Minecraft.net",
      "url": "https://minecraft.net/",
      "display_url": "https://minecraft.net",
      "status": "ONLINE",
      "last_checked": "2025-05-29T16:41:41.0165483Z",
      "response_time_ms": 3249
    },
    // 其他服务...
  ],
  "timestamp": "2025-05-29T16:41:37.7543238Z"
}
```

## 构建项目

使用 Visual Studio 或 dotnet CLI 构建项目：

```bash
dotnet build
```

## 运行测试程序

```bash
dotnet run --project MojangApiMonitorTest/MojangApiMonitorTest.csproj
```

## 依赖项

- .NET Standard 2.0
- Newtonsoft.Json (13.0.3+)

## 许可证

基于 MIT 许可证提供

## 免责声明

本项目不隶属于 Mojang Studios 或 Microsoft。Minecraft、Mojang 是 Mojang Studios 的商标。 
