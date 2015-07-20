#property indicator_chart_window               // Configuration for daily candle bodies

#import "nquotes/nquoteslib.ex4"
    int nquotes_setup(string className, string assemblyName);
    int nquotes_init();
    int nquotes_start();
    int nquotes_deinit();

	int nquotes_set_property_bool(string name, bool value);
	int nquotes_set_property_int(string name, int value);
	int nquotes_set_property_double(string name, double value);
	int nquotes_set_property_color(string name, color value);
	int nquotes_set_property_string(string name, string value);
	
   string nquotes_get_property_string(string name);
#import


    extern string UniqueID = "DailyCandles";	// --- Server Time --- GMT+2 ---
    extern int NumberOfDays = 50;
    extern string periodA_begin = "00:00";      // Day0
    extern string periodA_end = "00:00";		// Day1
    extern int nextDayA = 1;					// Set to zero if periodA_begin and periodA_end are on the same day.
												// Set to one if periodA_end is on the next day.
    extern string periodB_end = "00:00";		// Day1
    extern int nextDayB = 1;					// Set to zero if periodA_begin and periodB_end are on the same day.
												// Set to one if periodB_end is on the next day.

    extern color rectAB_color1 = PowderBlue;
    extern color rectAB_color2 = LightCoral;  // second rectangle color to indicate relative position of Open/Close

    extern bool rectAB_background = true;        // true - filled solid; false - outline
    extern color rectA_color = Red;
    extern bool rectA_background = false;       // true - filled solid; false - outline

    extern color rectB1_color = DarkOrchid;
    extern bool rectB1_background = false;       // true - filled solid; false - outline
    extern int rectB1_band = 0;           // Box Break-Out Band for the Period B rectangle

    extern color rectsB2_color = SkyBlue;
    extern bool rectsB2_background = true;        // true - filled solid, false - outline
    extern int rectsB2_band = 5;           // Box Break-Out Band for the two upper and lower rectangles

    extern bool OpenClose = true;				// false - High/Low boxes;  true - Open/Close boxes
    extern bool rectA_close_begin = true;       // false - period A close indication line begins at end of period A;  true - period A close line is drawn from beginning of period A
    extern bool periodB_ALERTS = false;         // Alerts are set on the upper and lower sides of the upper and lower rectangles - rectsB2
       
	string Name;   

int init()                          
{
   nquotes_setup("Metatrader.Indicators.BreakOutBox", "Metatrader.Indicators.BreakOutBox");
   
	Print("nquotes_init...");

    nquotes_set_property_string("UniqueID", UniqueID);
    nquotes_set_property_int("NumberOfDays", NumberOfDays);
    nquotes_set_property_string("periodA_begin", periodA_begin);
    nquotes_set_property_string("periodA_end", periodA_end);
    nquotes_set_property_int("nextDayA", nextDayA);
											
    nquotes_set_property_string("periodB_end", periodB_end);
    nquotes_set_property_int("nextDayB", nextDayB);				
										

    nquotes_set_property_color("rectAB_color1", rectAB_color1);
    nquotes_set_property_color("rectAB_color2", rectAB_color2);

    nquotes_set_property_bool("rectAB_background", rectAB_background);
    nquotes_set_property_color("rectA_color", rectA_color);
    nquotes_set_property_bool("rectA_background", rectA_background);

    nquotes_set_property_color("rectB1_color", rectB1_color);
    nquotes_set_property_bool("rectB1_background", rectB1_background);
    nquotes_set_property_int("rectB1_band", rectB1_band); 

    nquotes_set_property_color("rectsB2_color", rectsB2_color);
    nquotes_set_property_bool("rectsB2_background", rectsB2_background);
    nquotes_set_property_int("rectsB2_band", rectsB2_band); 

    nquotes_set_property_bool("OpenClose", OpenClose);
    nquotes_set_property_bool("rectA_close_begin", rectA_close_begin); 
    nquotes_set_property_bool("periodB_ALERTS", periodB_ALERTS); 

    Name = nquotes_get_property_string("Name");

    return (nquotes_init());
}

int start()                         
{
	int result = nquotes_start();

    if (result < 0)
    {
        Print(Name + " start has failed");
        return (result);
    }
    
    return (result);
}

int deinit()
{
    Print(Name+" deinit...");
    return (nquotes_deinit());
}


