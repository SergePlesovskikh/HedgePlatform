using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IPDFService
    {
        byte[] PdfConvert(string html);
    }
}
