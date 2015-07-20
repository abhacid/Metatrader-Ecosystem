
#property indicator_chart_window    // Indicator is drawn in the main window
#property indicator_buffers 2       // Number of buffers
#property indicator_color1 Blue     // Color of the 1st line
#property indicator_color2 Red      // Color of the 2nd line

#import "nquotes/nquoteslib.ex4"
    int nquotes_setup(string className, string assemblyName);
    int nquotes_init();
    int nquotes_start();
    int nquotes_deinit();
    int nquotes_get_property_array_size(string name);
    int nquotes_get_property_adouble(string name, double& value[]);
    int nquotes_get_property_astring(string name, string& value[]);
#import

double Buf0[];
double Buf1[];  

int init()                          
{
    nquotes_setup("Metatrader.Indicators.Candlesticks", "Metatrader.Indicators.Candlesticks");

    SetIndexBuffer(0, Buf0);         
    SetIndexBuffer(1, Buf1);         
    
    Print("nquotes_init...");

    return (nquotes_init());
}

int start()                         
{
    int result = nquotes_start();
    if (result < 0)
    {
        Print("nquotes_start has failed");
        return (result);
    }
    
    int size = nquotes_get_property_array_size("extMapBuffer0");
    double extMapBuffer0[];
    ArrayResize(extMapBuffer0, size);
    nquotes_get_property_adouble("extMapBuffer0", extMapBuffer0);

    size = nquotes_get_property_array_size("extMapBuffer1");
    double extMapBuffer1[];
    ArrayResize(extMapBuffer1, size);
    nquotes_get_property_adouble("extMapBuffer1", extMapBuffer1);

    for (int i = 0; i < size; i++)
    {
        Buf0[i] = extMapBuffer0[i];
        Buf1[i] = extMapBuffer1[i];
    }
    
    return (result);
}

int deinit()
{
    Print("nquotes_deinit...");
    return (nquotes_deinit());
}




