using Microsoft.EntityFrameworkCore;
using SB.WebAPI.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.WebAPI.Persistence.Repositories
{
    /// <summary>
    /// Topic Repository
    /// </summary>
    public class TopicRepository : ITopicRepository
    {
        private readonly ServiceBusContext _context;
        /// <summary>
        /// Creates a new instance of TopicRepository
        /// </summary>
        /// <param name="context">SB Context to use in the repository</param>
        public TopicRepository(ServiceBusContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new TopicLog entry
        /// </summary>
        /// <param name="topicLog">the entry to add</param>
        public async Task AddTopicLogAsync(TopicLog topicLog)
        {
            topicLog.Timestamp = DateTime.UtcNow;
            await _context.TopicLog.AddAsync(topicLog);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get all rows in TopicLog
        /// </summary>
        /// <returns>A collection of TopiClog</returns>
        public async Task<List<TopicLog>> GetAllTopicLogAsync()
        {
            var actions = await _context.TopicLog.ToListAsync();
            return actions;
        }
    }
}