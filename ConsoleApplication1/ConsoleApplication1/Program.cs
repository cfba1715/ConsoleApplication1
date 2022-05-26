using System;
using System.Diagnostics;
using Gtk;

namespace Yoyo
{
    internal class Program
    {
        private static Window win;

        public static void Main(string[] args)
        {
            var gameData = new Game.Game();
            gameData.GameStart();
            // Application.Init();
            // Window win = new Window("Test");
            // win.SetDefaultSize(400, 600);
            // //窗体关闭后退出应用
            // win.DeleteEvent += (s, e) => { Application.Quit(); };
            // win.WindowPosition = WindowPosition.Center;
            // win.Resizable = true;
            // var label = new Label("桌面的牌!");
            // win.Add(label);
            // Console.WriteLine("Hello World!");
            // foreach (var value in gameData.deskCards)
            // {
            //     win.Add(new Label(String.Format("花色:{0}\t值{1}", value & 0xf0, value & 0x0f)));
            // }
            //
            // label = new Label("玩家1的牌!");
            // win.Add(label);
            // foreach (var value in gameData.player1Cards)
            // {
            //     win.Add(new Label(String.Format("花色:{0}\t值{1}", value & 0xf0, value & 0x0f)));
            // }
            //
            // label = new Label("玩家2的牌!");
            // win.Add(label);
            // foreach (var value in gameData.player2Cards)
            // {
            //     win.Add(new Label(String.Format("花色:{0}\t值{1}", value & 0xf0, value & 0x0f)));
            // }
            //
            // label = new Label(gameData.winStr);
            // win.Add(label);
            // win.ShowAll();
            // Application.Run();
        }
    }
}