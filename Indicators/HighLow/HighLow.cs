using NQuotes;

namespace Metatrader.Indicators
{
    public class HighLow : MqlApi
    {
        public double[] Buffer0NewValues;
        public double[] Buffer1NewValues;

        public override int start()
        {
            int countedBars = IndicatorCounted();

            //---- check for possible errors
            if (countedBars < 0)
                countedBars = 0;

            //---- the last counted bar will be recounted
            if (countedBars > 0)
                countedBars--;

            int barsToCount = Bars - countedBars;
            Buffer0NewValues = new double[barsToCount];
            Buffer1NewValues = new double[barsToCount];

            ArrayCopy(Buffer0NewValues, High, 0, 0, barsToCount);
            ArrayCopy(Buffer1NewValues, Low, 0, 0, barsToCount);

            return 0;
        }
    }
}
