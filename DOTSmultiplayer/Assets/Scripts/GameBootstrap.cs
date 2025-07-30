using UnityEngine;
using Unity.NetCode;

//Required by DOCS attribute
[UnityEngine.Scripting.Preserve]
//Creating a connection
public class GameBootstrap : ClientServerBootstrap      
{
    public override bool Initialize(string defaultWorldName)
    {
        AutoConnectPort = 7979; //Default port
        return base.Initialize(defaultWorldName);   
    }
}
