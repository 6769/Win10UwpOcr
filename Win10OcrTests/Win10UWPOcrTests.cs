using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win10Ocr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win10Ocr.Tests
{
    [TestClass()]
    public class Win10UWPOcrTests
    {
        Win10UWPOcr win10UWPOcr = new Win10UWPOcr();
        string imageFile = "1.png";

        [TestMethod()]
        public void RecognizeSyncStringTest()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            var res = win10UWPOcr.RecognizeString(imageFile);
            Console.WriteLine(res);
        }

        [TestMethod()]
        public void RecognizeJsonStringTest()
        {
            var res = win10UWPOcr.RecognizeJsonStringWord(imageFile);
            Console.WriteLine(res);
        }

        [TestMethod()]
        public void RecognizeJsonStringLineTest()
        {
            var res = win10UWPOcr.RecognizeJsonStringLine(imageFile);
            Console.WriteLine(res);
        }
    }
}