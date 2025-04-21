using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Vita.ViewModels
{
    public class Disease
    {
        public class HumanDiseaseOntology
        {
            [JsonPropertyName("graphs")]
            public List<Graph> Graphs { get; set; }
        }

        public class Graph
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("meta")]
            public Meta Meta { get; set; }

            [JsonPropertyName("nodes")]
            public List<Node> Nodes { get; set; }

            [JsonPropertyName("edges")]
            public List<Edge> Edges { get; set; }
        }

        public class Meta
        {
            [JsonPropertyName("basicPropertyValues")]
            public List<BasicPropertyValue> BasicPropertyValues { get; set; }

            [JsonPropertyName("version")]
            public string Version { get; set; }
        }

        public class NodeWithMeta
        {
            public Node Node { get; set; }
            public NodeMeta Meta { get; set; }
        }

        public class BasicPropertyValue
        {
            [JsonPropertyName("pred")]
            public string Predicate { get; set; }

            [JsonPropertyName("val")]
            public string Value { get; set; }
        }

        public class Node
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("lbl")]
            public string Label { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("meta")]
            public NodeMeta Meta { get; set; }
        }

        public class NodeMeta
        {
            [JsonPropertyName("definition")]
            public Definition Definition { get; set; }

            [JsonPropertyName("subsets")]
            public List<string> Subsets { get; set; }

            [JsonPropertyName("synonyms")]
            public List<Synonym> Synonyms { get; set; }

            [JsonPropertyName("xrefs")]
            public List<Xref> Xrefs { get; set; }

            [JsonPropertyName("basicPropertyValues")]
            public List<BasicPropertyValue> BasicPropertyValues { get; set; }
        }

        public class Definition
        {
            [JsonPropertyName("val")]
            public string Value { get; set; }

            [JsonPropertyName("xrefs")]
            public List<string> Xrefs { get; set; }
        }

        public class Synonym
        {
            [JsonPropertyName("pred")]
            public string Predicate { get; set; }

            [JsonPropertyName("val")]
            public string Value { get; set; }
        }

        public class Xref
        {
            [JsonPropertyName("val")]
            public string Value { get; set; }
        }

        public class Edge
        {
            [JsonPropertyName(")")]
            public string Subject { get; set; }

            [JsonPropertyName("pred")]
            public string Predicate { get; set; }

            [JsonPropertyName("obj")]
            public string Object { get; set; }
        }

        // Functions
        public async Task<Node> ReturnDiseases(string diseaseLabel)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("HumanDO.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ontology = JsonSerializer.Deserialize<HumanDiseaseOntology>(json, options);

                if (ontology?.Graphs == null) return null;

                foreach (var graph in ontology.Graphs)
                {
                    if (graph.Nodes == null) continue;
                    var diseaseNode = graph.Nodes.FirstOrDefault(n =>
                        n.Label != null &&
                        n.Label.Equals(diseaseLabel, StringComparison.OrdinalIgnoreCase));
                    if (diseaseNode != null)
                    {
                        return diseaseNode;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load or parse JSON: {e.Message}", "OK");
                return null;
            }
        }

        public async Task<List<Node>> LoadDataAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("HumanDO.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ontology = JsonSerializer.Deserialize<HumanDiseaseOntology>(json, options);

                if (ontology?.Graphs == null)
                {
                    return new List<Node>();
                }

                var nodes = ontology.Graphs
                    .Where(graph => graph.Nodes != null)
                    .SelectMany(graph => graph.Nodes)
                    .Where(node => node.Label != null)
                    .ToList();

                return nodes;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load data: {e.Message}", "OK");
                return new List<Node>();
            }
        }

        public async Task<List<NodeWithMeta>> LoadDataWithMetaAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("HumanDO.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ontology = JsonSerializer.Deserialize<HumanDiseaseOntology>(json, options);

                if (ontology?.Graphs == null)
                {
                    return new List<NodeWithMeta>();
                }

                var nodesWithMeta = ontology.Graphs
                    .Where(graph => graph.Nodes != null)
                    .SelectMany(graph => graph.Nodes)
                    .Where(node => node.Label != null)
                    .Select(node => new NodeWithMeta
                    {
                        Node = node,
                        Meta = node.Meta
                    })
                    .ToList();

                return nodesWithMeta;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load data: {e.Message}", "OK");
                return new List<NodeWithMeta>();
            }
        }
    }
}