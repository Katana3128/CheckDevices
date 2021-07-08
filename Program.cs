using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

/// <summary>
/// Check Devices
/// </summary>
/// <remarks>Inventura Webex zařízení a připojených periferií.</remarks>
namespace CheckDevices
{
    /// <summary>
    /// Vlastní program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Spuštění programu
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Check Devices\nNástroj na vypsání všech Webex zařízení v administraci s jejich SN a k nim připojeným periferiím.\nPzn.: netýká se mikrofonů, které jsou analogové.\n");
            //token
            string token = ReadSetting("token");
            if (token != "Nenalezeno")
            {
                //spuštění vlastní rutiny
                RunAsync(token).Wait();
            }
            else
            {
                Console.WriteLine("Není token");
            }

        }

        /// <summary>Načtení nastavení</summary>
        /// <param name="key">Klíč nastavení, tedy "token".</param>
        /// <returns>Webex token.</returns>
        /// <remarks>V případě, že token není nalezen, nebo neexistuje klíč, vrátí "Nenalezeno"</remarks>
        private static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Nenalezeno";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Nepodařilo se najít klíč nastavení: " + key);
                return "Nenalezeno";
            }
        }
        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="token">Token.</param>
        static async Task RunAsync(string token)
        {
            try
            {
                //Vše zapiš do souboru
                using StreamWriter file = new("WbxDeviceAPeriferie.txt", append: true);
                
                //Zjištění všech zařízení ve Webexu
                var clientDevices = new RestClient("https://webexapis.com/v1/devices");
                clientDevices.Timeout = -1;
                var requestDevices = new RestRequest(Method.GET);
                requestDevices.AddHeader("Authorization", "Bearer " + token);
                IRestResponse responseDevices = clientDevices.Execute(requestDevices);
                //Deserializace JSON na strukturu v WbxDevices.cs
                WbxDevices WbxDevices = JsonSerializer.Deserialize<WbxDevices>(responseDevices.Content);
                //Projdi každou položku ze seznamu WbxDevices
                foreach (var WbxDevice in WbxDevices.items)
                {
                    //Výpis Webex zařízení (Název, Typ, SN)
                    Console.WriteLine(WbxDevice.displayName);
                    await file.WriteLineAsync(WbxDevice.displayName);
                    Console.WriteLine("\tProduct:\t" + WbxDevice.product);
                    await file.WriteLineAsync("\tProduct:\t" + WbxDevice.product);
                    Console.WriteLine("\tSN:\t\t" + WbxDevice.serial);
                    await file.WriteLineAsync("\tSN:\t\t" + WbxDevice.serial);

                    //Zjištění všech periferií
                    string UriWbxPeriferies = "https://webexapis.com/v1/xapi/status?deviceId=" + WbxDevice.id + "&name=Peripherals.*";
                    var clientPeriferies = new RestClient(UriWbxPeriferies);
                    clientPeriferies.Timeout = -1;
                    var requestPeriferies = new RestRequest(Method.GET);
                    requestPeriferies.AddHeader("Authorization", "Bearer " + token);
                    IRestResponse responsePeriferies = clientPeriferies.Execute(requestPeriferies);
                    //Jestliže je odpověď
                    if (responsePeriferies.IsSuccessful)
                    {
                        //Deserializace JSON
                        WbxPeriferies wbxPeriferies = JsonSerializer.Deserialize<WbxPeriferies>(responsePeriferies.Content);
                        //Jestliže není seznam periferií prázdný
                        if (wbxPeriferies.result.Peripherals != null)
                        {
                            Console.WriteLine("\tPeriferie:");
                            await file.WriteLineAsync("\tPeriferie:");
                            foreach (Connecteddevice WbxPeriferie in wbxPeriferies.result.Peripherals.ConnectedDevice)
                            {
                                Console.WriteLine("\t\tType:\t" + WbxPeriferie.Type);
                                await file.WriteLineAsync("\t\tType:\t" + WbxPeriferie.Type);
                                Console.WriteLine("\t\tSN:\t" + WbxPeriferie.SerialNumber);
                                await file.WriteLineAsync("\t\tSN:\t" + WbxPeriferie.SerialNumber);
                            }
                        }
                        else
                        {
                            //Jinak zapiš, že je seznam prázdný
                            Console.WriteLine("\tPeriferie: Seznam je prázdný");
                            await file.WriteLineAsync("\tPeriferie: Seznam je prázdný");
                        }
                    }
                    else
                    {
                        //Jinak zapiš, že nevrátil žádná data
                        Console.WriteLine("\tPeriferie: Žádná data");
                        await file.WriteLineAsync("\tPeriferie: Žádná data");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
