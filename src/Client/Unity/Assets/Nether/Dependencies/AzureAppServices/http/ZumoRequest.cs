﻿using UnityEngine;
using System.Text;
using RESTClient;

namespace Azure.AppServices {
  public sealed class ZumoRequest : RestRequest {

    private bool excludeSystemProperties; // strips App Services System Properties from request body
    public ZumoRequest(string url, Method httpMethod = Method.GET, bool excludeSystemProperties = true, AuthenticatedUser user = null) : base(url, httpMethod) {
      this.excludeSystemProperties = excludeSystemProperties;
      this.AddHeader("ZUMO-API-VERSION", "2.0.0");
      this.AddHeader("Accept", "application/json");
      this.AddHeader("Content-Type", "application/json; charset=utf-8");
      // User Authentictated request
      if (user != null && !string.IsNullOrEmpty(user.authenticationToken)) {
        this.AddHeader("X-ZUMO-AUTH", user.authenticationToken);
      }
    }

    public void AddBodyAccessToken(string token) {
      AccessToken accessToken = new AccessToken(token);
      this.AddBody<AccessToken>(accessToken);
    }

    public override void AddBody<T>(T data, string contentType = "application/json; charset=utf-8") {
      string jsonString = excludeSystemProperties ? JsonHelper.ToJsonExcludingSystemProperties(data) : JsonUtility.ToJson(data);
      byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
      this.AddBody(bytes, contentType);
    }

  }
}
