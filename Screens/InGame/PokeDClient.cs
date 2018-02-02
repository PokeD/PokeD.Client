using System;
using System.Collections.Generic;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using PCLExt.Network;

using PokeD.Core.Packets.PokeD;
using PokeD.Core.Packets.PokeD.Authorization;
using PokeD.Core.Packets.PokeD.Overworld;
using PokeD.Core.Packets.PokeD.Overworld.Map;
using PokeD.CPGL.Physics.Collision;

using TMXParserPCL;

namespace PokeD.CPGL.Screens.InGame
{
    public partial class PokeDClient : IPosition2DChanged
    {
        public Action<Map> OnMapLoaded { get; }
        public Action<Microsoft.Xna.Framework.Vector2> Position2DChangedAction { get; }

        public VarInt ID { get; set; }
        public string Name { get; set; }

#if DEBUG
        // -- Debug -- //
        List<Packet> Received { get; } = new List<Packet>();
        List<Packet> Sended { get; } = new List<Packet>();
        // -- Debug -- //
#endif

        ISocketClient Client { get; }
        ProtobufStream Stream { get; }



        public PokeDClient(Action<Map> onMapLoaded, string name, bool debugPackets = false)
        {
            Name = name;

            Client = SocketClient.CreateTCP();
            Stream = new ProtobufStream(Client);

            OnMapLoaded = onMapLoaded;
            Position2DChangedAction = OnPositionChanged;
        }
        private void OnPositionChanged(Microsoft.Xna.Framework.Vector2 vector2)
        {
            SendPacket(new PositionPacket() { Position = new Vector3(vector2.X, 0.0f, vector2.Y) });
        }

        public bool Connect(string ip, ushort port = 15130)
        {
            Disconnect();

            Stream.Connect(ip, port);

            SendPacket(new AuthorizationRequestPacket() { Name = Name });

            return true;
        }
        public bool Disconnect()
        {
            if (Stream.IsConnected)
                Stream.Disconnect();

            return true;
        }



        public void Update()
        {
            if (Stream.IsConnected)
            {
                if (Stream.DataAvailable > 0)
                {
                    var dataLength = Stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        //Logger.Log(LogType.GlobalError, $"Protobuf Reading Error: Packet Length size is 0. Disconnecting.");
                        SendPacket(new AuthorizationDisconnectPacket { Reason = "Packet Length size is 0!" });
                        //Dispose();
                        return;
                    }

                    var data = Stream.Receive(dataLength);

                    HandleData(data);
                }
            }
            else
                ; //Dispose();
        }
        private void HandleData(byte[] data)
        {
            if (data != null)
            {
                using (var reader = new ProtobufDataReader(data))
                {
                    var id = reader.Read<VarInt>();

                    Func<PokeDPacket> func;
                    if (PokeDPacketResponses.TryGetPacketFunc(id, out func))
                    {
                        if (func != null)
                        {
                            var packet = func().ReadPacket(reader);

                            HandlePacket(packet);

#if DEBUG
                            Received.Add(packet);
#endif
                        }
                        else
                        {
                            //Logger.Log(LogType.GlobalError, $"SCON Reading Error: Packet ID {id} is not correct, Packet Data: {data}. Disconnecting.");
                            SendPacket(new DisconnectPacket() {Reason = $"Packet ID {id} is not correct!"});
                            //Dispose();
                        }
                    }
                    else
                    {
                        //Logger.Log(LogType.GlobalError, $"SCON Reading Error: Packet ID {id} is not correct, Packet Data: {data}. Disconnecting.");
                        SendPacket(new DisconnectPacket {Reason = $"Packet ID {id} is not correct!"});
                        //Dispose();
                    }
                }
            }
            else
                ;//Logger.Log(LogType.GlobalError, $"SCON Reading Error: Packet Data is null.");
        }
        private void HandlePacket(ProtobufPacket packet)
        {
            switch ((PokeDPacketTypes) (int) packet.ID)
            {
                case PokeDPacketTypes.AuthorizationResponse:
                    HandleAuthorizationResponse((AuthorizationResponsePacket) packet);
                    break;
                case PokeDPacketTypes.EncryptionRequest:
                    HandleEncryptionRequest((EncryptionRequestPacket) packet);
                    break;
                case PokeDPacketTypes.AuthorizationComplete:
                    HandleAuthorizationComplete((AuthorizationCompletePacket) packet);
                    break;
                case PokeDPacketTypes.AuthorizationDisconnect:
                    HandleAuthorizationDisconnect((AuthorizationDisconnectPacket) packet);
                    break;

                case PokeDPacketTypes.Ping:
                    break;

                case PokeDPacketTypes.Map:
                    HandleMap((MapPacket) packet);
                    break;
                case PokeDPacketTypes.TileSetResponse:
                    HandleTileSetResponse((TileSetResponsePacket) packet);
                    break;
                case PokeDPacketTypes.Position:
                    HandlePosition((PositionPacket) packet);
                    break;

            }
        }


        private void SendPacket(Packet packet)
        {

            Stream.SendPacket(packet);

#if DEBUG
            Sended.Add(packet);
#endif
        }
    }
}
