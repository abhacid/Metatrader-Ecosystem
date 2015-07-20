using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace Metrader.Incubator
{
    public class Ex4Generator : IVsSingleFileGenerator
    {
        int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".ex4";
            return pbstrDefaultExtension.Length;
        }

        int IVsSingleFileGenerator.Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            throw new NotImplementedException();
        }
    }
}
