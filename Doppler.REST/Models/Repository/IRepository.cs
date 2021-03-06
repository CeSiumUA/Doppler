﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Doppler.API.Authentication;
using Doppler.API.Social;
using Doppler.API.Social.Chatting;
using Doppler.API.Social.Likes;
using Doppler.API.Storage.FileStorage;
using Doppler.REST.Models.Social;
using Microsoft.AspNetCore.Http;

namespace Doppler.REST.Models.Repository
{
    public interface IRepository
    {
        public Task<DopplerUser> GetDopplerUserWithPassword(string login);
        public Task<bool> AddUserAsync(DopplerUser dopplerUser);
        public Task<JwtToken> AssignNewRefreshTokenAsync(DopplerUser dopplerUser, JwtToken jwtToken);
        public Task<List<User>> SearchUsersByWordAsync(string keyWord);
        public Task<List<UserContact>> GetUserContacts(User user, int? skip = 0, int? take = null);
        public Task<Data> GetFileData(Guid Id);
        public Task<User> GetContactAsync(string login);
        public Task<UserContact> GetUserContactAsync(User user, string login);
        public Task AddToContacts(User user, string login, string displayName = null);
        public Task<LikeResult> RateProfile(User user, string login, bool like);
        public Task<bool> CheckUserForLike(User user, string login);
        public Task<Guid> GetDialogueInstanceId(User user, string login);
        public Task<Guid> SetProfileImage(User user, IFormFile formFile);
        public Task<List<Conversation>> GetUserConversationsAsync(User user, int? skip = 0, int? take = null);
        public Task<Conversation> GetUserConversationAsync(User user, Guid id);
        public Task<List<ConversationMessage>> GetConversationMessages(User user, Guid conversationId, int? skip = 0, int? take = null);
        public Task<ConversationMessage> WriteMessageToChat(User user, Guid chatId, ConversationMessage message);
        public Task<IEnumerable<string>> GetConversationMembersPhones(Guid id);
    }
}
