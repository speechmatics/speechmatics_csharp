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
        /// <param name="lang"></param>
        /// <param name="values">Collection of additional form [parameters to upload along with file</param>
        /// <param name="diarize"></param>
        /// <returns>String response from server or null if an error occurs</returns>
        public static string UploadFileForTranscription(Uri uploadUri, string filename, Stream fileStream, string lang, NameValueCollection values, bool diarize)
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
                    buffer = Encoding.ASCII.GetBytes(
                        $"Content-Disposition: form-data; name=\"{name}\"{Environment.NewLine}{Environment.NewLine}");
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                if (!diarize)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(
                        $"Content-Disposition: form-data; name=\"diarisation\"{Environment.NewLine}{Environment.NewLine}");
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("false" + Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"model\"{Environment.NewLine}{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format(lang + Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"data_file\"; filename=\"{filename}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(
                    $"Content-Type: application/octet-stream{Environment.NewLine}{Environment.NewLine}");
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
            catch (WebException)
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
        /// <param name="lang"></param>
        /// <param name="values">Collection of additional form [parameters to upload along with file</param>
        /// <param name="textFilename"></param>
        /// <param name="textStream"></param>
        /// <returns>String response from server or null if an error occurs</returns>
        public static string UploadFilesForAlignment(Uri uploadUri, string filename, Stream fileStream, string textFilename, Stream textStream, string lang, NameValueCollection values)
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
                    buffer = Encoding.ASCII.GetBytes(
                        $"Content-Disposition: form-data; name=\"{name}\"{Environment.NewLine}{Environment.NewLine}");
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"model\"{Environment.NewLine}{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format(lang + Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"data_file\"; filename=\"{filename}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(
                    $"Content-Type: application/octet-stream{Environment.NewLine}{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);


                fileStream.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(
                    $"Content-Disposition: form-data; name=\"text_file\"; filename=\"{textFilename}\"{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(
                    $"Content-Type: application/octet-stream{Environment.NewLine}{Environment.NewLine}");
                requestStream.Write(buffer, 0, buffer.Length);


                textStream.CopyTo(requestStream);
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
