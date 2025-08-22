using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ResourceManager : MonoBehaviour
    {
        Dictionary<string, Object> resources = new Dictionary<string, Object>();
        public T Load<T>(string path) where T : Object
        {
            string key = $"{typeof(T)}.{path}";

            if (resources.ContainsKey(key))
                return resources[key] as T;

            T resource = Resources.Load<T>(path);

            if (resource == null) Debug.LogWarning($"ResourceManager: 리소스 로드 실패 - {key}");

            else resources.Add(key, resource);

            return resource;
        }

        // 게임 규모가 커지면 주의하자
        // 특정 패스 기준으로 리소스를 다불러오는건 위험하다. -> 메모리가 부족할 수 있음
        public T[] LoadAll<T>(string path) where T : Object
        {
            // 1) Resources.LoadAll로 에셋 배열 로드
            T[] assets = Resources.LoadAll<T>(path);
            if (assets == null || assets.Length == 0)
            {
                Debug.LogWarning($"ResourceManager: LoadAll 실패 또는 에셋 없음 - {typeof(T)} @ \"{path}\"");
                return assets;
            }

            // 2) (선택) 개별 에셋을 캐시 딕셔너리에 등록
            foreach (var asset in assets)
            {
                string key = $"{typeof(T)}.{path}/{asset.name}";
                if (!resources.ContainsKey(key))
                    resources.Add(key, asset);
            }

            return assets;
        }

        public List<T> InstantiateAll<T>(string path, Transform parent = null) where T : Object
        {
            var assets = LoadAll<T>(path);
            var instances = new List<T>();
            foreach (var a in assets)
            {
                var inst = Object.Instantiate(a, parent);
                instances.Add(inst);
            }
            return instances;
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
        {
            if (pooling)
                return GameManager.Pool.Get(original, position, rotation, parent);
            else
                return Object.Instantiate(original, position, rotation, parent);
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
        {
            return Instantiate(original, position, rotation, null, pooling);
        }

        public new T Instantiate<T>(T original, Transform parent, bool pooling = false) where T : Object
        {
            return Instantiate(original, Vector3.zero, Quaternion.identity, parent, pooling);
        }

        public T Instantiate<T>(T original, bool pooling = false) where T : Object
        {
            return Instantiate(original, Vector3.zero, Quaternion.identity, null, pooling);
        }

        // 경로(path)에 따라 오브젝트를 생성하는 기능
        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
        {
            T original = Load<T>(path);
            return Instantiate(original, position, rotation, parent, pooling);
        }

        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
        {
            return Instantiate<T>(path, position, rotation, null, pooling);
        }

        public T Instantiate<T>(string path, Transform parent, bool pooling = false) where T : Object
        {
            return Instantiate<T>(path, Vector3.zero, Quaternion.identity, parent, pooling);
        }

        public T Instantiate<T>(string path, bool pooling = false) where T : Object
        {
            return Instantiate<T>(path, Vector3.zero, Quaternion.identity, null, pooling);
        }

        public void Destroy(GameObject go)
        {
            if (GameManager.Pool.IsContain(go))
                GameManager.Pool.Release(go);
            else
                Object.Destroy(go);
        }

        public void Destroy(GameObject go, float delay)
        {
            if (GameManager.Pool.IsContain(go))
                StartCoroutine(DelayReleaseRoutine(go, delay));
            else
                Object.Destroy(go, delay);
        }

        IEnumerator DelayReleaseRoutine(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            GameManager.Pool.Release(go);
        }

        public void Destroy(Component component, float delay = 0f)
        {
            Object.Destroy(component, delay);
    }
    }
