namespace API.MySwagger.Attributes
{
    public class SwaggerDefaultValue : Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SwaggerDefaultValue(string value)
        {
            this.Value = value;
        }

        public SwaggerDefaultValue(string name, string value) : this(value)
        {
            this.Name = name;
        }
    }
}
