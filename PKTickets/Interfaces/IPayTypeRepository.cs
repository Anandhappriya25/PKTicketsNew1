using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface IPayTypeRepository
    {
        public List<PayType> GetAllPayTypes();
        public PayType PayTypeById(int id);
        public PayType PayTypeByName(string name);
        public Messages DeletePayType(int payTypeId);
        public Messages CreatePayType(PayType payType);
        public Messages UpdatePayType(PayType payType);
    }
}
