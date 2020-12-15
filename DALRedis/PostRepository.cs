using DAL;
using DAL.Enteties;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALRedis
{
    class PostRepository
    {

        IMongoDatabase database;
        IMongoCollection<Post> collection;
        public PostRepository()
        {
            database = MongoConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<Post>(GetTableName());
        }

        private string GetTableName()
        {
            return "post";
        }

        public void Add(string host, Post post)
        {
            using (RedisClient redisClient = new RedisClient(host) )
            {
                redisClient.Set(post.Id.ToString(), post.Text);
            }
        }
        public void Update(string host, ObjectId id, Post post)
        {
            using (RedisClient redisClient = new RedisClient(host))
            {
                redisClient.Set(id.ToString(), post.Text);
            }
            collection.ReplaceOne(p => p.Id == id, post);
        }

        public void UpdatePost(string host, ObjectId postId, string newTxt)
        {
            using (RedisClient redisClient = new RedisClient(host))
            {
                redisClient.Set(postId.ToString(), newTxt);
            }
        }

        public void AddLike(string UserNickname, ObjectId postId)
        {

            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Inc("Like", 1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Push("PersonsWhoLike", UserNickname);
            collection.UpdateOne(filter, update);

        }

    }
}
