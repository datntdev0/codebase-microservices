﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datntdev.Abp.Application.Services;
using datntdev.Abp.BackgroundJobs;
using datntdev.Abp.Json;
using datntdev.Abp.Webhooks.BackgroundWorker;
using datntdev.Abp.Collections.Extensions;

namespace datntdev.Abp.Webhooks
{
    public class DefaultWebhookPublisher : ApplicationService, IWebhookPublisher
    {
        public IWebhookEventStore WebhookEventStore { get; set; }

        private readonly IGuidGenerator _guidGenerator;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;
        private readonly IWebhooksConfiguration _webhooksConfiguration;

        public DefaultWebhookPublisher(
            IWebhookSubscriptionManager webhookSubscriptionManager,
            IWebhooksConfiguration webhooksConfiguration,
            IGuidGenerator guidGenerator,
            IBackgroundJobManager backgroundJobManager)
        {
            _guidGenerator = guidGenerator;
            _backgroundJobManager = backgroundJobManager;
            _webhookSubscriptionManager = webhookSubscriptionManager;
            _webhooksConfiguration = webhooksConfiguration;

            WebhookEventStore = NullWebhookEventStore.Instance;
        }
        
        #region Async Publish Methods
        public virtual async Task PublishAsync(string webhookName, object data, bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGrantedAsync(AbpSession.TenantId, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual async Task PublishAsync(string webhookName, object data, int? tenantId,
            bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGrantedAsync(tenantId, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual async Task PublishAsync(int?[] tenantIds, string webhookName, object data,
            bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(tenantIds, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        private async Task PublishAsync(string webhookName, object data, List<WebhookSubscription> webhookSubscriptions,
            bool sendExactSameData = false, WebhookHeader headers = null)
        {
            if (webhookSubscriptions.IsNullOrEmpty())
            {
                return;
            }

            var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

            foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
            {
                var webhookInfo = await SaveAndGetWebhookAsync(subscriptionGroupedByTenant.Key, webhookName, data);

                foreach (var webhookSubscription in subscriptionGroupedByTenant)
                {
                    var headersToSend = webhookSubscription.Headers;
                    if (headers != null)
                    {
                        if (headers.UseOnlyGivenHeaders)//do not use the headers defined in subscription
                        {
                            headersToSend = headers.Headers;
                        }
                        else
                        {
                            //use the headers defined in subscription. If additional headers has same header, use additional headers value.
                            foreach (var additionalHeader in headers.Headers)
                            {
                                headersToSend[additionalHeader.Key] = additionalHeader.Value;
                            }
                        }
                    }
                    
                    await _backgroundJobManager.EnqueueAsync<WebhookSenderJob, WebhookSenderArgs>(new WebhookSenderArgs
                    {
                        TenantId = webhookSubscription.TenantId,
                        WebhookEventId = webhookInfo.Id,
                        Data = webhookInfo.Data,
                        WebhookName = webhookInfo.WebhookName,
                        WebhookSubscriptionId = webhookSubscription.Id,
                        Headers = headersToSend,
                        Secret = webhookSubscription.Secret,
                        WebhookUri = webhookSubscription.WebhookUri,
                        SendExactSameData = sendExactSameData
                    });
                }
            }
        }
        
        #endregion
        
        #region Sync Publish Methods
        public virtual void Publish(string webhookName, object data, bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions =
                _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGranted(AbpSession.TenantId, webhookName);
            Publish(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual void Publish(string webhookName, object data, int? tenantId, bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions = _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGranted(tenantId, webhookName);
            Publish(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual void Publish(int?[] tenantIds, string webhookName, object data, bool sendExactSameData = false, WebhookHeader headers = null)
        {
            var subscriptions =
                _webhookSubscriptionManager.GetAllSubscriptionsOfTenantsIfFeaturesGranted(tenantIds, webhookName);
            Publish(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        private void Publish(string webhookName, object data, List<WebhookSubscription> webhookSubscriptions,
            bool sendExactSameData = false, WebhookHeader headers = null)
        {
            if (webhookSubscriptions.IsNullOrEmpty())
            {
                return;
            }

            var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

            foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
            {
                var webhookInfo = SaveAndGetWebhook(subscriptionGroupedByTenant.Key, webhookName, data);

                foreach (var webhookSubscription in subscriptionGroupedByTenant)
                {
                    var headersToSend = webhookSubscription.Headers;
                    if (headers != null)
                    {
                        if (headers.UseOnlyGivenHeaders)//do not use the headers defined in subscription
                        {
                            headersToSend = headers.Headers;
                        }
                        else
                        {
                            //use the headers defined in subscription. If additional headers has same header, use additional headers value.
                            foreach (var additionalHeader in headers.Headers)
                            {
                                headersToSend[additionalHeader.Key] = additionalHeader.Value;
                            }
                        }
                    }

                    _backgroundJobManager.Enqueue<WebhookSenderJob, WebhookSenderArgs>(new WebhookSenderArgs
                    {
                        TenantId = webhookSubscription.TenantId,
                        WebhookEventId = webhookInfo.Id,
                        Data = webhookInfo.Data,
                        WebhookName = webhookInfo.WebhookName,
                        WebhookSubscriptionId = webhookSubscription.Id,
                        Headers = headersToSend,
                        Secret = webhookSubscription.Secret,
                        WebhookUri = webhookSubscription.WebhookUri,
                        SendExactSameData = sendExactSameData
                    });
                }
            }
        }

        #endregion

        protected virtual async Task<WebhookEvent> SaveAndGetWebhookAsync(int? tenantId, string webhookName,
            object data)
        {
            var webhookInfo = new WebhookEvent
            {
                Id = _guidGenerator.Create(),
                WebhookName = webhookName,
                Data = _webhooksConfiguration.JsonSerializerOptions != null
                    ? data.ToJsonString(_webhooksConfiguration.JsonSerializerOptions)
                    : data.ToJsonString(),
                TenantId = tenantId
            };

            await WebhookEventStore.InsertAndGetIdAsync(webhookInfo);
            return webhookInfo;
        }

        protected virtual WebhookEvent SaveAndGetWebhook(int? tenantId, string webhookName, object data)
        {
            var webhookInfo = new WebhookEvent
            {
                Id = _guidGenerator.Create(),
                WebhookName = webhookName,
                Data = _webhooksConfiguration.JsonSerializerOptions != null
                    ? data.ToJsonString(_webhooksConfiguration.JsonSerializerOptions)
                    : data.ToJsonString(),
                TenantId = tenantId
            };

            WebhookEventStore.InsertAndGetId(webhookInfo);
            return webhookInfo;
        }
    }
}