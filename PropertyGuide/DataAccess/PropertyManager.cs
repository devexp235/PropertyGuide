using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyGuide.Models;
using WebGrease.Css.Extensions;

namespace PropertyGuide.DataAccess
{
    public partial class PropertyManager : IDisposable, IPropertyManager
    {
        // Flag: Has Dispose already been called?
        bool _disposed = false;
        // Instantiate a SafeHandle instance.
        ApplicationDbContext _db;

        public PropertyManager()
        {
            _db = new ApplicationDbContext();
        }

        public async Task<Property> Get(int? id)
        {
            return await _db.Properties.FindAsync(id);
        }

        public async Task<List<Property>> GetList(string userName)
        {
            var seller = await _db.Users.FirstAsync(x => x.Email == userName);
            var sellerId = seller.Id;

            return await _db.Properties.Where(x => x.SellerId == sellerId).OrderByDescending(x => x.DateCreated).ToListAsync();
        }

        public async Task<int> Add(PropertyViewModel model, string userName)
        {
            var user = await _db.Users.FirstAsync(x => x.Email == userName);
            var sellerId = user.Id;

            var property = new Property
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DateCreated = DateTime.Now,
                SellerId = sellerId,
            };

            _db.Properties.Add(property);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> Update(PropertyViewModel model)
        {
            var property = await _db.Properties.Include(x => x.Seller).FirstOrDefaultAsync(x => x.Id == model.Id);

            property.Title = model.Title;
            property.Description = model.Description;
            property.DateModified = DateTime.Now;

            _db.Entry(property).State = EntityState.Modified;

            return await _db.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var property = await _db.Properties.FindAsync(id);

            _db.Properties.Remove(property);
            return await _db.SaveChangesAsync();
        }

        public async Task<List<PropertyViewModel>> GetListWithOffers(string userName)
        {
            var seller = await _db.Users.FirstAsync(x => x.Email == userName);
            var sellerId = seller.Id;

            var list = await _db.Properties
                        .Select(p => new PropertyViewModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            SellerId = p.SellerId,
                            OfferList = p.PropertyOffers.Where(x => x.DateRejected == null && x.DateAccepted == null).Select(po => new PropertyOfferViewModel
                            {
                                Id = po.Id,
                                PriceOffered = po.PriceOffered,
                                BuyerId = po.Buyer.Id,
                                BuyerFirstName = po.Buyer.FirstName,
                                BuyerLastName = po.Buyer.LastName,
                                BuyerEmail = po.Buyer.Email,
                            })
                        }).Where(x => x.SellerId == sellerId && x.OfferList.Any()).OrderByDescending(x => x.Id).ToListAsync();


            return list;
        }

        public async Task<List<PropertyViewModel>> GetListWithOffersAccepted(string userName)
        {
            var seller = await _db.Users.FirstAsync(x => x.Email == userName);
            var sellerId = seller.Id;

            var list = await _db.Properties
                        .Select(p => new PropertyViewModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            SellerId = p.SellerId,
                            OfferList = p.PropertyOffers.Where(x => x.DateAccepted != null).Select(po => new PropertyOfferViewModel
                            {
                                Id = po.Id,
                                PriceOffered = po.PriceOffered,
                                BuyerId = po.Buyer.Id,
                                BuyerFirstName = po.Buyer.FirstName,
                                BuyerLastName = po.Buyer.LastName,
                                BuyerEmail = po.Buyer.Email,

                                DateAccepted = po.DateAccepted
                            })
                        }).Where(x => x.SellerId == sellerId && x.OfferList.Any()).ToListAsync();


            return list;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _db.Dispose();
                _db = null;
            }

            // Free any unmanaged objects here.
            //
            _disposed = true;
        }
    }
}
