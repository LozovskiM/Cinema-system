using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Models
{
    public class Response
    {
        public bool IsSuccessful { get; set; }
        public List<string> ErrorDescriptions { get; set; }
        
        public Response()
        {
            ErrorDescriptions = new List<string>();
            IsSuccessful = true;
        }

        public Response (string ErrorMessage)
        {
            ErrorDescriptions = new List<string>
            {
                ErrorMessage
            };
            IsSuccessful = false;
        }

        public Response(ModelStateDictionary model)
        {
            ErrorDescriptions.AddRange(model.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
            IsSuccessful = false;
        }
    }

    public class GetResponse<T> : Response
    {
        public T RequestedData { get; set; }

        public GetResponse(T data)
        {
            IsSuccessful = true;
            RequestedData = data;   
        }
    }

    public class CreateResponse : Response
    {
        public int Id { get; set; }

        public CreateResponse(int id)
        {
            IsSuccessful = true;
            Id = id;
        }
    }

    public class LoginResponse : Response
    {
        public string UserName { get; set; }
        public string Token { get; set; }

        public LoginResponse(UserLoginInfo user)
        {
            IsSuccessful = true;
            Token = user.Token;
            UserName = user.UserName;
        }

        public LoginResponse(string token)
        {
            IsSuccessful = true;
            Token = token;
        }
    }

}
