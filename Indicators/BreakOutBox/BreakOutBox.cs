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
using System;
using System.Drawing;

namespace Metatrader.Indicators
{
    /// <summary>
    ///             //+------------------------------------------------------------------+
    ///            //|                                               BreakOut_BOX_5.mq4 |
    ///            //|                                                        hapalkos  |
    ///            //|                                                       2007.02.16 |
    ///            //|      Code based on TIME1_modified.mq4 shown below                |
    ///            //|         posted to Forex Factory by glenn5t                       |
    ///            //|      Concept of the break-out box form by Markj                  |
    ///            //|                                                                  |
    ///            //|   ++ modified so that rectangles do not overlay                  |
    ///            //|   ++ this makes color selection more versatile                   |
    ///            //|  +++ code consolidated to facilitate changes                     |
    ///            //|  +++ changed Period B rectangle and added offset value           |
    ///            //|  +++ corrected error in bar selection code                            |
    ///            //| ++++ added ability to cross 00:00(nextDayA, nextDayB)            |
    ///            //| ++++ added Unique ID to allow for multiple indicators            |
    ///            //| ++++ added ability to delete rectangles specific to an indicator |
    ///            //|+++++ added Open/Close boxes and lines per Accrete                |
    ///            //|+++++ added Alerts - can be used with Tesla's Alerter EA          |
    ///            //+------------------------------------------------------------------+
    /// </summary>
    public class BreakOutBox : MqlApi
    {
        [ExternVariable]
        string UniqueID = "DailyCandles";  // --- Server Time --- GMT+2 ---
        [ExternVariable]
        int NumberOfDays = 50;
        [ExternVariable]
        string periodA_begin = "00:00";      // Day0
        [ExternVariable]
        string periodA_end = "00:00";      // Day1
        [ExternVariable]
        int nextDayA = 1;            // Set to zero if periodA_begin and periodA_end are on the same day.
                                     // Set to one if periodA_end is on the next day.
        [ExternVariable]
        string periodB_end = "00:00";      // Day1
        [ExternVariable]
        int nextDayB = 1;            // Set to zero if periodA_begin and periodB_end are on the same day.
                                     // Set to one if periodB_end is on the next day.

        [ExternVariable]
        Color rectAB_color1 = Color.PowderBlue;
        [ExternVariable]
        Color rectAB_color2 = Color.LightCoral;  // second rectangle color to indicate relative position of Open/Close

        [ExternVariable]
        bool rectAB_background = true;        // true - filled solid; false - outline
        [ExternVariable]
        Color rectA_color = Color.Red;
        [ExternVariable]
        bool rectA_background = false;       // true - filled solid; false - outline

        [ExternVariable]
        Color rectB1_color = Color.DarkOrchid;
        [ExternVariable]
        bool rectB1_background = false;       // true - filled solid; false - outline
        [ExternVariable]
        int rectB1_band = 0;           // Box Break-Out Band for the Period B rectangle

        [ExternVariable]
        Color rectsB2_color = Color.SkyBlue;
        [ExternVariable]
        bool rectsB2_background = true;        // true - filled solid, false - outline
        [ExternVariable]
        int rectsB2_band = 5;           // Box Break-Out Band for the two upper and lower rectangles

        [ExternVariable]
        bool OpenClose = true;          // false - High/Low boxes;  true - Open/Close boxes
        [ExternVariable]
        bool rectA_close_begin = true;          // false - period A close indication line begins at end of period A;  true - period A close line is drawn from beginning of period A
        [ExternVariable]
        bool periodB_ALERTS = false;         // Alerts are set on the upper and lower sides of the upper and lower rectangles - rectsB2
                                             // rectsB2_band determines the location of the Alerts
                                             // Alert_xx added to the decription so that Alerts can be activated by Tesla's Alerter EA

        private string Name
        {
            get{ return ToString(); }
        }

        /// <summary>
        /// Custom indicator initialization function  
        /// </summary>
        /// <returns></returns>
        public override int init()
        {
            DeleteObjects();

            return 0;
        }

        /// <summary>
        /// Custom indicator deinitialization function  
        /// </summary>
        public override int deinit()
        {
            DeleteObjects();

            return 0;
        }

        /// <summary>
        /// Remove all indicator Rectangles
        /// </summary>
        void DeleteObjects()
        {
            DateTime dtTradeDate = TimeCurrent();
            string sRectABname;

            if (OpenClose)
                sRectABname = " BoxOC  ";
            else
                sRectABname = " BoxHL  ";

            for (int i = 0; i < NumberOfDays; i++)
            {

                ObjectDelete(UniqueID + sRectABname + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxOpen  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxClose  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxBO_High  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxBO_HIGH ALERT " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxBO_Low  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxBO_LOW ALERT  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxPeriodA  " + TimeToStr(dtTradeDate, TIME_DATE));
                ObjectDelete(UniqueID + " BoxPeriodB  " + TimeToStr(dtTradeDate, TIME_DATE));

                dtTradeDate = decrementTradeDate(dtTradeDate);
                while (TimeDayOfWeek(dtTradeDate) > 5 || TimeDayOfWeek(dtTradeDate) < 1) dtTradeDate = decrementTradeDate(dtTradeDate);     // Removed Sundays from plots
            }
        }

        //+------------------------------------------------------------------+
        //| Custom indicator iteration function                              |
        //+------------------------------------------------------------------+
        public override int start()
        {
            DateTime dtTradeDate = TimeCurrent();
            string sRectABname;

            if (OpenClose)
                sRectABname = " BoxOC  ";
            else
                sRectABname = " BoxHL  ";

            for (int i = 0; i < NumberOfDays; i++)
            {
                DrawObjects(dtTradeDate, UniqueID + sRectABname + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectAB_color1, rectAB_color2, 0, 1, rectAB_background, nextDayA, nextDayB, OpenClose, false);
                DrawObjects(dtTradeDate, UniqueID + " BoxOpen  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectAB_color1, rectAB_color2, 0, 6, false, nextDayA, nextDayB, OpenClose, true);
                DrawObjects(dtTradeDate, UniqueID + " BoxClose  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectAB_color1, rectAB_color2, 0, 7, false, nextDayA, nextDayB, OpenClose, false);
                DrawObjects(dtTradeDate, UniqueID + " BoxBO_High  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectsB2_color, rectAB_color2, rectsB2_band, 2, rectsB2_background, nextDayA, nextDayB, OpenClose, false);

                if (periodB_ALERTS)
                    DrawObjects(dtTradeDate, UniqueID + " BoxBO_HIGH ALERT " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectsB2_color, rectAB_color2, rectsB2_band, 8, false, nextDayA, nextDayB, OpenClose, false);

                DrawObjects(dtTradeDate, UniqueID + " BoxBO_Low  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectsB2_color, rectAB_color2, rectsB2_band, 3, rectsB2_background, nextDayA, nextDayB, OpenClose, false);

                if (periodB_ALERTS)
                    DrawObjects(dtTradeDate, UniqueID + " BoxBO_LOW ALERT  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectsB2_color, rectAB_color2, rectsB2_band, 9, false, nextDayA, nextDayB, OpenClose, false);

                DrawObjects(dtTradeDate, UniqueID + " BoxPeriodA  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodA_end, rectA_color, rectAB_color2, 0, 4, rectA_background, nextDayA, nextDayB, OpenClose, false);
                DrawObjects(dtTradeDate, UniqueID + " BoxPeriodB  " + TimeToStr(dtTradeDate, TIME_DATE), periodA_begin, periodA_end, periodB_end, rectB1_color, rectAB_color2, rectB1_band, 5, rectB1_background, nextDayA, nextDayB, OpenClose, false);

                dtTradeDate = decrementTradeDate(dtTradeDate);
                while (TimeDayOfWeek(dtTradeDate) > 5 || TimeDayOfWeek(dtTradeDate) < 1) dtTradeDate = decrementTradeDate(dtTradeDate);     // Removed Sundays from plots
            }

            return 0;
        }

        /// <summary>
        /// Create Objects - Rectangles and Trend lines
        /// </summary>
        /// <param name="dtTradeDate"></param>
        /// <param name="sObjName"></param>
        /// <param name="sTimeBegin"></param>
        /// <param name="sTimeEnd"></param>
        /// <param name="sTimeObjEnd"></param>
        /// <param name="cObjColor1"></param>
        /// <param name="cObjColor2"></param>
        /// <param name="iOffSet"></param>
        /// <param name="iForm"></param>
        /// <param name="background"></param>
        /// <param name="nextDayA"></param>
        /// <param name="nextDayB"></param>
        /// <param name="OpenClose"></param>
        /// <param name="rectA_close_begin"></param>
        void DrawObjects(DateTime dtTradeDate, string sObjName, string sTimeBegin, string sTimeEnd, string sTimeObjEnd, Color cObjColor1, Color cObjColor2, int iOffSet, int iForm, bool background, int nextDayA, int nextDayB, bool OpenClose, bool rectA_close_begin)
        {
            DateTime dtTimeBegin, dtTimeEnd, dtTimeObjEnd;
            double dPriceHigh, dPriceLow, dPriceOpen, dPriceClose;
            int iBarBegin, iBarEnd;
            string sObjDesc;

            dtTimeBegin = StrToTime(TimeToStr(dtTradeDate, TIME_DATE) + " " + sTimeBegin);
            dtTimeEnd = StrToTime(TimeToStr(dtTradeDate, TIME_DATE) + " " + sTimeEnd);
            dtTimeObjEnd = StrToTime(TimeToStr(dtTradeDate, TIME_DATE) + " " + sTimeObjEnd);

            if (nextDayA == 1)
                dtTimeEnd = dtTimeEnd.AddDays(1);
            if (nextDayB == 1)
                dtTimeObjEnd = dtTimeObjEnd.AddDays(1);
            if (nextDayA == 1 && TimeDayOfWeek(dtTradeDate) == 5)
                dtTimeEnd = dtTimeEnd.AddDays(2);
            if (nextDayB == 1 && TimeDayOfWeek(dtTradeDate) == 5)
                dtTimeObjEnd = dtTimeObjEnd.AddDays(2);

            iBarBegin = iBarShift(null, 0, dtTimeBegin) + 1;                                    // added 1 to bar count to correct calculation for highest price for the period
            iBarEnd = iBarShift(null, 0, dtTimeEnd) + 1;                                        // added 1 to bar count to correct calculation for lowest price for the period 

            dPriceHigh = High[iHighest(null, 0, MODE_HIGH, iBarBegin - iBarEnd, iBarEnd)];
            dPriceLow = Low[iLowest(null, 0, MODE_LOW, iBarBegin - iBarEnd, iBarEnd)];
            dPriceOpen = Open[iBarBegin - 1];                                                 // Open/Close added to enable Open/Close rectangles
            dPriceClose = Close[iBarEnd];

            if (OpenClose)
            {                                                                // Selection of extremes of Open/Close values
                dPriceHigh = MathMax(dPriceOpen, dPriceClose);
                dPriceLow = MathMin(dPriceOpen, dPriceClose);
            }

            //---- High-Low Rectangle - Period A and B combined
            if (iForm == 1)
            {
                ObjectCreate(sObjName, OBJ_RECTANGLE, 0, DateTime.MinValue, 0, DateTime.MinValue, 0);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeBegin);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceHigh);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceLow);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                if (dPriceClose < dPriceOpen && OpenClose == true) cObjColor1 = cObjColor2;        // Color change of rectangle dependent on Open/Close positions
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
            }

            //---- Upper Rectangle  - Period B
            if (iForm == 2)
            {
                ObjectCreate(sObjName, OBJ_RECTANGLE, 0, DateTime.MinValue, 0, DateTime.MinValue, 0);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceHigh);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceHigh + iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
            }

            //---- Lower Rectangle - Period B
            if (iForm == 3)
            {
                ObjectCreate(sObjName, OBJ_RECTANGLE, 0, DateTime.MinValue, 0, DateTime.MinValue, 0);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceLow - iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceLow);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
            }

            //---- Period A Rectangle
            if (iForm == 4)
            {
                ObjectCreate(sObjName, OBJ_RECTANGLE, 0, DateTime.MinValue, 0, DateTime.MinValue, 0);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeBegin);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceHigh);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceLow);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_WIDTH, 2);
                ObjectSet(sObjName, OBJPROP_BACK, background);
                sObjDesc = StringConcatenate("High: ", dPriceHigh, "  Low: ", dPriceLow);
                ObjectSetText(sObjName, sObjDesc, 10, "Times New Roman", Color.Black);
            }
            //---- Period B Rectangle
            if (iForm == 5)
            {
                ObjectCreate(sObjName, OBJ_RECTANGLE, 0, DateTime.MinValue, 0, DateTime.MinValue, 0);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceHigh + iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceLow - iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_WIDTH, 2);
                ObjectSet(sObjName, OBJPROP_BACK, background);
            }
            //---- Period A Open Line
            if (iForm == 6)
            {
                ObjectCreate(sObjName, OBJ_TREND, 0, DateTime.MinValue, 0, DateTime.MinValue);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeBegin);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceOpen);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceOpen);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_DASH);
                ObjectSet(sObjName, OBJPROP_COLOR, Green);
                ObjectSet(sObjName, OBJPROP_WIDTH, 1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
                ObjectSet(sObjName, OBJPROP_RAY, false);
            }
            //---- Period A Close Line
            rectA_close_begin = true;
            if (iForm == 7)
            {
                ObjectCreate(sObjName, OBJ_TREND, 0, DateTime.MinValue, 0, DateTime.MinValue);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                if (rectA_close_begin) ObjectSet(sObjName, OBJPROP_TIME1, dtTimeBegin);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceClose);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceClose);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_DASHDOTDOT);
                ObjectSet(sObjName, OBJPROP_COLOR, Red);
                ObjectSet(sObjName, OBJPROP_WIDTH, 1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
                ObjectSet(sObjName, OBJPROP_RAY, false);
            }
            //---- Period B HIGH Alert
            if (iForm == 8)
            {
                ObjectCreate(sObjName, OBJ_TREND, 0, DateTime.MinValue, 0, DateTime.MinValue);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceHigh + iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceHigh + iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_WIDTH, 1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
                ObjectSet(sObjName, OBJPROP_RAY, false);
                sObjDesc = StringConcatenate("Period B - HIGH Alert_01 - ", dPriceHigh + iOffSet * Point);
                ObjectSetText(sObjName, sObjDesc, 10, "Times New Roman", Color.Black);
                //      if(Bid >= (dPriceHigh + iOffSet*Point)) Alert(sObjName," - ",dPriceHigh + iOffSet*Point);
            }
            //---- Period B LOW Alert
            if (iForm == 9)
            {
                ObjectCreate(sObjName, OBJ_TREND, 0, DateTime.MinValue, 0, DateTime.MinValue);
                ObjectSet(sObjName, OBJPROP_TIME1, dtTimeEnd);
                ObjectSet(sObjName, OBJPROP_TIME2, dtTimeObjEnd);
                ObjectSet(sObjName, OBJPROP_PRICE1, dPriceLow - iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_PRICE2, dPriceLow - iOffSet * Point);
                ObjectSet(sObjName, OBJPROP_STYLE, STYLE_SOLID);
                ObjectSet(sObjName, OBJPROP_COLOR, cObjColor1);
                ObjectSet(sObjName, OBJPROP_WIDTH, 1);
                ObjectSet(sObjName, OBJPROP_BACK, background);
                ObjectSet(sObjName, OBJPROP_RAY, false);
                sObjDesc = StringConcatenate("Period B - LOW Alert_01 - ", dPriceLow - iOffSet * Point);
                ObjectSetText(sObjName, sObjDesc, 10, "Times New Roman", Color.Black);
                //      if(Bid <= (dPriceLow - iOffSet*Point)) Alert(sObjName," - ",dPriceLow - iOffSet*Point);    
            }
        }

        /// <summary>
        /// Decrement Date to draw objects in the past
        /// </summary>
        /// <param name="dtTimeDate"></param>
        /// <returns></returns>
        DateTime decrementTradeDate(DateTime dtTimeDate)
        {
            int iTimeYear = TimeYear(dtTimeDate);
            int iTimeMonth = TimeMonth(dtTimeDate);
            int iTimeDay = TimeDay(dtTimeDate);
            int iTimeHour = TimeHour(dtTimeDate);
            int iTimeMinute = TimeMinute(dtTimeDate);

            if (--iTimeDay == 0)
            {
                if (--iTimeMonth == 0)
                {
                    iTimeYear--;
                    iTimeMonth = 12;
                }

                // Thirty days hath September...  
                if (iTimeMonth == 4 || iTimeMonth == 6 || iTimeMonth == 9 || iTimeMonth == 11)
                    iTimeDay = 30;
                // ...all the rest have thirty-one...
                if (iTimeMonth == 1 || iTimeMonth == 3 || iTimeMonth == 5 || iTimeMonth == 7 || iTimeMonth == 8 || iTimeMonth == 10 || iTimeMonth == 12)
                    iTimeDay = 31;
                // ...except bisextil year...
                if (iTimeMonth == 2)
                    if (MathMod(iTimeYear, 4) == 0)
                        iTimeDay = 29;
                    else
                        iTimeDay = 28;
            }

            return (StrToTime(iTimeYear + "." + iTimeMonth + "." + iTimeDay + " " + iTimeHour + ":" + iTimeMinute));
        }




    }
}
