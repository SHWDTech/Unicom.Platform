namespace Unicom.Commander
{
    /// <summary>
    /// 指令包装
    /// </summary>
    public class CommandWarpper
    {
        /// <summary>
        /// 指令类型名称
        /// </summary>
        public string CommandType { get; set; }

        public object CommandObject { get; set; }
    }
}
