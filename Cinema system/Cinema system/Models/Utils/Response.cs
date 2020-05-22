using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public class Response
    {
        public bool IsSuccessful { get; set; }
        public List<string> ErrorDescriptions { get; set; }
        
        public Response()
        {
            ErrorDescriptions = new List<string>();
        }
    }

    public class GetResponse<T> : Response
    {
        public T RequestedData { get; set; }
    }

    public class CreateResponse : Response
    {
        public int Id { get; set; }
    }
    
}
