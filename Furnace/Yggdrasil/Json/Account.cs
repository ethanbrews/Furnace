﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Yggdrasil.Json.Account;
//
//    var mojangAccount = MojangAccount.FromJson(jsonString);

namespace Furnace.Yggdrasil.Json.Account
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MojangAccount
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("clientToken")]
        public string ClientToken { get; set; }

        [JsonProperty("availableProfiles")]
        public Profile[] AvailableProfiles { get; set; }

        [JsonProperty("selectedProfile")]
        public Profile SelectedProfile { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        public bool IsSelected { get; set; }
    }

    public partial class Profile
    {
        [JsonProperty("agent", NullValueHandling = NullValueHandling.Ignore)]
        public string Agent { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("legacyProfile")]
        public bool LegacyProfile { get; set; }

        [JsonProperty("suspended")]
        public bool Suspended { get; set; }

        [JsonProperty("paid")]
        public bool Paid { get; set; }

        [JsonProperty("migrated")]
        public bool Migrated { get; set; }

        [JsonProperty("legacy")]
        public bool Legacy { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("registerIp")]
        public string RegisterIp { get; set; }

        [JsonProperty("migratedFrom")]
        public string MigratedFrom { get; set; }

        [JsonProperty("migratedAt")]
        public long MigratedAt { get; set; }

        [JsonProperty("registeredAt")]
        public long RegisteredAt { get; set; }

        [JsonProperty("passwordChangedAt")]
        public long PasswordChangedAt { get; set; }

        [JsonProperty("dateOfBirth")]
        public long DateOfBirth { get; set; }

        [JsonProperty("suspended")]
        public bool Suspended { get; set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; set; }

        [JsonProperty("secured")]
        public bool Secured { get; set; }

        [JsonProperty("migrated")]
        public bool Migrated { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("legacyUser")]
        public bool LegacyUser { get; set; }

        [JsonProperty("verifiedByParent")]
        public bool VerifiedByParent { get; set; }

        [JsonProperty("properties")]
        public Property[] Properties { get; set; }
    }

    public partial class Property
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class MojangAccount
    {
        public static MojangAccount FromJson(string json) => JsonConvert.DeserializeObject<MojangAccount>(json, Furnace.Yggdrasil.Json.Account.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MojangAccount self) => JsonConvert.SerializeObject(self, Furnace.Yggdrasil.Json.Account.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}