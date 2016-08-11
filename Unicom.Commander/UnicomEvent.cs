namespace Unicom.Commander
{
    /// <summary>
    /// TCP客户端数据接收委托
    /// </summary>
    public delegate void ClientReceivedDataEventHandler(ICommanderConnection conn);

    /// <summary>
    /// TCP客户端连接断开委托
    /// </summary>
    /// <param name="conn"></param>
    public delegate void ClientDisconnectEventHandler(ICommanderConnection conn);
}
