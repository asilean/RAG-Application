namespace ProjectManagament_WebApp.Sevices
{
    public class JsonTemplates
    {
        public Dictionary<Guid, string> Templates { get; set; }

        public JsonTemplates()
        {
            Templates = new Dictionary<Guid, string>
        {
            { new Guid("your-module-id-1"), @"
                {modüle göre jsonlar!!}"
            },
            { new Guid("your-module-id-2"), @"
                {modüle göre jsonlar!!}"
            },
            { new Guid("your-module-id-3"), @"
                {modüle göre jsonlar!!}"
            }
        };
        }

        public string GetTemplate(Guid moduleId)
        {
            if (Templates.TryGetValue(moduleId, out var template))
            {
                return template;
            }
            return "";
        }
    }

}
