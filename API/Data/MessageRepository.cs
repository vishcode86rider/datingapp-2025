using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(string messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams 
           messageParams)
    {
        var query = context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Outbox" => query.Where(m => m.SenderId == messageParams.MemberId 
                && m.SenderDeleted == false),
            _ => query.Where(m => m.RecipientId == messageParams.MemberId 
                   && m.RecipientDeleted == false)
        };

        var messageQuery = query.Select(MessageExtensions.ToDtoProjection());  

        return await PaginationHelper.CreateAsync(messageQuery, messageParams.PageNumber, 
            messageParams.PageSize);  
    }

    public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
    {
        await context.Messages
            .Where(m => m.RecipientId == currentMemberId && m.SenderId == recipientId 
               && m.DataRead == null)
            .ExecuteUpdateAsync(setter => setter
            .SetProperty(m => m.DataRead, DateTime.UtcNow));  

        return await context.Messages
            .Where(m => (m.RecipientId == currentMemberId && m.RecipientDeleted == false 
               && m.SenderId == recipientId) 
               || (m.RecipientId == recipientId && m.SenderDeleted == false 
               && m.SenderId == currentMemberId))  
            .OrderBy(m => m.MessageSent)
            .Select(MessageExtensions.ToDtoProjection())
            .ToListAsync(); 
                        
            
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
