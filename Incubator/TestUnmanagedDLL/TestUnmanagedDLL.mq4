// Project Hosting for Open Source Software on Github : https://github.com/abhacid/Metatrader-Ecosystem

//+------------------------------------------------------------------+
//|                                                   testDLL600.mq4 |
//|                               Copyright (c) 2014, Patrick M. White |
//|                     https://sites.google.com/site/marketformula/ |
//|                                    updated 5/12/2014             |
//+------------------------------------------------------------------+
#property copyright "Copyright (c) 2014, Patrick M. White"
#property link      "https://sites.google.com/site/marketformula/"
#property version   "1.00"
#property strict

#import "Metatrader.Incubator.dll"
   // nothing changed on the MT4 side. String handling changed in C#
   int AddInteger(int Value1, int Value2);
   double AddDouble(double Value1, double Value2);
   string AddDoubleString(double Value1, double Value2);
   string returnString(string Input);
   double ReturnDouble2(); 
   
   // note that the two calls below are identical, the key is in the C# dll adding 
   // [In, Out... to PassDoubleArrayByref
   double PassDoubleArray(double &data[], int datasize);
   double PassDoubleArrayByref(double &data[], int datasize); 
#import

//+------------------------------------------------------------------+
//| Script program start function                                    |
//+------------------------------------------------------------------+
void OnStart()
  {
	   Print("AddInteger: " + DoubleToStr(AddInteger(250, 750),0));
	   double a = AddDouble(250,750);
	   Print("AddDouble: " + DoubleToStr(a,4));

	   double d = StrToDouble(AddDoubleString(250, 750));
	   Print("AddDoubleString: " + DoubleToStr(d,4));
	   string temp = "Send to DLL";
	   string recv = returnString(temp);
	   Print(recv);
	   double dd = ReturnDouble2();
	   Print("Returning Double from C#: " + DoubleToStr(dd, 4));
   
	   double data[];
	   ArrayResize(data, 5);
	   string s = "Sending: ";
	   for (int i = 0; i < ArraySize(data); i++) {
		  data[i] = i + 1.0;
		  s = s + " " + DoubleToStr(data[i],1);
	   }
	   Print(s);
	   double ret = PassDoubleArray(data, ArraySize(data));
	   Print("Mean of 1+2+3+4+5: " + DoubleToStr(ret, 4));
   
	   ret = PassDoubleArrayByref(data, ArraySize(data));
	   s = "Receiving sent values less their mean: ";
	   for (int i = 0; i < ArraySize(data); i++) {
		  s = s + " " + DoubleToStr(data[i],2);
	   }
	   Print(s);
  }
