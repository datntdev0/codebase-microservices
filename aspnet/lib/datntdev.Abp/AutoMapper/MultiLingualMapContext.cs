using datntdev.Abp.Configuration;

namespace datntdev.Abp.AutoMapper;

public class MultiLingualMapContext
{
    public ISettingManager SettingManager { get; set; }

    public MultiLingualMapContext(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }
}