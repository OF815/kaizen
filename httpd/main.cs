using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] prefixes = { "http://localhost:8080/" };
            string publicFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public");

            if (!Directory.Exists(publicFolderPath))
            {
                Directory.CreateDirectory(publicFolderPath);
            }

            using (HttpListener listener = new HttpListener())
            {
                foreach (string prefix in prefixes)
                {
                    listener.Prefixes.Add(prefix);
                }

                listener.Start();
                Console.WriteLine("Listening...");

                Task.Run(() =>
                {
                    while (true)
                    {
                        HttpListenerContext context = listener.GetContext();
                        ProcessRequest(context, publicFolderPath);
                    }
                }).GetAwaiter().GetResult();
            }
        }

        private static void ProcessRequest(HttpListenerContext context, string publicFolderPath)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

	    string Part = request.Url.LocalPath.TrimStart('/');
	    if (String.IsNullOrEmpty(Part)) {
	        Part = "index.html";
	    }

            string filePath = Path.Combine(publicFolderPath, Part);
            if (! File.Exists(filePath))
            {
		string srcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "noname.html");
		string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", filePath);
                Console.WriteLine("new:" + newPath + "");

		try {
		    File.Copy(srcPath, newPath + "");

		} 
		catch(IOException ex) {
                    Console.WriteLine("Error create file: " + ex.Message);
                    SendErrorResponse(response, 404, "File Not Found");
		    return;
		}

   	    }

            if (File.Exists(filePath))
	    {

                try
                {
                    byte[] fileContents = File.ReadAllBytes(filePath);
                    response.ContentType = GetContentType(filePath);
                    response.ContentLength64 = fileContents.Length;

                    using (Stream output = response.OutputStream)
                    {
                        output.Write(fileContents, 0, fileContents.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error serving file: " + ex.Message);
                    SendErrorResponse(response, 500, "Internal Server Error");
                }
            }
            else
            {
                SendErrorResponse(response, 404, "File Not Found");
            }

            //Console.WriteLine($"Request received: {request.Url}");
        }

        private static void SendErrorResponse(HttpListenerResponse response, int statusCode, string statusDescription)
        {
            response.StatusCode = statusCode;
            response.StatusDescription = statusDescription;

            //string responseString = $"<html><body>{statusCode} - {statusDescription}</body></html>";
            string responseString = "<html><body>" + statusCode + " - " + statusDescription + "</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/html";
            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        private static string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".html":
                case ".htm":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
