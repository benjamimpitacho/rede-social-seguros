using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceWebsite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace InsuranceWebsite.LibaxUtils
{
    public static class LibaxUtils
    {
        private static string GetAuthorizationToken()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                // Get Auth token
                var request = new HttpRequestMessage(HttpMethod.Post, "/token");
                var formData = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_USERNAME).Value),
                        new KeyValuePair<string, string>("password", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_PASSWORD).Value),
                        new KeyValuePair<string, string>("client_id", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_CLIENT_ID).Value)
                    };

                request.Content = new FormUrlEncodedContent(formData);
                HttpResponseMessage response = client.SendAsync(request).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                return result.access_token;
            }
        }

        public static int CreateEntity(CompanyDTO entity)
        {
            string authorizationToken = GetAuthorizationToken();

            List<CountryInfo> countries = GetCountryList();

            string parish = string.Empty;
            if ((entity.SameInformationForInvoice && entity.ID_Parish.HasValue)
                || (!entity.SameInformationForInvoice && entity.Invoice_ID_Parish.HasValue))
            {
                parish = InsuranceBusiness.BusinessLayer.GetParish(entity.SameInformationForInvoice ? entity.ID_Parish.Value : entity.Invoice_ID_Parish.Value).Name;
            }
            string county = string.Empty;
            if ((entity.SameInformationForInvoice && entity.ID_County.HasValue)
                || (!entity.SameInformationForInvoice && entity.Invoice_ID_County.HasValue))
            {
                county = InsuranceBusiness.BusinessLayer.GetCounty(entity.SameInformationForInvoice ? entity.ID_County.Value : entity.Invoice_ID_County.Value).Name;
            }
            string district = string.Empty;
            if ((entity.SameInformationForInvoice && entity.ID_District.HasValue)
                || (!entity.SameInformationForInvoice && entity.Invoice_ID_District.HasValue))
            {
                district = InsuranceBusiness.BusinessLayer.GetDistrict(entity.SameInformationForInvoice ? entity.ID_District.Value : entity.Invoice_ID_District.Value).Name;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/entities");
                var formData = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("number", entity.ID.ToString()),
                        new KeyValuePair<string, string>("civilState", "7"),
                        new KeyValuePair<string, string>("observation", ""),
                        new KeyValuePair<string, string>("name", string.IsNullOrEmpty(entity.BusinessName)?entity.Name:entity.BusinessName),
                        new KeyValuePair<string, string>("childrenCount", "0"),
                        new KeyValuePair<string, string>("birthDate", ""),
                        new KeyValuePair<string, string>("vatNumber", entity.NIF),
                        new KeyValuePair<string, string>("iban", entity.IBAN),
                        new KeyValuePair<string, string>("status", "1"),
                        new KeyValuePair<string, string>("email", entity.ContactEmail),
                        new KeyValuePair<string, string>("phone", entity.Telephone_1),
                        new KeyValuePair<string, string>("mobile", entity.MobilePhone_1),
                        new KeyValuePair<string, string>("fax", entity.Fax),
                        new KeyValuePair<string, string>("url", entity.Website),
                        new KeyValuePair<string, string>("city", string.Format("{0}, {1}",parish, county)),
                        new KeyValuePair<string, string>("countryID", countries.FirstOrDefault(i=>i.ISOCode=="PT").CountryID.ToString()),
                        new KeyValuePair<string, string>("code", entity.ID.ToString()),
                        new KeyValuePair<string, string>("street", entity.Address),
                        new KeyValuePair<string, string>("region", district),
                        new KeyValuePair<string, string>("ignoreAdvertising", "true")
                    };

                request.Content = new FormUrlEncodedContent(formData);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                HttpResponseMessage response = client.SendAsync(request).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return Int32.Parse(resultJSON);
                }

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                throw new Exception(result);
            }
        }

        public static void CreateInvoice(CompanyDTO entity, PaymentDTO payment)
        {
            string authorizationToken = GetAuthorizationToken();

            EntityInfo entityInfo = null;
            if(entity.LibaxEntityID.HasValue)
            {
                entityInfo = GetEntity(entity.LibaxEntityID.Value);
            }            
            if (null == entityInfo)
            {
                entity.LibaxEntityID = CreateEntity(entity);
                InsuranceBusiness.BusinessLayer.EditCompany(entity);
            }

            List<CountryInfo> countries = GetCountryList();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/invoices");

                //var product = new List<KeyValuePair<string, string>>()
                //{
                //    new KeyValuePair<string, string>("productID", ""),
                //    new KeyValuePair<string, string>("description", ""),
                //    new KeyValuePair<string, string>("productDetail", ""),
                //    new KeyValuePair<string, string>("quantity", "1"),
                //    new KeyValuePair<string, string>("unitID", ""),
                //    new KeyValuePair<string, string>("unitPrice", payment.t_value),
                //    new KeyValuePair<string, string>("discountRate", "0"),
                //    new KeyValuePair<string, string>("taxID", ""),
                //    new KeyValuePair<string, string>("order", ""),
                //    new KeyValuePair<string, string>("isComment", "false"),
                //    new KeyValuePair<string, string>("applyRetention", "false"),
                //};

                var product = new DetailsVM()
                {
                    ProductID = 1,
                    Description = "",
                    ProductDetail = "",
                    Quantity = 1,
                    UnitID = 1,
                    UnitPrice = 3.4M,
                    DiscountRate = 0,
                    TaxID = 0,
                    Order = 1,
                    IsComment = false,
                    ApplyRetention = false
                };

                var productJson = JsonConvert.SerializeObject(product, Formatting.Indented);

                //var product = "[{\"productID\": 1,\"description\": \"sample string 2\",\"productDetail\": \"sample string 3\",\"quantity\": 4.0,\"unitID\": 1,\"unitPrice\": 5.0,\"discountRate\": 6.0,\"taxID\": 1,\"order\": 7,\"isComment\": true,\"applyRetention\": true}]\"),";

                var formData = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("entityID", entity.LibaxEntityID.Value.ToString()),
                        new KeyValuePair<string, string>("serieID", "1"),
                        new KeyValuePair<string, string>("observation", "PAGAMENTO - FALAR SEGUROS"),
                        new KeyValuePair<string, string>("reason", ""),
                        new KeyValuePair<string, string>("saleDate", string.Format("{0}-{1}-{2}T{3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour+1, DateTime.Now.Minute, DateTime.Now.Second)), //"2018-03-30T10:55:45.1554406+01:00",
                        new KeyValuePair<string, string>("dueDays", "0"),
                        new KeyValuePair<string, string>("currencyID", countries.FirstOrDefault(i=>i.ISOCode=="PT").CountryID.ToString()),
                        new KeyValuePair<string, string>("exchangeRate", "0"),
                        new KeyValuePair<string, string>("products", "["+productJson+"]"),
                        new KeyValuePair<string, string>("relatedDocumentIDs", ""),
                        new KeyValuePair<string, string>("isDraft", "true"),
                        new KeyValuePair<string, string>("reference", payment.Payment_GUID.ToString()),
                        new KeyValuePair<string, string>("isCustomDelivery", "false"),
                        new KeyValuePair<string, string>("licensePlate", ""),
                        new KeyValuePair<string, string>("retentionIRC", "false"),
                        new KeyValuePair<string, string>("retentionRate", "0"),
                        new KeyValuePair<string, string>("retentionAmount", "0"),
                        new KeyValuePair<string, string>("sealAmount", "0"),
                        new KeyValuePair<string, string>("providerID", "")
                    };

                request.Content = new FormUrlEncodedContent(formData);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                HttpResponseMessage response = client.SendAsync(request).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);

                //throw new NotImplementedException();
            }
        }

        public static EntityInfo GetEntity(int entityID)
        {
            string authorizationToken = GetAuthorizationToken();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/entities/" + entityID);

                //request.Content = new FormUrlEncodedContent(formData);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                HttpResponseMessage response = client.SendAsync(request).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<EntityInfo>(resultJSON);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                throw new Exception(result);
            }
        }

        private static List<CountryInfo> GetCountryList()
        {
            string authorizationToken = GetAuthorizationToken();
            List<CountryInfo> countries = new List<CountryInfo>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                bool hasMoreCountries = true;
                int skipInterval = 0;
                int numberOfElements = 50;
                while (hasMoreCountries)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/v1/countries?Skip={0}&Take={1}", skipInterval, numberOfElements));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string resultJSON = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<CountryInfo> list = JsonConvert.DeserializeObject<List<CountryInfo>>(resultJSON);
                        countries = countries.Concat(list).ToList();
                        skipInterval += numberOfElements;
                        if (list.Count < numberOfElements)
                        {
                            hasMoreCountries = false;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        hasMoreCountries = false;
                    }
                    else
                    {
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                        throw new Exception(result);
                    }
                }
            }

            return countries;
        }
    }

    public class EntityInfo
    {
        public int EntityID { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string VatNumber { get; set; }
        public EntityStatus Status { get; set; }
        public int? OccupationID { get; set; }
        public string Iban { get; set; }
        public AddressInfo Address { get; set; }
        public ContactSimpleInfo Contact { get; set; }
        public decimal? Balance { get; set; }
        public byte? ChildrenCount { get; set; }
        public DateTime? BirthDate { get; set; }
        public CivilState CivilState { get; set; }
        public EntityType Type { get; set; }
    }

    public enum EntityStatus
    {
        Prospect = 0,
        Effective = 1,
        NotActive = 2
    }

    public class AddressInfo { }
    public class ContactSimpleInfo { }

    public enum CivilState
    {
        Undefined = 0,
        Married = 1,
        Single = 2,
        Union = 3,
        Divorced = 4,
        Separated = 5,
        Widower = 6,
        NotApplicable = 7
    }

    public enum EntityType
    {
        Undefined = 0,
        Male = 1,
        Female = 2,
        Company = 3,
        Association = 4,
        PublicOrganization = 5,
        IndividualCompany = 6,
        Condominium = 7,
        Other = 8
    }

    public class CountryInfo
    {
        public byte CountryID { get; set; }
        public string Name { get; set; }
        public string ISOCode { get; set; }
    }

    public class DetailsVM
    {
        public int ProductID { get; set; }
        public string Description { get; set; }
        public string ProductDetail { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public int TaxID { get; set; }
        public int Order { get; set; }
        public bool IsComment { get; set; }
        public bool ApplyRetention { get; set; }
    }
}
