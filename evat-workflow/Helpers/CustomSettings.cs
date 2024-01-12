namespace evat_workflow.Helpers
{
    public class CustomSettings
    {
        public static CustomSettings Current;

        public CustomSettings()
        {
            Current = this;
        }


        public string AppUrl { get; set; }
        public string Folder { get; set; } 
        public string ClientId { get; set; } 
        public string ClientSecret { get; set; }  
        public string SaveToken { get; set; } 
        public string Authority { get; set; } 
        public string AuthorityURL { get; set; } 
        public string TokenUrl { get; set; } 
        public string RequireHttpsMetadata { get; set; } 
    }
}
