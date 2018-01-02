using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SerializationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var f = new Foo();
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };


            var json = JsonConvert.SerializeObject(f, settings);


            var f2 = (Foo)JsonConvert.DeserializeObject(json, settings);

            f2.Hello();

            Console.ReadLine();
        }
    }

    class Foo
    {
        public int Number;
        private string name;
        private test po = new test();

        public void Hello()
        {
            Console.WriteLine(po.hello);
        }
    }

    public class test
    {
        public int hello { get; set; }
    }

    public class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(p => base.CreateProperty(p, memberSerialization))
                .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => base.CreateProperty(f, memberSerialization)))
                .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}
