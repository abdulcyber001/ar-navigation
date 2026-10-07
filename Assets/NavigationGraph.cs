using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathConnection
{
    public string fromId;
    public string toId;
    public float distance;
}

public class NavigationGraph : MonoBehaviour
{
    public static NavigationGraph Instance;

    public List<PathConnection> connections = new List<PathConnection>();

    private Dictionary<string, List<(string neighbour, float dist)>> graph = new Dictionary<string, List<(string, float)>>();

    void Awake()
    {
        Instance = this;
        BuildGraph();
    }

    void BuildGraph()
    {
        graph.Clear();

        foreach (var c in connections)
        {
            if (!graph.ContainsKey(c.fromId))
                graph[c.fromId] = new List<(string, float)>();
            if (!graph.ContainsKey(c.toId))
                graph[c.toId] = new List<(string, float)>();

            graph[c.fromId].Add((c.toId, c.distance));
            graph[c.toId].Add((c.fromId, c.distance)); // bidirectional
        }
    }

    public List<string> FindShortestPath(string startId, string endId)
    {
        if (startId == endId) return new List<string> { startId };

        var dist = new Dictionary<string, float>();
        var prev = new Dictionary<string, string>();
        var pq = new SortedSet<(float distance, string node)>();

        foreach (var key in graph.Keys)
            dist[key] = float.MaxValue;

        dist[startId] = 0;
        pq.Add((0, startId));

        while (pq.Count > 0)
        {
            var current = pq.Min;
            pq.Remove(current);

            if (current.node == endId) break;

            if (!graph.ContainsKey(current.node)) continue;

            foreach (var (neighbour, d) in graph[current.node])
            {
                float newDist = dist[current.node] + d;
                if (newDist < dist[neighbour])
                {
                    pq.Remove((dist[neighbour], neighbour));
                    dist[neighbour] = newDist;
                    prev[neighbour] = current.node;
                    pq.Add((newDist, neighbour));
                }
            }
        }

        var path = new List<string>();
        string u = endId;
        if (!prev.ContainsKey(u) && u != startId) return null;

        while (u != null)
        {
            path.Insert(0, u);
            prev.TryGetValue(u, out u);
        }
        return path;
    }

    public float GetPathDistance(List<string> path)
    {
        if (path == null || path.Count < 2) return 0f;

        float total = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            string a = path[i];
            string b = path[i + 1];
            if (graph.ContainsKey(a))
            {
                foreach (var (n, d) in graph[a])
                {
                    if (n == b)
                    {
                        total += d;
                        break;
                    }
                }
            }
        }
        return total;
    }
}