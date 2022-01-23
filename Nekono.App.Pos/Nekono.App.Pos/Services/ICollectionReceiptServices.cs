using Nekono.App.Pos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Services
{
    public interface ICollectionReceiptServices
    {
        Task<List<CollectionReceiptDetails>> GetToday();
        Task<string> Sale(CollectionReceiptDetails receiptDetails);
        Task<bool> Void(DeleteRequest voidRequest);
    }
}