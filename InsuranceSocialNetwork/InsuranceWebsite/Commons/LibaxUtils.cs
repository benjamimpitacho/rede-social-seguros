using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceWebsite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            List<CurrencyInfo> currencies = GetCurrencyList();

            List<ProductInfo> productList = GetProductList();

            using (WebClient client = new WebClient())
            {
                //client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                int productId;
                if(null==productList || productList.Count==0)
                {
                    // CREATE PRODUCT
                    InsertVM productToInsert = new InsertVM
                    {
                        Name = "FALAR_SEGUROS_REGISTER_FEE",
                        UnitID = 1,
                        TaxID = 1,
                        ApplyRetention = false,
                        DiscountRate = 0, //isto vai traduzir em 2%
                        ProductDetail = "Quota de registo no portal Falar Seguros",
                        ProductType = ProductType.Service,
                        SellPrice = decimal.Parse(payment.t_value),
                        Reference = payment.ep_reference //null para que seja calculada automaticamente
                        //StandardCost = 20, //preço sem impostos (IVA, etc)
                    };

                    //var webClient = new WebClient();
                    var dataString = JsonConvert.SerializeObject(productToInsert);
                    client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + authorizationToken);
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var res = client.UploadString(new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value + "/api/v1/products"), "POST", dataString);
                    if(!int.TryParse(res, out productId))
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    var product = productList.FirstOrDefault(i => i.Name == "FALAR_SEGUROS_REGISTER_FEE");
                    if(null == product)
                    {
                        // CREATE PRODUCT
                        InsertVM productToInsert = new InsertVM
                        {
                            Name = "FALAR_SEGUROS_REGISTER_FEE",
                            UnitID = 1,
                            TaxID = 1,
                            ApplyRetention = false,
                            DiscountRate = 0, //isto vai traduzir em 2%
                            ProductDetail = "Quota de registo no portal Falar Seguros",
                            ProductType = ProductType.Service,
                            SellPrice = decimal.Parse(payment.t_value),
                            Reference = payment.ep_reference //null para que seja calculada automaticamente
                                                             //StandardCost = 20, //preço sem impostos (IVA, etc)
                        };

                        var dataString = JsonConvert.SerializeObject(productToInsert);
                        client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + authorizationToken);
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                        var res = client.UploadString(new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value + "/api/v1/products"), "POST", dataString);
                        if (!int.TryParse(res, out productId))
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        productId = product.ProductID;
                    }
                }

                var seriesList = GetSeriesList();
                var validSerie = seriesList.FirstOrDefault(i => i.IsActive && i.StartDate <= DateTime.Now && (!i.EndDate.HasValue || i.EndDate >= DateTime.Now));
                if(null == validSerie)
                {
                    throw new NotImplementedException();
                }

                InvoiceVM invoiceToCreate = new InvoiceVM
                {
                    CurrencyID = currencies.FirstOrDefault(i => i.ISOCode == "EUR").CurrencyID, //CurrencyID que pode ser vista em lista neste endpoint 
                    ExchangeRate = 1, //Rate de cambio, ver online ou implementar uma solução vossa que apanhe o valor // 
                    IsDraft = false, //True se for um draft, permitindo assim que a fatura seja editada, uma vez que o valor seja false, a fatura torna-se definitiva
                    DueDays = 1, //Dias em que a fatura terá de ser paga
                    EntityID = entity.LibaxEntityID.Value, //Entidade a qual a fatura se destina
                    SerieID = validSerie.SerieID, //A Serie da fatura
                    SaleDate = DateTime.Now //Data da criação da fatura
                };

                var productToInclude = GetProduct(productId);
                invoiceToCreate.Products = new List<DetailsVM>() {
                    new DetailsVM()
                    {
                        ProductID = productToInclude.ProductID,
                        TaxID = productToInclude.TaxID,
                        ProductDetail = "Registo portal Falar Seguros",
                        DiscountRate = productToInclude.DiscountRate,
                        Quantity = 1,
                        UnitID = productToInclude.UnitID,
                        UnitPrice = 6.9M,//decimal.Parse(payment.t_value),
                        ApplyRetention = productToInclude.ApplyRetention,
                        Description = "Registo portal Falar Seguros",
                        Order = 1
                    }
                };

                var jsonDataString = JsonConvert.SerializeObject(invoiceToCreate);
                client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + authorizationToken);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var x = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value;
                var result = client.UploadString(new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value + "/api/v1/invoices"), "POST", jsonDataString);
                if (!int.TryParse(result, out productId))
                {
                    throw new NotImplementedException();
                }



                //var product = "[{\"productID\": 1,\"description\": \"sample string 2\",\"productDetail\": \"sample string 3\",\"quantity\": 4.0,\"unitID\": 1,\"unitPrice\": 5.0,\"discountRate\": 6.0,\"taxID\": 1,\"order\": 7,\"isComment\": true,\"applyRetention\": true}]\"),";

                //var invoice = new InvoiceVM()
                //{
                //    EntityID = entity.LibaxEntityID.Value,
                //    SerieID = 1,
                //    Observation = "PAGAMENTO - FALAR SEGUROS",
                //    Reason = "",
                //    SaleDate = DateTime.Now.AddHours(1), // string.Format("{0}-{1}-{2}T{3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second), //"2018-03-30T10:55:45.1554406+01:00",
                //    DueDays = 0,
                //    CurrencyID = countries.FirstOrDefault(i => i.ISOCode == "PT").CountryID.ToString(),
                //    ExchangeRate = 0,
                //    Products = new List<DetailsVM>() { new DetailsVM() {
                //        //ProductID = product.ProductID,
                //        ApplyRetention = false,
                //        Description = "",
                //        DiscountRate = 0,
                //        IsComment = false,
                //        Order = 1,
                //        ProductDetail = "",
                //        Quantity=1,
                //        TaxID = 0,
                //        UnitID=0,
                //        UnitPrice=Decimal.Parse( payment.t_value)
                //    } },
                //    RelatedDocumentIDs = new List<int>(),
                //    IsDraft = false,
                //    Reference = payment.Payment_GUID.ToString(),
                //    IsCustomDelivery = false,
                //    LicensePlate = "",
                //    RetentionIRC = false,
                //    RetentionRate = 0,
                //    RetentionAmount = 0,
                //    SealAmount = 0,
                //    ProviderID = 0
                //};

                //var formData = new List<KeyValuePair<string, string>>()
                //    {
                //        new KeyValuePair<string, string>("entityID", entity.LibaxEntityID.Value.ToString()),
                //        new KeyValuePair<string, string>("serieID", "1"),
                //        new KeyValuePair<string, string>("observation", "PAGAMENTO - FALAR SEGUROS"),
                //        new KeyValuePair<string, string>("reason", ""),
                //        new KeyValuePair<string, string>("saleDate", string.Format("{0}-{1}-{2}T{3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour+1, DateTime.Now.Minute, DateTime.Now.Second)), //"2018-03-30T10:55:45.1554406+01:00",
                //        new KeyValuePair<string, string>("dueDays", "0"),
                //        new KeyValuePair<string, string>("currencyID", countries.FirstOrDefault(i=>i.ISOCode=="PT").CountryID.ToString()),
                //        new KeyValuePair<string, string>("exchangeRate", "0"),
                //        new KeyValuePair<string, string>("products", "["+productJson+"]"),
                //        new KeyValuePair<string, string>("relatedDocumentIDs", ""),
                //        new KeyValuePair<string, string>("isDraft", "true"),
                //        new KeyValuePair<string, string>("reference", payment.Payment_GUID.ToString()),
                //        new KeyValuePair<string, string>("isCustomDelivery", "false"),
                //        new KeyValuePair<string, string>("licensePlate", ""),
                //        new KeyValuePair<string, string>("retentionIRC", "false"),
                //        new KeyValuePair<string, string>("retentionRate", "0"),
                //        new KeyValuePair<string, string>("retentionAmount", "0"),
                //        new KeyValuePair<string, string>("sealAmount", "0"),
                //        new KeyValuePair<string, string>("providerID", "")
                //    };

                //request.Content = new FormUrlEncodedContent(formData);
                //request.Content = JsonConvert.SerializeObject(invoice, Formatting.Indented);
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                //HttpResponseMessage response = client.SendAsync(request).Result;
                //string resultJSON = response.Content.ReadAsStringAsync().Result;
                //dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
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

        private static List<CurrencyInfo> GetCurrencyList()
        {
            string authorizationToken = GetAuthorizationToken();
            List<CurrencyInfo> currencies = new List<CurrencyInfo>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                bool hasMoreCurrencies = true;
                int skipInterval = 0;
                int numberOfElements = 50;
                while (hasMoreCurrencies)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/v1/currencies?Skip={0}&Take={1}", skipInterval, numberOfElements));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string resultJSON = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<CurrencyInfo> list = JsonConvert.DeserializeObject<List<CurrencyInfo>>(resultJSON);
                        currencies = currencies.Concat(list).ToList();
                        skipInterval += numberOfElements;
                        if (list.Count < numberOfElements)
                        {
                            hasMoreCurrencies = false;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        hasMoreCurrencies = false;
                    }
                    else
                    {
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                        throw new Exception(result);
                    }
                }
            }

            return currencies;
        }

        private static ProductInfo GetProduct(int productId)
        {
            string authorizationToken = GetAuthorizationToken();
            List<ProductInfo> products = new List<ProductInfo>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/v1/products/{0}", productId));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                HttpResponseMessage response = client.SendAsync(request).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<ProductInfo>(resultJSON);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                    throw new Exception(result);
                }
            }
        }

        private static List<ProductInfo> GetProductList()
        {
            string authorizationToken = GetAuthorizationToken();
            List<ProductInfo> products = new List<ProductInfo>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                bool hasMoreCountries = true;
                int skipInterval = 0;
                int numberOfElements = 50;
                while (hasMoreCountries)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/v1/products?Skip={0}&Take={1}", skipInterval, numberOfElements));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string resultJSON = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<ProductInfo> list = JsonConvert.DeserializeObject<List<ProductInfo>>(resultJSON);
                        products = products.Concat(list).ToList();
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

            return products;
        }

        private static List<SeriesInfo> GetSeriesList()
        {
            string authorizationToken = GetAuthorizationToken();
            List<SeriesInfo> series = new List<SeriesInfo>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.LIBAX_API_URL).Value);

                bool hasMoreSeries = true;
                int skipInterval = 0;
                int numberOfElements = 50;
                while (hasMoreSeries)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/v1/series?Skip={0}&Take={1}", skipInterval, numberOfElements));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string resultJSON = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<SeriesInfo> list = JsonConvert.DeserializeObject<List<SeriesInfo>>(resultJSON);
                        series = series.Concat(list).ToList();
                        skipInterval += numberOfElements;
                        if (list.Count < numberOfElements)
                        {
                            hasMoreSeries = false;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        hasMoreSeries = false;
                    }
                    else
                    {
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJSON);
                        throw new Exception(result);
                    }
                }
            }

            return series;
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
        public int? ChildrenCount { get; set; }
        public DateTime? BirthDate { get; set; }
        public CivilState CivilState { get; set; }
        public EntityType Type { get; set; }
    }

    public class SeriesInfo
    {
        public int SerieID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductInfo
    {
        public int ProductID { get; set; }
        public string Reference { get; set; }
        public int TaxID { get; set; }
        public decimal DiscountRate { get; set; }
        public string Name { get; set; }
        public bool ApplyRetention { get; set; }
        public ProductType ProductType { get; set; }
        public int UnitID { get; set; }
    }

    public class InsertVM
    {
        public string Name { get; set; }
        public int TaxID { get; set; }
        public decimal DiscountRate { get; set; }
        public bool ApplyRetention { get; set; }
        public bool AutoStockHistory { get; set; }
        public ProductType ProductType { get; set; }
        public int UnitID { get; set; }
        public string Reference { get; set; }
        public decimal SellPrice { get; set; }
        public StockCategory StockCategory { get; set; }
        public string ProductDetail { get; set; }
        public decimal StockAlertQty { get; set; }
        public decimal StockQty { get; set; }
        public decimal StandardCost { get; set; }
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

    public enum ProductType
    {
        Product = 0,
        Service = 1,
        Other = 2,
        Tax = 3
    }

    public enum StockCategory
    {
        Goods = 0,
        RawMaterial = 1,
        FinishProduct = 2,
        ByProduct = 3,
        WorkProduct = 4
    }

    public class CountryInfo
    {
        public int CountryID { get; set; }
        public string Name { get; set; }
        public string ISOCode { get; set; }
    }

    public class CurrencyInfo
    {
        public int CurrencyID { get; set; }
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

    public class CustomDeliveryVM
    {
        public DateTime SourceDate { get; set; }
        public DateTime TargetDate { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Region { get; set; }
        public int CountryID { get; set; }
    }

    public class InvoiceVM
    {
        public int EntityID { get; set; }
        public int SerieID { get; set; }
        public string Observation { get; set; }
        public string Reason { get; set; }
        public DateTime SaleDate { get; set; }
        public int DueDays { get; set; }
        public int CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public List<DetailsVM> Products { get; set; }
        public List<int> RelatedDocumentIDs { get; set; }
        public bool IsDraft { get; set; }
        public string Reference { get; set; }
        public bool IsCustomDelivery { get; set; }
        public string LicensePlate { get; set; }
        public bool RetentionIRC { get; set; }
        public decimal RetentionRate { get; set; }
        public decimal RetentionAmount { get; set; }
        public decimal SealAmount { get; set; }
        public int ProviderID { get; set; }
        public CustomDeliveryVM SourceAddress { get; set; }
        public CustomDeliveryVM TargetAddress { get; set; }
    }
}
