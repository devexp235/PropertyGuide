using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PagedList;
using PropertyGuide.Models;

namespace PropertyGuide.DataAccess
{
    public partial class PropertyManager
    {
        public IPagedList<PropertyOfferViewModel> GetPropertyOfferViewModelList(string userName,
            string keywords, int pageNumber, int pageSize)
        {
            var user = _db.Users.First(x => x.Email == userName);
            var buyerId = user.Id;

            var query = (from p in _db.Properties
                         join seller in _db.Users on p.SellerId equals seller.Id
                         join po in _db.PropertyOffers.Where(x => x.BuyerId == buyerId) on p.Id equals po.PropertyId into gpo
                         from offer in gpo.DefaultIfEmpty()
                         where p.DateSold == null
                         select new PropertyOfferViewModel
                         {
                             PropertyId = p.Id,
                             Title = p.Title,
                             Description = p.Description,
                             OfferSubmitted = offer != null,

                             SellerFirstName = seller.FirstName,
                             SellerLastName = seller.LastName,
                             PropertyDateCreated = p.DateCreated

                         }).AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.Title.Contains(keywords) || x.Description.Contains(keywords));
            }

            var list = query.OrderByDescending(x => x.PropertyId).ToPagedList(pageNumber, pageSize);

            return list;
        }

        public async Task<List<PropertyOfferViewModel>> GetMyPropertyOffers(string userName)
        {
            var user = await _db.Users.FirstAsync(x => x.Email == userName);
            var buyerId = user.Id;

            var list = await (from p in _db.Properties
                              join po in _db.PropertyOffers on p.Id equals po.PropertyId
                              where po.BuyerId == buyerId
                              select new PropertyOfferViewModel
                              {
                                  PropertyId = p.Id,
                                  Title = p.Title,
                                  Description = p.Description,

                                  DateAccepted = po.DateAccepted,
                                  DateRejected = po.DateRejected,
                                  DateCreated = po.DateCreated

                              }).OrderByDescending(x => x.DateCreated).ToListAsync();

            return list;
        }

        public async Task<PropertyOfferViewModel> GetPropertyOfferViewModel(int propertyId, string userName)
        {
            var user = await _db.Users.FirstAsync(x => x.Email == userName);
            var buyerId = user.Id;

            var model = await (from p in _db.Properties
                               join po in _db.PropertyOffers.Where(x => x.BuyerId == buyerId) on p.Id equals po.PropertyId into gpo
                               from offer in gpo.DefaultIfEmpty()
                               where p.Id == propertyId
                               select new PropertyOfferViewModel
                               {
                                   PropertyId = p.Id,
                                   Title = p.Title,
                                   Description = p.Description,
                                   OfferSubmitted = offer != null,

                               }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<PropertyOfferViewModel> GetPropertyOfferViewModel(int? offerId)
        {
            var model = await (from p in _db.Properties
                               join po in _db.PropertyOffers on p.Id equals po.PropertyId
                               join buyer in _db.Users on po.BuyerId equals buyer.Id
                               where po.Id == offerId
                               select new PropertyOfferViewModel
                               {
                                   PropertyId = p.Id,
                                   Title = p.Title,
                                   Description = p.Description,
                                   PriceOffered = po.PriceOffered,

                                   BuyerId = buyer.Id,
                                   BuyerFirstName = buyer.FirstName,
                                   BuyerLastName = buyer.LastName,
                                   BuyerEmail = buyer.Email
                               }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<int> AddPropertyOffer(PropertyOfferViewModel model, string userName)
        {
            var user = await _db.Users.FirstAsync(x => x.Email == userName);
            var buyerId = user.Id;
            var propertyId = model.Id;

            var offer = await _db.PropertyOffers
                .FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.BuyerId == buyerId);

            if (offer != null)
            {
                offer.PriceOffered = model.PriceOffered;

                return await _db.SaveChangesAsync();
            }

            offer = new PropertyOffer
            {
                PropertyId = propertyId,
                BuyerId = buyerId,
                PriceOffered = model.PriceOffered,
                DateCreated = DateTime.Now,
            };

            _db.PropertyOffers.Add(offer);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> Reject(int id)
        {
            var offer = await _db.PropertyOffers.FindAsync(id);

            offer.DateRejected = DateTime.Now;

            return await _db.SaveChangesAsync();
        }

        public async Task<int> Accept(int id)
        {
            var offer = await _db.PropertyOffers.FindAsync(id);

            //Accept the selected offer
            var offerId = offer.Id;
            offer.DateAccepted = DateTime.Now;

            //mark the property as sold
            var property = await _db.Properties.FindAsync(id);
            property.DateSold = DateTime.Now;

            //reject all other offers
            var propertyId = offer.PropertyId;
            var offerList = await _db.PropertyOffers
                    .Where(x => x.PropertyId == propertyId && x.Id != offerId).ToListAsync();

            foreach (var item in offerList)
            {
                item.DateRejected = DateTime.Now;
            }

            return await _db.SaveChangesAsync();
        }
    }
}