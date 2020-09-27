using AuthorizationMicroservice.Interfaces;
using AuthorizationMicroservice.Models;
using AuthorizationMicroservice.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Providers
{
    public class MedicalRepresentativeProvider
    {
        ILog logger = LogManager.GetLogger(typeof(MedicalRepresentativeProvider));
        private readonly IMedicalRepresentative repository;
        public MedicalRepresentativeProvider(IMedicalRepresentative repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Checks if credentials provided by user are correct
        /// </summary>
        /// <param name="representative"></param>
        /// <returns>true for valid credentials and false for invalid credentials</returns>
        public bool Validate(MedicalRepresentative representative)
        {
            try
            {
                IEnumerable<MedicalRepresentative> representativesList = repository.GetMedicalRepresentatives();
                MedicalRepresentative authRepresentative = representativesList.Where(r => r.Email == representative.Email).FirstOrDefault();
                if (authRepresentative != null && authRepresentative.Password == representative.Password)
                {
                    logger.Info("Successfully logged in "+authRepresentative.Name);
                    return true;
                }
                logger.Info("Invalid Credentials");
                return false;
            }
            catch(Exception e)
            {
                logger.Error("Exception arised in "+nameof(MedicalRepresentativeProvider)+"\n" + e.Message);
                throw e;
            }
        }
    }
}
