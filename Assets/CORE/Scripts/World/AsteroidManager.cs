using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace Core.World
{
    struct AsteroidSync : Unity.Netcode.INetworkSerializable
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public uint AsteroidID;

        // INetworkSerializable
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref AsteroidID);

        }
        // ~INetworkSerializable
    }

    [ExecuteAlways]
    public class AsteroidManager : NetworkBehaviour
    {

        [SerializeField] protected uint startingId = 0;
        [SerializeField] List<Asteroid> asteroidsIterable = new List<Asteroid>();
        Dictionary<uint, Asteroid> asteroids = new Dictionary<uint, Asteroid>();
        Dictionary<uint, bool> dirtyFlags = new Dictionary<uint, bool>(); // If an asteroid has moved

        void Awake()
        {
            if (!Application.isPlaying)
            {
                asteroidsIterable = new List<Asteroid>();
                asteroids = new Dictionary<uint, Asteroid>();
                dirtyFlags = new Dictionary<uint, bool>();
                uint id = startingId;
                Asteroid[] found = FindObjectsOfType<Asteroid>();
                Debug.Log("Found " + found.Length + " Asteroids");
                for (int i = 0; i < found.Length; i += 1)
                {
                    Asteroid a = found[i];
                    a.AsteroidID = id;
                    asteroidsIterable.Add(a);
                    asteroids.Add(id, a);
                    dirtyFlags.Add(id, false);
                    id += 1;
                }
                
            } 
            else
            {
                asteroidsIterable.ForEach((Asteroid a) =>
                {
                    // Rebuild dictionaries in playmode
                    asteroids.Add(a.AsteroidID, a);
                    dirtyFlags.Add(a.AsteroidID, false);
                });
            }
        }

        public void DistanceFromSquared(Vector2 target, uint AsteroidID)
        {
            Vector2 asteroidPosition = asteroids[AsteroidID].transform.position;
            float distance = (asteroidPosition - target).sqrMagnitude;
        }

        public Asteroid AsteroidAt(uint AsteroidID)
        {
            Debug.Log("Searching for asteroid " + AsteroidID);
            return asteroids[AsteroidID];
        }

            public void DestroyAsteroidAt(uint AsteroidID)
        {
            int index = asteroidsIterable.FindIndex((Asteroid a) =>
            {
                return a.AsteroidID == AsteroidID;
            });
            asteroidsIterable.RemoveAt(index);
            Asteroid a = asteroids[AsteroidID];
            asteroids.Remove(AsteroidID);
            Destroy(a.gameObject);
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (NetworkObject.IsOwner) 
            {
                AsteroidSync[] syncs = asteroidsIterable.FindAll((Asteroid a) =>
                {
                    if (a.IsDirty())
                    {
                        Debug.Log("YES IT IS DIRTY");
                    }
                    return a.IsDirty();
                }).ConvertAll<AsteroidSync>((Asteroid a) =>
                {
                    AsteroidSync sync = new AsteroidSync();
                    sync.Position = a.transform.position;
                    sync.Rotation = a.transform.rotation;
                    sync.AsteroidID = a.AsteroidID;
                    return sync;
                }).ToArray();

                Debug.Log("Syncing " + syncs.Length + " asteroids");
                SyncAsteroidPositionsClientRpc(syncs);
            }
        }

        [ClientRpc(Delivery = RpcDelivery.Unreliable)]
        void SyncAsteroidPositionsClientRpc(AsteroidSync[] values)
        {
            if (!NetworkObject.NetworkManager.IsHost && !NetworkObject.NetworkManager.IsServer)
            {
                for (int i = 0; i < values.Length; i += 1)
                {
                    AsteroidSync value = values[i];
                    Asteroid a = AsteroidAt(value.AsteroidID);
                    a.rb.MovePosition(value.Position);
                    a.rb.MoveRotation(value.Rotation);
                }
            }
        }
    }
}
