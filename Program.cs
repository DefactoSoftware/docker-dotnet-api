using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Security.Cryptography.X509Certificates;

namespace apicore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseUrls("http://*:5000")
                .UseKestrel(options => {
                    // Configure SSL - No cert implemented yet
                    //options.UseHttps(LoadCertificate());
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static X509Certificate2 LoadCertificate()
        {
            var cert = new X509Certificate2(Directory.GetCurrentDirectory() + "\\Config\\cert\\test.pfx", "test");
            return cert;
        }
    }
}
