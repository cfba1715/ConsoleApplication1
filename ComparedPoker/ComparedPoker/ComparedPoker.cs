using System;
using System.Collections.Generic;
using System.Linq;

namespace ComparedPoker
{
    delegate void Print(Object str);
    public class ComparedPoker
    {
        /// <summary>
        /// 配置需要的张数
        /// </summary>
        private int maxConfig = 5;

        Print print = Console.WriteLine;

        private int[] _winPlayerData;

        /// <summary>
        /// 获取win的玩家数据
        /// </summary>
        public int[] hiYoYoWinData
        {
            get { return _winPlayerData; }
        }

        private int[] player1;
        private int[] player2;

        private int[] deskCards;

        /// <summary>
        /// 对比玩家的牌大小
        /// </summary>
        /// <param name="deskCards"></param>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public ComparedPoker(int[] deskCards, int[] player1, int[] player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.deskCards = deskCards;

            // foreach (var value1 in GetAllTongHuaShun(AddCardToDesk(deskCards, player1).ToArray()))
            // {
            //     foreach (var value in value1)
            //     {
            //         print(value);
            //         print("花：" + (value & 0xf0).ToString());
            //         print("值：" + (value & 0x0f).ToString());
            //     }
            //
            //     _winPlayerData = player2;
            //
            //     print("--------------");
            // }


            if (CompareTonngHuaShun())
            {
            }
            else if (CompareTieZi())
            {
            }
            else if (CompareHuLu())
            {
            }
            else if (CompareTonngHua())
            {
            }
            else if (CompareShunZi())
            {
            }
            else if (CompareSanTiao())
            {
            }
            else if (Compare2Dui())
            {
            }
            else if (Compare1Dui())
            {
            }
            else if (CompareSanPai())
            {
            }
        }

        /// <summary>
        /// 比较葫芦
        /// 葫芦之间的比较：
        /// A、葫芦的大小比较只看3条的点数。
        /// B、3条点数大的胜。
        /// C、如果3条点数相同（只有都取公牌时才会出现这种情况），则比一对的大小，一对大者胜，如果一对也相等，则同大。
        /// </summary>
        /// <returns></returns>
        bool CompareHuLu()
        {
            var p1Data = GetAllHuLu(AddCardToDesk(deskCards, player1).ToArray());
            var p2Data = GetAllHuLu(AddCardToDesk(deskCards, player2).ToArray());
            var p1 = p1Data[2] & 0x0f;
            var p2 = p2Data[2] & 0x0f;
            if (p1 == p2)
            {
                if (p1 == 0)
                {
                    return false;
                }
                else
                {
                    CompareHandPai();
                    return true;
                }
            }
            else
            {
                _winPlayerData = (p1 > p2) ? player1 : player2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 比较铁支
        /// </summary>
        /// <returns></returns>
        bool CompareTieZi()
        {
            var p1Data = GetAllTieZi(AddCardToDesk(deskCards, player1).ToArray());
            var p2Data = GetAllTieZi(AddCardToDesk(deskCards, player2).ToArray());
            if (p1Data.Count == p2Data.Count)
            {
                if (p2Data.Count == 0)
                {
                    return false;
                }
                else
                {
                    var p1v = p1Data[0] & 0x0f;
                    var p2v = p2Data[0] & 0x0f;
                    if (p1v == p2v)
                    {
                        CompareHandPai();
                        return true;
                    }
                    else
                    {
                        _winPlayerData = (p1v > p2v) ? player1 : player2;
                        return true;
                    }
                }
            }
            else
            {
                _winPlayerData = (p1Data.Count > p2Data.Count) ? player1 : player2;
                return true;
            }
        }

        /// <summary>
        /// 比较手牌
        /// </summary>
        /// <returns></returns>
        bool CompareHandPai()
        {
            var sort1 = SortValue(player1);
            var sort2 = SortValue(player2);
            if (sort1[sort1.Count - 1] == sort2[sort2.Count - 1])
            {
                if (sort1[0] == sort2[0])
                {
                    _winPlayerData = null;
                    return true;
                }
                else
                {
                    _winPlayerData = sort1[0] > sort2[0] ? player1 : player2;
                    return true;
                }
            }
            else
            {
                _winPlayerData = sort1[sort1.Count - 1] > sort2[sort2.Count - 1] ? player1 : player2;
                return true;
            }
        }

        /// <summary>
        /// 同花之间的比较：
        /// A、同花之间的比较不看花色，而看单张最大一张牌的点数。
        /// B、如果最大的一张牌的点数一样大，则比相对小一点的那张牌点数；
        /// C、如果还一样大，再往下比，直到分出大小来；
        /// D、如果五张牌的点数都一样大，那么判断为同等大小。 （同花中，“A”为最大的牌） 
        /// </summary>
        /// <returns></returns>
        bool CompareTonngHua()
        {
            var p1values = GetAllTongHua(AddCardToDesk(deskCards, player1).ToArray());
            var p2values = GetAllTongHua(AddCardToDesk(deskCards, player2).ToArray());

            if (p1values.Count == 0 && p2values.Count == 0)
            {
                return false;
            }
            else
            {
                if (p1values.Count == 0 && p2values.Count > 0)
                {
                    _winPlayerData = player2;
                    return true;
                }
                else if (p1values.Count > 0 && p2values.Count == 0)
                {
                    _winPlayerData = player1;
                    return true;
                }
            }

            //获取最大的同花牌
            var p1max = new List<int>();
            var p2max = new List<int>();
            foreach (var values in p1values)
            {
                var value = SortValue(values);
                if (p1max.Count == 0)
                {
                    p1max = value;
                }
                else
                {
                    for (var index = 1; index <= maxConfig; index++)
                    {
                        if ((p1max[maxConfig - index] & 0x0f) < (value[maxConfig - index] & 0x0f))
                        {
                            p1max = value;
                            break;
                        }
                    }
                }
            }

            foreach (var values in p2values)
            {
                var value = SortValue(values);
                if (p2max.Count == 0)
                {
                    p2max = value;
                }
                else
                {
                    for (var index = 1; index <= maxConfig; index++)
                    {
                        if ((p2max[maxConfig - index] & 0x0f) < (value[maxConfig - index] & 0x0f))
                        {
                            p2max = value;
                            break;
                        }
                    }
                }
            }

            for (var index = 1; index <= maxConfig; index++)
            {
                var p1v = p1max[maxConfig - index] & 0x0f;
                var p2v = p2max[maxConfig - index] & 0x0f;
                if (p1v == p2v)
                {
                    continue;
                }
                else
                {
                    _winPlayerData = p1v > p2v ? player1 : player2;
                    return true;
                }
            }

            CompareHandPai();
            return true;
        }

        /// <summary>
        /// 比较顺子
        /// </summary>
        /// <returns></returns>
        bool CompareShunZi()
        {
            var p1values = GetAllShunZi(AddCardToDesk(deskCards, player1).ToArray());
            var p2values = GetAllShunZi(AddCardToDesk(deskCards, player2).ToArray());
            if (p1values.Count == 0 && p2values.Count == 0)
            {
                return false;
            }
            else
            {
                if (p1values.Count == 0 && p2values.Count > 0)
                {
                    _winPlayerData = player2;
                    return true;
                }
                else if (p1values.Count > 0 && p2values.Count == 0)
                {
                    _winPlayerData = player1;
                    return true;
                }
            }

            //获取最大的顺子
            var p1max = new List<int>();
            var p2max = new List<int>();
            foreach (var values in p1values)
            {
                var value = SortValue(values);
                if (p1max.Count == 0)
                {
                    p1max = value;
                }
                else
                {
                    if ((p1max[maxConfig - 1] & 0x0f) < (value[maxConfig - 1] & 0x0f))
                    {
                        p1max = value;
                    }
                }
            }

            foreach (var values in p2values)
            {
                var value = SortValue(values);
                if (p2max.Count == 0)
                {
                    p2max = value;
                }
                else
                {
                    if ((p2max[maxConfig - 1] & 0x0f) < (value[maxConfig - 1] & 0x0f))
                    {
                        p2max = value;
                        break;
                    }
                }
            }

            for (var index = 1; index <= maxConfig; index++)
            {
                var p1v = p1max[maxConfig - index] & 0x0f;
                var p2v = p2max[maxConfig - index] & 0x0f;
                if (p1v == p2v)
                {
                    continue;
                }
                else
                {
                    _winPlayerData = p1v > p2v ? player1 : player2;
                    return true;
                }
            }

            CompareHandPai();
            return true;
        }

        /// <summary>
        /// 三条之间的比较
        /// 三条之间的比较是先比3条的点数，3条大者胜。如果3条同大。则比最大的一张杂牌的大小，大者胜。如果最大的杂牌点数也相等，则比第二张杂牌的大小，大者胜。若还是相等，则同大。
        /// </summary>
        /// <returns></returns>
        bool CompareSanTiao()
        {
            var player1Values = GetAllSanTiao(AddCardToDesk(deskCards, player1).ToArray());
            var player2Values = GetAllSanTiao(AddCardToDesk(deskCards, player2).ToArray());
            if (player1Values[0] == 0 && player2Values[0] == 0)
            {
                return false;
            }
            else
            {
                if (player1Values[0] != 0 && player2Values[0] == 0)
                {
                    _winPlayerData = player1;
                    return true;
                }
                else if (player1Values[0] == 0 && player2Values[0] != 0)
                {
                    _winPlayerData = player2;
                    return true;
                }
            }

            //比较前3
            var p1 = player1Values[0] & 0x0f;
            var p2 = player2Values[0] & 0x0f;
            if (p1 == p2)
            {
                for (var index = 0; index < 2; index++)
                {
                    var pp1 = player1Values[3 + index] & 0x0f;
                    var pp2 = player2Values[3 + index] & 0x0f;
                    if (pp1 != pp2)
                    {
                        _winPlayerData = p1 > p2 ? player1 : player2;
                        return true;
                    }
                }
            }
            else
            {
                _winPlayerData = p1 > p2 ? player1 : player2;
                return true;
            }

            CompareHandPai();
            return true;
        }

        /// <summary>
        /// 对子之间的比较
        /// A、对子之间的比较先看对子的大小，点数大的胜；
        /// B、若点数相同，则再看最大的第一张杂牌，点数大的胜；
        /// C、最大的一张杂牌点数相同的话，比第二张大的杂牌，点数大的胜；
        /// D、如果再相同，则比最后一张杂牌的大小，点数大的胜；
        /// E、若都相同，则判断为相同大小。
        /// </summary>
        /// <returns></returns>
        bool Compare1Dui()
        {
            var player1Values = GetAll1Dui(AddCardToDesk(deskCards, player1).ToArray());
            var player2Values = GetAll1Dui(AddCardToDesk(deskCards, player2).ToArray());
            if (player1Values == null && player2Values == null)
            {
                return false;
            }
            else
            {
                if (player1Values != null && player2Values == null)
                {
                    _winPlayerData = player1;
                    return true;
                }
                else if (player1Values == null)
                {
                    _winPlayerData = player2;
                    return true;
                }
            }

            var p1 = player1Values[0] & 0x0f;
            var p2 = player2Values[0] & 0x0f;
            if (p1 == p2)
            {
                for (var index = 0; index < 3; index++)
                {
                    p1 = player1Values[index + 2] & 0x0f;
                    p2 = player2Values[index + 2] & 0x0f;
                    if (p1 == p2)
                    {
                    }
                    else
                    {
                        _winPlayerData = p1 > p2 ? player1 : player2;
                        return true;
                    }
                }

                CompareHandPai();
                return true;
            }
            else
            {
                _winPlayerData = p1 > p2 ? player1 : player2;
                return true;
            }
        }

        /// <summary>
        /// 二对之间的比较
        /// A、二对之间的比较先看最大的一对，点数大的胜；
        /// B、如果点数相同，则比第二对，同样是点数大的胜；
        /// C、若第二对也相同，则比杂牌的大小，杂牌点数大者胜；
        /// D、要是连杂牌点数都一样了，判断为相同大小。
        /// </summary>
        /// <returns></returns>
        bool Compare2Dui()
        {
            var p1Value = GetAll2Dui(AddCardToDesk(deskCards, player1).ToArray());
            var p2Value = GetAll2Dui(AddCardToDesk(deskCards, player2).ToArray());
            if (p1Value == null && p2Value == null)
            {
                return false;
            }
            else
            {
                if (p1Value != null && p2Value == null)
                {
                    _winPlayerData = player1;
                    return true;
                }
                else if (p1Value == null)
                {
                    _winPlayerData = player2;
                    return true;
                }
            }

            var p1 = p1Value[0] & 0x0f;
            var p2 = p2Value[0] & 0x0f;
            if (p1 == p2)
            {
                p1 = p1Value[2] & 0x0f;
                p2 = p2Value[2] & 0x0f;
                if (p1 == p2)
                {
                    p1 = p1Value[4] & 0x0f;
                    p2 = p2Value[4] & 0x0f;
                    if (p1 == p2)
                    {
                        CompareHandPai();
                        return true;
                    }
                    else
                    {
                        _winPlayerData = p1 > p2 ? player1 : player2;
                        return true;
                    }
                }
                else
                {
                    _winPlayerData = p1 > p2 ? player1 : player2;
                    return true;
                }
            }
            else
            {
                _winPlayerData = p1 > p2 ? player1 : player2;
                return true;
            }
        }


        /// <summary>
        /// 比较散牌
        /// </summary>
        /// <returns></returns>
        bool CompareSanPai()
        {
            var p1s = GetAllMaxDanPai(AddCardToDesk(deskCards, player1).ToArray());
            var p2s = GetAllMaxDanPai(AddCardToDesk(deskCards, player2).ToArray());
            for (var index = 1; index <= maxConfig; index++)
            {
                var p1 = p1s[maxConfig - index] & 0x0f;
                var p2 = p2s[maxConfig - index] & 0x0f;
                if (p1 == p2)
                {
                    continue;
                }
                else
                {
                    _winPlayerData = p1 > p2 ? player1 : player2;
                    return true;
                }
            }

            CompareHandPai();
            return true;
        }

        /// <summary>
        /// 比较同花顺
        /// 同花顺之间的比较：
        /// A、同花顺虽然以花色成牌型，但两个同花顺相比时不比花色，只比点数，从最大的一张牌开始比起，大者胜；
        /// B、果相同，则再比稍小一点的那张，大者胜；
        /// C、如果再相同再往下比，直到分出大小来；
        /// D、若是五张牌的点数都一样大，则两个同花顺同样大小。
        /// E、比如黑桃的10、J、Q、K、A与梅花的10、J、Q、K、A相比，两者是同样大小的。
        /// </summary>
        /// <returns>比较成功返回true 没比较出来返回false</returns>
        bool CompareTonngHuaShun()
        {
            //同花顺比较
            var player1Tonghushun = GetAllTongHuaShun(AddCardToDesk(deskCards, player1).ToArray());
            var player2Tonghushun = GetAllTongHuaShun(AddCardToDesk(deskCards, player2).ToArray());
            if (player1Tonghushun.Count < 0 && player2Tonghushun.Count > 0)
            {
                _winPlayerData = player2;
                return true;
            }
            else if (player1Tonghushun.Count > 0 && player2Tonghushun.Count < 0)
            {
                _winPlayerData = player1;
                return true;
            }
            else if (player1Tonghushun.Count > 0 && player2Tonghushun.Count > 0) //2个都是同花顺
            {
                //获取玩家最大的同花顺
                int[] maxP1 = Array.Empty<int>(), maxP2 = Array.Empty<int>();
                foreach (var value in player1Tonghushun)
                {
                    if (maxP1.Length == 0)
                    {
                        maxP1 = SortValue(value).ToArray();
                    }
                    else
                    {
                        var newValue = SortValue(value).ToArray();
                        var oldValue = SortValue(maxP1).ToArray();
                        if ((newValue[newValue.Length - 1] & 0x0f) > (oldValue[oldValue.Length - 1] & 0x0f))
                        {
                            maxP1 = newValue;
                        }
                    }
                }

                foreach (var value in player2Tonghushun)
                {
                    if (maxP2.Length == 0)
                    {
                        maxP2 = SortValue(value).ToArray();
                    }
                    else
                    {
                        var newValue = SortValue(value).ToArray();
                        var oldValue = SortValue(maxP2).ToArray();
                        if ((newValue[newValue.Length - 1] & 0x0f) > (oldValue[oldValue.Length - 1] & 0x0f))
                        {
                            maxP2 = newValue;
                        }
                    }
                }

                //皇家同花？
                var p1V = maxP1[maxP1.Length - 1] & 0x0f;
                var p2V = maxP2[maxP2.Length - 1] & 0x0f;
                if (p1V == p2V)
                {
                    //同样大
                    CompareHandPai();
                    return true;
                }
                else if (p1V > p2V)
                {
                    _winPlayerData = player1;
                }
                else
                {
                    _winPlayerData = player2;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取最大的5张散牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAllMaxDanPai(int[] cards)
        {
            var sortAllCards = SortValue(cards);
            sortAllCards.RemoveAt(0);
            sortAllCards.RemoveAt(0);
            return sortAllCards;
        }

        /// <summary>
        /// 获取三条牌型数据
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAllSanTiao(int[] cards)
        {
            var retData = new int[] {0, 0, 0, 0, 0};
            var dictionaryValue = DictionaryValue(cards);
            foreach (var values in dictionaryValue.Values)
            {
                if (values.Count == 3)
                {
                    if ((retData[0] & 0x0f) > (values[0] & 0x0f))
                    {
                        continue;
                    }

                    for (var index = 0; index < 3; index++)
                    {
                        retData[index] = values[index];
                    }
                }
                else if (values.Count == 1)
                {
                    var valueVal = values[0] & 0x0f;
                    var retMaxVal = retData[3] & 0x0f;
                    var retMinVal = retData[4] & 0x0f;
                    if (valueVal < retMinVal)
                    {
                        continue;
                    }
                    else
                    {
                        if (valueVal > retMaxVal)
                        {
                            retData[4] = retData[3];
                            retData[3] = values[0];
                        }
                        else
                        {
                            retData[4] = values[0];
                        }
                    }
                }
            }

            return retData.ToList();
        }

        /// <summary>
        /// 获取最大的对子牌型
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAll1Dui(int[] cards)
        {
            var retData = new int[] {0, 0, 0, 0, 0};

            var dictionaryValue = DictionaryValue(cards);
            foreach (var values in dictionaryValue.Values)
            {
                if (values.Count == 2)
                {
                    retData[0] = values[0];
                    retData[1] = values[1];
                }
                else if (values.Count == 1)
                {
                    var tempValue = values[0] & 0x0f;
                    var value1 = retData[2] & 0x0f;
                    var value2 = retData[3] & 0x0f;
                    var value3 = retData[4] & 0x0f;
                    if (tempValue > value3)
                    {
                        retData[4] = retData[0];
                    }

                    if (tempValue > value2)
                    {
                        retData[3] = retData[0];
                    }

                    if (tempValue > value1)
                    {
                        retData[2] = retData[0];
                    }
                }
            }

            if (retData[0] == 0)
            {
                return null;
            }

            return retData.ToList();
        }

        /// <summary>
        /// 获取2对牌型最大的
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAll2Dui(int[] cards)
        {
            var retData = new int[] {0, 0, 0, 0, 0};

            var dictionaryValue = DictionaryValue(cards);
            foreach (var values in dictionaryValue.Values)
            {
                if (values.Count == 2)
                {
                    
                    
                    
                    var maxValue = retData[0] & 0x0f;
                    var minValue = retData[2] & 0x0f;
                    var tempValue = values[0] & 0x0f;

                    if (tempValue > minValue)
                    {
                        if (tempValue > maxValue )
                        {
                            //大的给小的 并给大的重新赋值
                            retData[2] = retData[0];
                            retData[3] = retData[1];
                            
                            retData[0] = values[0];
                            retData[1] = values[1];
                        }
                        else
                        {
                            retData[2] = values[0];
                            retData[3] = values[1];
                        }
                    }
                }
                else if (values.Count == 1)
                {
                    var tempValue = values[0] & 0x0f;
                    var oneValue = retData[4] & 0x0f;
                    if (tempValue > oneValue)
                    {
                        retData[4] = values[0];
                    }
                }
            }

            if (retData[0] == retData[2])
            {
                return null;
            }

            return retData.ToList();
        }

        /// <summary>
        /// 获取葫芦的牌 最大的
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAllHuLu(int[] cards)
        {
            var retData = new int[] {0, 0, 0, 0, 0};

            var dictionaryValue = DictionaryValue(cards);
            foreach (var values in dictionaryValue.Values)
            {
                if (values.Count == 3)
                {
                    if ((retData[0] & 0x0f) > (values[0] & 0x0f))
                    {
                        continue;
                    }

                    for (var index = 0; index < 3; index++)
                    {
                        retData[index] = values[index];
                    }
                }
                else if (values.Count == 2)
                {
                    if ((retData[retData.Length - 1] & 0x0f) > (values[0] & 0x0f))
                    {
                        continue;
                    }

                    for (var index = 0; index < 2; index++)
                    {
                        retData[index + 3] = values[index];
                    }
                }
            }

            return retData.ToList();
        }


        /// <summary>
        /// 获取铁支
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> GetAllTieZi(int[] cards)
        {
            var dictionaryValue = DictionaryValue(cards);
            foreach (var value in dictionaryValue.Values)
            {
                if (value.Count >= maxConfig)
                {
                    return value;
                }
            }

            return new List<int>();
        }

        /// <summary>
        /// 获取同花顺
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int[]> GetAllTongHuaShun(int[] cards)
        {
            List<int[]> retData = new List<int[]>();
            var tonghua = GetAllTongHua(cards);
            var shunZi = GetAllShunZi(cards);
            foreach (var tonghuaValue in tonghua)
            {
                foreach (var shuzi in shunZi)
                {
                    if (tonghuaValue.SequenceEqual(shuzi))
                    {
                        retData.Add(tonghuaValue);
                    }
                }
            }

            return retData;
        }


        /// <summary>
        /// 2个牌放一个集合
        /// </summary>
        /// <param name="desk"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        List<int> AddCardToDesk(int[] desk, int[] player)
        {
            var cards = desk.ToList();
            cards.AddRange(player);
            return cards;
        }

        /// <summary>
        /// 牌更具值大小排序
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int> SortValue(int[] cards)
        {
            var cardsList = cards.ToList();
            cardsList.Sort(((i, i1) =>
            {
                var card1value = i & 0x0f;
                var card2value = i1 & 0x0f;
                if (card1value > card2value)
                {
                    return 1;
                }
                else if (card1value < card2value)
                {
                    return -1;
                }

                return 0;
            }));
            return cardsList;
        }

        /// <summary>
        /// 获得从n个不同元素中任意选取m个元素的组合的所有组合形式的列表
        /// </summary>
        /// <param name="elements">供组合选择的元素</param>
        /// <param name="m">组合中选取的元素个数</param>
        /// <returns>返回一个包含列表的列表，包含的每一个列表就是每一种组合可能</returns>
        List<List<int>> GetCombinationList(List<int> elements, int m)
        {
            List<List<int>> result = new List<List<int>>(); //存放返回的列表
            List<List<int>> temp = null; //临时存放从下一级递归调用中返回的结果
            List<int> oneList = null; //存放每次选取的第一个元素构成的列表，当只需选取一个元素时，用来存放剩下的元素分别取其中一个构成的列表；
            int oneElment; //每次选取的元素
            List<int> source = new List<int>(elements); //将传递进来的元素列表拷贝出来进行处理，防止后续步骤修改原始列表，造成递归返回后原始列表被修改；
            int n = 0; //待处理的元素个数

            if (elements != null)
            {
                n = elements.Count;
            }

            if (n == m && m != 1) //n=m时只需将剩下的元素作为一个列表全部输出
            {
                result.Add(source);
                return result;
            }

            if (m == 1) //只选取一个时，将列表中的元素依次列出
            {
                foreach (int el in source)
                {
                    oneList = new List<int>();
                    oneList.Add(el);
                    result.Add(oneList);
                    oneList = null;
                }

                return result;
            }

            for (int i = 0; i <= n - m; i++)
            {
                oneElment = source[0];
                source.RemoveAt(0);
                temp = GetCombinationList(source, m - 1);
                for (int j = 0; j < temp.Count; j++)
                {
                    oneList = new List<int>();
                    oneList.Add(oneElment);
                    oneList.AddRange(temp[j]);
                    result.Add(oneList);
                    oneList = null;
                }
            }


            return result;
        }

        /// <summary>
        /// 获取所有的同花集合
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int[]> GetAllTongHua(int[] cards)
        {
            List<int[]> retData = new List<int[]>();
            foreach (var sortCards in GetCombinationList(cards.ToList(), maxConfig))
            {
                bool isOk = true;
                for (int index = 0; index < sortCards.Count - 1; index++)
                {
                    var value = sortCards[index + 1] & 0xf0;
                    var value1 = sortCards[index] & 0xf0;
                    if (value != value1)
                    {
                        isOk = false;
                        break;
                    }
                }

                if (isOk)
                {
                    retData.Add(sortCards.ToArray());
                }
            }

            return retData;
        }

        /// <summary>
        /// 获取所有的顺子集合
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        List<int[]> GetAllShunZi(int[] cards)
        {
            List<int[]> retData = new List<int[]>();
            foreach (var listInt in GetCombinationList(cards.ToList(), maxConfig))
            {
                var sortCards = SortValue(listInt.ToArray());
                bool isOk = true;
                for (int index = 1; index < sortCards.Count - 1; index++)
                {
                    var value = sortCards[index - 1] & 0x0f;
                    var value1 = sortCards[index] & 0x0f;
                    var value2 = sortCards[index + 1] & 0x0f;
                    if (value + 1 != value1 || value1 + 1 != value2)
                    {
                        isOk = false;
                        break;
                    }
                }

                if (isOk)
                {
                    retData.Add(sortCards.ToArray());
                }
            }
            // var sortCards = SortValue(cards);
            // var dicValue = DictionaryValue(sortCards.ToArray());
            // var sortKeyValues = SortValue(dicValue.Keys.ToArray());
            // var shunZiKeys = new List<int>();
            // for (var index = 0; index < sortKeyValues.Count - 2; index++)
            // {
            //     var value = sortKeyValues[index];
            //     shunZiKeys.Add(value);
            //     if (value + 1 != sortKeyValues[index + 1])
            //     {
            //         shunZiKeys.Clear();
            //     }
            // }
            //
            // //有顺子
            // if (shunZiKeys.Count >= maxConfig)
            // {
            //     for (var index = 0; index < shunZiKeys.Count - maxConfig; index++)
            //     {
            //         // int[]
            //         for (var index_1 = index; index_1 < maxConfig + index; index_1++)
            //         {
            //         }
            //     }
            // }

            return retData;
        }

        /// <summary>
        /// 更具值分类
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        Dictionary<int, List<int>> DictionaryValue(int[] cards)
        {
            Dictionary<int, List<int>> valueDictionary = new Dictionary<int, List<int>>();
            foreach (int cardValue in cards)
            {
                var value = cardValue & 0x0f;
                if (!valueDictionary.ContainsKey(value))
                {
                    valueDictionary[value] = new List<int>();
                }

                valueDictionary[value].Add(cardValue);
            }

            return valueDictionary;
        }
    }
}