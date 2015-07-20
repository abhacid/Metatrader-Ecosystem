#region licence
/// <summary>
/// The MIT License(MIT)
///
/// Copyright(c) 2015 abdallah HACID, https://www.facebook.com/ab.hacid
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
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

using NQuotes;
using System.Drawing;

namespace Metatrader.Indicators
{
    public class Candlesticks : MqlApi
    {
        private const int nCandles = 400;

        public double[] extMapBuffer0 = new double [nCandles];
        public double[] extMapBuffer1 = new double [nCandles];

        private string[] patternText = new string[nCandles];
        int limit;

        /// <summary>
        /// Custom indicator initialization function
        /// </summary>
        /// <returns></returns>
        public override int init()
        {
            SetIndexStyle(0, DRAW_ARROW, EMPTY, 1, Color.Red);
            SetIndexArrow(0, 234);

            SetIndexStyle(1, DRAW_ARROW, EMPTY, 1, Color.Blue);
            SetIndexArrow(1, 233);

            return (0);
        }

        /// <summary>
        /// Custom indicator deinitialization function
        /// </summary>
        /// <returns></returns>
        public override int deinit()
        {
            ObjectsDeleteAll(0, OBJ_TEXT);

            return (0);
        }

        /// <summary>
        /// Custom indicator iteration function    
        /// </summary>
        /// <returns></returns>
        public override int start()
        {
            int counted_bars = IndicatorCounted();
            limit = Bars - counted_bars;

            for (int index = 1; index < limit; index++)
            {
                patternText[index] = CharToStr(index);

                //---- check for possible errors
                if (counted_bars < 0)
                {
                    Alert("NO Bars..");
                    return (-1);
                }

                if (isBearishEngulfingPattern(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Bearish Engulfing", 9, "Times New Roman", Color.Red);

                }

                if (isDarkCloudCoverPattern(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Dark Cloud Cover", 9, "Times New Roman", Color.Red);

                }

                if (isHangingManPatternDown(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Hanging Man", 9, "Times New Roman", Color.Red);
                }

                if (isHangingManPatternUp(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Hanging Man", 9, "Times New Roman", Color.Red);

                }

                if (isInvertedHammerUp(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Inverted Hammer Up", 9, "Times New Roman", Color.Red);
                }

                if (isShootingStarUp(index))
                {
                    extMapBuffer0[index + 1] = High[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], High[index + 1] + 12 * Point);
                    ObjectSetText(patternText[index + 1], "Shooting Star Up", 9, "Times New Roman", Color.Red);
                }

                //*********************************************************************************************************************************************************************************************************************************************************************************************************//            

                if (isBullishEngulfing(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Bullish Engulfing", 9, "Times New Roman", Color.Blue);
                }

                if (isPricing(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Pricing Pattern", 9, "Times New Roman", Color.Blue);

                }

                // 
                if (isHamerPatternDown(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Hammer", 9, "Times New Roman", Color.Blue);

                }

                if (isHamerPatternUp(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Hammer", 9, "Times New Roman", Color.Blue);
                }

                // 
                if (isShootingStarDown(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Shooting Star Down", 9, "Times New Roman", Color.Blue);

                }

                if (isInvertedHammerDown(index))
                {
                    extMapBuffer1[index + 1] = Low[index + 1];
                    ObjectCreate(patternText[index + 1], OBJ_TEXT, 0, Time[index + 1], Low[index + 1] - 12 * Point);
                    ObjectSetText(patternText[index + 1], "Inverted Hammer Down", 9, "Times New Roman", Color.Blue);
                }
            }

            return (0);
        }

        /// <summary>
        /// Check for a Bearish Engulfing pattern
        /// </summary>
        /// <returns></returns>
        private bool isBearishEngulfingPattern(int index)
        {
            if (High[index + 4] < High[index + 3] && Low[index + 4] < Low[index + 3] &&
                High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                High[index + 2] < High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Open[index + 2] < Close[index + 2] && Open[index + 1] > Close[index + 1] &&
                (Open[index + 1] - Close[index + 1]) > (High[index + 1] - Low[index + 1]) / 3 &&
                (Close[index + 2] - Open[index + 2]) > (High[index + 2] - Low[index + 2]) / 3)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Check for Shooting Star Down
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isShootingStarDown(int index)
        {
            if (High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                High[index + 2] > High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Close[index + 1] > Open[index + 1] &&
                (Open[index + 1] - Low[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Close[index + 1] - Open[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for Hammer
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isHamerPatternDown(int index)
        {
            if (High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                High[index + 2] > High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Open[index + 1] > Close[index + 1] && (High[index + 1] - Open[index + 1]) <=
                (High[index + 1] - Low[index + 1]) / 6 &&
                (Open[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isHamerPatternUp(int index)
        {
            if (High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                High[index + 2] > High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Open[index + 1] < Close[index + 1] &&
                (High[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Close[index + 1] - Open[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for a Dark Cloud Cover pattern 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isDarkCloudCoverPattern(int index)
        {
            if (High[index + 4] < High[index + 3] && Low[index + 4] < Low[index + 3] &&
                High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                High[index + 2] < High[index + 1] && Low[index + 2] < Low[index + 1] &&
                Open[index + 2] < Close[index + 2] && Open[index + 1] > Close[index + 1] &&
                (Open[index + 1] - Close[index + 1]) > (High[index + 1] - Low[index + 1]) / 3 &&
                (Close[index + 2] - Open[index + 2]) > (High[index + 2] - Low[index + 2]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check for Hanging Man   
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isHangingManPatternDown(int index)
        {
            if (High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                High[index + 2] < High[index + 1] && Low[index + 2] < Low[index + 1] &&
                Open[index + 1] > Close[index + 1] &&
                (High[index + 1] - Open[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Open[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check for Hanging Man   
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isHangingManPatternUp(int index)
        {
            if (High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                    High[index + 2] < High[index + 1] && Low[index + 2] < Low[index + 1] &&
                    Open[index + 1] < Close[index + 1] &&
                    (High[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                    (Close[index + 1] - Open[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for Inverted Hammer Up
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isInvertedHammerUp(int index)
        {
            if (High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                High[index + 2] < High[index + 1] && Low[index + 2] < Low[index + 1] &&
                Close[index + 1] < Open[index + 1] &&
                (Close[index + 1] - Low[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Open[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for Shooting Star Up
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isShootingStarUp(int index)
        {
            if (High[index + 3] < High[index + 2] && Low[index + 3] < Low[index + 2] &&
                High[index + 2] < High[index + 1] && Low[index + 2] < Low[index + 1] &&
                Close[index + 1] > Open[index + 1] &&
                (Open[index + 1] - Low[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Close[index + 1] - Open[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for a Bullish Engulfing pattern
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isBullishEngulfing(int index)
        {
            if (High[index + 4] > High[index + 3] && Low[index + 4] > Low[index + 3] &&
                    High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                    High[index + 2] < High[index + 1] && Low[index + 2] > Low[index + 1] &&
                    Open[index + 2] < Close[index + 2] && Open[index + 1] > Close[index + 1] &&
                    (Open[index + 1] - Close[index + 1]) > (High[index + 1] - Low[index + 1]) / 3 &&
                    (Close[index + 2] - Open[index + 2]) > (High[index + 2] - Low[index + 2]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for a Pricing Pattern
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isPricing(int index)
        {
            if (High[index + 4] > High[index + 3] && Low[index + 4] > Low[index + 3] &&
                High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                High[index + 2] > High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Open[index + 2] < Close[index + 2] && Open[index + 1] > Close[index + 1] &&
                (Open[index + 1] - Close[index + 1]) > (High[index + 1] - Low[index + 1]) / 3 &&
                (Close[index + 2] - Open[index + 2]) > (High[index + 2] - Low[index + 2]) / 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check for Inverted Hammer Down
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool isInvertedHammerDown(int index)
        {
            if (High[index + 3] > High[index + 2] && Low[index + 3] > Low[index + 2] &&
                High[index + 2] > High[index + 1] && Low[index + 2] > Low[index + 1] &&
                Open[index + 1] > Close[index + 1] &&
                (Close[index + 1] - Low[index + 1]) <= (High[index + 1] - Low[index + 1]) / 6 &&
                (Open[index + 1] - Close[index + 1]) <= (High[index + 1] - Low[index + 1]) / 3)
                return true;
            else
                return false;
        }
    }
}
