using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screenshot
{
    /// <summary>
    /// 提供配置文件中AppSettings节中对应的Key名称
    /// </summary>
    public static class AppSettingKeys
    {
        //基本设置
        public static string HotKeyMode = "HotKeyMode";
        public static string InfoBoxVisible = "InfoBoxVisible";
        public static string ToolBoxVisible = "ToolBoxVisible";
        public static string ZoomBoxVisible = "ZoomBoxVisible";
        public static string ZoomBoxWidth = "ZoomBoxWidth";
        public static string ZoomBoxHeight = "ZoomBoxHeight";
        public static string IsCutCursor = "IsCutCursor";
        //图片上传
        public static string PicDescFieldName = "PicDescFieldName";
        public static string ImageFieldName = "ImageFieldName";
        public static string PicDesc = "PicDesc";
        public static string UploadUrl = "UploadUrl";
        public static string DoUpload = "DoUpload";
        //自动保存
        public static string AutoSaveToDisk = "AutoSaveToDisk";
        public static string AutoSaveSubDir = "AutoSaveSubDir";
        public static string AutoSaveDirectory = "AutoSaveDirectory";
        public static string AutoSaveFileName1 = "AutoSaveFileName1";
        public static string AutoSaveFileName2 = "AutoSaveFileName2";
        public static string AutoSaveFileName3 = "AutoSaveFileName3";

    }
}
