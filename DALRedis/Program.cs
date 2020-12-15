using DAL.Enteties;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            PostRepository postRepository = new PostRepository();

            string host = "localhost";
            postRepository.Add(host, new Post());
            postRepository.Update(host, new ObjectId(),new Post());
            postRepository.UpdatePost(host, new ObjectId(),"someText");
        }
    }
}
