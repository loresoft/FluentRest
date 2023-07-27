using System;
using System.Text.Json.Serialization;

namespace FluentRest.Tests.GitHub.Models;


public class Milestone
{

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("labels_url")]
    public string LabelsUrl { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("creator")]
    public User Creator { get; set; }

    [JsonPropertyName("open_issues")]
    public int OpenIssues { get; set; }

    [JsonPropertyName("closed_issues")]
    public int ClosedIssues { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; }

    [JsonPropertyName("due_on")]
    public DateTime? DueOn { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTime? ClosedAt { get; set; }
}
