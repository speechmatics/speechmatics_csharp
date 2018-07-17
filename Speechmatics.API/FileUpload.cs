using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Speechmatics.API
{
    /// <summary>
    /// Helper static class for handling a simple multipart mime file upload 
    /// </summary>
    public class FileUpload
    {
        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="uploadUri">URI to post file to</param>
        /// <param name="filename">Name of file</param>
        /// <param name="fileStream">Stream coontaining file data</param>
        /// <param name="values">Collection of additional form [parameters to upload along with file</param>
        /// <returns>String response from server or null if an error occurs</returns>
        public static String UploadFileForTranscription(Uri uploadUri, string filename, Stream fileStream, string lang, NameValueCollection values, bool diarise)
        {
            var request = WebRequest.Create(uploadUri);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                byte[] buffer;

                // Write the values
                foreach (string name in values.Keys)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                if (!diarise)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", "diarisation", Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("false" + Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", "model", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format(lang + Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"{"data_file"}\"; filename=\"{filename}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "application/octet-stream", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                
                
                fileStream.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(buffer, 0, buffer.Length);
            }
            try
            {
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var stream = new MemoryStream())
                {
                    responseStream.CopyTo(stream);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (WebException )
            {
                return null;
            }
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="uploadUri">URI to post file to</param>
        /// <param name="filename">Name of file</param>
        /// <param name="fileStream">Stream coontaining file data</param>
        /// <param name="values">Collection of additional form [parameters to upload along with file</param>
        /// <returns>String response from server or null if an error occurs</returns>
        public static String UploadFilesForAlignment(Uri uploadUri, string filename, Stream fileStream, string filename2, Stream fileStream2, string lang, NameValueCollection values)
        {
            var request = WebRequest.Create(uploadUri);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                byte[] buffer;

                // Write the values
                foreach (string name in values.Keys)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", "model", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format(lang + Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"{"data_file"}\"; filename=\"{filename}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "application/octet-stream", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);


                fileStream.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"{"text_file"}\"; filename=\"{filename2}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "application/octet-stream", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);


                fileStream2.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(buffer, 0, buffer.Length);
            }
            try
            {
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var stream = new MemoryStream())
                {
                    responseStream.CopyTo(stream);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
