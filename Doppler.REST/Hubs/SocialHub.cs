﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.API.Social;
using Doppler.API.Social.Chatting;
using Doppler.API.Social.Likes;
using Doppler.REST.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doppler.REST.Hubs
{
    [Authorize]
    public class SocialHub : Hub
    {
        private readonly SocialService socialService;
        private readonly IdentityService identityService;
        private readonly HubClientsMappingService hubClientsMappingService;
        private readonly PushNotificationService pushNotificationService;
        public SocialHub(SocialService socialService, IdentityService identityService, HubClientsMappingService hubClientsMappingService, PushNotificationService pushNotificationService)
        {
            this.socialService = socialService;
            this.identityService = identityService;
            this.hubClientsMappingService = hubClientsMappingService;
            this.pushNotificationService = pushNotificationService;
        }
        public async Task<User> GetUser(string login)
        {
            return await socialService.GetContactAsync(login);
        }

        public async Task<UserContact> GetContact(string login)
        {
            return await socialService.GetUserContact(login);
        }
        public async Task<List<User>> SearchUsers(string pattern)
        {
            return await socialService.SearchUsersAsync(pattern);
        }

        public async Task<List<UserContact>> GetUserContacts(int? skip = 0, int? take = null)
        {
            return await socialService.GetUserContacts();
        }

        public async Task<LikeResult> RateProfile(string login, bool like)
        {
            return await socialService.RateProfile(login, like);
        }

        public async Task<bool> CheckUserForLike(string login)
        {
            return await this.socialService.CheckUserForLike(login);
        }
        public async Task AddToContacts(string login, string displayName = null)
        {
            await socialService.AddToContacts(login, displayName);
        }

        public async Task<Guid> GetDialogueInstanceId(string login)
        {
            return await this.socialService.GetDialogueInstanceId(login);
        }

        public async Task<List<Conversation>> GetUserConversations(int? skip = 0, int? take = null)
        {
            return await this.socialService.GetUserConversations(skip, take);
        }

        public async Task<Conversation> GetUserConversation(Guid id)
        {
            return await this.socialService.GetUserConversation(id);
        }

        public async Task<List<ConversationMessage>> GetConversationMessages(Guid chatId, int? skip = 0,
            int? take = null)
        {
            return await socialService.GetConversationMessages(chatId, skip, take);
        }

        public async Task<object> WriteMessageToChat(Guid chatId, ConversationMessage message)
        {
            var sendMesageResult = await this.socialService.WriteMessageToChat(chatId, message);
            var typer = this.Context?.User?.Identity?.Name;
            var phoneNumbers = await this.socialService.GetConversationMembersPhones(chatId);
            foreach (var phone in phoneNumbers)
            {
                if (phone != typer)
                {
                    var connectionId = this.hubClientsMappingService.Get(phone);
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await this.Clients.Client(connectionId).SendAsync("HandleNewMessageInput", chatId, typer);
                    }
                    else
                    {
                        await pushNotificationService.SendPersonalNotification();
                    }
                }
            }
            return new
            {
                messageId = sendMesageResult?.Id,
                clientGeneratedId = sendMesageResult?.ClientGeneratedId,
                delivered = sendMesageResult != null
            };
        }

        public async Task HandleTyping(Guid chatId)
        {
            var typer = this.Context?.User?.Identity?.Name;
            var phoneNumbers = await this.socialService.GetConversationMembersPhones(chatId);
            foreach (var phone in phoneNumbers)
            {
                if (phone != typer)
                {
                    var connectionId = this.hubClientsMappingService.Get(phone);
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await this.Clients.Client(connectionId).SendAsync("HandleChatTyping", chatId, typer);
                    }
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            var identityName = this.Context.User.Identity.Name;
            var connectionId = this.Context.ConnectionId;
            this.hubClientsMappingService.Set(identityName, connectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        { 
            this.hubClientsMappingService.Remove(this.Context.User.Identity.Name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
