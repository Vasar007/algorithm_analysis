namespace Microsoft.Extensions.Configuration.Json.Writable
{
    public sealed class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public WritableJsonConfigurationSource()
        {
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new WritableJsonConfigurationProvider(this);
        }
    }
}
