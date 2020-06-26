using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win10Ocr;

namespace ConsoleOcr
{
    class Program
    {
        static void Main(string[] args)
        {
            Win10UWPOcr win10UWPOcr = new Win10UWPOcr();

            var res = win10UWPOcr.RecognizeString(args[0]);
            Console.WriteLine(res);
        }
    }
}
