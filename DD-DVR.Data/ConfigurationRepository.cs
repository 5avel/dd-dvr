namespace DD_DVR.Data
{
    public class ConfigurationRepository : Repository<ConfigurationRepository>
    {
        public string OutputVodeoDir { set; get; }
        public string Key { set; get; }

    }
}
