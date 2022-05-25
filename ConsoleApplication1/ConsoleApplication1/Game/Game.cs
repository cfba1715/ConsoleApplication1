using System;
using System.Linq;

namespace Yoyo.Game
{
    delegate void Print(Object str);
    public class Game
    {
        Print print = Console.WriteLine;

        /// <summary>
        /// 牌库
        /// </summary>
        private int[] initCards = new int[]
        {
            0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E,
            0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E,
            0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E,
        };


        public void GameStart()
        {
            // print(0 & 0x0f);
            // return;

            var allCards = initCards.ToList();
            //桌面牌
            var deskCards = new int[5];
            //玩家1牌
            var player1HandCards = new int[2];
            //玩家2牌
            var player2HandCards = new int[2];

            for (var index = 0; index < 5; index++)
            {
                var random = new Random().Next(0, allCards.Count);
                deskCards[index] = allCards[random];
                allCards.RemoveAt(random);
            }

            for (var index = 0; index < 2; index++)
            {
                var random = new Random().Next(0, allCards.Count);
                player1HandCards[index] = allCards[random];
                allCards.RemoveAt(random);
            }

            for (var index = 0; index < 2; index++)
            {
                var random = new Random().Next(0, allCards.Count);
                player2HandCards[index] = allCards[random];
                allCards.RemoveAt(random);
            }

            // new ComparedPoker(deskCards, player1HandCards, player2HandCards);
            // deskCards = new[]
            // {
            //     0x1A, 0x1B, 0x1C, 0x1D, 0x1E,
            // };
            // player1HandCards = new[]
            // {
            //     0x2B, 0x2C,
            // };
            // player2HandCards = new[]
            // {
            //     0x38, 0x39,
            // };
            var compared = new ComparedPoker.ComparedPoker(deskCards, player1HandCards, player2HandCards);
            print("桌面的牌");
            foreach (var value in deskCards)
            {
                print(String.Format("牌是:花{0}\t值{1}", value & 0xf0, value & 0x0f));
            }

            print("玩家一的牌");
            foreach (var value in player1HandCards)
            {
                print(String.Format("牌是:花{0}\t值{1}", value & 0xf0, value & 0x0f));
            }

            print("玩家二的牌");
            foreach (var value in player2HandCards)
            {
                print(String.Format("牌是:花{0}\t值{1}", value & 0xf0, value & 0x0f));
            }

            if (compared.hiYoYoWinData.SequenceEqual(player1HandCards))
            {
                print("玩家1获胜");
            }
            else
            {
                print("玩家2获胜");
            }
        }
    }

}