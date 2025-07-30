using Unity.NetCode;
using UnityEngine;

// Sending message form client to server and form server to client
public struct SimpleRPC : IRpcCommand
{
    public int value;
}
