using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Caching;

namespace Cissy.Caching.Redis
{
    public interface IRedisCache : ICache
    {
        string this[CacheKey key] { get;  }
        IDisposable AcquireLock(CacheKey key, TimeSpan timeOut);
        IDisposable AcquireLock(CacheKey key);
        long AddGeoMember(CacheKey key, double longitude, double latitude, string member);
        void AddItemToList(CacheKey listId, string value);
        void AddItemToSet(CacheKey setId, string item);
        bool AddItemToSortedSet(CacheKey setId, string value);
        bool AddItemToSortedSet(CacheKey setId, string value, double score);
        void AddRangeToList(CacheKey listId, List<string> values);
        void AddRangeToSet(CacheKey setId, List<string> items);
        bool AddRangeToSortedSet(CacheKey setId, List<string> values, double score);
        bool AddRangeToSortedSet(CacheKey setId, List<string> values, long score);
        bool AddToHyperLog(CacheKey key, params string[] elements);
        long AppendToValue(CacheKey key, string value);

        string BlockingDequeueItemFromList(CacheKey listId, TimeSpan? timeOut);
        string BlockingPopAndPushItemBetweenLists(CacheKey fromListId, string toListId, TimeSpan? timeOut);
        string BlockingPopItemFromList(CacheKey listId, TimeSpan? timeOut);
        string BlockingRemoveStartFromList(CacheKey listId, TimeSpan? timeOut);
        double CalculateDistanceBetweenGeoMembers(CacheKey key, string fromMember, string toMember, string unit = null);
        bool ContainsKey(CacheKey key);
        long CountHyperLog(CacheKey key);
        long DecrementValue(CacheKey key);
        long DecrementValueBy(CacheKey key, int count);
        string DequeueItemFromList(CacheKey listId);
        void EnqueueItemOnList(CacheKey listId, string value);
        bool ExpireEntryAt(CacheKey key, DateTime expireAt);
        bool ExpireEntryIn(CacheKey key, TimeSpan expireIn);
        string[] FindGeoMembersInRadius(CacheKey key, string member, double radius, string unit);
        string[] FindGeoMembersInRadius(CacheKey key, double longitude, double latitude, double radius, string unit);
       
        Dictionary<string, string> GetAllEntriesFromHash(CacheKey hashId);
        List<string> GetAllItemsFromList(CacheKey listId);
        HashSet<string> GetAllItemsFromSet(CacheKey setId);
        List<string> GetAllItemsFromSortedSet(CacheKey setId);
        List<string> GetAllItemsFromSortedSetDesc(CacheKey setId);
        List<string> GetAllKeys();
        IDictionary<string, double> GetAllWithScoresFromSortedSet(CacheKey setId);
        string GetAndSetValue(CacheKey key, string value);
        string GetClient();
        List<Dictionary<string, string>> GetClientsInfo();
     
        HashSet<string> GetDifferencesFromSet(CacheKey fromSetId, params string[] withSetIds);
     
        string[] GetGeohashes(CacheKey key, params string[] members);
        long GetHashCount(CacheKey hashId);
        List<string> GetHashKeys(CacheKey hashId);
        List<string> GetHashValues(CacheKey hashId);
        HashSet<string> GetIntersectFromSets(params string[] setIds);
        string GetItemFromList(CacheKey listId, int listIndex);
        long GetItemIndexInSortedSet(CacheKey setId, string value);
        long GetItemIndexInSortedSetDesc(CacheKey setId, string value);
        double GetItemScoreInSortedSet(CacheKey setId, string value);
        long GetListCount(CacheKey listId);
        string GetRandomItemFromSet(string CacheKey);
        string GetRandomKey();
        List<string> GetRangeFromList(CacheKey listId, int startingFrom, int endingAt);
        List<string> GetRangeFromSortedList(CacheKey listId, int startingFrom, int endingAt);
        List<string> GetRangeFromSortedSet(CacheKey setId, int fromRank, int toRank);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore);
        List<string> GetRangeFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take);
        List<string> GetRangeFromSortedSetDesc(CacheKey setId, int fromRank, int toRank);
        IDictionary<string, double> GetRangeWithScoresFromSortedSet(CacheKey setId, int fromRank, int toRank);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, double fromScore, double toScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, long fromScore, long toScore, int? skip, int? take);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(CacheKey setId, string fromStringScore, string toStringScore);
        IDictionary<string, double> GetRangeWithScoresFromSortedSetDesc(CacheKey setId, int fromRank, int toRank);
       
        DateTime GetServerTime();
        long GetSetCount(CacheKey setId);
        List<string> GetSortedEntryValues(CacheKey key, int startingFrom, int endingAt);
        long GetSortedSetCount(CacheKey setId);
        long GetSortedSetCount(CacheKey setId, long fromScore, long toScore);
        long GetSortedSetCount(CacheKey setId, double fromScore, double toScore);
        long GetSortedSetCount(CacheKey setId, string fromStringScore, string toStringScore);
        long GetStringCount(CacheKey key);
        HashSet<string> GetUnionFromSets(params CacheKey[] setIds);
        string GetValue(CacheKey key);
        string GetValueFromHash(CacheKey hashId, CacheKey key);
        List<string> GetValues(List<CacheKey> keys);
        List<T> GetValues<T>(List<CacheKey> keys);
        List<string> GetValuesFromHash(string CacheKey, params string[] keys);
        Dictionary<string, string> GetValuesMap(List<CacheKey> keys);
        Dictionary<string, T> GetValuesMap<T>(List<CacheKey> keys);
        bool HashContainsEntry(CacheKey hashId, string key);      
        double IncrementItemInSortedSet(CacheKey setId, string value, double incrementBy);
        double IncrementItemInSortedSet(CacheKey setId, string value, long incrementBy);
        long IncrementValue(CacheKey key);
        long IncrementValueBy(CacheKey key, long count);
        double IncrementValueBy(CacheKey key, double count);
        long IncrementValueBy(CacheKey key, int count);
        double IncrementValueInHash(CacheKey hashId, CacheKey key, double incrementBy);
        long IncrementValueInHash(CacheKey hashId, CacheKey key, int incrementBy);
        
        void MoveBetweenSets(CacheKey fromSetId, CacheKey toSetId, string item);
       
        string PopAndPushItemBetweenLists(CacheKey fromListId, CacheKey toListId);
        string PopItemFromList(CacheKey listId);
        string PopItemFromSet(CacheKey setId);
        List<string> PopItemsFromSet(CacheKey setId, int count);
        string PopItemWithHighestScoreFromSortedSet(CacheKey setId);
        string PopItemWithLowestScoreFromSortedSet(CacheKey setId);
        void PrependItemToList(CacheKey listId, string value);
        void PrependRangeToList(CacheKey listId, List<string> values);   
        void PushItemToList(CacheKey listId, string value);
       
        bool Remove(CacheKey key);
      
        void RemoveAll(IEnumerable<CacheKey> keys);
        void RemoveAllFromList(CacheKey listId);      
        string RemoveEndFromList(CacheKey listId);
       
        bool RemoveEntryFromHash(CacheKey hashId, CacheKey key);
        long RemoveItemFromList(CacheKey listId, string value);
        long RemoveItemFromList(CacheKey listId, string value, int noOfMatches);
        void RemoveItemFromSet(CacheKey setId, string item);
        bool RemoveItemFromSortedSet(CacheKey setId, string value);
        long RemoveItemsFromSortedSet(CacheKey setId, List<string> values);
        long RemoveRangeFromSortedSet(CacheKey setId, int minRank, int maxRank);
        long RemoveRangeFromSortedSetByScore(CacheKey setId, double fromScore, double toScore);
        long RemoveRangeFromSortedSetByScore(CacheKey setId, long fromScore, long toScore);
        long RemoveRangeFromSortedSetBySearch(CacheKey setId, string start = null, string end = null);
        string RemoveStartFromList(CacheKey listId);
      
        IEnumerable<KeyValuePair<string, string>> ScanAllHashEntries(CacheKey hashId, string pattern = null, int pageSize = 1000);
        IEnumerable<string> ScanAllKeys(string pattern = null, int pageSize = 1000);
        IEnumerable<string> ScanAllSetItems(CacheKey setId, string pattern = null, int pageSize = 1000);
        IEnumerable<KeyValuePair<string, double>> ScanAllSortedSetItems(CacheKey setId, string pattern = null, int pageSize = 1000);
        List<string> SearchKeys(string pattern);
        List<string> SearchSortedSet(CacheKey setId, string start = null, string end = null, int? skip = null, int? take = null);
        long SearchSortedSetCount(CacheKey setId, string start = null, string end = null);

        bool SetContainsItem(CacheKey setId, string item);
        bool SetEntryInHash(CacheKey hashId, CacheKey key, string value);
        bool SetEntryInHashIfNotExists(CacheKey hashId, CacheKey key, string value);
        void SetItemInList(CacheKey listId, int listIndex, string value);
        void SetRangeInHash(CacheKey hashId, IEnumerable<KeyValuePair<string, string>> keyValuePairs);
        void SetValue(CacheKey key, string value);
        void SetValue(CacheKey key, string value, TimeSpan expireIn);
        bool SetValueIfExists(CacheKey key, string value);
        bool SetValueIfNotExists(CacheKey key, string value);
       
        bool SortedSetContainsItem(CacheKey setId, string value);
        void StoreAsHash<T>(T entity);
        void StoreDifferencesFromSet(CacheKey intoSetId, string fromSetId, params CacheKey[] withSetIds);
        void StoreIntersectFromSets(CacheKey intoSetId, params CacheKey[] setIds);
        long StoreIntersectFromSortedSets(CacheKey intoSetId, params CacheKey[] setIds);
        long StoreIntersectFromSortedSets(CacheKey intoSetId, CacheKey[] setIds, string[] args);
        object StoreObject(object entity);
        void StoreUnionFromSets(CacheKey intoSetId, params CacheKey[] setIds);
        long StoreUnionFromSortedSets(CacheKey intoSetId, CacheKey[] setIds, string[] args);
        long StoreUnionFromSortedSets(CacheKey intoSetId, params CacheKey[] setIds);
        void TrimList(CacheKey listId, int keepStartingFrom, int keepEndingAt);
        void Set<T>(CacheKey Key, T target) where T : class, IModel;
    }
}
