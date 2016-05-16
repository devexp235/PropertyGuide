using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PropertyGuide.Models;

namespace PropertyGuide.DataAccess
{
    public interface IPropertyManager
    {
        Task<Property> Get(int? id);
        Task<List<Property>> GetList(string userName);
        Task<int> Add(PropertyViewModel model, string userName);
        Task<int> Update(PropertyViewModel model);
        Task<int> Delete(int id);
        Task<List<PropertyViewModel>> GetListWithOffers(string userName);
        Task<List<PropertyViewModel>> GetListWithOffersAccepted(string userName);

        IPagedList<PropertyOfferViewModel> GetPropertyOfferViewModelList(string userName,
            string keywords, int pageNumber, int pageSize);
        Task<List<PropertyOfferViewModel>> GetMyPropertyOffers(string userName);
        Task<PropertyOfferViewModel> GetPropertyOfferViewModel(int propertyId, string userName);
        Task<PropertyOfferViewModel> GetPropertyOfferViewModel(int? offerId);
        Task<int> AddPropertyOffer(PropertyOfferViewModel model, string userName);
        Task<int> Reject(int id);
        Task<int> Accept(int id);
        void Dispose();

    }
}
