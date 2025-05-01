using MinecraftLaunch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TYMCL.Modules;

namespace TYMCL.Modules
{
    internal class GameTools
    {

        public static void GameLaunchActive() // 游戏启动方法
        {
            
        }

        public static void GameInstall() // 游戏安装方法
        { 
        
        }

        public static async Task JavaWatch() // Java
        {
            var javas = JavaUtil.EnumerableJavaAsync();

            Logger.Log.Info("Java列表:");

            await foreach (var java in javas)
            {
                var javabit = "32Bit";
                if (java.Is64bit)
                {
                    javabit = "64Bit";
                }
                Logger.Log.Info("Java" + java.JavaVersion + "-" + javabit + " " + java.JavaPath);
            }

        }
    }
}
