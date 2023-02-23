using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using QRCoder;

namespace AI_ERP.Application_Libs
{
    public static class QRCodeGenerator
    {
        public static Bitmap GetQRCode(string teks, int pixelsPerModule)
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(teks, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(pixelsPerModule);
        }
    }
}