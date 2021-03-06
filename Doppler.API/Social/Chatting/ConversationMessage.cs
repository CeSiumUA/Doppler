﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doppler.API.Social.Chatting
{
    public class ConversationMessage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ClientGeneratedId { get; set; }
        public long? SenderId { get; set; }
        public ConversationMember Sender { get; set; }
        public long? ReceiverId { get; set; }
        public ConversationMember? Receiver { get; set; }
        public bool Deleted { get; set; }
        public DateTime SentOn { get; set; }
        public Guid ContentId { get; set; }
        public ConversationMessageContent Content { get; set; }

        public ConversationMessage()
        {
            this.SentOn = DateTime.UtcNow;
        }
    }
}
