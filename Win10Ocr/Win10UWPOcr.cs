using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Media.Ocr;
using System.Threading;
using Newtonsoft.Json;
using Windows.Foundation;

namespace Win10Ocr
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ResultOCR
    {
        //自定义的数据结构
        public string value;
        public Rect rect;

    }
    public class Win10UWPOcr
    {
        OcrEngine engine;
        public Win10UWPOcr(string language)
        {
            var lang = new Windows.Globalization.Language(language);
            if (OcrEngine.IsLanguageSupported(lang))
            {
                engine = OcrEngine.TryCreateFromLanguage(lang);
                if (engine == null)
                {
                    throw new Exception(string.Format("TryCreateFromLanguage {0} Error", language));
                }
            }
            else
            {
                throw new Exception(string.Format("Language {0} is not supported", language));
            }
        }
        public Win10UWPOcr()
        {
            engine = OcrEngine.TryCreateFromUserProfileLanguages();
            if (engine == null)
            {
                throw new Exception(string.Format("TryCreateFromLanguage UserProfile Error"));
            }
        }



        public async Task<OcrResult> RecognizeAsyncRects(string imagePath)
        {
            var path = Path.GetFullPath(imagePath);

            if (!File.Exists(path))
            {
                return null;
            }
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);


            using (IRandomAccessStream randomAccessStream = await storageFile.OpenReadAsync())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
                using (SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied))
                {
                    OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
                    return ocrResult;
                }
            }


        }

        public OcrResult RecognizeRects(string imagePath)
        {
            return RecognizeAsyncRects(imagePath).GetAwaiter().GetResult();
        }
        public string RecognizeString(string imagePath)
        {
            var res = RecognizeAsyncRects(imagePath).GetAwaiter().GetResult();
            var result = "";
            foreach (var tempLine in res.Lines)
            {
                string line = "";
                foreach (var word in tempLine.Words)
                {
                    line += word.Text;
                }
                result += line + Environment.NewLine;
            }
            return result;
        }

        public string RecognizeJsonStringWord(string imagePath)
        {
            var res = RecognizeAsyncRects(imagePath).GetAwaiter().GetResult();

            var all_reslut = new List<List<ResultOCR>>();
            foreach (var line in res.Lines)
            {
                var lines_reslut = new List<ResultOCR>();

                foreach (var word in line.Words)
                {
                    var resi = new ResultOCR();
                    resi.value = word.Text;
                    resi.rect = word.BoundingRect;
                    lines_reslut.Add(resi);
                }
                all_reslut.Add(lines_reslut);
            }
            return JsonConvert.SerializeObject(all_reslut);
        }

        public string RecognizeJsonStringLine(string imagePath)
        {
            var res = RecognizeAsyncRects(imagePath).GetAwaiter().GetResult();
            var all_reslut = new List<ResultOCR>();
            foreach (var line in res.Lines)
            {
                var resi = new ResultOCR();
                resi.value = string.Concat(line.Words.Select(w=> w.Text));
                double left = line.Words.Min(o => o.BoundingRect.Left);
                double top = line.Words.Min(o => o.BoundingRect.Top);
                double right = line.Words.Max(o => o.BoundingRect.Right);
                double bottom = line.Words.Max(o => o.BoundingRect.Bottom);
                resi.rect = new Rect(left, top, right - left, bottom - top);
                all_reslut.Add(resi);
            }
            return JsonConvert.SerializeObject(all_reslut);
        }
    }

}
