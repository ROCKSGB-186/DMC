namespace DMC.Models
{
    /// <summary>
    /// 返回的用户专业/ 1：userId 用户Id/ 2：majorId 专业ID/ 3：majorName 专业名称/ 4：majorIds
    /// </summary>
    public class UserMajorsModel
    {
        /// <summary>
        /// 1：userId 用户Id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 2：majorId 专业ID
        /// </summary>
        public string majorId { get; set; }
        /// <summary>
        /// 3：majorName 专业名称
        /// </summary>
        public string majorName { get; set; }
        /// <summary>
        /// 4：majorIds
        /// </summary>
        public string majorIds { get; set; }
    }
}
