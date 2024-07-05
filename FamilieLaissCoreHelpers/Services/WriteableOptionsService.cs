using System;
using System.IO;
using FamilieLaissCoreHelpers.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FamilieLaissCoreHelpers.Services
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IOptionsMonitor<T> _Options;
        private readonly string _Section;
        private readonly string _Filename;

        public WritableOptions(
            IOptionsMonitor<T> options,
            string section,
            string filename)
        {
            _Options = options;
            _Section = section;
            _Filename = filename;
        }

        public T Value => _Options.CurrentValue;
        public T Get(string name) => _Options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_Filename));
            var sectionObject = jObject.TryGetValue(_Section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[_Section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(_Filename, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}