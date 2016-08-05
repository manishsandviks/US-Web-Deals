using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deals.earlymoments.com.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        public class ImageResult : ActionResult
        {
            private readonly string _agha;
            private Font objFont;
            private Random oRndm = new Random();
            private string strCode = "";
            public ImageResult(string agha)
            {
                _agha = agha;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                using (Bitmap oBit = new Bitmap(110, 35))
                using (Graphics oGraph = Graphics.FromImage(oBit))
                {
                    oGraph.Clear(Color.FromArgb(205, 227, 248));

                    for (int i = 0; i <= 200; )
                    {
                        oGraph.DrawLine(new Pen(Color.FromArgb(188, 210, 231)), 200, i, 0, i);
                        oGraph.DrawLine(new Pen(Color.FromArgb(188, 210, 231)), i, 0, i, 200);
                        i += 10;
                    }

                    int j = 1;
                    foreach (char c in _agha)
                    {
                        string tmp = c.ToString();
                        oGraph.DrawString(tmp, GetFont(), GetBrush(), j, oRndm.Next(1, 10));
                        j = j + 20;

                    }
                    context.HttpContext.Response.ContentType = "image/jpg";
                    oBit.Save(context.HttpContext.Response.OutputStream, ImageFormat.Gif);
                }
            }

            private Font GetFont()
            {
                int i = oRndm.Next(1, 5);
                switch (i)
                {
                    case 1:
                        objFont = new Font("Arial", oRndm.Next(14, 20), FontStyle.Regular);
                        break;
                    case 2:
                        objFont = new Font("Arial", oRndm.Next(14, 20), FontStyle.Bold);
                        break;
                    case 3:
                        objFont = new Font("Arial", oRndm.Next(14, 20), FontStyle.Italic);
                        break;
                    case 4:
                        objFont = new Font("Arial", oRndm.Next(14, 20), FontStyle.Bold);
                        break;
                    case 5:
                    default:
                        objFont = new Font("Arial", oRndm.Next(14, 20), FontStyle.Regular);
                        break;
                }
                return objFont;
            }

            private Brush GetBrush()
            {
                int i = oRndm.Next(1, 5);
                switch (i)
                {
                    case 1:
                        return Brushes.Red;
                    case 2:
                        return Brushes.Brown;
                    case 3:
                        return Brushes.Blue;
                    case 4:
                        return Brushes.Magenta;
                    case 5:
                    default:
                        return Brushes.Yellow;
                }
            }
        }

        public ActionResult Image()
        {
            string agha = "";
            string imageChars = GetRandomStringForImage();
            Session.Add("rndtext", imageChars);
            return new ImageResult(imageChars);
        }

        public string GetRandomStringForImage()
        {
            Random oRndm = new Random();
            //string alphabet = "23456789ABCDEFGHIJKLMNPQRTUVWXYZ";
            //string alphabet = "23456789abcdefghijklmnpqrtuvwxyz";
            string alphabet = "abcdefghkmnqrtuwxyz";
            string strCode = "";
            for (int i = 1; i < 100; )
            {
                string tmp = alphabet[oRndm.Next(alphabet.Length)].ToString();
                i = i + 20;
                strCode += tmp;
            }
            return strCode;
        }
    }
}