using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using RestSharp;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.IO;

namespace JsonServerProject
{
    internal class Program
    {
        //Initialize the RestClient
        private static RestClient client = new RestClient("http://localhost:3000");
       
        
        static async Task Main(string[] args)
        {
            //string path = @"C:\json-server-demo\bd.json";
            //File.Create(path);
            //call different methods to interact with API
            Console.WriteLine("Getting employee list");
            await GetEmployeeList();
            Console.WriteLine("Adding a new employee");
            //await AddNewEmployee("3", "Jack", "50000");
            Console.WriteLine("Adding multiple employees");
            //await AddMultipleEmployees();
            Console.WriteLine("Updating employee salary");
            //await UpdateEmployeeSalary("1", "400000");
            Console.WriteLine("Deleting an employee");
            //await DeleteEmployee(3);
            Console.WriteLine("All operations completed");

        }
        private static async Task GetEmployeeList()
        {
            var request = new RestRequest("Employees", Method.Get);
            var response = await client.ExecuteAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
                foreach(var emp in employeeList)
                {
                    Console.WriteLine($"Id: {emp.Id}, Name: {emp.Name}, Salary: {emp.Salary}");
                }
            }
            else
            {
                Console.WriteLine($"Failed to get employee list. Status: {response.StatusCode}");
            }
        }

        private static async Task AddNewEmployee(string Id, string Name, string Salary)
        {
            var request = new RestRequest("Employees", Method.Post);
            var jsonobj = new
            {
                Id = Id,
                Name = Name,
                Salary = Salary
            };
            request.AddJsonBody(jsonobj);   
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Console.WriteLine($"Added employee {employee.Name}, Salary:{employee.Salary}");
            }
            else
            {
                Console.WriteLine($"Failed to add employee. Status:{response.StatusCode}");
            }
        }

        private static async Task AddMultipleEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee {
                    Name = "Nagraj", Salary = "400000" },
                new Employee {
                    Name = "Sindhu", Salary = "500000" },
                new Employee {
                    Name = "Sajan", Salary = "300000" }
            };
            foreach (var emp in employees)
            {
                var request = new RestRequest("Employees", Method.Post);
                var jsonobj = new
                {
                    name = emp.Name,
                    salary = emp.Salary
                };
                request.AddJsonBody(jsonobj);
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var addedEmp = JsonConvert.DeserializeObject<Employee>(response.Content);
                    Console.WriteLine($"Added Employee: {addedEmp.Name}, Salary: {addedEmp.Salary}");
                }
                else
                {
                    Console.WriteLine($"Failed to add employee{emp.Name}. Status:{response.StatusCode}");
                }

            }
        }

        private static async Task UpdateEmployeeSalary(string id, string newsalary)
        {
            var request = new RestRequest($"Employees/{id}", Method.Put);
            var jsonObj = new
            {
                salary = newsalary,
            };
            request.AddJsonBody(jsonObj);

            var response = await client.ExecuteAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var updatedEmp = JsonConvert.DeserializeObject<Employee>(response.Content);
                Console.WriteLine($"Updated employee: {updatedEmp.Name}, new salary: {updatedEmp.Salary}");

            }
            else
            {
                Console.WriteLine($"Failed to update employee salary. Status: {response.StatusCode}");
            }
        }

        private static async Task DeleteEmployee(int id)
        {
            var request = new RestRequest($"Employees/{id}", Method.Delete);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully deleted employee with id {id}");

            }
            else
            {
                Console.WriteLine($"Failed to delete employee with id :{id}, status: {response.StatusCode}");
            }
        }    
    }
}
