using MinecraftLaunch.Base.Models.Authentication;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Utilities;
using MinecraftLaunch.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLaunch.Launch;

namespace TYMCL.Modules
{
    internal class GameTools
    {

        public static async Task GameLaunchActive() // 游戏启动方法
        {
            Logger.Log.Info("游戏启动", "正在启动游戏...");

            var gameCore = "1.12.2";// 游戏核心
            Logger.Log.Info("游戏启动", "游戏核心: " + gameCore);

            var account = new OfflineAuthenticator().Authenticate("PLAYER_NAME"); // 账户验证器
            Logger.Log.Info("游戏启动","账户信息: " + account.Name + " " + account.Uuid);

            Logger.Log.Info("游戏启动", "构建启动配置...");
            var launchConfig = new LaunchConfig()
            {
                Account = account,
                MaxMemorySize = 4096,
                MinMemorySize = 4096,
                JavaPath = await JavaUtil.EnumerableJavaAsync().FirstOrDefaultAsync(),
            };
            Logger.Log.Info("游戏启动", "最大内存分配: " + launchConfig.MaxMemorySize + "MB");
            Logger.Log.Info("游戏启动", "最小内存分配: " + launchConfig.MinMemorySize + "MB");
            Logger.Log.Info("游戏启动","Java信息: " + launchConfig.JavaPath);

            Logger.Log.Info("启动游戏", "启动游戏...");
            var minecraftRunner = new MinecraftRunner(launchConfig, ".minecraft"); // 变量名改为minecraftRunner
            var process = await minecraftRunner.RunAsync(gameCore);

            var exitCompletionSource = new TaskCompletionSource<bool>();
            if (process.Process.HasExited)
            {
                Logger.Log.Info("Minecraft实例", "游戏进程已退出");
            }
            else
            {
                Logger.Log.Info("Minecraft实例",$"游戏进程正在运行 (PID: {process.Process.Id})");
            }
            void OnExited(object s, EventArgs e)
            {
                int exitCode = process.Process.ExitCode;
                Logger.Log.Info("Minecraft实例", "游戏进程已退出-返回值: " + exitCode );
                exitCompletionSource.SetResult(true);
                process.Exited -= OnExited;
            }


            process.Exited += OnExited;


            await exitCompletionSource.Task;
        }

        public static void GameInstall() // 游戏安装方法
        { 
        
        }

        public static async Task JavaWatch() // Java查找器
        {
            var javas = JavaUtil.EnumerableJavaAsync();

            Logger.Log.Info("JAVA查找", "Java列表:");

            await foreach (var java in javas)
            {
                var javabit = "32Bit";
                if (java.Is64bit)
                {
                    javabit = "64Bit";
                }
                Logger.Log.Info("Java查找", "Java" + java.JavaVersion + "-" + javabit + " " + java.JavaPath);
            }

        }
    }
}
