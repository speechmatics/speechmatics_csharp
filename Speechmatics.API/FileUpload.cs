using System;
using System.Collections.Generic;
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
        private static readonly byte[] newline = {(byte) '\r', (byte) '\n'};

        private static void WriteStringToStream(string s, Stream stream)
        {
            var buffer = Encoding.ASCII.GetBytes(s);
            stream.Write(buffer, 0, buffer.Length);
            stream.Write(newline, 0, newline.Length);
        }

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
        public static string UploadFileForTranscription(Uri uploadUri, string filename, Stream fileStream, string lang, bool diarize)
        {
            var formParameters = new Dictionary<string, string>
            {
                ["diarise"] = diarize ? "true" : "false",
                ["model"] = lang
            };

            var request = WebRequest.Create(uploadUri);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (var entry in formParameters)
                {
                    WriteStringToStream(boundary, requestStream);
                    WriteStringToStream($"Content-Disposition: form-data; name=\"{entry.Key}\"\r\n", requestStream);
                    WriteStringToStream(entry.Value, requestStream);
                }

                WriteStringToStream(boundary, requestStream);
                WriteStringToStream($"Content-Disposition: form-data; name=\"data_file\"; filename=\"{filename}\"", requestStream);
                WriteStringToStream("Content-Type: application/octet-stream\r\n", requestStream);

                fileStream.CopyTo(requestStream);

                WriteStringToStream("\r\n" + boundary + "--", requestStream);
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
        public static string UploadFilesForAlignment(Uri uploadUri, string filename, Stream fileStream, string textFilename, Stream textStream, string lang)
        {
            var request = WebRequest.Create(uploadUri);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                var formParameters = new Dictionary<string, string>
                {
                    ["model"] = lang
                };

                // Write the values
                foreach (var entry in formParameters)
                {
                    WriteStringToStream(boundary, requestStream);
                    WriteStringToStream($"Content-Disposition: form-data; name=\"{entry.Key}\"\r\n", requestStream);
                    WriteStringToStream(entry.Value, requestStream);
                }

                WriteStringToStream(boundary, requestStream);
                WriteStringToStream($"Content-Disposition: form-data; name=\"data_file\"; filename=\"{filename}\"", requestStream);
                WriteStringToStream("Content-Type: application/octet-stream\r\n", requestStream);

                fileStream.CopyTo(requestStream);

                WriteStringToStream("\r\n", requestStream);
                WriteStringToStream(boundary, requestStream);
                WriteStringToStream($"Content-Disposition: form-data; name=\"text_file\"; filename=\"{textFilename}\"", requestStream);
                WriteStringToStream("Content-Type: application/octet-stream\r\n", requestStream);

                textStream.CopyTo(requestStream);

                WriteStringToStream("\r\n" + boundary + "--", requestStream);
            }
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var stream = new MemoryStream())
                        {
                            responseStream.CopyTo(stream);
                            return Encoding.UTF8.GetString(stream.ToArray());
                        }
                    }
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
