using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Newtonsoft.Json;
using Cissy;
using Cissy.Caching;
using ServiceStack.Model;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Pipeline;
using ServiceStack.Redis.Support;
using Cissy.Caching.Redis;

namespace Cissy.Redis
{
    public partial class RedisCluster : IRedisCache
    {
        #region IRedisCache
        public string this[CacheKey key] { get { return this.GetRedisClient()[key.ToString()]; } }

        public IDisposable AcquireLock(CacheKey key, TimeSpan timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AcquireLock(key.ToString(), timeOut);
            }
        }
        public IDisposable AcquireLock(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AcquireLock(key.ToString());
            }
        }
        public long AddGeoMember(CacheKey key, double longitude, double latitude, string member)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddGeoMember(key.ToString(), longitude, latitude, member);
            }
        }
        public long AddGeoMembers(CacheKey key, params RedisGeo[] geoPoints)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddGeoMembers(key.ToString(), geoPoints);
            }
        }
        public void AddItemToList(CacheKey listId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.AddItemToList(listId.ToString(), value);
            }
        }
        public void AddItemToSet(CacheKey setId, string item)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.AddItemToSet(setId.ToString(), item);
            }
        }
        public bool AddItemToSortedSet(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddItemToSortedSet(setId.ToString(), value);
            }
        }
        public bool AddItemToSortedSet(CacheKey setId, string value, double score)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddItemToSortedSet(setId.ToString(), value);
            }
        }
        public void AddRangeToList(CacheKey listId, List<string> values)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.AddRangeToList(listId.ToString(), values);
            }
        }
        public void AddRangeToSet(CacheKey setId, List<string> items)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.AddRangeToSet(setId.ToString(), items);
            }
        }
        public bool AddRangeToSortedSet(CacheKey setId, List<string> values, double score)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddRangeToSortedSet(setId.ToString(), values, score);
            }
        }
        public bool AddRangeToSortedSet(CacheKey setId, List<string> values, long score)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddRangeToSortedSet(setId.ToString(), values, score);
            }
        }
        public bool AddToHyperLog(CacheKey key, params string[] elements)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AddToHyperLog(key.ToString(), elements);
            }
        }
        public long AppendToValue(CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.AppendToValue(key.ToString(), value);
            }
        }

        public string BlockingDequeueItemFromList(CacheKey listId, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingDequeueItemFromList(listId.ToString(), timeOut);
            }
        }
        public ItemRef BlockingDequeueItemFromLists(CacheKey[] listIds, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingDequeueItemFromLists(listIds.Select(m => m.ToString()).ToArray(), timeOut);
            }
        }
        public string BlockingPopAndPushItemBetweenLists(CacheKey fromListId, string toListId, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingPopAndPushItemBetweenLists(fromListId.ToString(), toListId.ToString(), timeOut);
            }
        }
        public string BlockingPopItemFromList(CacheKey listId, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingPopItemFromList(listId.ToString(), timeOut);
            }
        }
        public ItemRef BlockingPopItemFromLists(CacheKey[] listIds, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingPopItemFromLists(listIds.Select(m => m.ToString()).ToArray(), timeOut);
            }
        }
        public string BlockingRemoveStartFromList(CacheKey listId, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingRemoveStartFromList(listId.ToString(), timeOut);
            }
        }
        public ItemRef BlockingRemoveStartFromLists(CacheKey[] listIds, TimeSpan? timeOut)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.BlockingRemoveStartFromLists(listIds.Select(m => m.ToString()).ToArray(), timeOut);
            }
        }
        public double CalculateDistanceBetweenGeoMembers(CacheKey key, string fromMember, string toMember, string unit = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.CalculateDistanceBetweenGeoMembers(key.ToString(), fromMember, toMember, unit);
            }
        }
        public bool ContainsKey(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ContainsKey(key.ToString());
            }
        }
        public long CountHyperLog(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.CountHyperLog(key.ToString());
            }
        }
        public IRedisPipeline CreatePipeline()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.CreatePipeline();
            }
        }
        public IRedisSubscription CreateSubscription()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.CreateSubscription();
            }
        }
        public IRedisTransaction CreateTransaction()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.CreateTransaction();
            }
        }
        public RedisText Custom(params object[] cmdWithArgs)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.Custom(cmdWithArgs);
            }
        }
        public long DecrementValue(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.DecrementValue(key.ToString());
            }
        }
        public long DecrementValueBy(CacheKey key, int count)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.DecrementValueBy(key.ToString(), count);
            }
        }
        public string DequeueItemFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.DequeueItemFromList(listId.ToString());
            }
        }
        public void EnqueueItemOnList(CacheKey listId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.EnqueueItemOnList(listId.ToString(), value);
            }
        }
        public bool ExpireEntryAt(CacheKey key, DateTime expireAt)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ExpireEntryAt(key.ToString(), expireAt);
            }
        }
        public bool ExpireEntryIn(CacheKey key, TimeSpan expireIn)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ExpireEntryIn(key.ToString(), expireIn);
            }
        }
        public string[] FindGeoMembersInRadius(CacheKey key, string member, double radius, string unit)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.FindGeoMembersInRadius(key.ToString(), member, radius, unit);
            }
        }
        public string[] FindGeoMembersInRadius(CacheKey key, double longitude, double latitude, double radius, string unit)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.FindGeoMembersInRadius(key.ToString(), longitude, latitude, radius, unit);
            }
        }
        public List<RedisGeoResult> FindGeoResultsInRadius(CacheKey key, string member, double radius, string unit, int? count = null, bool? sortByNearest = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.FindGeoResultsInRadius(key.ToString(), member, radius, unit, count, sortByNearest);
            }
        }
        public List<RedisGeoResult> FindGeoResultsInRadius(CacheKey key, double longitude, double latitude, double radius, string unit, int? count = null, bool? sortByNearest = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.FindGeoResultsInRadius(key.ToString(), longitude, latitude, radius, unit, count, sortByNearest);
            }
        }

        public Dictionary<string, string> GetAllEntriesFromHash(CacheKey hashId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllEntriesFromHash(hashId.ToString());
            }
        }
        public List<string> GetAllItemsFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllItemsFromList(listId.ToString());
            }
        }
        public HashSet<string> GetAllItemsFromSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllItemsFromSet(setId.ToString());
            }
        }
        public List<string> GetAllItemsFromSortedSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllItemsFromSortedSet(setId.ToString());
            }
        }
        public List<string> GetAllItemsFromSortedSetDesc(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllItemsFromSortedSetDesc(setId.ToString());
            }
        }
        public List<string> GetAllKeys()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllKeys();
            }
        }
        public IDictionary<string, double> GetAllWithScoresFromSortedSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAllWithScoresFromSortedSet(setId.ToString());
            }
        }
        public string GetAndSetValue(CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetAndSetValue(key.ToString(), value);
            }
        }
        public string GetClient()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetClient();
            }
        }
        public List<Dictionary<string, string>> GetClientsInfo()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetClientsInfo();
            }
        }

        public HashSet<string> GetDifferencesFromSet(CacheKey fromSetId, params string[] withSetIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetDifferencesFromSet(fromSetId.ToString(), withSetIds);
            }
        }
        public RedisKeyType GetEntryType(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetEntryType(key.ToString());
            }
        }

        public List<RedisGeo> GetGeoCoordinates(CacheKey key, params string[] members)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetGeoCoordinates(key.ToString(), members);
            }
        }
        public string[] GetGeohashes(CacheKey key, params string[] members)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetGeohashes(key.ToString(), members);
            }
        }
        public long GetHashCount(CacheKey hashId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetHashCount(hashId.ToString());
            }
        }
        public IEnumerable<string> GetKeysByPattern(string pattern)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetKeysByPattern(pattern);
            }
        }
        public List<string> GetHashKeys(CacheKey hashId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetHashKeys(hashId.ToString());
            }
        }
        public List<string> GetHashValues(CacheKey hashId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetHashValues(hashId.ToString());
            }
        }
        public HashSet<string> GetIntersectFromSets(params string[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetIntersectFromSets(setIds);
            }
        }
        public string GetItemFromList(CacheKey listId, int listIndex)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetItemFromList(listId.ToString(), listIndex);
            }
        }
        public long GetItemIndexInSortedSet(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetItemIndexInSortedSet(setId.ToString(), value);
            }
        }
        public long GetItemIndexInSortedSetDesc(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetItemIndexInSortedSetDesc(setId.ToString(), value);
            }
        }
        public double GetItemScoreInSortedSet(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetItemScoreInSortedSet(setId.ToString(), value);
            }
        }
        public long GetListCount(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetListCount(listId.ToString());
            }
        }
        public string GetRandomItemFromSet(string CacheKey)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRandomItemFromSet(CacheKey);
            }
        }
        public string GetRandomKey()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRandomKey();
            }
        }
        public List<string> GetRangeFromList(CacheKey listId, int startingFrom, int endingAt)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromList(listId.ToString(), startingFrom, endingAt);
            }
        }
        public List<string> GetRangeFromSortedList(CacheKey listId, int startingFrom, int endingAt)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedList(listId.ToString(), startingFrom, endingAt);
            }
        }
        public List<string> GetRangeFromSortedSet(CacheKey setId, int fromRank, int toRank)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSet(setId.ToString(), fromRank, toRank);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromStringScore, toStringScore);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromStringScore, toStringScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromStringScore, toStringScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromStringScore, toStringScore);
            }
        }
        public List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public List<string> GetRangeFromSortedSetDesc(CacheKey setId, int fromRank, int toRank)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeFromSortedSetDesc(setId.ToString(), fromRank, toRank);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSet(CacheKey setId, int fromRank, int toRank)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSet(setId.ToString(), fromRank, toRank);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromStringScore, toStringScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromStringScore, toStringScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByHighestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromStringScore, toStringScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromScore, toScore, skip, take);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetByLowestScore(setId.ToString(), fromStringScore, toStringScore);
            }
        }
        public IDictionary<string, double> GetRangeWithScoresFromSortedSetDesc(CacheKey setId, int fromRank, int toRank)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetRangeWithScoresFromSortedSetDesc(setId.ToString(), fromRank, toRank);
            }
        }

        public DateTime GetServerTime()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return GetServerTime();
            }
        }
        public long GetSetCount(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSetCount(setId.ToString());
            }
        }
        public List<string> GetSortedEntryValues(CacheKey key, int startingFrom, int endingAt)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedEntryValues(key.ToString(), startingFrom, endingAt);
            }
        }
        public List<string> GetSortedItemsFromList(CacheKey listId, SortOptions sortOptions)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedItemsFromList(listId.ToString(), sortOptions);
            }
        }
        public long GetSortedSetCount(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedSetCount(setId.ToString());
            }
        }
        public long GetSortedSetCount(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedSetCount(setId.ToString(), fromScore, toScore);
            }
        }
        public long GetSortedSetCount(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedSetCount(setId.ToString(), fromScore, toScore);
            }
        }
        public long GetSortedSetCount(CacheKey setId, string fromStringScore, string toStringScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetSortedSetCount(setId.ToString(), fromStringScore, toStringScore);
            }
        }
        public long GetStringCount(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetStringCount(key.ToString());
            }
        }
        public HashSet<string> GetUnionFromSets(params CacheKey[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetUnionFromSets(setIds.Select(m => m.ToString()).ToArray());
            }
        }
        public string GetValue(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValue(key.ToString());
            }
        }
        public string GetValue(string key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValue(key);
            }
        }
        public string GetValueFromHash(CacheKey hashId, CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValueFromHash(hashId.ToString(), key.ToString());
            }
        }
        public List<string> GetValues(List<CacheKey> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValues(keys.Select(m => m.ToString()).ToList());
            }
        }
        public List<T> GetValues<T>(List<CacheKey> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValues<T>(keys.Select(m => m.ToString()).ToList());
            }
        }
        public List<T> GetValues<T>(List<string> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValues<T>(keys);
            }
        }
        public List<string> GetValuesFromHash(string CacheKey, params string[] keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValuesFromHash(CacheKey, keys);
            }
        }
        public Dictionary<string, string> GetValuesMap(List<CacheKey> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValuesMap(keys.Select(m => m.ToString()).ToList());
            }
        }
        public Dictionary<string, T> GetValuesMap<T>(List<CacheKey> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.GetValuesMap<T>(keys.Select(m => m.ToString()).ToList());
            }
        }
        public bool HashContainsEntry(CacheKey hashId, string key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.HashContainsEntry(hashId.ToString(), key);
            }
        }
        public double IncrementItemInSortedSet(CacheKey setId, string value, double incrementBy)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementItemInSortedSet(setId.ToString(), value, incrementBy);
            }
        }
        public double IncrementItemInSortedSet(CacheKey setId, string value, long incrementBy)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementItemInSortedSet(setId.ToString(), value, incrementBy);
            }
        }
        public long IncrementValue(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValue(key.ToString());
            }
        }
        public long IncrementValueBy(CacheKey key, long count)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValueBy(key.ToString(), count);
            }
        }
        public double IncrementValueBy(CacheKey key, double count)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValueBy(key.ToString(), count);
            }
        }
        public long IncrementValueBy(CacheKey key, int count)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValueBy(key.ToString(), count);
            }
        }
        public double IncrementValueInHash(CacheKey hashId, CacheKey key, double incrementBy)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValueInHash(hashId.ToString(), key.ToString(), incrementBy);
            }
        }
        public long IncrementValueInHash(CacheKey hashId, CacheKey key, int incrementBy)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.IncrementValueInHash(hashId.ToString(), key.ToString(), incrementBy);
            }
        }

        public void MoveBetweenSets(CacheKey fromSetId, CacheKey toSetId, string item)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.MoveBetweenSets(fromSetId.ToString(), toSetId.ToString(), item);
            }
        }

        public string PopAndPushItemBetweenLists(CacheKey fromListId, CacheKey toListId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopAndPushItemBetweenLists(fromListId.ToString(), toListId.ToString());
            }
        }
        public string PopItemFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopItemFromList(listId.ToString());
            }
        }
        public string PopItemFromSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopItemFromSet(setId.ToString());
            }
        }
        public List<string> PopItemsFromSet(CacheKey setId, int count)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopItemsFromSet(setId.ToString(), count);
            }
        }
        public string PopItemWithHighestScoreFromSortedSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopItemWithHighestScoreFromSortedSet(setId.ToString());
            }
        }
        public string PopItemWithLowestScoreFromSortedSet(CacheKey setId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.PopItemWithLowestScoreFromSortedSet(setId.ToString());
            }
        }
        public void PrependItemToList(CacheKey listId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.PrependItemToList(listId.ToString(), value);
            }
        }
        public void PrependRangeToList(CacheKey listId, List<string> values)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.PrependRangeToList(listId.ToString(), values);
            }
        }
        public void PushItemToList(CacheKey listId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.PushItemToList(listId.ToString(), value);
            }
        }
        public void RemoveAllFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.RemoveAllFromList(listId.ToString());
            }
        }
        public string RemoveEndFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveEndFromList(listId.ToString());
            }
        }

        public bool RemoveEntryFromHash(CacheKey hashId, CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveEntryFromHash(hashId.ToString(), key.ToString());
            }
        }

        public long RemoveItemFromList(CacheKey listId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveItemFromList(listId.ToString(), value);
            }
        }
        public long RemoveItemFromList(CacheKey listId, string value, int noOfMatches)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveItemFromList(listId.ToString(), value, noOfMatches);
            }
        }
        public void RemoveItemFromSet(CacheKey setId, string item)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.RemoveItemFromSet(setId.ToString(), item);
            }
        }
        public bool RemoveItemFromSortedSet(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveItemFromSortedSet(setId.ToString(), value);
            }
        }
        public long RemoveItemsFromSortedSet(CacheKey setId, List<string> values)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveItemsFromSortedSet(setId.ToString(), values);
            }
        }
        public long RemoveRangeFromSortedSet(CacheKey setId, int minRank, int maxRank)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveRangeFromSortedSet(setId.ToString(), minRank, maxRank);
            }
        }
        public long RemoveRangeFromSortedSetByScore(CacheKey setId, double fromScore, double toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveRangeFromSortedSetByScore(setId.ToString(), fromScore, toScore);
            }
        }
        public long RemoveRangeFromSortedSetByScore(CacheKey setId, long fromScore, long toScore)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveRangeFromSortedSetByScore(setId.ToString(), fromScore, toScore);
            }
        }
        public long RemoveRangeFromSortedSetBySearch(CacheKey setId, string start = null, string end = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveRangeFromSortedSetBySearch(setId.ToString(), start, end);
            }
        }
        public string RemoveStartFromList(CacheKey listId)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.RemoveStartFromList(listId.ToString());
            }
        }

        public IEnumerable<KeyValuePair<string, string>> ScanAllHashEntries(CacheKey hashId, string pattern = null, int pageSize = 1000)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ScanAllHashEntries(hashId.ToString(), pattern, pageSize);
            }
        }
        public IEnumerable<string> ScanAllKeys(string pattern = null, int pageSize = 1000)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ScanAllKeys(pattern, pageSize);
            }
        }
        public IEnumerable<string> ScanAllSetItems(CacheKey setId, string pattern = null, int pageSize = 1000)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ScanAllSetItems(setId.ToString(), pattern, pageSize);
            }
        }
        public IEnumerable<KeyValuePair<string, double>> ScanAllSortedSetItems(CacheKey setId, string pattern = null, int pageSize = 1000)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.ScanAllSortedSetItems(setId.ToString(), pattern, pageSize);
            }
        }
        public List<string> SearchKeys(string pattern)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SearchKeys(pattern);
            }
        }
        public List<string> SearchSortedSet(CacheKey setId, string start = null, string end = null, int? skip = null, int? take = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SearchSortedSet(setId.ToString(), start, end, skip, take);
            }
        }
        public long SearchSortedSetCount(CacheKey setId, string start = null, string end = null)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SearchSortedSetCount(setId.ToString(), start, end);
            }
        }

        public bool SetContainsItem(CacheKey setId, string item)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SetContainsItem(setId.ToString(), item);
            }
        }
        public bool SetEntryInHash(CacheKey hashId, CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SetEntryInHash(hashId.ToString(), key.ToString(), value);
            }
        }
        public bool SetEntryInHashIfNotExists(CacheKey hashId, CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SetEntryInHashIfNotExists(hashId.ToString(), key.ToString(), value);
            }
        }
        public void SetItemInList(CacheKey listId, int listIndex, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.SetItemInList(listId.ToString(), listIndex, value);
            }
        }
        public void SetRangeInHash(CacheKey hashId, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.SetRangeInHash(hashId.ToString(), keyValuePairs);
            }
        }
        public void SetValue(CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.SetValue(key.ToString(), value);
            }
        }
        public void SetValue(CacheKey key, string value, TimeSpan expireIn)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.SetValue(key.ToString(), value, expireIn);
            }
        }
        public bool SetValueIfExists(CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SetValueIfExists(key.ToString(), value);
            }
        }
        public bool SetValueIfNotExists(CacheKey key, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SetValueIfNotExists(key.ToString(), value);
            }
        }

        public bool SortedSetContainsItem(CacheKey setId, string value)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.SortedSetContainsItem(setId.ToString(), value);
            }
        }

        public void StoreAsHash<T>(T entity)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.StoreAsHash<T>(entity);
            }
        }
        public void StoreDifferencesFromSet(CacheKey intoSetId, string fromSetId, params CacheKey[] withSetIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.StoreDifferencesFromSet(intoSetId.ToString(), fromSetId, withSetIds.Select(m => m.ToString()).ToArray());
            }
        }
        public void StoreIntersectFromSets(CacheKey intoSetId, params CacheKey[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.StoreIntersectFromSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray());
            }
        }
        public long StoreIntersectFromSortedSets(CacheKey intoSetId, params CacheKey[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.StoreIntersectFromSortedSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray());
            }
        }
        public long StoreIntersectFromSortedSets(CacheKey intoSetId, CacheKey[] setIds, string[] args)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.StoreIntersectFromSortedSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray(), args);
            }
        }
        public object StoreObject(object entity)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.StoreObject(entity);
            }
        }
        public void StoreUnionFromSets(CacheKey intoSetId, params CacheKey[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.StoreUnionFromSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray());
            }
        }
        public long StoreUnionFromSortedSets(CacheKey intoSetId, CacheKey[] setIds, string[] args)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.StoreUnionFromSortedSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray(), args);
            }
        }
        public long StoreUnionFromSortedSets(CacheKey intoSetId, params CacheKey[] setIds)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.StoreUnionFromSortedSets(intoSetId.ToString(), setIds.Select(m => m.ToString()).ToArray());
            }
        }
        public void TrimList(CacheKey listId, int keepStartingFrom, int keepEndingAt)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.TrimList(listId.ToString(), keepStartingFrom, keepEndingAt);
            }
        }
        public bool Remove(CacheKey key)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return redisClient.Remove(key.ToString());
            }
        }
        public async Task RemoveAsync<T>(CacheKey Key) where T : class, IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                await Task.Run(() =>
                {
                    redisClient.Remove(Key.ToString());
                });
            }
        }
        public void RemoveAll(IEnumerable<CacheKey> keys)
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.RemoveAll(keys.Select(m => m.ToString()));
            }
        }
        public T Get<T>(CacheKey Key, Func<T> func) where T : class, IModel
        {
            T t = default(T);
            string json = this.GetValue(Key);
            if (ModelExtensions.JsonIsEmpty(json))
            {
                t = func();
                this.SetValue(Key, t.ModelToJson());
            }
            else
            {
                t = json.JsonToModel<T>();
            }
            return t;
        }
        public void Set<T>(CacheKey Key, T target) where T : class, IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.Set<T>(Key.ToString(), target);
            }
        }
        #endregion
    }
}
