using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class PayTypeRepository : IPayTypeRepository
    {
        private readonly PKTicketsDbContext db;
        public PayTypeRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<PayType> GetAllPayTypes()
        {
            return db.PayTypes.Where(x => x.IsActive == true).ToList();
        }
  
        public PayType PayTypeById(int id)
        {
            var payType = db.PayTypes.Where(x=>x.IsActive==true).FirstOrDefault(x => x.TypeId == id);
            return payType;
        }
        public PayType PayTypeByName(string name)
        {
            var payType = db.PayTypes.Where(x => x.IsActive == true).FirstOrDefault(x => x.Type == name);
            return payType;
        }
        public Messages DeletePayType(int payTypeId)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var payType = PayTypeById(payTypeId);
            if (payType == null)
            {
                messages.Message = "ReservationType Id is not found";
            }
            else
            {
                payType.IsActive = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "PayType is succssfully deleted";
            }
            return messages;
        }

        public Messages CreatePayType(PayType payType)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var payTypeExist = db.PayTypes.FirstOrDefault(x=>x.Type== payType.Type);
            if (payTypeExist != null)
            {
                messages.Message = "PayType Name is already Registered.";
            }
            else
            {
                db.PayTypes.Add(payType);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "PayType is succssfully added";
            }
            return messages;
        }

        public Messages UpdatePayType(PayType payType)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var payTypeExist = PayTypeById(payType.TypeId);
            var nameExist = db.PayTypes.FirstOrDefault(x => x.Type == payType.Type);
            if (payTypeExist == null)
            {
                messages.Message = "PayType Id is not found";
            }
            else if (nameExist != null && nameExist.TypeId != payTypeExist.TypeId)
            {
                messages.Message = "PayType Name is already registered";
            }
            else
            {
                payTypeExist.Type = payType.Type;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "PayType is succssfully Updated";
            }
            return messages;
        }

    }
}
