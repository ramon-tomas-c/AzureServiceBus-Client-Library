using System.Collections.Generic;
using System.Threading.Tasks;
using SB.WebAPI.Persistence.Entities;

namespace SB.WebAPI.Persistence.Repositories
{
    /// <summary>
    /// Interface for Topic Repository
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        /// Add a TopicLog async
        /// </summary>
        /// <param name="topicLog"></param>
        /// <returns></returns>
        Task AddTopicLogAsync(TopicLog topicLog);

        /// <summary>
        /// Get all TopicLogs async
        /// </summary>
        /// <returns>A collection of TopicLog</returns>
        Task<List<TopicLog>> GetAllTopicLogAsync();
    }
}