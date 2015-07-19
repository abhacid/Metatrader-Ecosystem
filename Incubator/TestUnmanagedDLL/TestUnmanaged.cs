#region Infos
// Project Hosting for Open Source Software on Github : https://github.com/abhacid/Metatrader-Ecosystem

#endregion

using System;
using System.Text;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Metatrader.Incubator
{
   class TestUnmanaged
    {
	   [DllExport("AddInteger", CallingConvention = CallingConvention.StdCall)]
	   public static int AddInteger(int left, int right)
	   {
		   return left + right;
	   }

        [DllExport("AddDouble", CallingConvention = CallingConvention.StdCall)]
        public static double AddDouble(double Value1, double Value2) {
            MessageBox.Show("AddDouble: " + Value1.ToString() + " " + Value2.ToString());
            double Value3 = Value1 + Value2;
            return (Value3);
        }

        [DllExport("AddDoubleString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]  // note this change for build 600+
        public static string AddDoubleString(double  Value1, double Value2) {
            MessageBox.Show("AddDoubleString: " + Value1.ToString() + " " + Value2.ToString());
            double Value3 = Value1 + Value2;
            return (Value3.ToString() );
        }

        [DllExport("returnString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)] // note this change for build 600+ as well as MarshalAs statement on the next line...
        public static string returnString ([MarshalAs(UnmanagedType.LPWStr)] string Input) {
            MessageBox.Show("Received: " + Input);
            return ("SEND to MT4");
        }

        // many thanks to anonymous for the code sample below!
        [DllExport("ReturnDouble2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static double ReturnDouble2() {
            return 4.5;
        }

       // a double array is passed in as a parameter from MT4, for the purposes of this demo, 
       // only the first five (or fewer) items in array data are processed.
       // An average is created from the first 5 items in data and returned to MT4, 
       // and those first 5 items items are shown in a message box for validation purposes.
        [DllExport("PassDoubleArray", CallingConvention = CallingConvention.StdCall)]
        public static double PassDoubleArray ([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] data,
            int datasize)  
        {
            string ff = "first five items (or less):";
            int end = 5;
            if (end > data.Length) end = data.Length;
            double ave = 0.0;
            for (int i = 0; i < end; i++) {
                ff = ff + " " + data[i].ToString();
                ave += data[i];
            }
            if (end == 0) end = 1;
            ave /= end;
            MessageBox.Show("Received " + ff);
            return (ave);
        }

       /*
       NOTE:

        The key with array passing (as a parameter to C#) is the SizeParamIndex field (above).
        When SizeParamIndex is equal to 1 (shown above) the size of the array data must be contained
        In the parameter in slot 1 (the 2nd parameter). 
        This is fulfilled by the variable datasize. If datasize were the first parameter, then SizeParamIndex = 0 would
        be appropriate.

       This is inferred from this page:
       www.mql5.com/en/articles/249
       (See 4.3 example 3 as well as some others) 
       Note how both arrays reference SizeParamIndex = 1 which is the "len" parameter in slot 1 (2nd parameter).
       
       Also note the use of [In Out MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] when passing an array by reference in that example,
       and below in PassDoubleArrayByref example.
       */

        [DllExport("PassDoubleArrayByref", CallingConvention = CallingConvention.StdCall)]
        public static double PassDoubleArrayByref ([In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] data,
            int datasize) {
            string ff = "first five items (or less):";
            int end = 5;
            if (end > data.Length) end = data.Length;
            double mean = 0.0;
            for (int i = 0; i < end; i++) {
                ff = ff + " " + data[i].ToString();
                mean += data[i];
            }
            
            if (end == 0) end = 1;
            mean /= end;
            // subtract the mean from each of the first values in data
            ff = ff + "\r\n" + "Data passed back to MT4: " + "\r\n" ;
            for (int i = 0; i < end; i++) {
                data[i] -= mean; // subtract the mean
                ff = ff + " " + data[i].ToString();
            }
            MessageBox.Show("Received " + ff);
            return (mean);
        }


   }
}
