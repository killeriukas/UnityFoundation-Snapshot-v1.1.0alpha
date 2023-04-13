using System;
using System.Collections;
using System.Collections.Generic;
using TMI.Helper;

namespace TMI.Collection {

    public class DeferredDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> {

        private Dictionary<TKey, TValue> normalDictionary = new Dictionary<TKey, TValue>();
        private Dictionary<TKey, TValue> temporaryAddDictionary = new Dictionary<TKey, TValue>();
        private List<TKey> temporaryRemoveList = new List<TKey>();

        public event Action<IEnumerable<TKey>> onItemsRemoved;

        public DeferredDictionary() {
        }

        public bool ContainsKey(TKey key) {
            return normalDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value) {
            temporaryAddDictionary.Add(key, value);
        }

        public void Remove(TKey key) {
            temporaryRemoveList.Add(key);
        }

        public int Count {
            get {
                return normalDictionary.Count;
            }
        }

        public void ApplyChanges() {
            if(temporaryRemoveList.Count > 0) {
                foreach(TKey key in temporaryRemoveList) {
                    normalDictionary.Remove(key);
                }
                onItemsRemoved?.Invoke(temporaryRemoveList);
                temporaryRemoveList.Clear();
            }

            foreach(var kvp in temporaryAddDictionary) {
                normalDictionary.Add(kvp.Key, kvp.Value);
            }
            temporaryAddDictionary.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return normalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return normalDictionary.GetEnumerator();
        }
    }


}

