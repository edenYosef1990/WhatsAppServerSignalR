﻿using EdenWhatsApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using WhatsAppServer.Services;
using System.Threading.Channels;
using System.Reactive.Linq;
using System.Reactive;

namespace EdenWhatsApp.Hubs
{
    public class ChatHub : Hub
    {

        private IMessageRepoistory _messageRepository;

        public ChatHub(IMessageRepoistory messageRepoistory)
        {
            _messageRepository = messageRepoistory;
        }
        public async Task BroadcastChatData(MessageModel data)
        {
            _messageRepository.InsertMessage(data);
            await Clients.All.SendAsync("broadcastChatData", data);
        }

        public async Task transferMessageToClients(MessageModel message)
        {
            _messageRepository.InsertMessage(message);
            await Clients.All.SendAsync("broadcastNewMessage", message);
        }

        public void TransferMessageFromClient(MessageModel message)
        {
            _messageRepository.InsertMessage(message);
        }

        public IObservable<MessageModel> ObserveMessages()
        {
            return _messageRepository.MessageObservable;
        }

        public IAsyncEnumerable<MessageModel> StreamMessages()
        {
            return _messageRepository.MessageObservable.ToAsyncEnumerable();
        }

        //transferMessage
    }

}
