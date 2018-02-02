using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Aragas.Network;
using Aragas.Network.Extensions;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

using PCLExt.FileStorage;

using PokeD.Core.Packets.PokeD.Authorization;
using PokeD.Core.Packets.PokeD.Chat;
using PokeD.Core.Packets.PokeD.Overworld;
using PokeD.Core.Packets.PokeD.Overworld.Map;
using PokeD.CPGL.Storage.Folders.GameFolders;

using TMXParserPCL;

namespace PokeD.CPGL.Screens.InGame
{
    public partial class PokeDClient
    {
        Map Map { get; set; }

        AuthorizationStatus AuthorizationStatus { get; set; }
        bool Authorized { get; set; }

        private void HandleAuthorizationResponse(AuthorizationResponsePacket packet)
        {
            if (Authorized)
                return;

            AuthorizationStatus = packet.AuthorizationStatus;

            //if (!AuthorizationStatus.HasFlag(AuthorizationStatus.EncryprionEnabled))
            //    SendPacket(new AuthorizationPasswordPacket { PasswordHash = Password.Hash });
        }

        private void HandleEncryptionRequest(EncryptionRequestPacket packet)
        {
            if (Authorized)
                return;

            if (AuthorizationStatus.HasFlag(AuthorizationStatus.EncryprionEnabled))
            {
                var generator = new CipherKeyGenerator();
                generator.Init(new KeyGenerationParameters(new SecureRandom(), 16 * 8));
                var sharedKey = generator.GenerateKey();

                var pkcs = new PKCS1Signer(packet.PublicKey);
                var signedSecret = pkcs.SignData(sharedKey);
                var signedVerify = pkcs.SignData(packet.VerificationToken);

                SendPacket(new EncryptionResponsePacket { SharedSecret = signedSecret, VerificationToken = signedVerify });

                Stream.InitializeEncryption(sharedKey);

            }
            else
                throw new Exception("Encryption was not enabled!");
        }

        private void HandleAuthorizationComplete(AuthorizationCompletePacket packet)
        {
            Authorized = true;

            ID = packet.PlayerID;

            SendPacket(new ChatGlobalMessagePacket() { Message = "/login 123"});
        }

        private void HandleAuthorizationDisconnect(AuthorizationDisconnectPacket packet)
        {
            Authorized = false;
            var reason = packet.Reason;
        }


        private void HandleMap(MapPacket packet)
        {
            var mapsFolder = new OldContentFolder().CreateFolder("Maps", CreationCollisionOption.OpenIfExists);
            var tileSetsFolder = mapsFolder.CreateFolder("TileSets", CreationCollisionOption.OpenIfExists);

            Map = Map.Load(new MemoryStream(Encoding.UTF8.GetBytes(packet.MapData)));

            var tileSetList = Map.TileSets.Where(tileSet =>
                {
                    // -- TileSet not found
                    if (tileSetsFolder.CheckExists(tileSet.Source) == ExistenceCheckResult.NotFound ||
                        tileSetsFolder.CheckExists(tileSet.Source.Replace(".tsx", ".png")) == ExistenceCheckResult.NotFound)
                        return true;

                    // -- TileSet hash not matching
                    else
                    {
                        var externalHash = packet.TileSetHashes.First(externalTileSet => tileSet.Source.Replace(".tsx", "") == externalTileSet.Name).Hash;
                        var hash = tileSetsFolder.GetFile(tileSet.Source).MD5Hash();

                        return externalHash != hash;
                    }
                }).Select(tileSet => tileSet.Source.Replace(".tsx", "")).ToList();


            // -- If resources not found on disk, download them from server
            if (tileSetList.Any())
                SendPacket(new TileSetRequestPacket() { TileSetNames = tileSetList.ToArray() });

            // -- Else use cached resources
            else
            {
                Map.TileSets = Map.TileSets.Select(tileSet =>
                {
                    using (var fileStream = tileSetsFolder.GetFile(tileSet.Source).Open(FileAccess.Read))
                        return TileSet.LoadExternal(fileStream, tileSet);  
                }).ToList();

                OnMapLoaded?.Invoke(Map);
            }
        }
        private void HandleTileSetResponse(TileSetResponsePacket packet)
        {
            var mapsFolder = new OldContentFolder().GetFolder("Maps");
            var tileSetsFolder = mapsFolder.GetFolder("TileSets");

            var loadedTileSets = new List<TileSet>();
            foreach (var externalTileSet in packet.TileSets)
            {
                var originTileSet = Map.TileSets.Find(tileSet => tileSet.Source.Replace(".tsx", "") == externalTileSet.Name);

                tileSetsFolder.CreateFile($"{externalTileSet.Name}.tsx", CreationCollisionOption.ReplaceExisting).WriteAllText(externalTileSet.TileSetData);

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(externalTileSet.TileSetData)))
                    loadedTileSets.Add(TileSet.LoadExternal(stream, originTileSet));
            }

            foreach (var image in packet.Images)
                using (var fileStream = tileSetsFolder.CreateFile($"{image.Name}.png", CreationCollisionOption.ReplaceExisting).Open(FileAccess.ReadAndWrite))
                {
                    fileStream.SetLength(0);
                    fileStream.Write(image.ImageData, 0, image.ImageData.Length);
                }
            
            Map.TileSets = loadedTileSets;

            OnMapLoaded?.Invoke(Map);
        }
        private void HandlePosition(PositionPacket packet)
        {
            if (ID != packet.ID)
            {
                
            }
        }
    }
}
