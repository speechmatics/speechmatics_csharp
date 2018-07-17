using System;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Runtime.Serialization;

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
            wc = new WebClient();
            baseUri = new Uri("https://api.speechmatics.com/v1.0");
            this.userId = userId;
            this.authToken = authToken;
        }

        /// <summary>
        /// Get User information from the API
        /// </summary>
        /// <returns>User object containing account details or null if an error occurs</returns>
        public User GetUser()
        {
            Uri userUri = createUserRelativeUri("/");
            dynamic userJson = getJson(userUri);
            if (userJson != null)
            {
                return new User((int)userJson.user.id, (string)userJson.user.email, (int)userJson.user.balance); ;
            }
            else { return null; }
        }

        /// <summary>
        /// Uploads the given audio file to the speechmatics API for transcription
        /// </summary>
        /// <param name="audioFilename">Full path to audio file</param>
        /// <returns>Response object or null if an error occurs</returns>
        public CreateJobResponse CreateTranscriptionJob(string audioFilename, string lang, bool diarise)
        {
            Uri uploadUri = createUserRelativeUri("/jobs/");
            using (FileStream fileStream = new FileStream(audioFilename, FileMode.Open))
            {
                string jsonResponse = FileUpload.UploadFileForTranscription(uploadUri, Path.GetFileName(audioFilename), fileStream, lang, new NameValueCollection(), diarise);
                if (jsonResponse != null)
                {
                    dynamic jobJson = JsonConvert.DeserializeObject(jsonResponse);
                    return new CreateJobResponse((int)jobJson.id, (int)jobJson.cost, (int)jobJson.balance);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Uploads the given audio and text files to the speechmatics API for alignment
        /// </summary>
        /// <param name="audioFilename">Full path to audio file</param>
        /// <param name="textFilename">Full path to text file</param>
        /// <returns>Response object or null if an error occurs</returns>
        public CreateJobResponse CreateAlignmentJob(string audioFilename, string textFileName, string lang)
        {
            Uri uploadUri = createUserRelativeUri("/jobs/");
            using (FileStream fileStream = new FileStream(audioFilename, FileMode.Open))
            {
                using (FileStream fileStream2 = new FileStream(textFileName, FileMode.Open))
                {
                    string jsonResponse = FileUpload.UploadFilesForAlignment(uploadUri, Path.GetFileName(audioFilename), fileStream, Path.GetFileName(textFileName), fileStream2, lang, new NameValueCollection());
                    if (jsonResponse != null)
                    {
                        dynamic jobJson = JsonConvert.DeserializeObject(jsonResponse);
                        return new CreateJobResponse((int)jobJson.id, (int)jobJson.cost, (int)jobJson.balance);
                    }
                    else
                    {
                        return null;
                    }
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
            Uri uploadUri = createUserRelativeUri($"/jobs/{job.Id}/");
            dynamic jobJson = getJson(uploadUri);
            if (jobJson != null)
            {
                job.Name = jobJson.job.name;
                job.Status = jobJson.job.job_status;
                return job;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the transcript in text format for the given Job (which should have a status of 'processed')
        /// </summary>
        /// <param name="job">Job to get transcript of</param>
        /// <returns>Transcript in text format or null if an error occurs</returns>
        public String getTranscript(Job job, string format)
        {
            NameValueCollection reqParams = new NameValueCollection();
            reqParams.Add("format", format);
            Uri uploadUri = createUserRelativeUri($"/jobs/{job.Id}/transcript", reqParams);
             
            return getString(uploadUri);
        }

        /// <summary>
        /// Get the alignment for the given Job (which should have a status of 'processed')
        /// </summary>
        /// <param name="job">Job to get alignment of</param>
        /// <returns>Alignment text or null if an error occurs</returns>
        public String getAlignment(Job job, bool onePerLine)
        {
            NameValueCollection reqParams = new NameValueCollection();
            if (onePerLine)
            {
                reqParams.Add("tags", "one_per_line");
            }           
            Uri uploadUri = createUserRelativeUri($"/jobs/{job.Id}/alignment", reqParams);

            return getString(uploadUri);
        }

        #region Private Helper Methods
        
        private Uri createUserRelativeUri(String path, NameValueCollection requestParams = null)
        {
            if (requestParams == null)
            {
                requestParams = new NameValueCollection();
            }
            requestParams.Add("auth_token", authToken);
            String paramString = "?";
            foreach (string name in requestParams.Keys){
                paramString += name+"="+requestParams[name]+"&";
            }
            return new Uri(baseUri, $"/v1.0/user/{userId.ToString()}{path}{paramString}");
        }

        private string getString(Uri uri)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
        private dynamic getJson(Uri uri)
        {
            string resp = getString(uri);
            return resp == null ? null : JsonConvert.DeserializeObject(resp);
        }

        #endregion

        #region Private Members

        private WebClient wc;
        private Uri baseUri;
        private int userId;
        private string authToken;

        #endregion
    }
}
   

