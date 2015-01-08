using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monarch.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScriptEngine_Smoke_Test();
            JObject json = GetJObjectFromFile("Content\\candidate.json");
            string code = GetCodeFromFile("Content\\script.js");

            using (var engine = new V8ScriptEngine())
            {
                // expose a host type
                engine.AddHostType("Console", typeof(Console));
                
                //Evaluate the json 
                engine.Evaluate("var entity = " + json.ToString() + ";");
                
                ////add the candidate json
                //engine.AddHostObject("employee", json);
                //engine.Execute("employee.fullName = employee.firstName + ' ' + employee.lastName;Console.WriteLine(employee)");
                engine.Execute(code);

                var result = engine.Evaluate("JSON.stringify(entity)").ToString();

                var jsonResult = JObject.Parse(result);

                Console.WriteLine(jsonResult.ToString());
            
            }


            Console.ReadKey();
        }

        private static string GetCodeFromFile(string p)
        {
            using (var reader = new StreamReader("Content\\script.js"))
            {
                return reader.ReadToEnd();
            }
        }

        private static JObject GetJObjectFromFile(string path)
        {
            using (var streamReader = new StreamReader("Content\\candidate.json"))
            {
                // the json text reader 
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    return JObject.Load(reader);
                }
            }
        }

        private static void ScriptEngine_Smoke_Test()
        {
            using (var engine = new V8ScriptEngine())
            {
                // expose a host type
                engine.AddHostType("Console", typeof(Console));

                var employee = new Employee { FirstName = "Andrea", LastName = "Ardila" };

                engine.AddHostObject("employee", employee);

                engine.Execute("employee.FullName = employee.FirstName + ' ' + employee.LastName;");

                engine.Execute("Console.WriteLine('FullName = {0}', employee.FullName)");

            }
        }

        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
        }

        
    }
}
