using System;
using Silanis.ESL.SDK.Internal;
using Silanis.ESL.API;

namespace Silanis.ESL.SDK
{
    internal class ReminderApiClient
    {
        private readonly UrlTemplate _template;
        private readonly RestClient _restClient;

        public ReminderApiClient(RestClient restClient, string baseUrl)
        {
            _restClient = restClient;
            _template = new UrlTemplate (baseUrl);
        }
        
        private string Path( string packageId )
        {
            return _template.UrlFor (UrlTemplate.REMINDER_PATH)
                .Replace( "{packageId}", packageId )
                .Build ();
        }
        
        public PackageReminderSchedule GetReminderScheduleForPackage( string packageId )
        {
            try {
                var response = _restClient.Get(Path(packageId));
                if (response.Length == 0) {
                    return null;
                }
                var apiResponse = Json.DeserializeWithSettings<PackageReminderSchedule> (response );
                return apiResponse;
            } 
            catch (EslServerException e) {
                throw new EslServerException ("Failed to retrieve reminder schedule for package with id: " + packageId + ". Exception: " + e.Message, e.ServerError, e);
            }
            catch (Exception e) {
                throw new EslException ("Failed to retrieve reminder schedule for package with id: " + packageId + ". Exception: " + e.Message, e);
            }
        }

        [Obsolete("Please use CreateReminderScheduleForPackage(ReminderSchedule) instead")]  
        public PackageReminderSchedule SetReminderScheduleForPackage( PackageReminderSchedule apiPayload )
        {
            return CreateReminderScheduleForPackage(apiPayload);
        }

        public PackageReminderSchedule CreateReminderScheduleForPackage( PackageReminderSchedule apiPayload )
        {
            try {
                var response = _restClient.Post(Path(apiPayload.PackageId), Json.SerializeWithSettings (apiPayload));
                var apiResponse = Json.DeserializeWithSettings<PackageReminderSchedule> (response );
                return apiResponse;
            }
            catch (EslServerException e) {
                throw new EslServerException ("Failed to create reminder schedule for package with id: " + apiPayload.PackageId + ". Exception: " + e.Message, e.ServerError, e);
            }
            catch (Exception e) {
                throw new EslException ("Failed to create reminder schedule for package with id: " + apiPayload.PackageId + ". Exception: " + e.Message, e);
            }
        }

        public PackageReminderSchedule UpdateReminderScheduleForPackage( PackageReminderSchedule apiPayload )
        {
            try {
                var response = _restClient.Put(Path(apiPayload.PackageId), Json.SerializeWithSettings (apiPayload));
                var apiResponse = Json.DeserializeWithSettings<PackageReminderSchedule> (response );
                return apiResponse;
            }
            catch (EslServerException e) {
                throw new EslServerException ("Failed to update reminder schedule for package with id: " + apiPayload.PackageId + ". Exception: " + e.Message, e.ServerError, e);
            }
            catch (Exception e) {
                throw new EslException ("Failed to update reminder schedule for package with id: " + apiPayload.PackageId + ". Exception: " + e.Message, e);
            }
        }

        public void ClearReminderScheduleForPackage( string packageId )
        {
            try {
                _restClient.Delete(Path(packageId));
            } 
            catch (EslServerException e) {
                throw new EslServerException ("Failed to remove reminder schedule for package with id: " + packageId + ". Exception: " + e.Message, e.ServerError, e);
            }
            catch (Exception e) {
                throw new EslException ("Failed to remove reminder schedule for package with id: " + packageId + ". Exception: " + e.Message, e);
            }
        }
    }
}

