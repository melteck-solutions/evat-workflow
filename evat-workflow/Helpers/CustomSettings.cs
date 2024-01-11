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
    }
}
