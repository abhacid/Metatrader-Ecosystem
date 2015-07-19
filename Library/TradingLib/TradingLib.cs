#region licence
/// <summary>
/// The MIT License(MIT)
///
/// Copyright(c) 2015 abdallah HACID, https://www.facebook.com/ab.hacid
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
/// </summary>

// Project Hosting for Open Source Software on Github : https://github.com/abhacid/Metatrader-Ecosystem

#endregion
using System;

namespace Metatrader.Lib
{
    public class Functions 
    {
        public  enum TradeType { Buy, Sell, BuyLimit, SellLimit, BuyStop, SellStop}

        private static int MacgicNumber = 317811; //FibonnacciNumber(29)
        static int MagicNumber
        {
            get { return MagicNumber; }
        }

        public int randomMagic(bool isFixeMagicNumber)
        {
            int magic = MacgicNumber;
            int randomMagicLower = 100000;
            int randomMagicUpper = 200000;
            Random random = new Random((int)DateTime.Now.Ticks);

            if (isFixeMagicNumber)
            {
                double randomNumber = random.Next(randomMagicLower, randomMagicUpper);
                magic = (int)Math.Round(randomNumber);
            }

            return magic;
        }

        /// <summary>
        /// Generate 8 random characters for comment
        /// Another way for a random number is using the previous closed order ticket number, 
        /// and then using that number for the comment in the next trade. That way, you can 
        /// keep track of everything as it goes.
        /// </summary>
        /// <returns></returns>
        public string generateRandomString()
        {
            string[] randomchars = new string[7];
            string alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456";
            Random random = new Random((int)DateTime.Now.Ticks);

            string randomString = "";

            for (int indexRandomString = 0; indexRandomString < 8; indexRandomString++)
            {
                int randomNumber = random.Next();
                randomNumber &= 31;
                randomchars[indexRandomString] = alphanumeric.Substring(randomNumber, 1);
                randomString = randomString + randomchars[indexRandomString];
            }

            //Comment("\n\n" + randomchars.ToString()); //Just display so we can see its random  

            return randomchars.ToString();
        }

        public double furtiftrailingStop(double bid, double ask, double trailingStart, double trailingStop, double entryPrice, double pipSize, TradeType tradeType)
        {
            bool isBuy = tradeType == TradeType.Buy;
            double newStopLoss = isBuy ? 0 : 10000;
            int factor = isBuy ? 1 : -1;
            double price = isBuy ? bid : ask;

            if ((price-entryPrice) * factor > trailingStart * pipSize) 
            {
                if ((price - entryPrice) * factor > trailingStop * pipSize)
                {
                    newStopLoss = price + factor * trailingStop * pipSize;

                }
            }

            return newStopLoss;
        }

}
}
