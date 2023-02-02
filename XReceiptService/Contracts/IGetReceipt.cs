using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XReceiptService.Helper;

namespace XReceiptService.Contracts
{
    public interface IGetReceipt
    {
        Task<(ReceipServicetResponse, int)> AcceptGetReceiptAsync(string command, string htmlTemplate);
    }
}