using IdentityModel.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            do
            {
                Console.WriteLine("Select the number of your choice: ");
                Console.WriteLine("1: Register Patient");
                Console.WriteLine("2: Display Patients");
                Console.WriteLine("3: To test fails");
                Console.WriteLine("4: Quit");
                Console.Write("Enter the number of your choice: ");
                string choice = Console.ReadLine();

                if (choice == "4")
                {
                    Environment.Exit(0);
                }

                if (choice == "1")
                {
                    Console.Write("Patient FirstName: ");
                    string patientFirstName = Console.ReadLine();
                    Console.Write("Patient LastName: ");
                    string patientSurname = Console.ReadLine();
                    Console.Write("Patient Age: ");
                    int patientAge = int.Parse(Console.ReadLine());
                    Console.Write("Test_date '-' seperator YYYY-MM-DD: ");
                    DateTime patientTestDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Patient Email: ");
                    string patientEmail = Console.ReadLine();

                    HttpClient client = new HttpClient();

                    var app = PublicClientApplicationBuilder.Create("16e7c4dc-393f-4a7a-84f0-3c711e5addb1")
                        .WithAuthority("https://login.microsoftonline.com/52a0b183-1f44-4c78-9f64-a9eef2f29a8e/oauth2/v2.0/")
                        .WithDefaultRedirectUri()
                        .Build();
                    

/*                    var result = await app.AcquireTokenWithDeviceCode(new[] { "api://16e7c4dc-393f-4a7a-84f0-3c711e5addb1/.default" },
                        async r =>
                        {
                            Console.WriteLine(r.Message);
                        })
                        .ExecuteAsync();

                    var token = result.AccessToken;

                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer " + token);*/

                    Patient p = new Patient()
                    {
                        FirstName = patientFirstName,
                        Surname = patientSurname,
                        Age = patientAge,
                        DateOfThePositiveTest = patientTestDate,
                        Email = patientEmail
                    };
                    string usertJson = System.Text.Json.JsonSerializer.Serialize(p);

                    await client.PostAsync("https://localhost:5001/api/patients",
                        new StringContent(usertJson, Encoding.UTF8, "application/json"));
                }
                if (choice == "2")
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/patients");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Patient> patientList = JsonConvert.DeserializeObject<List<Patient>>(responseBody);
                    foreach (var patient in patientList)
                    {
                        Console.WriteLine("Name: {0} {1},\n Age: {2},\n Test_date: {3},\n Email: {4}\n", patient.FirstName, patient.Surname, patient.Age, patient.DateOfThePositiveTest, patient.Email);
                    }
                }
                if (choice == "3")
                {
                    HttpClient client = new HttpClient();

                    string e = "Error";

                    string usertJson = System.Text.Json.JsonSerializer.Serialize(e);

                    await client.PutAsync("https://localhost:5001/api/patients",
                        new StringContent(usertJson, Encoding.UTF8, "application/json"));

                    await client.PutAsync("https://localhost:5002/api/email",
                        new StringContent(usertJson, Encoding.UTF8, "application/json"));
                }
            } while (true);
        }

        private static async Task<string> GetToken()
        {
            using var client = new HttpClient();

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = "https://login.microsoftonline.com/52a0b183-1f44-4c78-9f64-a9eef2f29a8e/oauth2/v2.0/token",
                ClientId = "16e7c4dc-393f-4a7a-84f0-3c711e5addb1",
                ClientSecret = "~5vn~erM1J-ctUejtIVGWNJuCE-kt9~vh7",
                Scope = "api://16e7c4dc-393f-4a7a-84f0-3c711e5addb1/.default"
            }; // all to change

            var token = await client.RequestClientCredentialsTokenAsync(tokenRequest);

            if (token.IsError) throw new InvalidOperationException($"Couldn't gather token. Details: {token.Error}");

            return token.AccessToken;
        }

    }

    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public DateTime DateOfThePositiveTest { get; set; }
        public string Email { get; set; }
    }
}
