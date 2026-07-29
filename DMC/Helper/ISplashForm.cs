namespace DMC.Helper
{
    /// <summary>
    /// 等待界面窗体
    /// </summary>
    public interface ISplashForm
    {
        /// <summary>
        /// 加载等待窗体的文字
        /// </summary>
        /// <param name="NewStatusInfo"></param>
        void SetStatusInfo(string NewStatusInfo);
    }
}
