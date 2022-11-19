using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Cissy;
using Cissy.Serialization;

namespace Cissy.IS4
{

    public abstract class ApiController : Controller
    {
        public JsonResult JsonPack<T>(JsonBase<T> json)
        {
            return new JsonResult(json);
        }
        public JsonResult ErrMessage(int State, string Message = "", string Data = "")
        {
            return JsonPack(new Err(State, Message, Data));
        }
        public JsonResult Success<T>(T Data, string Message = "")
        {
            return JsonPack(new Success<T>(Data, Message));
        }
        public ActionResult Success(string Message)
        {
            return JsonPack(new Success<string>(string.Empty, Message));
        }
    }
}
