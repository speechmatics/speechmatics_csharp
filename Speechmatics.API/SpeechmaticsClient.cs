using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Speechmatics.API
{
    /// <summary>
    /// Main Client class for interacting with the Speechmatics API
    /// </summary>
    public class SpeechmaticsClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Unique user Id </param>
        /// <param name="authToken">Authentication token obtained from https://app.speechmatics.com/account/ </param>
        public SpeechmaticsClient(int userId, string authToken)
        {
            _wc = new WebClient();
            _baseUri = new Uri("https://api.speechmatics.com/v1.0");
            _userId = userId;
            _authToken = authToken;
        }

        /// <summary>
        /// Get User information from the API
        /// </summary>
        /// <returns>User object containing account details or null if an error occurs</returns>
        public User GetUser()
        {
            var userUri = CreateUserRelativeUri("/");
            var userJson = GetJson(userUri);
            if (userJson != null)
            {
                return new User((int)userJson.user.id, (string)userJson.user.email, (int)userJson.user.balance); ;
            }
            return null;
        }

        /// <summary>
        /// Uploads the given audio file to the speechmatics API for transcription
        /// </summary>
        /// <param name="audioFilename">Full path to audio file</param>
        /// <param name="lang"></param>
        /// <param name="diarize"></param>
        /// <returns>Response object or null if an error occurs</returns>
        public CreateJobResponse CreateTranscriptionJob(string audioFilename, string lang, bool diarize)
        {
            var uploadUri = CreateUserRelativeUri("/jobs/");
            using (var fileStream = new FileStream(audioFilename, FileMode.Open))
            {
                var jsonResponse = FileUpload.UploadFileForTranscription(uploadUri, Path.GetFileName(audioFilename), fileStream, lang, new NameValueCollection(), diarize);
                if (jsonResponse != null)
                {
                    dynamic jobJson = JsonConvert.DeserializeObject(jsonResponse);
                    return new CreateJobResponse((int)jobJson.id, (int)jobJson.cost, (int)jobJson.balance);
                }
                return null;
            }
        }

        /// <summary>
        /// Uploads the given audio and text files to the speechmatics API for alignment
        /// </summary>
        /// <param name="audioFilename">Full path to audio file</param>
        /// <param name="textFilename">Full path to text file</param>
        /// <returns>Response object or null if an error occurs</returns>
        public CreateJobResponse CreateAlignmentJob(string audioFilename, string textFilename, string lang)
        {
            var uploadUri = CreateUserRelativeUri("/jobs/");
            using (var fileStream = new FileStream(audioFilename, FileMode.Open))
            {
                using (var fileStream2 = new FileStream(textFilename, FileMode.Open))
                {
                    var jsonResponse = FileUpload.UploadFilesForAlignment(uploadUri, Path.GetFileName(audioFilename), fileStream, Path.GetFileName(textFilename), fileStream2, lang, new NameValueCollection());
                    if (jsonResponse != null)
                    {
                        dynamic jobJson = JsonConvert.DeserializeObject(jsonResponse);
                        return new CreateJobResponse((int)jobJson.id, (int)jobJson.cost, (int)jobJson.balance);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Polls the API for the new status of a particular job
        /// </summary>
        /// <param name="job">Job to check status on</param>
        /// <returns>Job object or null if an error occurs</returns>
        public Job UpdateJobStatus(Job job)
        {
            var uploadUri = CreateUserRelativeUri($"/jobs/{job.Id}/");
            var jobJson = GetJson(uploadUri);
            if (jobJson != null)
            {
                job.Name = jobJson.job.name;
                job.Status = jobJson.job.job_status;
                return job;
            }
            return null;
        }

        /// <summary>
        /// Get the transcript in text format for the given Job (which should have a status of 'processed')
        /// </summary>
        /// <param name="job">Job to get transcript of</param>
        /// <param name="format"></param>
        /// <returns>Transcript in text format or null if an error occurs</returns>
        public String GetTranscript(Job job, string format)
        {
            var reqParams = new NameValueCollection
            {
                { "format", format }
            };
            var uploadUri = CreateUserRelativeUri($"/jobs/{job.Id}/transcript", reqParams);
             
            return GetString(uploadUri);
        }

        /// <summary>
        /// Get the alignment for the given Job (which should have a status of 'processed')
        /// </summary>
        /// <param name="job">Job to get alignment of</param>
        /// <param name="onePerLine"></param>
        /// <returns>Alignment text or null if an error occurs</returns>
        public String GetAlignment(Job job, bool onePerLine)
        {
            var reqParams = new NameValueCollection();
            if (onePerLine)
            {
                reqParams.Add("tags", "one_per_line");
            }           
            var uploadUri = CreateUserRelativeUri($"/jobs/{job.Id}/alignment", reqParams);

            return GetString(uploadUri);
        }

        #region Private Helper Methods
        
        private Uri CreateUserRelativeUri(String path, NameValueCollection requestParams = null)
        {
            if (requestParams == null)
            {
                requestParams = new NameValueCollection();
            }
            requestParams.Add("auth_token", _authToken);
            var paramString = "?";
            foreach (string name in requestParams.Keys){
                paramString += name+"="+requestParams[name]+"&";
            }
            return new Uri(_baseUri, $"/v1.0/user/{_userId}{path}{paramString}");
        }

        private string GetString(Uri uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
        private dynamic GetJson(Uri uri)
        {
            var resp = GetString(uri);
            return resp == null ? null : JsonConvert.DeserializeObject(resp);
        }

        #endregion

        #region Private Members

        private WebClient _wc;
        private Uri _baseUri;
        private int _userId;
        private string _authToken;

        #endregion
    }
}
   

