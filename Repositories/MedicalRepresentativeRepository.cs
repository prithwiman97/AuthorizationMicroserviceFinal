using AuthorizationMicroservice.Interfaces;
using AuthorizationMicroservice.Models;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Repositories
{
    public class MedicalRepresentativeRepository:IMedicalRepresentative
    {
        private IConfiguration Configuration;
        ILog logger = LogManager.GetLogger(typeof(MedicalRepresentativeRepository));
        Uri baseAddress;
        HttpClient client;
        public MedicalRepresentativeRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:MedicalRepresentativeSchedule"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            logger.Info("Constructor Initialized");
        }

        /// <summary>
        /// Fetches List of Medical Representatives from MedicalRepresentativeSchedule Microservice
        /// </summary>
        /// <returns>List of MedicalRepresentative entity</returns>
        public IEnumerable<MedicalRepresentative> GetMedicalRepresentatives()
        {
            try
            {
                List<MedicalRepresentative> representativesList = new List<MedicalRepresentative>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    
                    string data = response.Content.ReadAsStringAsync().Result;
                    representativesList = JsonConvert.DeserializeObject<List<MedicalRepresentative>>(data);
                    logger.Info("List successfully fetched from Microservice");
                    return representativesList;
                }
                logger.Error("Invalid credentials Response Code:" + response.StatusCode);
                return representativesList;
            }
            catch(Exception e)
            {
                logger.Error("("+nameof(MedicalRepresentativeRepository)+") RepSchedule Microservice not responding "+e.Message);
                throw e;
            }
        }
    }
}
