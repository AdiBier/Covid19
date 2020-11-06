using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string choice = Console.ReadLine();

            do
            {
                Console.WriteLine("Select the number of your choice: ");
                Console.WriteLine("1: Register Patient");
                Console.WriteLine("2: Display Patients");
                Console.WriteLine("3: Quit");
                Console.Write("Enter the number of your choice: ");
                choice = Console.ReadLine();

                if (choice == "3")
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

                    Patient p = new Patient()
                    {
                        FirstName = patientFirstName,
                        Surname = patientSurname,
                        Age = patientAge,
                        DateOfThePositiveTest = patientTestDate,
                        Email = patientEmail
                    };
                    string usertJson = System.Text.Json.JsonSerializer.Serialize(p);

                    await client.PostAsync("https://localhost:44327/api/patients",
                        new StringContent(usertJson, Encoding.UTF8, "application/json"));
                }
                if (choice == "2")
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("https://localhost:44327/api/patients");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Patient> patientList = JsonConvert.DeserializeObject<List<Patient>>(responseBody);
                    foreach (var patient in patientList)
                    {
                        Console.WriteLine("Name: {0} {1},\n Age: {2},\n Test_date: {3},\n Email: {4}\n", patient.FirstName, patient.Surname, patient.Age, patient.DateOfThePositiveTest, patient.Email);
                    }
                }
            } while (true);
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
